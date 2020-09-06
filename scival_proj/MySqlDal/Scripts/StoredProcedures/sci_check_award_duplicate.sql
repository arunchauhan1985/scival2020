CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_check_award_duplicate`(
   p_workflowid           INTEGER,
   p_userid               INTEGER,
   p_transitionalid       INTEGER,
   p_mode                 INTEGER,
   p_QueryMode            INTEGER,
   p_TaskName             VARCHAR(4000)
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
   DECLARE v_count_name          INTEGER;
   DECLARE v_transitionalid      INTEGER  DEFAULT  0;
   DECLARE v_awardid             INTEGER;
   DECLARE v_opportunityid       INTEGER;
   DECLARE v_fundingbody_id      INTEGER;
   DECLARE V_NAME                LONGTEXT;
   DECLARE v_startdate           DATETIME;
   DECLARE v_amount              INTEGER;
   DECLARE v_amount_cnt          INTEGER;
   DECLARE v_fundingbody_cnt     INTEGER;
   DECLARE v_receipents          VARCHAR (500);
   DECLARE P_O_STATUS1           INTEGER;
   DECLARE P_O_ERROR1            VARCHAR (500);
   DECLARE V_indexedname         VARCHAR (1000);

   DECLARE x_WFLOWID             INTEGER;
 
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

   SELECT   fundingbody_id, STARTDATE
     INTO   v_fundingbody_id, v_startdate
     FROM   award
    WHERE   AWARD_ID = v_id;
  SELECT   count(*)
     INTO   v_count_name
     FROM   SCI_LANGUAGE_DETAIL
    WHERE   SCIVAL_ID = v_id AND COLUMN_ID = 5
      AND language_id = 52;

    IF v_count_name < 1 THEN
       SELECT   COLUMN_DESC
         INTO   V_NAME
         FROM   SCI_LANGUAGE_DETAIL s
         join award_master am on S.SCIVAL_ID=AM.AWARDID and to_char(S.COLUMN_DESC)=AM.AWARDNAME
        WHERE   SCIVAL_ID = v_id AND COLUMN_ID = 5 AND  moduleid=4
       LIMIT 1  ;
           
    else
       SELECT   COLUMN_DESC
         INTO   V_NAME
         FROM   SCI_LANGUAGE_DETAIL
        WHERE   SCIVAL_ID = v_id AND COLUMN_ID = 5
         AND language_id = 52       
         And  moduleid=4 
         LIMIT 1;
           
            
    END IF;


   BEGIN
      DECLARE EXIT HANDLER FOR SQLEXCEPTION
      BEGIN
         SET v_amount=NULL;
      END;
      SELECT   AMOUNT_TEXT
        INTO   v_amount
        FROM   amount
       WHERE   AWARD_ID = v_id AND AWARDEE_ID IS NULL  
      LIMIT 1; 
     END;

   SELECT   awname.indexedname
     INTO   V_indexedname
     FROM   award a,
            awardees awes,
            awardee awe,
            AWARDEENAME awname
    WHERE       a.award_id = awes.award_id
            AND awe.awardees_id = awes.awardees_id
            AND awname.awardee_id = awe.awardee_id
            AND awe.TYPE IN ('PI', 'investigator', 'institution') 
            AND a.award_id = v_id
   LIMIT 1;
   IF p_QueryMode = 0
   THEN
      SELECT   COUNT(*)
        INTO   v_count
        FROM   award a,
               award_master am, 
               awardee awe,
               awardees awes,
               AWARDEEINSTITUTION awins,
               sci_language_detail ld,
               fundingbody_master fm,
               amount amt,
               AWARDEENAME awname
       WHERE       awname.awardee_id = awe.awardee_id
               AND awe.awardees_id = awes.awardees_id
               AND a.award_id = ld.scival_id
               AND ld.column_id = 5
               AND a.fundingbody_id = fm.fundingbody_id
               AND a.award_id = amt.award_id
               AND awe.awardee_id = awins.awardee_id
               AND a.award_id = awes.award_id
               AND A.FUNDINGBODY_ID = v_fundingbody_id
               AND a.award_id <> v_id
               AND AM.AWARDID = A.AWARD_ID
               AND NOT EXISTS(
                      SELECT    1
                         FROM   SCI_XMLDELIVERYDETAIL
                        WHERE   id = am.awardid AND am.statuscode = 3)
               AND awe.TYPE IN ('PI', 'investigator', 'institution')
               AND a.fUNDINGBODYAWARDID = 'Not Available'
               AND TRUNCATE (a.startdate, 0) = v_startdate
               AND TO_CHAR (ld.column_desc) = TO_CHAR (V_NAME)
               AND awname.indexedname = V_indexedname
               AND amt.amount_text = v_amount;
   ELSE
      UPDATE   sci_workflow
         SET   STATUSID = 11
       WHERE   workflowid = p_workflowid;
   END IF;

   IF p_mode = 10
   THEN
      IF p_TaskName = 'QA'
      THEN
            SELECT   workflowid,
                     award_id,
                     fundingbodyawardid,
                     NAME
              FROM   award op, sci_workflow sw
             WHERE       fundingbody_id = v_fundingbody_id
                     AND op.award_id = sw.ID
                     AND moduleid = 4
                     AND sw.CYCLE = 0
                     AND taskid = 2
                     AND IFNULL (statusid, 0) NOT IN (8, 7, 11)
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
                     AND IFNULL (STATUSID, 0) NOT IN (7, 8, 11)
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
      ELSEIF p_TaskName = 'New'
      THEN
      -- Commented by Neha--To disscuss---
         /*SCI_AW_AWNEW (v_fundingbody_id,
                       p_userid,
                       x_WFLOWID,
                       P_O_STATUS1,
                       P_O_ERROR1); */

                  SELECT   x_WFLOWID workflowid,
                     NULL award_id,
                     NULL fundingbodyawardid,
                     NULL NAME
              FROM   DUAL;
      END IF;
   ELSE
               SELECT   NULL workflowid,
                  NULL award_id,
                  NULL fundingbodyawardid,
                  NULL NAME
           FROM   DUAL;
   END IF;
END