CREATE PROCEDURE `SCI_OP_OPNEW`(P_FUNDINGBODYID INTEGER, P_USERID INTEGER,out x_WORKFLOWID     INTEGER)
BEGIN
   DECLARE V_NULLCOUNT       INTEGER;
   DECLARE V_OPPORTUNITYID   INTEGER;
   DECLARE V_WORKFLOWID      INTEGER;
  -- x_WORKFLOWID      INTEGER;
   DECLARE V_TEMPLATEID      INTEGER;
   
   DECLARE V_SEQSTEP  INTEGER;
   
   SELECT   COUNT( * )
     INTO   V_NULLCOUNT
     FROM   OPPORTUNITY_MASTER OM, SCI_WORKFLOW SW
    WHERE       FUNDINGBODYID = P_FUNDINGBODYID
            AND OM.OPPORTUNITYID = SW.ID
            AND OPPORTUNITYNAME IS NULL
            AND SW.TASKID = 1
            AND STATUSID IS NULL;

   IF V_NULLCOUNT > 0
   THEN
      SELECT   OPPORTUNITYID, WORKFLOWID
        INTO   V_OPPORTUNITYID, x_WORKFLOWID
        FROM   OPPORTUNITY_MASTER OM, SCI_WORKFLOW SW
       WHERE       FUNDINGBODYID = P_FUNDINGBODYID
               AND OM.OPPORTUNITYID = SW.ID
               AND OPPORTUNITYNAME IS NULL
               AND STATUSID IS NULL
               AND SW.TASKID = 1
      LIMIT 1;

      UPDATE   SCI_WORKFLOW
         SET   STARTDATE = SYSDATE(), STARTBY = P_USERID, STATUSID = 7
       WHERE   WORKFLOWID = x_WORKFLOWID AND STATUSID IS NULL;
   ELSEIF V_NULLCOUNT = 0
   THEN
      
      SELECT   OPPORTUNITYID_SEQ.NEXTVAL INTO V_OPPORTUNITYID FROM DUAL;

      SELECT   TEMPLATEID
        INTO   V_TEMPLATEID
        FROM   SCI_DEFAULTTEMPLATE
       WHERE   ACTIVE = 1 AND MODULEID = 3;

      INSERT INTO OPPORTUNITY_MASTER (OPPORTUNITYID,
                                      FUNDINGBODYID,
                                      CREATEDBY,
                                      CYCLE,
                                      STATUSCODE)
        VALUES   (V_OPPORTUNITYID,
                  P_FUNDINGBODYID,
                  P_USERID,
                  0,
                  1);

/*
      DECLARE I CURSOR FOR SELECT   *
                  FROM   SCI_WORKFLOWTEMPLATE
                 WHERE   TEMPLATEID = (SELECT   TEMPLATEID
                                         FROM   SCI_DEFAULTTEMPLATE
                                        WHERE   ACTIVE = 1 AND MODULEID = 3);
      OPEN I;
      FETCH I INTO;
      WHILE NOT_FOUND=0
      DO
         SELECT   WORKFLOW_SEQ.NEXTVAL INTO V_WORKFLOWID FROM DUAL;

         IF I.TASKID = 1
         THEN
         
          SET x_WORKFLOWID= V_WORKFLOWID;
            INSERT INTO SCI_WORKFLOW (WORKFLOWID,
                                      MODULEID,
                                      ID,
                                      CYCLE,
                                      TEMPLATEID,
                                      TASKID,
                                      SEQUENCE,
                                      STARTDATE,
                                      STARTBY,
                                      STATUSID)
              VALUES   (V_WORKFLOWID,
                        3,
                        V_OPPORTUNITYID,
                        0,
                        V_TEMPLATEID,
                        I.TASKID,
                        I.SEQUENCE,
                        SYSDATE(),
                        P_USERID,
                        7);
                        ELSE
                         INSERT INTO SCI_WORKFLOW (WORKFLOWID,
                                      MODULEID,
                                      ID,
                                      CYCLE,
                                      TEMPLATEID,
                                      TASKID,
                                      SEQUENCE
                                      -- STARTDATE,
                                      -- STARTBY,
                                     -- STATUSID
                                      )
              VALUES   (V_WORKFLOWID,
                        3,
                        V_OPPORTUNITYID,
                        0,
                        V_TEMPLATEID,
                        I.TASKID,
                        I.SEQUENCE
                        -- SYSDATE,
                       -- P_USERID,
                       -- 7
                        );

         END IF;
      FETCH  INTO;
      END WHILE;
      CLOSE ;
      */
   END IF;
END;
