CREATE DEFINER=`root`@`localhost` PROCEDURE `SCI_AW_INSTITUTIONLIST`(
   P_AWARDEEID       INTEGER
)
BEGIN
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
                         AND AWARDEE_ID = P_AWARDEEID) T;

END