CREATE PROCEDURE `SCI_ORGNELIGIBILITYY_DML_S`(
P_DELETEGROUPID                      INTEGER /* DEFAULT NULL */,
   P_WORKFLOWID                         INTEGER,
   P_NOT_SPECIFIED                      VARCHAR(4000) /* DEFAULT NULL */ ,
   P_ACADEMIC                           VARCHAR(4000) /* DEFAULT NULL */ ,
   P_COMMERCIAL                         VARCHAR(4000) /* DEFAULT NULL */ ,
   P_GOVERNMENT                         VARCHAR(4000) /* DEFAULT NULL */ ,
   P_NONPROFIT                          VARCHAR(4000) /* DEFAULT NULL */ ,
   P_SME                                VARCHAR(4000) /* DEFAULT NULL */ ,
   P_NORESTRICTION                      VARCHAR(4000) /* DEFAULT NULL */ ,
   P_CITY                               VARCHAR(4000) /* DEFAULT NULL */ ,
   P_STATE                              VARCHAR(4000) /* DEFAULT NULL */ ,
   P_COUNTRY                            VARCHAR(4000) /* DEFAULT NULL */ ,
   P_REGIONSPECIFIC_TEXT                VARCHAR(4000) /* DEFAULT NULL */ ,
   P_CITIZENSHIP_TEXT                   VARCHAR(4000) /* DEFAULT NULL */ ,
   P_ELIGIBILITYCLASSIFICATION_ID       INTEGER /* DEFAULT NULL */ ,
   P_ORGANIZATIONELIGIBILITY_ID         INTEGER /* DEFAULT NULL */ ,
   p_lang                                VARCHAR(4000) /* DEFAULT NULL */ ,
   P_MODE                               INTEGER             -- -*0 FOR INSERT
)
sp_lbl:

BEGIN
     DECLARE V_ID                             INTEGER;
   DECLARE V_TRANSITIONALID                 INTEGER;
   DECLARE V_MODULEID                       INTEGER;
   DECLARE V_TASKID                         INTEGER;
   DECLARE V_CYCLE                          INTEGER;
   DECLARE V_FUNDINGBODY_ID                 INTEGER;
   DECLARE V_COUNT                          INTEGER;
   DECLARE L_ELIGCLASSIFICATION_SEQ         INTEGER;
   DECLARE L_COUNT                          INTEGER;
   DECLARE L_ORGANIZATIONELIGIBILITY_SEQ    INTEGER;
   DECLARE L_ELIGIBILITYCLASSIFICATION_ID   INTEGER;
   DECLARE L_ORGANIZATION_ELIG              INTEGER  DEFAULT  0;
   DECLARE L_ORGANIZATIONELIGIBILITY_ID     INTEGER;
   DECLARE L_REGION_CNT                     INTEGER;
   DECLARE V_COUNTRY                        VARCHAR(500);
DECLARE NOT_FOUND INT DEFAULT 0;  

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

      SELECT   ORGANIZATIONELIGIBILITY_SEQ.NEXTVAL
        INTO   L_ORGANIZATIONELIGIBILITY_SEQ
        FROM   DUAL;


      INSERT INTO ORGANIZATIONELIGIBILITY (NOT_SPECIFIED,
                                           ACADEMIC,
                                           COMMERCIAL,
                                           GOVERNMENT,
                                           NONPROFIT,
                                           SME,
                                           ORGANIZATIONELIGIBILITY_ID,
                                           ELIGIBILITYCLASSIFICATION_ID)
        VALUES   (P_NOT_SPECIFIED,
                  P_ACADEMIC,
                  P_COMMERCIAL,
                  P_GOVERNMENT,
                  P_NONPROFIT,
                  P_SME,
                  L_ORGANIZATIONELIGIBILITY_SEQ,
                  L_ELIGCLASSIFICATION_SEQ);

         INSERT INTO REGIONSPECIFIC (NORESTRICTION,
                                     CITY,
                                     STATE,
                                     COUNTRY,
                                     REGIONSPECIFIC_TEXT,
                                     ORGANIZATIONELIGIBILITY_ID)
           VALUES   (P_NORESTRICTION,
                     P_CITY,
                     P_STATE,
                     P_COUNTRY,
                     P_REGIONSPECIFIC_TEXT,
                     L_ORGANIZATIONELIGIBILITY_SEQ);
   
   ELSEIF V_MODULEID = 3 AND P_MODE = 1                        -- --------UPDATE
   THEN
      UPDATE   ORGANIZATIONELIGIBILITY
         SET   NOT_SPECIFIED = P_NOT_SPECIFIED,
               ACADEMIC = P_ACADEMIC,
               COMMERCIAL = P_COMMERCIAL,
               GOVERNMENT = P_GOVERNMENT,
               NONPROFIT = P_NONPROFIT,
               SME = P_SME
       WHERE   ELIGIBILITYCLASSIFICATION_ID = P_ELIGIBILITYCLASSIFICATION_ID
               AND ORGANIZATIONELIGIBILITY_ID = P_ORGANIZATIONELIGIBILITY_ID;


      SELECT   COUNT( * )
        INTO   L_REGION_CNT
        FROM   REGIONSPECIFIC
       WHERE   ORGANIZATIONELIGIBILITY_ID = P_ORGANIZATIONELIGIBILITY_ID;

      IF L_REGION_CNT > 0
      THEN
         UPDATE   REGIONSPECIFIC
            SET   NORESTRICTION = P_NORESTRICTION,
                  CITY = P_CITY,
                  STATE = P_STATE,
                  COUNTRY = P_COUNTRY,
                  REGIONSPECIFIC_TEXT = P_REGIONSPECIFIC_TEXT
          WHERE   ORGANIZATIONELIGIBILITY_ID = P_ORGANIZATIONELIGIBILITY_ID;
      ELSEIF L_REGION_CNT = 0-- AND P_COUNTRY IS NOT NULL      --if condation is comment on 25 oct 2018 now values will be inserted in all case
      THEN
         INSERT INTO REGIONSPECIFIC (NORESTRICTION,
                                     CITY,
                                     STATE,
                                     COUNTRY,
                                     REGIONSPECIFIC_TEXT,
                                     ORGANIZATIONELIGIBILITY_ID)
           VALUES   (P_NORESTRICTION,
                     P_CITY,
                     P_STATE,
                     P_COUNTRY,
                     P_REGIONSPECIFIC_TEXT,
                     P_ORGANIZATIONELIGIBILITY_ID);
      END IF;
           
      
   ELSEIF V_MODULEID = 3 AND P_MODE = 2              -- --------DELETE
   THEN
      IF P_DELETEGROUPID IS NULL THEN
        DELETE FROM  REGIONSPECIFIC
          WHERE   ORGANIZATIONELIGIBILITY_ID = P_ORGANIZATIONELIGIBILITY_ID;


        DELETE FROM  ORGANIZATIONELIGIBILITY
          WHERE   ELIGIBILITYCLASSIFICATION_ID = P_ELIGIBILITYCLASSIFICATION_ID
               AND ORGANIZATIONELIGIBILITY_ID = P_ORGANIZATIONELIGIBILITY_ID;

        DELETE  FROM ELIGIBILITYCLASSIFICATION
           WHERE   ELIGIBILITYCLASSIFICATION_ID = P_ELIGIBILITYCLASSIFICATION_ID;
      ELSE
        delete FROM REGIONSPECIFIC where organizationeligibility_id in
           (select ORGANIZATIONELIGIBILITY_ID from ORGANIZATIONELIGIBILITY
            where eligibilityclassification_id in
             (select ELIGIBILITYCLASSIFICATION_ID from ELIGIBILITYCLASSIFICATION
              where opportunity_id = V_ID));

        delete FROM ORGANIZATIONELIGIBILITY where eligibilityclassification_id in
          (select ELIGIBILITYCLASSIFICATION_ID from ELIGIBILITYCLASSIFICATION where opportunity_id = V_ID);

        DELETE FROM ELIGIBILITYCLASSIFICATION E WHERE
        NOT EXISTS (SELECT 1 FROM INDIVIDUALELIGIBILITY I WHERE I.ELIGIBILITYCLASSIFICATION_ID= E.ELIGIBILITYCLASSIFICATION_ID ) AND
        NOT EXISTS (SELECT 1 FROM ORGANIZATIONELIGIBILITY O WHERE O.ELIGIBILITYCLASSIFICATION_ID = E.ELIGIBILITYCLASSIFICATION_ID) AND
        NOT EXISTS (SELECT 1 FROM RESTRICTIONS R WHERE R.ELIGIBILITYCLASSIFICATION_ID = E.ELIGIBILITYCLASSIFICATION_ID) and
        eligibilityclassification_id in (select ELIGIBILITYCLASSIFICATION_ID from ELIGIBILITYCLASSIFICATION where opportunity_id = V_ID);
        
      END IF;
   END IF;   
END;
