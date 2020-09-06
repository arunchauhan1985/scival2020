CREATE DEFINER=`root`@`localhost` PROCEDURE `SCI_AW_AWARDEEINSDEL_50`(
   P_WORKFLOWID           INTEGER,
   P_INSDEL               INTEGER,
   P_TYPE                 VARCHAR(4000),
   P_SCOPUSAUTHORID       INTEGER,
   P_externalRes_Id       VARCHAR(4000),
   P_ORCID                VARCHAR(4000),
   P_INDEXEDNAME          VARCHAR(4000),
   P_GIVENNAME            VARCHAR(4000),
   P_INITIALS             VARCHAR(4000),
   P_SURNAME              VARCHAR(4000),
   p_AWARDEE_ID           INTEGER,
   p_currency             VARCHAR(4000) ,
   p_amount               INTEGER,
   P_link                 VARCHAR(4000),
   P_DUNS                 VARCHAR(4000),
   P_ROR                 VARCHAR(4000),
   P_WIKIDATA            VARCHAR(4000)
   )
BEGIN
   DECLARE V_ID                     INTEGER;
   DECLARE V_MODULEID               INTEGER;
   DECLARE V_AWARDEESID             INTEGER;
   DECLARE V_AWARDEEID              INTEGER;
   DECLARE V_INSCOUNT               INTEGER;
   DECLARE v_delcount               INTEGER;
   DECLARE V_AWARDEEINSTITUTIONID   INTEGER;
   DECLARE l_awrdcount              INTEGER;
   DECLARE l_awardees_id            INTEGER;
   DECLARE l_awaree_cnt             INTEGER;
   DECLARE V_INDEXCHK               INTEGER;   
   DECLARE l_amt_cnt                integer;
   DECLARE V_AWARDEE_NAME_ID_SEQ    INTEGER;
   
   SELECT   ID, MODULEID
     INTO   V_ID, V_MODULEID
     FROM   SCI_WORKFLOW
    WHERE   WORKFLOWID = P_WORKFLOWID;

 SELECT   COUNT(1)
     INTO   V_INDEXCHK
     FROM   (SELECT   TRIM(LEADING CHAR (9 USING ASCII) FROM TRIM (INDEXEDNAME)) INDEXEDNAME
               FROM   AWARDEENAME
              WHERE   AWARDEE_ID IN
                            (SELECT   AWARDEE_ID
                               FROM   AWARDEENAME
                              WHERE   AWARDEE_ID IN
                                            (SELECT   AWARDEE_ID
                                               FROM   AWARDEE
                                              WHERE   AWARDEES_ID IN
                                                            (SELECT
      AWARDEES_ID
                                                               FROM   AWARDEES
                                                              WHERE   AWARD_ID
      =
                                                                         V_ID)
      ))
                      AND INDEXEDNAME LIKE '%' or CONCAT(IFNULL(P_INDEXEDNAME, '') , '%')) T
    WHERE   INDEXEDNAME = P_INDEXEDNAME;

   IF V_INDEXCHK  <= 0
   THEN
      SELECT   COUNT(*)
        INTO   l_awrdcount
        FROM   AWARDEES
       WHERE   AWARD_ID = V_ID;

      IF l_awrdcount > 0
      THEN
        SELECT   ifnull(AWARDEES_ID,0)
              INTO   l_awardees_id
              FROM   AWARDEES
             WHERE   AWARD_ID = V_ID;
             IF P_TYPE in ( 'PI') 
			THEN
            SELECT   COUNT(*)
              INTO   l_awaree_cnt
              FROM   AWARDEE
             WHERE   AWARDEES_ID = l_awardees_id AND TYPE = P_TYPE;
            
            SELECT   COUNT(*)
              INTO   l_awaree_cnt
              FROM   AWARDEE
             WHERE   AWARDEES_ID = l_awardees_id AND TYPE in ( 'PI', 'institution');

            END IF;
      
      IF P_INSDEL = 0
      THEN
         SELECT   COUNT(*)
           INTO   V_INSCOUNT
           FROM   AWARDEES
          WHERE   AWARD_ID = V_ID;

         IF V_INSCOUNT = 0
         THEN
         SELECT   AWARDEESID_SEQ.NEXTVAL INTO V_AWARDEESID FROM DUAL;

            INSERT INTO AWARDEES (AWARDEES_ID, AWARD_ID)
              VALUES   (V_AWARDEESID, V_ID);
         ELSE
            SELECT   AWARDEES_ID
              INTO   V_AWARDEESID
              FROM   AWARDEES
             WHERE   AWARD_ID = V_ID;
         END IF;



         SELECT   AWARDEEID_SEQ.NEXTVAL INTO V_AWARDEEID FROM DUAL;
         SELECT AWARDEE_NAME_ID_SEQ.NEXTVAL  INTO v_awardee_name_id_seq FROM DUAL;
INSERT
INTO AWARDEE
  (
    TYPE,
    SCOPUSAUTHORID,
    AWARDEE_ID,
    AWARDEES_ID,
    NAME,
    AWARDEEAFFILIATIONID,
    ACTIVITYTYPE,
    DEPARTMENTNAME,
    FBORGANIZATIONID,
    LINK,
    DUNS,
    ROR,
    WIKIDATA,
    VATNUMBER
  )
  VALUES
  (
    P_TYPE,
    P_SCOPUSAUTHORID,
    V_AWARDEEID,
    V_AWARDEESID,
    P_GIVENNAME,
    P_INITIALS,
    P_SURNAME,
    P_INDEXEDNAME,
    P_ORCID,
    P_link,
    P_DUNS,
    P_ROR,
    P_WIKIDATA,
    P_externalRes_Id
  );
  IF p_INDEXEDNAME IS NOT NULL and 1=2
         THEN
            INSERT INTO AWARDEENAME (INDEXEDNAME,
                                     GIVENNAME,
                                     INITIALS,
                                     SURNAME,
                                     AWARDEE_ID,
                                      AWADEENAME_ID,
                                     LANG)
              VALUES   (p_INDEXEDNAME,
                        p_GIVENNAME,
                        p_INITIALS,
                        p_SURNAME,
                        V_AWARDEEID,
                        v_awardee_name_id_seq,
                      'en'
                        );
         END IF;

         select count(*) into l_amt_cnt from AMOUNT where AWARDEE_ID= V_AWARDEEID;

         if l_amt_cnt=0 then
            IF p_amount < 1 OR p_amount is null THEN
               INSERT INTO  AMOUNT(CURRENCY,AMOUNT_TEXT, AWARD_ID,AWARDEE_ID)
                                   VALUES('USD',p_amount, V_ID,V_AWARDEEID);
            ELSE
               INSERT INTO  AMOUNT(CURRENCY,AMOUNT_TEXT, AWARD_ID,AWARDEE_ID)
                                   VALUES(p_currency,p_amount, V_ID,
      V_AWARDEEID);
            END IF;
         end if ;
 SELECT   AWARDEEINSTITUTIONID_SEQ.NEXTVAL
           INTO   V_AWARDEEINSTITUTIONID
           FROM   DUAL;
         INSERT INTO AWARDEEINSTITUTION (AWARDEE_ID, AWARDEEINSTITUTION_ID)
           VALUES   (V_AWARDEEID, V_AWARDEEINSTITUTIONID);
         IF p_type = 'institution'
         THEN
            INSERT INTO AFFILIATION (
                                        ORG,
                                        AFFILIATION_ID,
                                        AWARDEEINSTITUTION_ID,lang
                       )
              VALUES   (
                           P_INDEXEDNAME,
                           AFFILIATIONID_SEQ.NEXTVAL,
                           V_AWARDEEINSTITUTIONID,'en'
                       );
         ELSE
            INSERT INTO AFFILIATION (
                                        ORG,
                                        lang,
                                        AFFILIATION_ID,
                                        AWARDEEINSTITUTION_ID
                       )
              VALUES   (
                           'Not Available',
                           'en',
                           AFFILIATIONID_SEQ.NEXTVAL,
                           V_AWARDEEINSTITUTIONID
                       );

         END IF;
      ELSEIF P_INSDEL = 1
      THEN
         SELECT   AWARDEES_ID
           INTO   v_AWARDEESID
           FROM   awardee
          WHERE   AWARDEE_ID = p_AWARDEE_ID;

         DELETE FROM   AWARDEENAME
               WHERE   AWARDEE_ID = p_AWARDEE_ID;

         DELETE FROM   telephone
               WHERE   AFFILIATION_ID IN
                             (SELECT   AFFILIATION_ID
                                FROM   AFFILIATION
                               WHERE   AWARDEEINSTITUTION_ID IN
                                             (SELECT   AWARDEEINSTITUTION_ID
                                                FROM   AWARDEEINSTITUTION
                                               WHERE   AWARDEE_ID =
                                                          p_AWARDEE_ID));


         DELETE FROM   fax
               WHERE   AFFILIATION_ID IN
                             (SELECT   AFFILIATION_ID
                                FROM   AFFILIATION
                               WHERE   AWARDEEINSTITUTION_ID IN
                                             (SELECT   AWARDEEINSTITUTION_ID
                                                FROM   AWARDEEINSTITUTION
                                               WHERE   AWARDEE_ID =
                                                          p_AWARDEE_ID));
         DELETE FROM   address
               WHERE   AFFILIATION_ID IN
                             (SELECT   AFFILIATION_ID
                                FROM   AFFILIATION
                               WHERE   AWARDEEINSTITUTION_ID IN
                                             (SELECT   AWARDEEINSTITUTION_ID
                                                FROM   AWARDEEINSTITUTION
                                               WHERE   AWARDEE_ID =
                                                          p_AWARDEE_ID));


         DELETE FROM   AWARDADDRESS
               WHERE   AFFILIATION_ID IN
                             (SELECT   AFFILIATION_ID
                                FROM   AFFILIATION
                               WHERE   AWARDEEINSTITUTION_ID IN
                                             (SELECT   AWARDEEINSTITUTION_ID
                                                FROM   AWARDEEINSTITUTION
                                               WHERE   AWARDEE_ID =
                                                          p_AWARDEE_ID));

         DELETE FROM   AFFILIATION
               WHERE   AWARDEEINSTITUTION_ID IN
                             (SELECT   AWARDEEINSTITUTION_ID
                                FROM   AWARDEEINSTITUTION
                               WHERE   AWARDEE_ID = p_AWARDEE_ID);


         DELETE FROM   AWARDEEINSTITUTION
               WHERE   AWARDEE_ID = p_AWARDEE_ID;
                DELETE FROM   amount
               WHERE   AWARDEE_ID = p_AWARDEE_Id;
            delete from   AWARDEENAME_AFTER_UPDATE
              WHERE   AWARDEE_ID = p_AWARDEE_ID;
        
         DELETE FROM   awardee
               WHERE   AWARDEE_ID = p_AWARDEE_ID;



         SELECT   COUNT(*)
           INTO   v_delcount
           FROM   awardee
          WHERE   AWARDEEs_ID = v_AWARDEESID;

         IF v_delcount = 0
         THEN
            DELETE FROM   awardees
                  WHERE   AWARDEEs_ID = v_AWARDEESID;
         END IF;
      ELSEIF P_INSDEL = 2
      THEN
         UPDATE   awardee
            SET   TYPE = p_TYPE, SCOPUSAUTHORID = p_SCOPUSAUTHORID,externalresearcheridentifier=p_externalres_id,orcid=p_orcid
          WHERE   AWARDEE_ID = p_AWARDEE_ID;


         UPDATE   AWARDEENAME
            SET   INDEXEDNAME = p_INDEXEDNAME,
                  GIVENNAME = p_GIVENNAME,
                  INITIALS = p_INITIALS,
                  SURNAME = p_SURNAME
          WHERE   AWARDEE_ID = p_AWARDEE_ID;
            UPDATE   amount
            SET   CURRENCY = p_currency, AMOUNT_TEXT = p_amount
          WHERE   AWARDEE_ID = p_AWARDEE_ID;
           

         IF p_type = 'institution'
         THEN
            SELECT   COUNT(*)
              INTO   V_INSCOUNT
              FROM   awardeeinstitution
             WHERE   awardee_id = p_AWARDEE_ID;

            IF V_INSCOUNT > 0
            THEN
               SELECT   AWARDEEINSTITUTION_ID
                 INTO   V_AWARDEEINSTITUTIONID
                 FROM   awardeeinstitution
                WHERE   awardee_id = p_AWARDEE_ID;

               UPDATE   AFFILIATION
                  SET   org = P_INDEXEDNAME
                WHERE   AWARDEEINSTITUTION_ID = V_AWARDEEINSTITUTIONID;
            ELSE
               SELECT   AWARDEEINSTITUTIONID_SEQ.NEXTVAL
                 INTO   V_AWARDEEINSTITUTIONID
                 FROM   DUAL;

               INSERT INTO AWARDEEINSTITUTION (
                                                  AWARDEE_ID,
                                                  AWARDEEINSTITUTION_ID
                          )
                 VALUES   (V_AWARDEEID, V_AWARDEEINSTITUTIONID);


               INSERT INTO AFFILIATION (
                                           ORG,
                                           lang,
                                           AFFILIATION_ID,
                                           AWARDEEINSTITUTION_ID
                          )
                 VALUES   (
                              P_INDEXEDNAME,
                              'en',
                              AFFILIATIONID_SEQ.NEXTVAL,
                              V_AWARDEEINSTITUTIONID
                          );
            END IF;

         END IF;
      END IF;

       SELECT  AWARD_ID,
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
                    DUNS,
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
                              B.DUNS,
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
   END IF;
end IF;


END