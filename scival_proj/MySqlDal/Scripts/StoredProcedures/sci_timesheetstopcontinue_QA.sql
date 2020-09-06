CREATE PROCEDURE `sci_timesheetstopcontinue_QA`(
p_workflowid           INTEGER,
                                      p_userid               INTEGER,
                                      p_transitionalid       INTEGER,
                                      p_remarks              VARCHAR(4000)
)
BEGIN
   DECLARE v_moduleid         integer;
   DECLARE v_id               integer;
   DECLARE v_FUNDINGBODY_ID   integer;
   
   CALL sci_timesheetstop(p_workflowid,
                      p_userid,
                      p_transitionalid,
                      8,
                      p_remarks,
                      p_o_status,
                      p_o_error);

   SELECT   moduleid, id
     INTO   v_moduleid, v_id
     FROM   sci_workflow
    WHERE   workflowid = p_workflowid;

   IF v_moduleid = 3
   THEN
      SELECT   FUNDINGBODY_ID
        INTO   v_FUNDINGBODY_ID
        FROM   opportunity
       WHERE   OPPORTUNITY_ID = v_id;

         SELECT   WORKFLOWID,
                  OPPORTUNITY_ID,
                  FUNDINGBODYOPPORTUNITYID,
                  NAME
           FROM   OPPORTUNITY op, sci_workflow sw
          WHERE       FUNDINGBODY_ID = v_FUNDINGBODY_ID
                  AND op.OPPORTUNITY_ID = sw.id
                  AND MODULEID = 3
                  AND taskid = 2
                  AND sw.cycle = 0
                  AND IFNULL(STATUSID, 0) NOT IN (8, 7)
                  AND COMPLETEDDATE IS NULL
                  AND NOT EXISTS
                        (SELECT   1
                           FROM   sci_workflow
                          WHERE       ID = sw.ID
                                  AND CYCLE = sw.CYCLE
                                  AND SEQUENCE < sw.SEQUENCE
                                  AND completeddate IS NULL)
         UNION ALL
         SELECT   WORKFLOWID,
                  OPPORTUNITYID,
                  NULL FUNDINGBODYOPPORTUNITYID,
                  OPPORTUNITYNAME name
           FROM   OPPORTUNITY_master om, sci_workflow sw
          WHERE       OM.OPPORTUNITYID = sw.id
                  AND OM.CYCLE = sw.cycle
                  AND sw.taskid = 2
                  AND sw.MODULEID = 3
                  AND FUNDINGBODYID = v_FUNDINGBODY_ID
                  AND isautomated = 1
                  AND om.cycle = 0
                  AND statuscode = 1
                  AND IFNULL(STATUSID, 0) NOT IN (7, 8)
                  AND COMPLETEDDATE IS NULL
                  AND NOT EXISTS (SELECT   1
                                    FROM   OPPORTUNITY
                                   WHERE   OPPORTUNITY_ID = om.OPPORTUNITYID)
                  AND NOT EXISTS
                        (SELECT   1
                           FROM   sci_workflow
                          WHERE       ID = sw.ID
                                  AND CYCLE = sw.CYCLE
                                  AND SEQUENCE < sw.SEQUENCE
                                  AND completeddate IS NULL)
         ORDER BY   name
         LIMIT 1;
   ELSEIF v_moduleid = 4
   THEN
      SELECT   FUNDINGBODY_ID
        INTO   v_FUNDINGBODY_ID
        FROM   award
       WHERE   AWARD_ID = v_id;

         SELECT   workflowid,
                  award_id,
                  fundingbodyawardid,
                  NAME
           FROM   award op, sci_workflow sw,award_master am
          WHERE       fundingbody_id = v_fundingbody_id
                  AND op.award_id = sw.ID
                  And am.awardid=op.award_id
                  And am.statuscode in (1,2)
                  AND moduleid = 4
                  AND sw.CYCLE = 0
                  AND taskid = 2
                  AND IFNULL (statusid, 0) NOT IN (8, 7)
                  AND COMPLETEDDATE IS NULL
                  AND NOT EXISTS
                        (SELECT   1
                           FROM   sci_workflow
                          WHERE       ID = sw.ID
                                  AND CYCLE = sw.CYCLE
                                  AND SEQUENCE < sw.SEQUENCE
                                  AND completeddate IS NULL)
         UNION ALL
         SELECT   WORKFLOWID,
                  AWARDID,
                  NULL FUNDINGBODYOPPORTUNITYID,
                  AWARDNAME name
           FROM   award_master om, sci_workflow sw
          WHERE       OM.awardID = sw.id
                  AND OM.CYCLE = sw.cycle
                  AND sw.taskid = 2
                  AND sw.MODULEID = 4
                  AND FUNDINGBODYID = v_FUNDINGBODY_ID
                  AND isautomated = 1
                  AND om.cycle = 0
                  AND statuscode = 1
                  AND IFNULL (STATUSID, 0) NOT IN (7, 8)
                  AND COMPLETEDDATE IS NULL
                  AND NOT EXISTS (SELECT   1
                                    FROM   AWARD
                                   WHERE   AWARD_ID = om.AWARDID)
                  AND NOT EXISTS
                        (SELECT   1
                           FROM   sci_workflow
                          WHERE       ID = sw.ID
                                  AND CYCLE = sw.CYCLE
                                  AND SEQUENCE < sw.SEQUENCE
                                  AND completeddate IS NULL)
         ORDER BY   name
         LIMIT 1;
   END IF;

END;
