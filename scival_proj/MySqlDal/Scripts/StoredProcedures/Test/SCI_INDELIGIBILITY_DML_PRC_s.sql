CREATE PROCEDURE `SCI_INDELIGIBILITY_DML_PRC_s`(
P_DELETEGROUPID                      INTEGER /* DEFAULT NULL */,
   P_WORKFLOWID                         INTEGER,
   P_NOT_SPECIFIED                      VARCHAR(4000) /* DEFAULT NULL */ ,
   P_DEGREE                             VARCHAR(4000) /* DEFAULT NULL */ ,
   P_GRADUATE                           VARCHAR(4000) /* DEFAULT NULL */ ,
   P_NEWFACULTY                         VARCHAR(4000) /* DEFAULT NULL */ ,
   P_UNDERGRADUATE                      VARCHAR(4000) /* DEFAULT NULL */ ,
   P_NORESTRICTION                      VARCHAR(4000) /* DEFAULT NULL */ ,
   P_COUNTRY                            VARCHAR(4000) /* DEFAULT NULL */ ,
   P_CITIZENSHIP_TEXT                   VARCHAR(4000) /* DEFAULT NULL */ ,
   P_ELIGIBILITYCLASSIFICATION_ID       INTEGER /* DEFAULT NULL */ ,
   P_INDIVIDUALELIGIBILITY_ID           INTEGER /* DEFAULT NULL */ ,
   p_lang                               VARCHAR(4000) /* DEFAULT NULL */ ,
   P_MODE                               INTEGER               -- -0 FOR INSERT
)
sp_lbl:

BEGIN
 
   DECLARE V_ID                             INTEGER;
   DECLARE V_TRANSITIONALID                 INTEGER;
   DECLARE V_MODULEID                       INTEGER;
   DECLARE V_TASKID                         INTEGER;
   DECLARE V_CYCLE                          INTEGER;
   DECLARE V_FUNDINGBODY_ID                 INTEGER;
   DECLARE V_NAME                           LONGTEXT;
   DECLARE V_STARTDATE                      DATETIME;
   DECLARE V_AMOUNT                         INTEGER;
   DECLARE V_AMOUNT_CNT                     INTEGER;
   DECLARE V_FUNDINGBODY_CNT                INTEGER;
   DECLARE V_INDEXEDNAME                    VARCHAR (1000);
   DECLARE V_COUNT                          INTEGER;
   DECLARE L_ELIGCLASSIFICATION_SEQ         INTEGER;
   DECLARE L_COUNT                          INTEGER;
   DECLARE L_INDIVIDUALELIGIBILITY_ID_SEQ   INTEGER;
   DECLARE L_ELIGIBILITYCLASSIFICATION_ID   INTEGER;
   DECLARE L_INDIVIDUAL_ELIG                INTEGER  DEFAULT  0;
   DECLARE L_INDIVIDUALELIGIBILITY_ID       INTEGER;
   DECLARE L_CITIZEN_CNT                    INTEGER;

   SELECT   ID,
            MODULEID,
            TASKID,
            CYCLE
     INTO   V_ID,
            V_MODULEID,
            V_TASKID,
            V_CYCLE
     FROM   SCI_WORKFLOW
    WHERE   WORKFLOWID = P_WORKFLOWID;

   IF V_MODULEID = 3 AND P_MODE = 0
   THEN
  
      SELECT   ELIGIBILITYCLASSIFICATION_SEQ.NEXTVAL
        INTO   L_ELIGCLASSIFICATION_SEQ
        FROM   DUAL;

      INSERT INTO ELIGIBILITYCLASSIFICATION (
                                                ELIGIBILITYCLASSIFICATION_ID,
                                                OPPORTUNITY_ID,lang
                 )
        VALUES   (L_ELIGCLASSIFICATION_SEQ, V_ID,p_lang);

      SELECT   INDIVIDUALELIGIBILITY_ID_SEQ.NEXTVAL
        INTO   L_INDIVIDUALELIGIBILITY_ID_SEQ
        FROM   DUAL;


      INSERT INTO INDIVIDUALELIGIBILITY (NOT_SPECIFIED,
                                         DEGREE,
                                         GRADUATE,
                                         NEWFACULTY,
                                         UNDERGRADUATE,
                                         INDIVIDUALELIGIBILITY_ID,
                                         ELIGIBILITYCLASSIFICATION_ID)
        VALUES   (P_NOT_SPECIFIED,
                  P_DEGREE,
                  P_GRADUATE,
                  P_NEWFACULTY,
                  P_UNDERGRADUATE,
                  L_INDIVIDUALELIGIBILITY_ID_SEQ,
                  L_ELIGCLASSIFICATION_SEQ);

         INSERT INTO CITIZENSHIP (NORESTRICTION,
                                  COUNTRY,
                                  CITIZENSHIP_TEXT,
                                  INDIVIDUALELIGIBILITY_ID)
           VALUES   (P_NORESTRICTION,
                     P_COUNTRY,
                     P_CITIZENSHIP_TEXT,
                     L_INDIVIDUALELIGIBILITY_ID_SEQ);
   ELSEIF V_MODULEID = 3 AND P_MODE = 1                            -- FOR UPDATE
   THEN
      UPDATE   INDIVIDUALELIGIBILITY
         SET   NOT_SPECIFIED = P_NOT_SPECIFIED,
               DEGREE = P_DEGREE,
               GRADUATE = P_GRADUATE,
               NEWFACULTY = P_NEWFACULTY,
               UNDERGRADUATE = P_UNDERGRADUATE
       WHERE   ELIGIBILITYCLASSIFICATION_ID = P_ELIGIBILITYCLASSIFICATION_ID
               AND INDIVIDUALELIGIBILITY_ID = P_INDIVIDUALELIGIBILITY_ID;


      SELECT   COUNT( * )
        INTO   L_CITIZEN_CNT
        FROM   CITIZENSHIP
       WHERE   INDIVIDUALELIGIBILITY_ID = P_INDIVIDUALELIGIBILITY_ID;

      IF L_CITIZEN_CNT > 0
      THEN
         UPDATE   CITIZENSHIP
            SET   NORESTRICTION = P_NORESTRICTION,
                  COUNTRY = P_COUNTRY,
                  CITIZENSHIP_TEXT = P_CITIZENSHIP_TEXT
          WHERE   INDIVIDUALELIGIBILITY_ID = P_INDIVIDUALELIGIBILITY_ID;
      ELSEIF  L_CITIZEN_CNT = 0 
      THEN
         INSERT INTO CITIZENSHIP (NORESTRICTION,
                                  COUNTRY,
                                  CITIZENSHIP_TEXT,
                                  INDIVIDUALELIGIBILITY_ID)
           VALUES   (P_NORESTRICTION,
                     P_COUNTRY,
                     P_CITIZENSHIP_TEXT,
                     P_INDIVIDUALELIGIBILITY_ID);
      END IF;
   
   ELSEIF V_MODULEID = 3 AND P_MODE = 2                           -- FOR DELETE
   THEN
      IF  p_deletegroupid IS NULL   THEN
        DELETE FROM  CITIZENSHIP
         WHERE   INDIVIDUALELIGIBILITY_ID = P_INDIVIDUALELIGIBILITY_ID;

        DELETE  FROM INDIVIDUALELIGIBILITY
         WHERE   INDIVIDUALELIGIBILITY_ID = P_INDIVIDUALELIGIBILITY_ID
                 AND ELIGIBILITYCLASSIFICATION_ID =
                     P_ELIGIBILITYCLASSIFICATION_ID;

        DELETE FROM  ELIGIBILITYCLASSIFICATION
         WHERE   ELIGIBILITYCLASSIFICATION_ID = P_ELIGIBILITYCLASSIFICATION_ID;
      
      ELSE
        DELETE  FROM CITIZENSHIP where INDIVIDUALELIGIBILITY_ID in
         (select INDIVIDUALELIGIBILITY_ID
           FROM INDIVIDUALELIGIBILITY
             WHERE eligibilityclassification_id in
              (select ELIGIBILITYCLASSIFICATION_ID
                from ELIGIBILITYCLASSIFICATION where opportunity_id = V_ID));       

        DELETE FROM INDIVIDUALELIGIBILITY
          where eligibilityclassification_id in
            (select ELIGIBILITYCLASSIFICATION_ID
               from ELIGIBILITYCLASSIFICATION where opportunity_id = V_ID);       

        DELETE FROM ELIGIBILITYCLASSIFICATION E WHERE
        NOT EXISTS (SELECT 1 FROM INDIVIDUALELIGIBILITY I WHERE I.ELIGIBILITYCLASSIFICATION_ID= E.ELIGIBILITYCLASSIFICATION_ID ) AND
        NOT EXISTS (SELECT 1 FROM ORGANIZATIONELIGIBILITY O WHERE O.ELIGIBILITYCLASSIFICATION_ID = E.ELIGIBILITYCLASSIFICATION_ID) AND
        NOT EXISTS (SELECT 1 FROM RESTRICTIONS R WHERE R.ELIGIBILITYCLASSIFICATION_ID = E.ELIGIBILITYCLASSIFICATION_ID) and
        eligibilityclassification_id in (select ELIGIBILITYCLASSIFICATION_ID from ELIGIBILITYCLASSIFICATION where opportunity_id = V_ID);
      END IF; 
   END IF;
END;
