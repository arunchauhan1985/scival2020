CREATE PROCEDURE `SCI_LIMITEDSUBMISSION_DML_PRC`(
P_WORKFLOWID                         INTEGER,
   P_NOT_SPECIFIED                      VARCHAR(4000) /* DEFAULT NULL */ ,
   P_DISABILITIES                       VARCHAR(4000) /* DEFAULT NULL */ ,
   P_INVITATIONONLY                     VARCHAR(4000) /* DEFAULT NULL */ ,
   P_MEMBERONLY                         VARCHAR(4000) /* DEFAULT NULL */ ,
   P_NOMINATIONONLY                     VARCHAR(4000) /* DEFAULT NULL */ ,
   P_MINORTIES                          VARCHAR(4000) /* DEFAULT NULL */ ,
   P_WOMEN                              VARCHAR(4000) /* DEFAULT NULL */ ,
   P_NUMBEROFAPPLICANTSALLOWED          INTEGER /* DEFAULT NULL */ ,
   P_LIMITEDSUBMISSION_TEXT             VARCHAR(4000) /* DEFAULT NULL */ ,
   P_ELIGIBILITYCLASSIFICATION_ID       INTEGER /* DEFAULT NULL */ ,
   P_RESTRICTIONS_ID                    INTEGER /* DEFAULT NULL */ ,
   p_lang                               VARCHAR(4000) /* DEFAULT NULL */ ,
   P_MODE                               INTEGER             -- -*0 FOR INSERT
)
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
   DECLARE L_RESTRICTIONS_ID_SEQ            INTEGER;
   DECLARE L_ELIGIBILITYCLASSIFICATION_ID   INTEGER;
   DECLARE L_RESTRICTIONS_CNT               INTEGER  DEFAULT  0;
   DECLARE L_RESTRICTIONS_ID                INTEGER;
   DECLARE L_SUBMISSION_CNT                 INTEGER;

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

      SELECT   RESTRICTIONS_ID_SEQ.NEXTVAL INTO L_RESTRICTIONS_ID_SEQ FROM DUAL;

      INSERT INTO RESTRICTIONS (NOT_SPECIFIED,
                                DISABILITIES,
                                INVITATIONONLY,
                                MEMBERONLY,
                                NOMINATIONONLY,
                                MINORTIES,
                                WOMEN,
                                RESTRICTIONS_ID,
                                ELIGIBILITYCLASSIFICATION_ID)
        VALUES   (P_NOT_SPECIFIED,
                  P_DISABILITIES,
                  P_INVITATIONONLY,
                  P_MEMBERONLY,
                  P_NOMINATIONONLY,
                  P_MINORTIES,
                  P_WOMEN,
                  L_RESTRICTIONS_ID_SEQ,
                  L_ELIGCLASSIFICATION_SEQ);


      INSERT INTO LIMITEDSUBMISSION (
                                        NUMBEROFAPPLICANTSALLOWED,
                                        LIMITEDSUBMISSION_TEXT,
                                        RESTRICTION_ID
                 )
        VALUES   (
                     P_NUMBEROFAPPLICANTSALLOWED,
                     P_LIMITEDSUBMISSION_TEXT,
                     L_RESTRICTIONS_ID_SEQ
                 );

   ELSEIF V_MODULEID = 3 AND P_MODE = 1                               -- -UPDATE
   THEN
      UPDATE   RESTRICTIONS
         SET   NOT_SPECIFIED = P_NOT_SPECIFIED,
               DISABILITIES = P_DISABILITIES,
               INVITATIONONLY = P_INVITATIONONLY,
               MEMBERONLY = P_MEMBERONLY,
               NOMINATIONONLY = P_NOMINATIONONLY,
               MINORTIES = P_MINORTIES,
               WOMEN = P_WOMEN
       WHERE   ELIGIBILITYCLASSIFICATION_ID = P_ELIGIBILITYCLASSIFICATION_ID
               AND RESTRICTIONS_ID = P_RESTRICTIONS_ID;


      SELECT   COUNT( * )
        INTO   L_SUBMISSION_CNT
        FROM   LIMITEDSUBMISSION
       WHERE   RESTRICTION_ID = P_RESTRICTIONS_ID;


      UPDATE   LIMITEDSUBMISSION
         SET   NUMBEROFAPPLICANTSALLOWED = P_NUMBEROFAPPLICANTSALLOWED,
               LIMITEDSUBMISSION_TEXT = P_LIMITEDSUBMISSION_TEXT
       WHERE   RESTRICTION_ID = P_RESTRICTIONS_ID;
              
   ELSEIF V_MODULEID = 3 AND P_MODE = 2                               -- -DELETE
   THEN
      DELETE FROM  LIMITEDSUBMISSION
       WHERE   RESTRICTION_ID = P_RESTRICTIONS_ID;

      DELETE FROM  RESTRICTIONS
       WHERE   ELIGIBILITYCLASSIFICATION_ID = P_ELIGIBILITYCLASSIFICATION_ID
               AND RESTRICTIONS_ID = P_RESTRICTIONS_ID;


      DELETE FROM  ELIGIBILITYCLASSIFICATION
       WHERE   ELIGIBILITYCLASSIFICATION_ID = P_ELIGIBILITYCLASSIFICATION_ID;
   END IF;
END;
