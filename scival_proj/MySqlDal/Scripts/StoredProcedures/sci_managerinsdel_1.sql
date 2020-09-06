CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_managerinsdel_1`(
   p_workflowid         INTEGER,
   p_insdel             INTEGER,
   p_contactid          INTEGER,
   p_type               VARCHAR(4000),
   p_title              VARCHAR(4000),
   p_telephone          VARCHAR(4000),
   p_fax                VARCHAR(4000),
   p_email              VARCHAR(4000),
   p_url                VARCHAR(4000),
   p_website_text       VARCHAR(4000),
   p_MANAGERID          integer,
   p_country            VARCHAR(4000),
   p_room               VARCHAR(4000),
   p_street             VARCHAR(4000),
   p_state              VARCHAR(4000),
   p_city               VARCHAR(4000),
   p_postalcode         VARCHAR(4000),
   p_prefix             VARCHAR(4000),
   p_givenname          VARCHAR(4000),
   p_middlename         VARCHAR(4000),
   p_surname            VARCHAR(4000),
   p_suffix             VARCHAR(4000)
  
)
BEGIN
   DECLARE v_id             INTEGER;
   DECLARE v_value          INTEGER;
   DECLARE v_MANAGERSID     integer;
   DECLARE v_MANAGERID      integer;
   DECLARE v_contact        INTEGER;
   DECLARE v_count          INTEGER;
   DECLARE v_contactcount   INTEGER;
   DECLARE v_count1         INTEGER;
   DECLARE v_moduleid       integer;
 
   SELECT   ID, moduleid
     INTO   v_id, v_moduleid
     FROM   sci_workflow
    WHERE   workflowid = p_workflowid;


   IF p_insdel = 0
   THEN
      SELECT   COUNT(*)
        INTO   v_count
        FROM   Awardmanagers
       WHERE   AWARD_ID = v_id;

      IF v_count = 0
      THEN
         SELECT   AWARDMANAGERSID_SEQ.NEXTVAL INTO v_MANAGERSID FROM DUAL;

         INSERT INTO Awardmanagers (AWARDMANAGERS_ID, AWARD_ID)
           VALUES   (v_MANAGERSID, v_id);
      ELSE
         SELECT   AWARDMANAGERS_ID
           INTO   v_MANAGERSID
           FROM   Awardmanagers
          WHERE   AWARD_ID = v_id;
      END IF;

      SELECT   AWARDMANAGERID_SEQ.NEXTVAL INTO v_MANAGERID FROM DUAL;

      INSERT INTO Awardmanager (TYPE,
                                TITLE,
                                TELEPHONE,
                                FAX,
                                EMAIL,
                                AWARDMANAGER_ID,
                                AWARDMANAGERS_ID)
        VALUES   (p_TYPE,
                  p_TITLE,
                  p_TELEPHONE,
                  p_FAX,
                  p_EMAIL,
                  v_MANAGERID,
                  v_MANAGERSID);

      IF p_url IS NOT NULL
      THEN
         INSERT INTO awardwebsite (url, website_text, AWARDMANAGER_ID)
           VALUES   (p_url, p_website_text, v_contact);
      END IF;

      IF p_COUNTRY IS NOT NULL
      THEN
         INSERT INTO address (COUNTRY,
                              ROOM,
                              STREET,
                              CITY,
                              STATE,
                              POSTALCODE,
                              AWARDMANAGER_ID)
           VALUES   (p_COUNTRY,
                     p_ROOM,
                     p_STREET,
                     p_CITY,
                     p_STATE,
                     p_POSTALCODE,
                     v_MANAGERID);
      END IF;

      IF p_givenname IS NOT NULL AND p_surname IS NOT NULL
      THEN
         INSERT INTO AWARDCONTACTNAME (PREFIX,
                                       GIVENNAME,
                                       MIDDLENAME,
                                       SURNAME,
                                       SUFFIX,
                                       AWARDMANAGER_ID)
           VALUES   (p_PREFIX,
                     p_GIVENNAME,
                     p_MIDDLENAME,
                     p_SURNAME,
                     p_SUFFIX,
                     v_MANAGERID);
      END IF;

      IF p_url IS NOT NULL
      THEN
         INSERT INTO AWARDWEBSITE (URL, WEBSITE_TEXT, AWARDMANAGER_ID)
           VALUES   (p_url, p_WEBSITE_TEXT, v_MANAGERID);
      END IF;
   -- ***********************************************************************

   ELSEIF p_insdel = 1
   THEN
      select AWARDMANAGERS_ID into v_MANAGERSID from Awardmanager
            WHERE   AWARDMANAGER_ID = p_MANAGERID;
            
      DELETE FROM   AWARDWEBSITE
            WHERE   AWARDMANAGER_ID = p_MANAGERID;

      DELETE FROM   AWARDCONTACTNAME
            WHERE   AWARDMANAGER_ID = p_MANAGERID;

      DELETE FROM   address
            WHERE   AWARDMANAGER_ID = p_MANAGERID;

      DELETE FROM   Awardmanager
            WHERE   AWARDMANAGER_ID = p_MANAGERID;

      SELECT   COUNT(*)
        INTO   v_contactcount
        FROM   Awardmanager
       WHERE   AWARDMANAGERS_ID = v_MANAGERSID;

      IF v_contactcount = 0
      THEN
         DELETE FROM   Awardmanagers
               WHERE   AWARDMANAGERS_ID = v_MANAGERSID;
      END IF;

   ELSEIF p_insdel = 2
   THEN
      SELECT   COUNT(*)
        INTO   v_count1
        FROM   awardwebsite
       WHERE   AWARDMANAGER_ID = p_contactid;

      IF v_count1 = 0 AND p_url IS NOT NULL
      THEN
         INSERT INTO awardwebsite (url, website_text, AWARDMANAGER_ID)
           VALUES   (p_url, p_website_text, p_contactid);
      END IF;


      UPDATE   awardwebsite
         SET   url = p_url, website_text = p_website_text
       WHERE   AWARDMANAGER_ID = p_contactid;


      SELECT   COUNT(*)
        INTO   v_count1
        FROM   address
       WHERE   AWARDMANAGER_ID = p_contactid;

      IF v_count1 = 0 AND p_country IS NOT NULL
      THEN
         INSERT INTO address (country,
                              room,
                              street,
                              city,
                              state,
                              postalcode,
                              AWARDMANAGER_ID)
           VALUES   (p_country,
                     p_room,
                     p_street,
                     p_city,
                     p_state,
                     p_postalcode,
                     p_contactid);
      END IF;

      IF p_country IS NOT NULL
      THEN
         UPDATE   address
            SET   country = p_country,
                  room = p_room,
                  street = p_street,
                  city = p_city,
                  state = p_state,
                  postalcode = p_postalcode
          WHERE   AWARDMANAGER_ID = p_contactid;
      END IF;

      SELECT   COUNT(*)
        INTO   v_count1
        FROM   awardcontactname
       WHERE   AWARDMANAGER_ID = p_contactid;

      IF v_count1 = 0
         AND (   p_prefix IS NOT NULL
              OR p_givenname IS NOT NULL
              OR p_middlename IS NOT NULL
              OR p_surname IS NOT NULL
              OR p_suffix IS NOT NULL)
      THEN
         INSERT INTO awardcontactname (prefix,
                                       givenname,
                                       middlename,
                                       surname,
                                       suffix,
                                       AWARDMANAGER_ID)
           VALUES   (p_prefix,
                     p_givenname,
                     p_middlename,
                     p_surname,
                     p_suffix,
                     p_contactid);
      END IF;

      UPDATE   awardcontactname
         SET   prefix = p_prefix,
               givenname = p_givenname,
               middlename = p_middlename,
               surname = p_surname,
               suffix = p_suffix
       WHERE   AWARDMANAGER_ID = p_contactid;

      UPDATE   awardmanager
         SET   TYPE = p_type,
               title = p_title,
               telephone = p_telephone,
               fax = p_fax,
               email = p_email
       WHERE   AWARDMANAGER_ID = p_contactid;
   END IF;

   -- ********************************************************
   DELETE FROM   awardcontactname
         WHERE       prefix IS NULL
                 AND givenname IS NULL
                 AND middlename IS NULL
                 AND surname IS NULL
                 AND suffix IS NULL;

   -- ----******************************************************************

   -- OPEN p_awmanager FOR
      SELECT   b.TYPE,
               b.TITLE,
               b.TELEPHONE,
               b.FAX,
               b.EMAIL,
               b.AWARDMANAGER_ID contact_id,
               c.country countrycode,
               cc.name countryname,
               c.ROOM,
               c.STREET,
               c.CITY,
               IFNULL (c.state, c.state) statecode,
               IFNULL (sc.name, c.state) statename,
               c.POSTALCODE,
               d.PREFIX,
               d.GIVENNAME,
               d.MIDDLENAME,
               d.SURNAME,
               d.SUFFIX,
               e.URL,
               e.WEBSITE_TEXT
        FROM   Awardmanagers a,
               Awardmanager b,
               address c,
               AWARDCONTACTNAME d,
               AWARDWEBSITE e,
               SCI_COUNTRYCODES cc,
               SCI_STATECODES sc
       WHERE       a.AWARDMANAGERS_ID = b.AWARDMANAGERS_ID
               AND sc.code = c.STATE
               AND cc.LCODE = c.COUNTRY
               AND b.AWARDMANAGER_ID = c.AWARDMANAGER_ID
               AND b.AWARDMANAGER_ID = d.AWARDMANAGER_ID
               AND b.AWARDMANAGER_ID = e.AWARDMANAGER_ID
               AND AWARD_ID = v_id;   
END