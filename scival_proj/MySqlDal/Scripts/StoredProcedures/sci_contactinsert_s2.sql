CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_contactinsert_s2`(
   p_workflowid         INTEGER,
   p_mode               INTEGER,
   p_insdel             INTEGER,
   p_contactid          INTEGER,
   p_type               VARCHAR(4000),
   p_title              VARCHAR(4000),
   p_telephone          VARCHAR(4000),
   p_fax                VARCHAR(4000),
   p_email              VARCHAR(4000),
   p_url                VARCHAR(4000),
   p_website_text       VARCHAR(4000),
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
   p_suffix             VARCHAR(4000),
   p_LANG               VARCHAR(4000)
)
sp_lbl:

BEGIN
	DECLARE v_fundingbodyid   INTEGER;
	DECLARE v_value           INTEGER;
	DECLARE v_contact         INTEGER;
	DECLARE v_count           INTEGER;
	DECLARE v_contactcount    INTEGER;
	DECLARE v_count1          INTEGER;
	DECLARE v_moduleid        integer;
   
	SELECT   ID, moduleid INTO   v_fundingbodyid, v_moduleid FROM   sci_workflow WHERE   workflowid = p_workflowid;

	IF v_moduleid = 2
	THEN
		IF p_mode = 1 AND p_insdel = 0
		THEN
			SELECT   COUNT(*) INTO   v_count FROM   contacts WHERE   fundingbody_id = v_fundingbodyid;

			IF v_count = 0
			THEN
				SELECT   contacts_seq.NEXTVAL INTO v_value FROM DUAL;

				INSERT INTO contacts VALUES   (v_value, v_fundingbodyid);
			ELSE
				SELECT   contacts_id INTO   v_value FROM   contacts
				WHERE   fundingbody_id = v_fundingbodyid;
			END IF;

			SELECT   contact_seq.NEXTVAL INTO v_contact FROM DUAL;

			INSERT INTO contact (TYPE,
                              title,
                              telephone,
                              fax,
                              email,
                              contact_id,
                              contacts_id)
			VALUES   (p_type,
                     p_title,
                     p_telephone,
                     p_fax,
                     p_email,
                     v_contact,
                     v_value);

			IF p_url IS NOT NULL
			THEN
				INSERT INTO website (url, website_text, contact_id,Lang)
				VALUES   (p_url, p_website_text, v_contact,p_LANG);
			END IF;

			IF p_country IS NOT NULL
			THEN
				INSERT INTO address (countryTEST,
                                 room,
                                 street,
                                 city,
                                 state,
                                 postalcode,
                                 contact_id)
				VALUES   (p_country,
                        p_room,
                        p_street,
                        p_city,
                        p_state,
                        p_postalcode,
                        v_contact);
			END IF;

			IF p_givenname IS NOT NULL AND p_surname IS NOT NULL
			THEN
				INSERT INTO contactname (prefix,
                                     givenname,
                                     middlename,
                                     surname,
                                     suffix,
                                     contact_id)
				VALUES   (p_prefix,
                        p_givenname,
                        p_middlename,
                        p_surname,
                        p_suffix,
                        v_contact);
			END IF;
      -- ***********************************************************************
		ELSEIF p_mode = 2 AND p_insdel = 0
		THEN
			SELECT   COUNT(*) INTO   v_count FROM   officers WHERE   fundingbody_id = v_fundingbodyid;

			IF v_count = 0
			THEN
				SELECT   officers_seq.NEXTVAL INTO v_value FROM DUAL;

				INSERT INTO officers VALUES   (v_value, v_fundingbodyid);
			ELSE
				SELECT   officers_id INTO   v_value FROM   officers WHERE   fundingbody_id = v_fundingbodyid;
			END IF;

			SELECT   contact_seq.NEXTVAL INTO v_contact FROM DUAL;

			INSERT INTO contact (TYPE,
                              title,
                              telephone,
                              fax,
                              email,
                              contact_id,
                              officers_id)
			VALUES   (p_type,
                     p_title,
                     p_telephone,
                     p_fax,
                     p_email,
                     v_contact,
                     v_value);

			IF p_url IS NOT NULL
			THEN
				INSERT INTO website (url, website_text, contact_id) VALUES   (p_url, p_website_text, v_contact);
			END IF;

			IF p_country IS NOT NULL
			THEN
				INSERT INTO address (countryTEST,
                                 room,
                                 street,
                                 city,
                                 state,
                                 postalcode,
                                 contact_id)
				VALUES   (p_country,
                        p_room,
                        p_street,
                        p_city,
                        p_state,
                        p_postalcode,
                        v_contact);
			END IF;

			IF p_givenname IS NOT NULL AND p_surname IS NOT NULL
			THEN
				INSERT INTO contactname (prefix,
                                     givenname,
                                     middlename,
                                     surname,
                                     suffix,
                                     contact_id)
				VALUES   (p_prefix,
                        p_givenname,
                        p_middlename,
                        p_surname,
                        p_suffix,
                        v_contact);
			END IF;
      -- **************************************************************************
		ELSEIF p_mode = 3 AND p_insdel = 0
		THEN
			SELECT   COUNT(*) INTO   v_count FROM   contactinfo WHERE   fundingbody_id = v_fundingbodyid;

			IF v_count = 0
			THEN
				SELECT   contactinfo_seq.NEXTVAL INTO v_value FROM DUAL;

				INSERT INTO contactinfo (contactinfo_id, fundingbody_id) VALUES   (v_value, v_fundingbodyid);
			ELSE
				SELECT   contactinfo_id INTO   v_value FROM   contactinfo WHERE   fundingbody_id = v_fundingbodyid;
			END IF;

			SELECT   contact_seq.NEXTVAL INTO v_contact FROM DUAL;

			INSERT INTO contact (TYPE,
                              title,
                              telephone,
                              fax,
                              email,
                              contact_id,
                              contactinfo_id)
			VALUES   (p_type,
                     p_title,
                     p_telephone,
                     p_fax,
                     p_email,
                     v_contact,
                     v_value);

			IF p_url IS NOT NULL
			THEN
				INSERT INTO website (url, website_text, contact_id,LANG) VALUES   (p_url, p_website_text, v_contact,p_LANG);
			END IF;

			IF p_country IS NOT NULL
			THEN
				INSERT INTO address (countryTEST,
                                 room,
                                 street,
                                 city,
                                 state,
                                 postalcode,
                                 contact_id)
				VALUES   (p_country,
                        p_room,
                        p_street,
                        p_city,
                        p_state,
                        p_postalcode,
                        v_contact);
			END IF;

			IF p_givenname IS NOT NULL AND p_surname IS NOT NULL
			THEN
				INSERT INTO contactname (prefix,
                                     givenname,
                                     middlename,
                                     surname,
                                     suffix,
                                     contact_id)
				VALUES   (p_prefix,
                        p_givenname,
                        p_middlename,
                        p_surname,
                        p_suffix,
                        v_contact);
			END IF;
      -- --**********************************************************************
		ELSEIF p_mode = 1 AND p_insdel = 1
		THEN
			DELETE FROM   website WHERE   contact_id = p_contactid;

			DELETE FROM   address WHERE   contact_id = p_contactid;

			DELETE FROM   contactname WHERE   contact_id = p_contactid;

			DELETE FROM   contact WHERE   contact_id = p_contactid;

			SELECT   COUNT(*)
			INTO   v_contactcount
			FROM   contact
			WHERE   contacts_id = (SELECT   contacts_id
                                   FROM   contacts
                                  WHERE   fundingbody_id = v_fundingbodyid);

			IF v_contactcount = 0
			THEN
				DELETE FROM   contacts WHERE   fundingbody_id = v_fundingbodyid;
			END IF;
      -- *******************************************************************
		ELSEIF p_mode = 2 AND p_insdel = 1
		THEN
			DELETE FROM   website WHERE   contact_id = p_contactid;

			DELETE FROM   address WHERE   contact_id = p_contactid;

			DELETE FROM   contactname WHERE   contact_id = p_contactid;

			DELETE FROM   contact WHERE   contact_id = p_contactid;

			SELECT   COUNT(*)
			INTO   v_contactcount
			FROM   contact
			WHERE   officers_id = (SELECT   officers_id
                                   FROM   officers
                                  WHERE   fundingbody_id = v_fundingbodyid);

			IF v_contactcount = 0
			THEN
				DELETE FROM   officers WHERE   fundingbody_id = v_fundingbodyid;
			END IF;
      -- **************************************************************************
		ELSEIF p_mode = 3 AND p_insdel = 1
		THEN
			DELETE FROM   website WHERE   contact_id = p_contactid;

			DELETE FROM   address WHERE   contact_id = p_contactid;

			DELETE FROM   contactname WHERE   contact_id = p_contactid;

			DELETE FROM   contact WHERE   contact_id = p_contactid;

			SELECT   COUNT(*)
			INTO   v_contactcount
			FROM   contact
			WHERE   contactinfo_id =
                     (SELECT   contactinfo_id
                        FROM   contactinfo
                       WHERE   fundingbody_id = v_fundingbodyid);

			IF v_contactcount = 0
			THEN
				DELETE FROM   contactinfo WHERE   fundingbody_id = v_fundingbodyid;
			END IF;
      -- *************************************************************************
		ELSEIF p_insdel = 2
		THEN
			SELECT   COUNT(*)
			INTO   v_count1
			FROM   website
			WHERE   contact_id = p_contactid;

			IF v_count1 = 0 AND p_url IS NOT NULL
			THEN
				INSERT INTO website (url, website_text, contact_id)
				VALUES   (p_url, p_website_text, p_contactid);
			END IF;
         
            UPDATE   website
               SET   url = p_url, website_text = p_website_text,lang=p_LANG
             WHERE   contact_id = p_contactid;
        
			SELECT   COUNT(*)
			INTO   v_count1
			FROM   address
			WHERE   contact_id = p_contactid;

			IF v_count1 = 0 AND p_country IS NOT NULL
			THEN
				INSERT INTO address (countryTEST,
                                 room,
                                 street,
                                 city,
                                 state,
                                 postalcode,
                                 contact_id)
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
				SET   countryTEST = p_country,
                     room = p_room,
                     street = p_street,
                     city = p_city,
                     state = p_state,
                     postalcode = p_postalcode
				WHERE   contact_id = p_contactid;
			END IF;

			SELECT   COUNT(*)
			INTO   v_count1
			FROM   contactname
			WHERE   contact_id = p_contactid;

			IF v_count1 = 0 AND p_surname IS NOT NULL                  
			THEN
				INSERT INTO contactname (prefix,
                                     givenname,
                                     middlename,
                                     surname,
                                     suffix,
                                     contact_id)
				VALUES   (p_prefix,
                        p_givenname,
                        p_middlename,
                        p_surname,
                        p_suffix,
                        p_contactid);					
			ELSE
            IF p_givenname IS NOT NULL AND p_surname IS NOT NULL 
			THEN
				UPDATE  contactname
                  SET    prefix = p_prefix,
                      givenname = p_givenname,
                     middlename = p_middlename,
                        surname = p_surname,
                         suffix = p_suffix
				WHERE contact_id = p_contactid;               
            ELSE
				DELETE FROM contactname WHERE contact_id = p_contactid;
                              
        END IF;
    END IF;

		UPDATE   contact SET   TYPE = p_type,
					  title = p_title,
					  telephone = p_telephone,
					  fax = p_fax,
					  email = p_email
		WHERE   contact_id = p_contactid;
	 
	END IF;

      -- ********************************************************
      DELETE FROM   contactname
            WHERE       prefix IS NULL
                    AND givenname IS NULL
                    AND middlename IS NULL
                    AND surname IS NULL
                    AND suffix IS NULL;

      -- ----******************************************************************
    IF p_mode = 1
    THEN         
        SELECT   c.TYPE,
                     c.title,
                     c.telephone,
                     c.email,
                     c.fax,
                     c.contact_id,
                     w.url,
                     w.website_text,
                    Case  when w.LANG is null then 'en' 
                     else w.Lang End LANG,
                     a.countryTEST countrycode,
                     cc.name  countryname,
                     a.room,
                     a.street,
                     a.city,
                     ifnull(a.state,a.state) statecode,
                      ifnull(sc.name,a.state) statename,
                     a.postalcode,
                     cn.prefix,
                     cn.givenname,
                     cn.middlename,
                     cn.surname,
                     cn.suffix
        FROM   contact c
        LEFT JOIN website w
        ON c.contact_id = w.contact_id
        LEFT JOIN address a
        ON c.contact_id = a.contact_id
        LEFT JOIN contactname cn
        ON c.contact_id = cn.contact_id
        RIGHT JOIN contacts cs
        ON sc.code=a.STATE
        RIGHT JOIN SCI_COUNTRYCODES cc
        ON cc.LCODE=a.COUNTRYTEST,        
		SCI_STATECODES sc
        WHERE c.contacts_id = cs.contacts_id
		AND cs.fundingbody_id = v_fundingbodyid;
        
    ELSEIF p_mode = 2
    THEN         
            SELECT   c.TYPE,
                     c.title,
                     c.telephone,
                     c.email,
                     c.fax,
                     c.contact_id,
                     w.url,
                     w.website_text,
                     a.countryTEST countrycode,
                     cc.name  countryname,
                     a.room,
                     a.street,
                     a.city,
                    ifnull(a.state,a.state) statecode,
                      ifnull(sc.name,a.state) statename,
                     a.postalcode,
                     cn.prefix,
                     cn.givenname,
                     cn.middlename,
                     cn.surname,
                     cn.suffix
            FROM   contact c
            LEFT JOIN website w
            ON c.contact_id = w.contact_id
            LEFT JOIN address a
            ON c.contact_id = a.contact_id
            LEFT JOIN contactname cn
            ON c.contact_id = cn.contact_id
            RIGHT JOIN SCI_STATECODES sc
            ON sc.code=a.STATE
            RIGHT JOIN SCI_COUNTRYCODES cc
            ON cc.LCODE=a.COUNTRYTEST,
                     officers cs
			WHERE c.officers_id = cs.officers_id
			AND cs.fundingbody_id = v_fundingbodyid;
            
    ELSEIF p_mode = 3
    THEN
         
            SELECT   c.TYPE,
                     c.title,
                     c.telephone,
                     c.email,
                     c.fax,
                     c.contact_id,
                     w.url,
                     w.website_text,
                     a.countryTEST countrycode,
                     cc.name  countryname,
                     a.room,
                     a.street,
                     a.city,
                   ifnull(a.state,a.state) statecode,
                      ifnull(sc.name,a.state) statename,
                     a.postalcode,
                     cn.prefix,
                     cn.givenname,
                     cn.middlename,
                     cn.surname,
                     cn.suffix
              FROM   contact c
              LEFT JOIN website w
              ON C.CONTACT_ID = w.contact_id
              LEFT JOIN address a
              ON c.contact_id = a.contact_id
              LEFT JOIN contactname cn
              ON c.contact_id = cn.contact_id
              RIGHT JOIN SCI_STATECODES sc
              ON sc.code=a.STATE
              RIGHT JOIN SCI_COUNTRYCODES cc
              ON cc.LCODE=a.COUNTRYTEST,
                     contactinfo cs                     
             WHERE c.contactinfo_id = cs.contactinfo_id
			 AND cs.FUNDINGBODY_ID = v_fundingbodyid;
    END IF;
      
    ELSEIF v_moduleid = 3
    THEN
		IF p_mode = 3 AND p_insdel = 0
		THEN
			SELECT   COUNT(*)
			INTO   v_count
			FROM   contactinfo
			WHERE   OPPORTUNITY_ID = v_fundingbodyid;

			IF v_count = 0
			THEN
				SELECT   contactinfo_seq.NEXTVAL INTO v_value FROM DUAL;

				INSERT INTO contactinfo (contactinfo_id, OPPORTUNITY_ID)
				VALUES   (v_value, v_fundingbodyid);
			ELSE
				SELECT   contactinfo_id
				INTO   v_value
				FROM   contactinfo
				WHERE   OPPORTUNITY_ID = v_fundingbodyid;
			END IF;

			SELECT   contact_seq.NEXTVAL INTO v_contact FROM DUAL;

			INSERT INTO contact (TYPE,
                              title,
                              telephone,
                              fax,
                              email,
                              contact_id,
                              contactinfo_id)
			VALUES   (p_type,
                     p_title,
                     p_telephone,
                     p_fax,
                     p_email,
                     v_contact,
                     v_value);

			IF p_url IS NOT NULL
			THEN
				INSERT INTO website (url, website_text, contact_id,LANG)
				VALUES   (p_url, p_website_text, v_contact,p_LANG);
			END IF;

			IF p_country IS NOT NULL
			THEN
				INSERT INTO address (countryTEST,
                                 room,
                                 street,
                                 city,
                                 state,
                                 postalcode,
                                 contact_id)
				VALUES   (p_country,
                        p_room,
                        p_street,
                        p_city,
                        p_state,
                        p_postalcode,
                        v_contact);
			END IF;

			IF p_givenname IS NOT NULL AND p_surname IS NOT NULL
			THEN
				INSERT INTO contactname (prefix,
                                     givenname,
                                     middlename,
                                     surname,
                                     suffix,
                                     contact_id)
				VALUES   (p_prefix,
                        p_givenname,
                        p_middlename,
                        p_surname,
                        p_suffix,
                        v_contact);
			END IF;
      -- **************************************************************************
		ELSEIF p_mode = 3 AND p_insdel = 1
		THEN
			DELETE FROM   website
               WHERE   contact_id = p_contactid;

			DELETE FROM   address
               WHERE   contact_id = p_contactid;

			DELETE FROM   contactname
               WHERE   contact_id = p_contactid;

			DELETE FROM   contact
               WHERE   contact_id = p_contactid;

			SELECT   COUNT(*)
			INTO   v_contactcount
			FROM   contact
			WHERE   contactinfo_id =
                     (SELECT   contactinfo_id
                        FROM   contactinfo
                       WHERE   OPPORTUNITY_ID = v_fundingbodyid);

			IF v_contactcount = 0
			THEN
				DELETE FROM   contactinfo
                  WHERE   OPPORTUNITY_ID = v_fundingbodyid;
			END IF;
      -- *************************************************************************     
		ELSEIF p_insdel = 2
		THEN
			SELECT   COUNT(*)
			INTO   v_count1
			FROM   website
			WHERE   contact_id = p_contactid;

			IF v_count1 = 0 AND p_url IS NOT NULL
			THEN
				INSERT INTO website (url, website_text, contact_id,lang)
				VALUES   (p_url, p_website_text, p_contactid,p_LANG);
			END IF;
         
            UPDATE   website
               SET   url = p_url, website_text = p_website_text,Lang=p_LANG
            WHERE   contact_id = p_contactid;         

			SELECT   COUNT(*)
			INTO   v_count1
			FROM   address
			WHERE   contact_id = p_contactid;

			IF v_count1 = 0 AND p_country IS NOT NULL
			THEN
				INSERT INTO address (countryTEST,
                                 room,
                                 street,
                                 city,
                                 state,
                                 postalcode,
                                 contact_id)
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
				SET   countryTEST = p_country,
                     room = p_room,
                     street = p_street,
                     city = p_city,
                     state = p_state,
                     postalcode = p_postalcode
				WHERE   contact_id = p_contactid;
			END IF;

			SELECT   COUNT(*)
			INTO   v_count1
			FROM   contactname
			WHERE   contact_id = p_contactid;
         -- -----------------------------------------------------------------------
         
			IF v_count1 = 0 
			THEN
         
				IF  p_givenname IS NOT NULL AND p_surname IS NOT NULL 
				THEN
					INSERT INTO contactname (prefix,
                                     givenname,
                                     middlename,
                                     surname,
                                     suffix,
                                     contact_id)
					VALUES   (p_prefix,
                        p_givenname,
                        p_middlename,
                        p_surname,
                        p_suffix,
                        p_contactid);									
            END IF;
		
        IF p_givenname IS NULL AND p_surname   IS NULL AND p_prefix IS NULL AND p_givenname IS NULL AND p_middlename IS NULL 
			AND p_surnam IS NULL AND p_suffix IS NULL 
		THEN
			DELETE FROM contactname WHERE contact_id = p_contactid;
              
		ELSEIF   
              p_givenname IS NULL OR
              p_surname   IS NULL THEN
            
             UPDATE   contactname
               SET    prefix = p_prefix,
                   givenname = p_givenname,
                  middlename = p_middlename,
                     surname = p_surname,
                      suffix = p_suffix
              WHERE   contact_id = p_contactid;
        END IF;
    END IF;
    
         UPDATE   contact
            SET   TYPE = p_type,
                  title = p_title,
                  telephone = p_telephone,
                  fax = p_fax,
                  email = p_email
          WHERE   contact_id = p_contactid;
              
         
END IF;

      -- ********************************************************
      DELETE FROM   contactname
            WHERE       prefix IS NULL
                    AND givenname IS NULL
                    AND middlename IS NULL
                    AND surname IS NULL
                    AND suffix IS NULL;

      -- ----******************************************************************
IF p_mode = 3
THEN
      
            SELECT            
              c.TYPE,
                     c.title,
                     c.telephone,
                     c.email,
                     c.fax,
                     c.contact_id,
                     w.url,
                     w.website_text,
                     Case  when w.LANG is null then 'en' 
                  else w.Lang End LANG,
                     a.countryTEST countrycode,
                     cc.name  countryname,
                     a.room,
                     a.street,
                     a.city,
                      ifnull(a.state,a.state) statecode,
                      ifnull(sc.name,a.state) statename,
                     a.postalcode,
                     cn.prefix,
                     cn.givenname,
                     cn.middlename,
                     cn.surname,  
                     cn.suffix
              FROM   contact c 
              LEFT JOIN website w
              ON C.CONTACT_ID = w.contact_id
              LEFT JOIN address a
              ON c.contact_id = a.contact_id
              LEFT JOIN contactname cn
              ON c.contact_id = cn.contact_id
              RIGHT JOIN SCI_STATECODES sc
              ON sc.code=a.STATE
              RIGHT JOIN SCI_COUNTRYCODES cc
              ON cc.LCODE=a.COUNTRYTEST,
              contactinfo cs
             WHERE c.contactinfo_id = cs.contactinfo_id
                     AND cs.OPPORTUNITY_ID = v_fundingbodyid;                                          
                   
      END IF;  
  END IF;
      
END ;;
