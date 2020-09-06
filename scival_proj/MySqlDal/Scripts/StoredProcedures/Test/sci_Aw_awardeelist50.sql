CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_Aw_awardeelist50`(
   P_WORKFLOWID       INTEGER
)
BEGIN
   DECLARE V_ID         INTEGER;
   DECLARE V_MODULEID   INTEGER;
 
  SELECT   ID, MODULEID
     INTO   V_ID, V_MODULEID
     FROM   SCI_WORKFLOW
    WHERE   WORKFLOWID = P_WORKFLOWID;



  
        SELECT   AWARD_ID,
                    AWARDEES_ID,
                    AWARDEE_ID,
                    TYPE,
                    VALUE,
                    SCOPUSAUTHORID,
                    externalResearcherIdentifier,
                    ORCID,
                     activityType,
                     awardeeAffiliationId,
                    departmentName,
                    FBORGANIZATIONID,
                    link,
                    name,
                    ROR,
                    VATNUMBER,
                    WIKIDATA,
                    INDEXEDNAME,
                    GIVENNAME,
                    INITIALS,
                    SURNAME,
                    CURRENCY,
                    AMOUNT_TEXT,
                    (CASE WHEN FLAG > 0 THEN 1 ELSE 0 END) FLAG
             FROM   (SELECT   A.AWARD_ID,
                              A.AWARDEES_ID,
                              B.AWARDEE_ID,
                              B.TYPE,
                              d.VALUE,
                              B.SCOPUSAUTHORID,
                              B.externalResearcherIdentifier,
                              B.ORCID,
                              activityType,
                              awardeeAffiliationId,
                              departmentName,
                              B.FBORGANIZATIONID,
                              B.link,
                              B.name,
                              B.ROR,
                              B.VATNUMBER,
                              B.WIKIDATA,
                              INDEXEDNAME,
                              GIVENNAME,
                              INITIALS,
                              SURNAME,
                              CURRENCY,
                              AMOUNT_TEXT,
                              (SELECT   COUNT (1)
                                 FROM   AWARDEEINSTITUTION
                                WHERE   AWARDEE_ID = B.AWARDEE_ID)
                                 FLAG
                       FROM   AWARDEES A,
                              AWARDEE B,
                              AWARDEENAME C,
                              SCI_AWARDEETYPETYPE d,  amount am
                      WHERE       A.AWARDEES_ID = B.AWARDEES_ID
                              AND d.CODE = B.TYPE
                              and AM.AWARDEE_ID= B.AWARDEE_ID
                              AND B.AWARDEE_ID = C.AWARDEE_ID
                              AND a.AWARD_ID = V_ID) T
         ORDER BY   AWARDEE_ID DESC;

   SELECT   * FROM SCI_AWARDEETYPETYPE where code not in ('PI', 'coPI', 'institution');
   
           SELECT code, CONCAT(IFNULL(VALUE, '') , ' (' , ifnull(code, '') , ')') AS VALUE
           FROM sci_currencytype;
  END