CREATE DEFINER=`root`@`localhost` PROCEDURE `SCI_AW_INSTITUTIONINSDEL41`(
   P_AWARDEEID                 INTEGER,
   P_INSDEL                    INTEGER, 
   P_SCOPUSINSTITUTIONID       INTEGER,
   P_ORG                       VARCHAR(4000),
   P_DEPT                      VARCHAR(4000),
   P_STARTDATE                 DATETIME,
   P_ENDDATE                   DATETIME,
   P_EMAIL                     VARCHAR(4000),
   P_WEBPAGE                   VARCHAR(4000),
   P_EXTAFFI_Id                VARCHAR(4000),
   P_AFFILIATION_ID            INTEGER  
)
BEGIN
   DECLARE V_INSCOUNT               INTEGER;
   DECLARE V_AWARDEEINSTITUTIONID   INTEGER;
   DECLARE DELCOUNT                 INTEGER;
   DECLARE v_count1                 INTEGER;
   DECLARE v_cnt                    INTEGER;
   DECLARE l_affiliationid_seq      INTEGER;
   DECLARE l_type                   varchar(50);
   IF P_INSDEL = 0
   THEN
      SELECT   COUNT(*)
        INTO   V_INSCOUNT
        FROM   AWARDEEINSTITUTION
       WHERE   AWARDEE_ID = P_AWARDEEID;

      -- ---------------------------------------
      IF v_inscount > 0
      THEN
         SELECT   AWARDEEINSTITUTION_ID
           INTO   V_AWARDEEINSTITUTIONID
           FROM   AWARDEEINSTITUTION
          WHERE   AWARDEE_ID = P_AWARDEEID;


         SELECT   COUNT(*)
           INTO   v_count1
           FROM   AFFILIATION
          WHERE       AWARDEEINSTITUTION_ID = V_AWARDEEINSTITUTIONID
                  AND ORG = 'Not Available'
                  AND SCOPUSINSTITUTIONID IS NULL
                  AND DEPT IS NULL
                  AND STARTDATE IS NULL
                  AND ENDDATE IS NULL
                  AND EMAIL IS NULL
                  AND WEBPAGE IS NULL
                  AND EXTERNALAFFILIATIONIDENTIFIER IS NULL;


         IF v_count1 > 0
         THEN
            UPDATE   AFFILIATION
               SET   SCOPUSINSTITUTIONID = P_SCOPUSINSTITUTIONID,
                     ORG = P_ORG,
                     DEPT = P_DEPT,
                     STARTDATE = P_STARTDATE,
                     ENDDATE = P_ENDDATE,
                     EMAIL = P_EMAIL,
                     WEBPAGE = P_WEBPAGE,
                     EXTERNALAFFILIATIONIDENTIFIER = P_EXTAFFI_Id
             WHERE       AWARDEEINSTITUTION_ID = V_AWARDEEINSTITUTIONID
                     AND ORG = 'Not Available'
                     AND SCOPUSINSTITUTIONID IS NULL
                     AND DEPT IS NULL
                     AND STARTDATE IS NULL
                     AND ENDDATE IS NULL
                     AND EMAIL IS NULL
                     AND WEBPAGE IS NULL
                     AND EXTERNALAFFILIATIONIDENTIFIER IS NULL;

                 SELECT   AWARDEE_ID,
                          AWARDEEINSTITUTION_ID,
                          AFFILIATION_ID,
                          SCOPUSINSTITUTIONID,
                          ORG,
                          DEPT,
                          STARTDATE,
                          ENDDATE,
                          EMAIL,
                          WEBPAGE,
                          EXTERNALAFFILIATIONIDENTIFIER,
                          (CASE WHEN FLAGPOHE > 0 THEN 1 ELSE 0 END) FLAGPOHE,
                          (CASE WHEN FLAGFAX > 0 THEN 1 ELSE 0 END) FLAGFAX,
                          (CASE WHEN FLAGADDRESS > 0 THEN 1 ELSE 0 END)
                             FLAGADDRESS
                   FROM   (SELECT   A.AWARDEE_ID,
                                    A.AWARDEEINSTITUTION_ID,
                                    B.AFFILIATION_ID,
                                    B.SCOPUSINSTITUTIONID,
                                    B.ORG,
                                    B.DEPT,
                                    B.STARTDATE,
                                    B.ENDDATE,
                                    B.EMAIL,
                                    B.WEBPAGE,
                                    B.EXTERNALAFFILIATIONIDENTIFIER,
                                    (SELECT   COUNT(*)
                                       FROM   TELEPHONE
                                      WHERE   AFFILIATION_ID = B.AFFILIATION_ID)
                                       FLAGPOHE,
                                    (SELECT   COUNT(*)
                                       FROM   FAX
                                      WHERE   AFFILIATION_ID = B.AFFILIATION_ID)
                                       FLAGFAX,
                                    (SELECT   COUNT(*)
                                       FROM   AWARDADDRESS
                                      WHERE   AFFILIATION_ID = B.AFFILIATION_ID)
                                       FLAGADDRESS
                             FROM   AWARDEEINSTITUTION A, AFFILIATION B
                            WHERE   A.AWARDEEINSTITUTION_ID =
                                       B.AWARDEEINSTITUTION_ID
                                    AND AWARDEE_ID = P_AWARDEEID) T
               ORDER BY   AFFILIATION_ID DESC;
         END IF;
      END IF;

      -- -----------------------------------------------

      IF V_INSCOUNT = 0
      THEN
         SELECT   AWARDEEINSTITUTIONID_SEQ.NEXTVAL
           INTO   V_AWARDEEINSTITUTIONID
           FROM   DUAL;

         INSERT INTO AWARDEEINSTITUTION (AWARDEE_ID, AWARDEEINSTITUTION_ID)
           VALUES   (P_AWARDEEID, V_AWARDEEINSTITUTIONID);
      ELSE
         SELECT   AWARDEEINSTITUTION_ID
           INTO   V_AWARDEEINSTITUTIONID
           FROM   AWARDEEINSTITUTION
          WHERE   AWARDEE_ID = P_AWARDEEID;
      END IF;


      SELECT   COUNT(1)
        INTO   v_cnt
        FROM   AFFILIATION
       WHERE   IFNULL (SCOPUSINSTITUTIONID, 0) = IFNULL (P_SCOPUSINSTITUTIONID, 0)
               AND TRIM(IFNULL (ORG, '0')) = TRIM(IFNULL (p_ORG, '0'))
               AND AWARDEEINSTITUTION_ID = V_AWARDEEINSTITUTIONID;

      IF v_cnt = 0
      THEN
         SELECT   AWARDEEINSTITUTIONID_SEQ.NEXTVAL
           INTO   V_AWARDEEINSTITUTIONID
           FROM   DUAL;

         SELECT   AFFILIATIONID_SEQ.NEXTVAL INTO l_affiliationid_seq FROM DUAL;

         INSERT INTO AFFILIATION (SCOPUSINSTITUTIONID,
                                  ORG,
                                  DEPT,
                                  STARTDATE,
                                  ENDDATE,
                                  EMAIL,
                                  WEBPAGE,
                                 EXTERNALAFFILIATIONIDENTIFIER,
                                  AFFILIATION_ID,
                                  AWARDEEINSTITUTION_ID)
           VALUES   (P_SCOPUSINSTITUTIONID,
                     P_ORG,
                     P_DEPT,
                     P_STARTDATE,
                     P_ENDDATE,
                     P_EMAIL,
                     P_WEBPAGE,
                     P_EXTAFFI_Id,
                     l_affiliationid_seq,
                     V_AWARDEEINSTITUTIONID);
         SELECT   TYPE
           INTO   l_type
           FROM   awardee
          WHERE   awardee_id = P_AWARDEEID;

         INSERT INTO org (RELTYPE,
                          ORG_TEXT,
                          LANG,
                          AFFLIATION_ID)
           VALUES   (l_type,
                     P_ORG,
                     'en',
                     l_affiliationid_seq);
      ELSE
         UPDATE   AFFILIATION
            SET   SCOPUSINSTITUTIONID = P_SCOPUSINSTITUTIONID,
                  ORG = P_ORG,
                  DEPT = P_DEPT,
                  STARTDATE = P_STARTDATE,
                  ENDDATE = P_ENDDATE,
                  EMAIL = P_EMAIL,
                  WEBPAGE = P_WEBPAGE,
                  EXTERNALAFFILIATIONIDENTIFIER=P_EXTAFFI_Id
          WHERE   IFNULL (SCOPUSINSTITUTIONID, 0) =
                     IFNULL (P_SCOPUSINSTITUTIONID, 0)
                  AND TRIM (IFNULL (ORG, '0')) = TRIM (IFNULL (p_ORG, '0'))
                  AND AWARDEEINSTITUTION_ID = V_AWARDEEINSTITUTIONID;
      END IF;
   ELSEIF P_INSDEL = 1
   THEN
      SELECT   AWARDEEINSTITUTION_ID
        INTO   V_AWARDEEINSTITUTIONID
        FROM   AFFILIATION
       WHERE   AFFILIATION_ID = P_AFFILIATION_ID;

      DELETE FROM   TELEPHONE
            WHERE   AFFILIATION_ID = p_AFFILIATION_ID;

      DELETE FROM   FAX
            WHERE   AFFILIATION_ID = p_AFFILIATION_ID;

      DELETE FROM   AWARDADDRESS
            WHERE   AFFILIATION_ID = p_AFFILIATION_ID;

      DELETE FROM   AFFILIATION
            WHERE   AFFILIATION_ID = p_AFFILIATION_ID;


      SELECT   COUNT(*)
        INTO   DELCOUNT
        FROM   AFFILIATION
       WHERE   AWARDEEINSTITUTION_ID = V_AWARDEEINSTITUTIONID;

      IF DELCOUNT = 0
      THEN
         DELETE FROM   AWARDEEINSTITUTION
               WHERE   AWARDEEINSTITUTION_ID = V_AWARDEEINSTITUTIONID;
      END IF;

   ELSEIF P_INSDEL = 2
   THEN
   
    
         SELECT   TYPE
           INTO   l_type
           FROM   awardee
          WHERE   awardee_id = P_AWARDEEID;

      UPDATE   AFFILIATION
         SET   SCOPUSINSTITUTIONID = P_SCOPUSINSTITUTIONID,
               ORG = P_ORG,
               DEPT = P_DEPT,
               STARTDATE = P_STARTDATE,
               ENDDATE = P_ENDDATE,
               EMAIL = P_EMAIL,
               WEBPAGE = P_WEBPAGE,
               EXTERNALAFFILIATIONIDENTIFIER=P_EXTAFFI_Id
       WHERE   AFFILIATION_ID = P_AFFILIATION_ID;
       
       update org set ORG_TEXT=P_ORG, RELTYPE=l_type
       where AFFLIATION_ID= P_AFFILIATION_ID;
   END IF;

    SELECT   AWARDEE_ID,
                 AWARDEEINSTITUTION_ID,
                 AFFILIATION_ID,
                 SCOPUSINSTITUTIONID,
                 ORG,
                 DEPT,
                 STARTDATE,
                 ENDDATE,
                 EMAIL,
                 WEBPAGE,
                 EXTERNALAFFILIATIONIDENTIFIER,
                 (CASE WHEN FLAGPOHE > 0 THEN 1 ELSE 0 END) FLAGPOHE,
                 (CASE WHEN FLAGFAX > 0 THEN 1 ELSE 0 END) FLAGFAX,
                 (CASE WHEN FLAGADDRESS > 0 THEN 1 ELSE 0 END) FLAGADDRESS
          FROM   (SELECT   A.AWARDEE_ID,
                           A.AWARDEEINSTITUTION_ID,
                           B.AFFILIATION_ID,
                           B.SCOPUSINSTITUTIONID,
                           B.ORG,
                           B.DEPT,
                           B.STARTDATE,
                           B.ENDDATE,
                           B.EMAIL,
                           B.WEBPAGE,
                           B.EXTERNALAFFILIATIONIDENTIFIER,
                           (SELECT   COUNT(*)
                              FROM   TELEPHONE
                             WHERE   AFFILIATION_ID = B.AFFILIATION_ID)
                              FLAGPOHE,
                           (SELECT   COUNT(*)
                              FROM   FAX
                             WHERE   AFFILIATION_ID = B.AFFILIATION_ID)
                              FLAGFAX,
                           (SELECT   COUNT(*)
                              FROM   AWARDADDRESS
                             WHERE   AFFILIATION_ID = B.AFFILIATION_ID)
                              FLAGADDRESS
                    FROM   AWARDEEINSTITUTION A, AFFILIATION B
                   WHERE   A.AWARDEEINSTITUTION_ID = B.AWARDEEINSTITUTION_ID
                           AND AWARDEE_ID = P_AWARDEEID) T
      ORDER BY   AFFILIATION_ID DESC;
END