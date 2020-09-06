CREATE PROCEDURE `SCI_AW_AWNEW`(
 P_FUNDINGBODYID       INTEGER,
   P_USERID              INTEGER
)
BEGIN
   DECLARE V_NULLCOUNT    INTEGER;
   DECLARE V_AWARDID      INTEGER;
   DECLARE V_WORKFLOWID   INTEGER;
   DECLARE V_TEMPLATEID   INTEGER;
   DECLARE x_WORKFLOWID 	INTEGER;
   
   SELECT   COUNT( * )
     INTO   V_NULLCOUNT
     FROM   AWARD_MASTER OM, SCI_WORKFLOW SW
    WHERE       FUNDINGBODYID = P_FUNDINGBODYID
            AND OM.AWARDID = SW.ID
            AND AWARDNAME IS NULL
            AND SW.TASKID = 1
            AND STATUSID IS NULL;

   IF V_NULLCOUNT > 0
   THEN
      SELECT   AWARDID, WORKFLOWID
        INTO   V_AWARDID, x_WORKFLOWID
        FROM   AWARD_MASTER OM, SCI_WORKFLOW SW
       WHERE       FUNDINGBODYID = P_FUNDINGBODYID
               AND OM.AWARDID = SW.ID
               AND AWARDNAME IS NULL
               AND STATUSID IS NULL
               AND SW.TASKID = 1
      LIMIT 1;

      UPDATE   SCI_WORKFLOW
         SET   STARTDATE = SYSDATE(), STARTBY = P_USERID, STATUSID = 7
       WHERE   WORKFLOWID = x_WORKFLOWID AND STATUSID IS NULL;
   ELSEIF V_NULLCOUNT = 0
   THEN

      SELECT   Awardid_SEQ.NEXTVAL INTO V_AWARDID FROM DUAL;
   
      SELECT   TEMPLATEID
        INTO   V_TEMPLATEID
        FROM   SCI_DEFAULTTEMPLATE
       WHERE   ACTIVE = 1 AND MODULEID = 4;

      INSERT INTO award_MASTER (AWARDID,
                                FUNDINGBODYID,
                                CREATEDBY,
                                CYCLE,
                                STATUSCODE)
        VALUES   (V_AWARDID,
                  P_FUNDINGBODYID,
                  P_USERID,
                  0,
                  1);
/*
      DECLARE I CURSOR FOR SELECT TASKID FROM   SCI_WORKFLOWTEMPLATE WHERE  TEMPLATEID IN (SELECT  TEMPLATEID FROM SCI_DEFAULTTEMPLATE WHERE   ACTIVE = 1 AND MODULEID = 4);
      OPEN I;
      FETCH I INTO TempVar;
      WHILE NOT_FOUND=0
      DO
         SELECT   WORKFLOW_SEQ.NEXTVAL INTO V_WORKFLOWID FROM DUAL;

         IF TempVar = 1
         THEN
            SET x_WORKFLOWID = V_WORKFLOWID;

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
                        4,
                        V_AWARDID,
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
                                      )
              VALUES   (V_WORKFLOWID,
                        4,
                        V_AWARDID,
                        0,
                        V_TEMPLATEID,
                        I.TASKID,
                        I.SEQUENCE);
         END IF;
      FETCH I INTO A;
      END WHILE;
      CLOSE I;
      */
   END IF;
END;
