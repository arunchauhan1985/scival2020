CREATE PROCEDURE `sci_timesheetstop`(
 p_workflowid           INTEGER,
   p_userid               INTEGER,
   p_transitionalid       INTEGER,
   p_mode                 INTEGER,   -- 8 * stop, 5 * pause, 6 * pause and stop, 10 * exit ,4* hold
   p_remarks              VARCHAR(4000)
)
BEGIN
   DECLARE v_check               INTEGER;
   DECLARE v_moduleid            INTEGER;
   DECLARE v_id                  INTEGER;
   DECLARE v_cycle               INTEGER;
   DECLARE v_sequence            INTEGER;
   DECLARE vm_sequence           INTEGER;
   DECLARE v_taskid              INTEGER;
   DECLARE x_revisionhistoryid   INTEGER;
   DECLARE xx_count              INTEGER;
   DECLARE v_count               INTEGER;
   DECLARE v_transitionalid      INTEGER  DEFAULT  0;
   DECLARE v_awardid             INTEGER;
   DECLARE v_opportunityid       INTEGER;
 
   SELECT   moduleid,
            ID,
            CYCLE,
            SEQUENCE,
            taskid
     INTO   v_moduleid,
            v_id,
            v_cycle,
            v_sequence,
            v_taskid
     FROM   sci_workflow
    WHERE   workflowid = p_workflowid;


   IF v_moduleid = 3 AND P_MODE IN (5, 6)
   THEN
      UPDATE   SCI_EXPIRED_OPPLIST
         SET   FLAG = 0
       WHERE   ID = V_ID;
   END IF;



   SELECT   MAX (SEQUENCE)
     INTO   vm_sequence
     FROM   sci_workflow
    WHERE   moduleid = v_moduleid AND ID = v_id AND CYCLE = v_cycle;

   IF p_mode = 8                                                        -- stop
   THEN
      UPDATE   sci_timesheet
         SET   enddate = SYSDATE(), statuscode = p_mode
       WHERE       transitionalid = p_transitionalid
               AND workflowid = p_workflowid
               AND userid = p_userid;

      UPDATE   sci_workflow
         SET   completeddate = SYSDATE(),
               completedby = p_userid,
               statusid = p_mode
       WHERE   workflowid = p_workflowid;
       commit;-- -suraj

      -- -------------------------------------------------------------------------------------
      -- Fundingbody
      IF v_taskid = 2 AND v_moduleid = 2 AND IFNULL (v_cycle, 0) <> 0
      THEN
         UPDATE   fundingbody_master
            SET   cyclecompletedby = p_userid, cyclecompletiondate = SYSDATE()
          WHERE   fundingbody_id = v_id;

         SELECT   MAX (revisionhistory_id)
           INTO   x_revisionhistoryid
           FROM   revisionhistory
          WHERE   fundingbody_id = v_id;

         UPDATE   revisionhistory
            SET   status = 'update'
          WHERE   revisionhistory_id = x_revisionhistoryid;

         SELECT   IFNULL (MAX (VERSION), 0) + 1
           INTO   xx_count
           FROM   reviseddate
          WHERE   revisionhistory_id = x_revisionhistoryid;

         IF xx_count = 1
         THEN
            INSERT INTO reviseddate (
                                        VERSION,
                                        reviseddate_text,
                                        revisionhistory_id
                       )
              VALUES   (xx_count, SYSDATE(), x_revisionhistoryid);
         ELSE
            UPDATE   reviseddate
               SET   VERSION = xx_count, reviseddate_text = SYSDATE()
             WHERE   revisionhistory_id = x_revisionhistoryid;
         END IF;
      END IF;

      -- ------------------------------------------------------------------------------------------------------------------
      -- Opportunity
      IF v_taskid IN (2, 7) AND v_moduleid = 3 AND IFNULL (v_cycle, 0) <> 0
      THEN
         UPDATE   opportunity_master
            SET   cyclecompletedby = p_userid, cyclecompletiondate = SYSDATE()
          WHERE   opportunityid = v_id;

         SELECT   MAX (revisionhistory_id)
           INTO   x_revisionhistoryid
           FROM   revisionhistory
          WHERE   opportunity_id = v_id;

         UPDATE   revisionhistory
            SET   status = 'update'
          WHERE   revisionhistory_id = x_revisionhistoryid;

         SELECT   IFNULL (MAX (VERSION), 0) + 1
           INTO   xx_count
           FROM   reviseddate
          WHERE   revisionhistory_id = x_revisionhistoryid;

         IF xx_count = 1
         THEN
            INSERT INTO reviseddate (
                                        VERSION,
                                        reviseddate_text,
                                        revisionhistory_id
                       )
              VALUES   (xx_count, SYSDATE(), x_revisionhistoryid);
         ELSE
            UPDATE   reviseddate
               SET   VERSION = xx_count, reviseddate_text = SYSDATE()
             WHERE   revisionhistory_id = x_revisionhistoryid;
         END IF;

         -- Update on 24 April 2019 by S.S.Shah, because opportunity was visible even after stopping with p_mode=8
         UPDATE   SCI_EXPIRED_OPPLIST
            SET   FLAG = 1
          WHERE   ID = V_ID;
      END IF;

      -- -------------------------------------------------------------
      -- Award
      IF v_taskid = 2 AND v_moduleid = 4 AND IFNULL (v_cycle, 0) <> 0
      THEN
         UPDATE   award_master
            SET   cyclecompletedby = p_userid, cyclecompletiondate = SYSDATE()
          WHERE   awardid = v_id;

         SELECT   MAX (revisionhistory_id)
           INTO   x_revisionhistoryid
           FROM   revisionhistory
          WHERE   award_id = v_id;

         UPDATE   revisionhistory
            SET   status = 'update'
          WHERE   revisionhistory_id = x_revisionhistoryid;

         SELECT   IFNULL (MAX (VERSION), 0) + 1
           INTO   xx_count
           FROM   reviseddate
          WHERE   revisionhistory_id = x_revisionhistoryid;

         IF xx_count = 1
         THEN
            INSERT INTO reviseddate (
                                        VERSION,
                                        reviseddate_text,
                                        revisionhistory_id
                       )
              VALUES   (xx_count, SYSDATE(), x_revisionhistoryid);
         ELSE
            UPDATE   reviseddate
               SET   VERSION = xx_count, reviseddate_text = SYSDATE()
             WHERE   revisionhistory_id = x_revisionhistoryid;
         END IF;
      END IF;

      -- ----------------------------------------------------------------------------------
      IF v_taskid = 2 AND v_moduleid = 2
      THEN
         UPDATE   fundingbody_master
            SET   cyclecompletedby = p_userid, cyclecompletiondate = SYSDATE()
          WHERE   fundingbody_id = v_id;
      END IF;

      IF v_taskid = 2 AND v_moduleid = 3
      THEN
         UPDATE   opportunity_master
            SET   cyclecompletedby = p_userid, cyclecompletiondate = SYSDATE()
          WHERE   opportunityid = v_id;
      END IF;

      IF v_taskid = 2 AND v_moduleid = 4
      THEN
         UPDATE   award_master
            SET   cyclecompletedby = p_userid, cyclecompletiondate = SYSDATE()
          WHERE   awardid = v_id;
      END IF;
   ELSEIF p_mode = 5                                                   -- -pause
   THEN
      UPDATE   sci_timesheet
         SET   enddate = SYSDATE()
       WHERE       transitionalid = p_transitionalid
               AND workflowid = p_workflowid
               AND userid = p_userid;

      IF v_moduleid = 3
      THEN
         SELECT   COUNT( * )
           INTO   v_count
           FROM   opportunity
          WHERE   opportunity_id = v_id;

         IF v_count = 0
         THEN
            SELECT   transitionalid
              INTO   v_transitionalid
              FROM   sci_timesheet
             WHERE   ID = v_id AND moduleid = 3;

            DELETE FROM   sci_timesheetremarks
                  WHERE   transitionalid = v_transitionalid;

            DELETE FROM   sci_timesheet
                  WHERE   ID = v_id AND moduleid = 3;

            UPDATE   sci_workflow
               SET   statusid = NULL, startdate = NULL, startby = NULL
             WHERE   workflowid = p_workflowid;
             commit;
         END IF;
      END IF;

      IF v_moduleid = 4
      THEN                                                  -- pause and logoff
         SELECT   COUNT( * )
           INTO   v_count
           FROM   award
          WHERE   award_id = v_id;

         IF v_count = 0
         THEN
            SELECT   transitionalid
              INTO   v_transitionalid
              FROM   sci_timesheet
             WHERE   ID = v_id AND moduleid = 4;

            DELETE FROM   sci_timesheetremarks
                  WHERE   transitionalid = v_transitionalid;

            DELETE FROM   sci_timesheet
                  WHERE   ID = v_id AND moduleid = 4;

            UPDATE   sci_workflow
               SET   statusid = NULL, startdate = NULL, startby = NULL
             WHERE   workflowid = p_workflowid;
             commit;-- --suraj
         END IF;
      END IF;
   ELSEIF p_mode = 6                                         -- pause and logoff
   THEN
      UPDATE   sci_timesheet
         SET   enddate = SYSDATE(), statuscode = p_mode
       WHERE       transitionalid = p_transitionalid
               AND workflowid = p_workflowid
               AND userid = p_userid;

      UPDATE   sci_workflow
         SET   statusid = p_mode
       WHERE   workflowid = p_workflowid;
       commit;-- ---suraj

      IF v_moduleid = 3
      THEN
         SELECT   COUNT( * )
           INTO   v_count
           FROM   opportunity
          WHERE   opportunity_id = v_id;

         IF v_count = 0
         THEN
            SELECT   transitionalid
              INTO   v_transitionalid
              FROM   sci_timesheet
             WHERE   ID = v_id AND moduleid = 3;

            DELETE FROM   sci_timesheetremarks
                  WHERE   transitionalid = v_transitionalid;

            DELETE FROM   sci_timesheet
                  WHERE   ID = v_id AND moduleid = 3;

            UPDATE   sci_workflow
               SET   statusid = NULL, startdate = NULL, startby = NULL
             WHERE   workflowid = p_workflowid;
             commit;-- -suraj
         END IF;
      END IF;

      IF v_moduleid = 4
      THEN                                                  -- pause and logoff
         SELECT   COUNT( * )
           INTO   v_count
           FROM   award
          WHERE   award_id = v_id;

         IF v_count = 0
         THEN
            SELECT   transitionalid
              INTO   v_transitionalid
              FROM   sci_timesheet
             WHERE   ID = v_id AND moduleid = 4;

            DELETE FROM   sci_timesheetremarks
                  WHERE   transitionalid = v_transitionalid;

            DELETE FROM   sci_timesheet
                  WHERE   ID = v_id AND moduleid = 4;

            UPDATE   sci_workflow
               SET   statusid = NULL, startdate = NULL, startby = NULL
             WHERE   workflowid = p_workflowid;
             commit;-- -suraj
         END IF;
      END IF;
   ELSEIF p_mode = 10                                                -- not save
   THEN
      UPDATE   sci_timesheet
         SET   enddate = SYSDATE(), statuscode = p_mode
       WHERE       transitionalid = p_transitionalid
               AND workflowid = p_workflowid
               AND userid = p_userid;

      UPDATE   sci_workflow
         SET   assignby = NULL,
               assigndate = NULL,
               statusid = NULL,
               startby = NULL,
               startdate = NULL
       WHERE   workflowid = p_workflowid;
       commit;-- -suraj
   ELSEIF p_mode = 4
   THEN
      UPDATE   sci_timesheet
         SET   enddate = SYSDATE(), statuscode = p_mode
       WHERE       transitionalid = p_transitionalid
               AND workflowid = p_workflowid
               AND userid = p_userid;

      UPDATE   sci_workflow
         SET   assignby = NULL,
               assigndate = NULL,
               statusid = NULL,
               startby = NULL,
               startdate = NULL
       WHERE   workflowid = p_workflowid;
       commit;-- -suraj

      IF v_moduleid = 2
      THEN
         UPDATE   fundingbody_master
            SET   statuscode = p_mode
          WHERE   fundingbody_id = v_id;
      END IF;
   END IF;

   IF v_moduleid = 3
   THEN
      SELECT   COUNT( * )
        INTO   v_count
        FROM   opportunity
       WHERE   opportunity_id = v_id;
   ELSEIF v_moduleid = 4
   THEN
      SELECT   COUNT( * )
        INTO   v_count
        FROM   AWARD
       WHERE   AWARD_id = v_id;
   ELSEIF v_moduleid = 2
   THEN
      SELECT   COUNT( * )
        INTO   v_count
        FROM   FUNDINGBODY
       WHERE   FUNDINGBODY_id = v_id;
   END IF;

   IF p_remarks IS NOT NULL AND v_count <> 0
   THEN
      INSERT INTO sci_timesheetremarks (workflowid,
                                        transitionalid,
                                        statuscode,
                                        userid,
                                        remarks)
        VALUES   (p_workflowid,
                  p_transitionalid,
                  p_mode,
                  p_userid,
                  p_remarks);
   END IF;

   -- ------------------------------------------------------
   /* Change made by Yogita Johar */
   IF v_moduleid = 3 AND v_cycle = 0
   THEN
      SELECT   COUNT( * )
        INTO   v_check
        FROM   sci_oldsequence
       WHERE   opportunityid IN (SELECT   ID
                                   FROM   opportunity
                                  WHERE   opportunity_id = v_id);

      IF (v_check) = 0
      THEN
         SELECT   COUNT( * )
           INTO   v_count
           FROM   sci_backtoqa
          WHERE   ID = v_id;

         IF v_count = 0
         THEN
                    IF v_opportunityid <> 0
            THEN
               UPDATE   opportunity
                  SET   ID = v_id
                WHERE   opportunity_id = v_id;

               UPDATE   sci_missedsequence
                  SET   OPPORTUNITYFLAG = 1
                WHERE   OPPORTUNITYID = v_opportunityid;
            ELSE
               UPDATE   opportunity
                       SET   ID = v_id
                WHERE   opportunity_id = v_id AND 1 = 2;
            END IF;
         END IF;
      END IF;
   ELSEIF v_moduleid = 4 AND v_cycle = 0
   THEN
      SELECT   COUNT( * )
        INTO   v_check
        FROM   sci_oldsequence
       WHERE   awardid IN (SELECT   ID
                             FROM   award
                            WHERE   award_id = v_id);

      IF (v_check) = 0
      THEN
         SELECT   COUNT( * )
           INTO   v_count
           FROM   sci_backtoqa
          WHERE   ID = v_id;

         IF v_count > 0
         THEN
            SELECT   IFNULL(MIN (awardid), 0)
              INTO   v_awardid
              FROM   sci_missedsequence
             WHERE   awardflag = 0;

            IF v_awardid <> 0
            THEN
               UPDATE   award
                  SET   ID = v_awardid
                WHERE   award_id = v_id;

               UPDATE   sci_missedsequence
                  SET   AWARDFLAG = 1
                WHERE   AWARDID = v_AWARDID;
            ELSE
               UPDATE   award
                             SET   ID = v_id
                WHERE   award_id = v_id;
            END IF;
         END IF;
      END IF;
   END IF;

   -- --------------------------------------------------------------------------
   COMMIT;
END;
