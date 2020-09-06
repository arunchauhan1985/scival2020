CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_iteminserttemp`(
   p_workflowid        INTEGER,
   p_mode              INTEGER,
   p_insdel            INTEGER,
   p_itemid            INTEGER,
   p_RELTYPE           VARCHAR(4000),
   p_DESCRIPTION       VARCHAR(4000),
   p_url               VARCHAR(4000),
   p_urltext           VARCHAR(4000),
   p_lang              VARCHAR(4000)
)
BEGIN
   DECLARE v_fundingbodyid                  INTEGER;
   DECLARE v_value                          INTEGER;
   DECLARE v_itemid                         INTEGER;
   DECLARE v_count                          INTEGER;
   DECLARE v_itemcount                      INTEGER;
   DECLARE v_moduleid                       INTEGER;
   DECLARE l_region_cnt                     INTEGER;
   DECLARE l_eligibilitydescription_id      INTEGER;
   DECLARE v_itemid_seq                     INTEGER;
   DECLARE v_itemd                          INTEGER;
   DECLARE l_limitedsubmissiondesc_id_seq   INTEGER;
   DECLARE v_ELIGIBILITYDESCRIPTION_ID      INTEGER;
   DECLARE l_estimatedamountdesc_id        INTEGER;
 
   SELECT   ID, moduleid
     INTO   v_fundingbodyid, V_moduleid
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;

   IF v_moduleid = 2
   THEN
      IF p_mode = 1 AND p_insdel = 0
      THEN
         SELECT   COUNT(*)
           INTO   v_count
           FROM   about
          WHERE   FUNDINGBODY_ID = v_fundingbodyid;

         IF v_count = 0
         THEN
            SELECT   about_SEQ.NEXTVAL INTO v_value FROM DUAL;

            INSERT INTO about
              VALUES   (v_value, v_fundingbodyid);
         ELSE
            SELECT   ABOUT_ID
              INTO   v_value
              FROM   about
             WHERE   FUNDINGBODY_ID = v_fundingbodyid;
         END IF;

         SELECT   item_SEQ.NEXTVAL INTO v_itemid FROM DUAL;



         INSERT INTO item (RELTYPE,
                           DESCRIPTION,
                           ITEM_ID,
                           ABOUT_ID,
                           LANG)
           VALUES   (p_RELTYPE,
                     p_DESCRIPTION,
                     v_itemid,
                     v_value,
                     p_lang);

         INSERT INTO link (URL, LINK_TEXT, ITEM_ID)
           VALUES   (p_url, p_urltext, v_itemid);
      
      ELSEIF p_mode = 1 AND p_insdel = 1
      THEN
         DELETE FROM   link
               WHERE   ITEM_ID = p_itemid;

         DELETE FROM   item
               WHERE   ITEM_ID = p_itemid;



         SELECT   COUNT(*)
           INTO   v_itemcount
           FROM   item
          WHERE   ABOUT_ID = (SELECT   about_id
                                FROM   about
                               WHERE   FUNDINGBODY_ID = v_fundingbodyid);

         IF v_itemcount = 0
         THEN
            DELETE FROM   about
                  WHERE   FUNDINGBODY_ID = v_fundingbodyid;
         END IF;
      ELSEIF p_mode = 1 AND p_insdel = 2
      THEN
         UPDATE   item
            SET   RELTYPE = p_RELTYPE,
                  DESCRIPTION = p_DESCRIPTION,
                  LANG = p_lang
          WHERE   ITEM_ID = p_itemid;

         UPDATE   link
            SET   URL = p_URL, LINK_TEXT = p_urltext
          WHERE   ITEM_ID = p_itemid;
	ELSEIF p_mode = 2 AND p_insdel = 0
      THEN
         SELECT   COUNT(*)
           INTO   v_count
           FROM   FUNDINGPOLICY
          WHERE   FUNDINGBODY_ID = v_fundingbodyid;

         IF v_count = 0
         THEN
            SELECT   FUNDINGPOLICY_SEQ.NEXTVAL INTO v_value FROM DUAL;

            INSERT INTO FUNDINGPOLICY (FUNDINGPOLICY_ID, FUNDINGBODY_ID)
              VALUES   (v_value, v_fundingbodyid);
         ELSE
            SELECT   FUNDINGPOLICY_ID
              INTO   v_value
              FROM   FUNDINGPOLICY
             WHERE   FUNDINGBODY_ID = v_fundingbodyid;
         END IF;
         SELECT   item_SEQ.NEXTVAL INTO v_itemid FROM DUAL;
         INSERT INTO item (RELTYPE,
                           DESCRIPTION,
                           ITEM_ID,
                           FUNDINGPOLICY_ID,
                           LANG)
           VALUES   (p_RELTYPE,
                     p_DESCRIPTION,
                     v_itemid,
                     v_value,
                     p_lang);

         INSERT INTO link (URL, LINK_TEXT, ITEM_ID)
           VALUES   (p_url, p_urltext, v_itemid);

      ELSEIF p_mode = 2 AND p_insdel = 1
      THEN
         DELETE FROM   link
               WHERE   ITEM_ID = p_itemid;

         DELETE FROM   item
               WHERE   ITEM_ID = p_itemid;

         SELECT   COUNT(*)
           INTO   v_itemcount
           FROM   item
          WHERE   APPINFO_ID = (SELECT   APPINFO_ID
                                  FROM   appinfo
                                 WHERE   FUNDINGBODY_ID = v_fundingbodyid);

         IF v_itemcount = 0
         THEN
            DELETE FROM   appinfo
                  WHERE   FUNDINGBODY_ID = v_fundingbodyid;
         END IF;
      ELSEIF p_mode = 2 AND p_insdel = 2
      THEN
         UPDATE   item
            SET   RELTYPE = p_RELTYPE,
                  DESCRIPTION = p_DESCRIPTION,
                  LANG = p_lang
          WHERE   ITEM_ID = p_itemid;

         UPDATE   link
            SET   URL = p_URL, LINK_TEXT = p_urltext
          WHERE   ITEM_ID = p_itemid;
      
      ELSEIF p_mode = 3 AND p_insdel = 0
      THEN
         SELECT   COUNT(*)
           INTO   v_count
           FROM   geoscope
          WHERE   FUNDINGBODY_ID = v_fundingbodyid;

         IF v_count = 0
         THEN
            SELECT   geoscope_SEQ.NEXTVAL INTO v_value FROM DUAL;

            INSERT INTO geoscope
              VALUES   (v_value, v_fundingbodyid);
         ELSE
            SELECT   GEOSCOPE_ID
              INTO   v_value
              FROM   geoscope
             WHERE   FUNDINGBODY_ID = v_fundingbodyid;
         END IF;
         SELECT   item_SEQ.NEXTVAL INTO v_itemid FROM DUAL;
         INSERT INTO item (RELTYPE,
                           DESCRIPTION,
                           ITEM_ID,
                           GEOSCOPE_ID,
                           LANG)
           VALUES   (p_RELTYPE,
                     p_DESCRIPTION,
                     v_itemid,
                     v_value,
                     p_lang);

         INSERT INTO link (URL, LINK_TEXT, ITEM_ID)
           VALUES   (p_url, p_urltext, v_itemid);

      ELSEIF p_mode = 3 AND p_insdel = 1
      THEN
         DELETE FROM   link
               WHERE   ITEM_ID = p_itemid;

         DELETE FROM   item
               WHERE   ITEM_ID = p_itemid;


         SELECT   COUNT(*)
           INTO   v_itemcount
           FROM   item
          WHERE   GEOSCOPE_ID = (SELECT   APPINFO_ID
                                   FROM   geoscope
                                  WHERE   FUNDINGBODY_ID = v_fundingbodyid);

         IF v_itemcount = 0
         THEN
            DELETE FROM   geoscope
                  WHERE   FUNDINGBODY_ID = v_fundingbodyid;
         END IF;

      ELSEIF p_mode = 3 AND p_insdel = 2
      THEN
         UPDATE   item
            SET   RELTYPE = p_RELTYPE,
                  DESCRIPTION = p_DESCRIPTION,
                  LANG = p_lang
          WHERE   ITEM_ID = p_itemid;

         UPDATE   link
            SET   URL = p_URL, LINK_TEXT = p_urltext
          WHERE   ITEM_ID = p_itemid;
      
      ELSEIF p_mode = 4 AND p_insdel = 0
      THEN
         SELECT   COUNT(*)
           INTO   v_count
           FROM   relateditems
          WHERE   FUNDINGBODY_ID = v_fundingbodyid;

         IF v_count = 0
         THEN
            SELECT   relateditems_SEQ.NEXTVAL INTO v_value FROM DUAL;

            INSERT INTO relateditems (RELATEDITEMS_ID, FUNDINGBODY_ID)
              VALUES   (v_value, v_fundingbodyid);
         ELSE
            SELECT   RELATEDITEMS_ID
              INTO   v_value
              FROM   relateditems
             WHERE   FUNDINGBODY_ID = v_fundingbodyid;
         END IF;


         SELECT   item_SEQ.NEXTVAL INTO v_itemid FROM DUAL;



         INSERT INTO item (RELTYPE,
                           DESCRIPTION,
                           ITEM_ID,
                           RELATEDITEMS_ID,
                           LANG)
           VALUES   (p_RELTYPE,
                     p_DESCRIPTION,
                     v_itemid,
                     v_value,
                     p_lang);

         INSERT INTO link (URL, LINK_TEXT, ITEM_ID)
           VALUES   (p_url, p_urltext, v_itemid);
      
      ELSEIF p_mode = 4 AND p_insdel = 1
      THEN
         DELETE FROM   link
               WHERE   ITEM_ID = p_itemid;

         DELETE FROM   item
               WHERE   ITEM_ID = p_itemid AND RELATEDITEMS_ID = v_value;

       SELECT   COUNT(*)
           INTO   v_itemcount
           FROM   item
          WHERE   RELATEDITEMS_ID =
                     (SELECT   RELATEDITEMS_ID
                        FROM   relateditems
                       WHERE   FUNDINGBODY_ID = v_fundingbodyid);

         IF v_itemcount = 0
         THEN
            DELETE FROM   relateditems
                  WHERE   FUNDINGBODY_ID = v_fundingbodyid;
         END IF;
      ELSEIF p_mode = 4 AND p_insdel = 2
      THEN
         UPDATE   item
            SET   RELTYPE = p_RELTYPE,
                  DESCRIPTION = p_DESCRIPTION,
                  lang = p_lang
          WHERE   ITEM_ID = p_itemid;

         UPDATE   link
            SET   URL = p_URL, LINK_TEXT = p_urltext
          WHERE   ITEM_ID = p_itemid;
      
      ELSEIF p_mode = 7 AND p_insdel = 0
      THEN
         SELECT   COUNT(*)
           INTO   l_region_cnt
           FROM   region
          WHERE   fundingbody_id = v_fundingbodyid;

         IF l_region_cnt = 0
         THEN
            INSERT INTO region (LANG, REGION_TEXT, FUNDINGBODY_ID)
              VALUES   (p_lang, p_DESCRIPTION, v_fundingbodyid);
         ELSE
            UPDATE   region
               SET   REGION_TEXT = p_DESCRIPTION, lang = p_lang
             WHERE   fundingbody_id = v_fundingbodyid;
         END IF;
      END IF;

     IF p_mode = 1
      THEN
--         OPEN p_mRELATION FOR
            SELECT   it.RELTYPE,
                     it.description,
                     it.ITEM_ID,
                     it.ABOUT_ID id,
                     ch.FUNDINGBODY_ID,
                     l.URL,
                     l.LINK_TEXT,
                     AWARDSTATISTICS_ID,
                     IT.LANG
              FROM   item it, about ch, link l
             WHERE       it.about_id = ch.about_id
                     AND l.item_id = it.item_id
                     AND ch.FUNDINGBODY_ID = v_fundingbodyid;
      ELSEIF p_mode = 2
      THEN
  --       OPEN p_mRELATION FOR
            SELECT   it.RELTYPE,
                     it.description,
                     it.ITEM_ID,
                     it.APPINFO_ID id,
                    ch.FUNDINGBODY_ID,
                     l.URL,
                     l.LINK_TEXT,
                     AWARDSTATISTICS_ID,
                     IT.LANG
              FROM   item it, appinfo ch, link l
             WHERE       it.APPINFO_ID = ch.APPINFO_ID
                     AND l.item_id = it.item_id
                     AND ch.FUNDINGBODY_ID = v_fundingbodyid;
      ELSEIF p_mode = 3
      THEN
        -- OPEN p_mRELATION FOR
            SELECT   it.RELTYPE,
                     it.description,
                     it.ITEM_ID,
                     it.GEOSCOPE_ID id,
                     it.lang,
                     ch.FUNDINGBODY_ID,
                     l.URL,
                     l.LINK_TEXT,
                     AWARDSTATISTICS_ID
              FROM   item it, geoscope ch, link l
             WHERE       it.geoscope_id = ch.geoscope_id
                     AND l.item_id = it.item_id
                     AND ch.FUNDINGBODY_ID = v_fundingbodyid;
      ELSEIF p_mode = 4
      THEN
         -- OPEN p_mRELATION FOR
            SELECT   it.RELTYPE,
                     it.description,
                     it.ITEM_ID,
                     it.RELATEDITEMS_ID id,
                     it.lang,
                     ch.FUNDINGBODY_ID,
                     l.URL,
                     l.LINK_TEXT,
                     AWARDSTATISTICS_ID
              FROM   item it, relateditems ch, link l
             WHERE       it.RELATEDITEMS_ID = ch.RELATEDITEMS_ID
                     AND l.item_id = it.item_id
                     AND ch.FUNDINGBODY_ID = v_fundingbodyid;
      ELSEIF p_mode = 7
      THEN
         -- OPEN p_mRELATION FOR
            SELECT   REGION_TEXT, lang, FUNDINGBODY_ID
              FROM   region
             WHERE   FUNDINGBODY_ID = v_fundingbodyid;
      END IF;
     
   ELSEIF v_moduleid = 3
   THEN
      IF p_mode = 2 AND p_insdel = 0
      THEN
         SELECT   COUNT(*)
           INTO   v_count
           FROM   appinfo
          WHERE   OPPORTUNITY_ID = v_fundingbodyid;

         IF v_count = 0
         THEN
            SELECT   appinfo_SEQ.NEXTVAL INTO v_value FROM DUAL;

            INSERT INTO appinfo (APPINFO_ID, OPPORTUNITY_ID)
              VALUES   (v_value, v_fundingbodyid);
         ELSE
            SELECT   APPINFO_ID
              INTO   v_value
              FROM   appinfo
             WHERE   OPPORTUNITY_ID = v_fundingbodyid;
         END IF;



         SELECT   item_SEQ.NEXTVAL INTO v_itemid FROM DUAL;



         INSERT INTO item (RELTYPE,
                           DESCRIPTION,
                           ITEM_ID,
                           APPINFO_ID,
                           lang)
           VALUES   (p_RELTYPE,
                     p_DESCRIPTION,
                     v_itemid,
                     v_value,
                     p_lang);

         INSERT INTO link (URL, LINK_TEXT, ITEM_ID)
           VALUES   (p_url, p_urltext, v_itemid);
      
      ELSEIF p_mode = 2 AND p_insdel = 1
      THEN
         DELETE FROM   link
               WHERE   ITEM_ID = p_itemid;

         DELETE FROM   item
               WHERE   ITEM_ID = p_itemid;

         SELECT   COUNT(*)
           INTO   v_itemcount
           FROM   item
          WHERE   APPINFO_ID = (SELECT   APPINFO_ID
                                  FROM   appinfo
                                 WHERE   OPPORTUNITY_ID = v_fundingbodyid);

         IF v_itemcount = 0
         THEN
            DELETE FROM   appinfo
                  WHERE   OPPORTUNITY_ID = v_fundingbodyid;
         END IF;
      ELSEIF p_mode = 2 AND p_insdel = 2
      THEN
         UPDATE   item
            SET   RELTYPE = p_RELTYPE,
                  DESCRIPTION = p_DESCRIPTION,
                  lang = p_lang
          WHERE   ITEM_ID = p_itemid;

         UPDATE   link
            SET   URL = p_URL, LINK_TEXT = p_urltext
          WHERE   ITEM_ID = p_itemid;
      ELSEIF p_mode = 4 AND p_insdel = 0
      THEN
         SELECT   COUNT(*)
           INTO   v_count
           FROM   relateditems
          WHERE   OPPORTUNITY_ID = v_fundingbodyid;

         IF v_count = 0
         THEN
            SELECT   relateditems_SEQ.NEXTVAL INTO v_value FROM DUAL;

            INSERT INTO relateditems (RELATEDITEMS_ID, OPPORTUNITY_ID)
              VALUES   (v_value, v_fundingbodyid);
         ELSE
            SELECT   RELATEDITEMS_ID
              INTO   v_value
              FROM   relateditems
             WHERE   OPPORTUNITY_ID = v_fundingbodyid;
         END IF;


         SELECT   item_SEQ.NEXTVAL INTO v_itemid FROM DUAL;



         INSERT INTO item (RELTYPE,
                           DESCRIPTION,
                           ITEM_ID,
                           RELATEDITEMS_ID,
                           lang)
           VALUES   (p_RELTYPE,
                     p_DESCRIPTION,
                     v_itemid,
                     v_value,
                     p_lang);

         INSERT INTO link (URL, LINK_TEXT, ITEM_ID)
           VALUES   (p_url, p_urltext, v_itemid);
      -- ***********************************--**************************************
      ELSEIF p_mode = 4 AND p_insdel = 1
      THEN
         DELETE FROM   link
               WHERE   ITEM_ID = p_itemid;

         DELETE FROM   item
               WHERE   ITEM_ID = p_itemid AND RELATEDITEMS_ID = v_value;



         SELECT   COUNT(*)
           INTO   v_itemcount
           FROM   item
          WHERE   RELATEDITEMS_ID =
                     (SELECT   RELATEDITEMS_ID
                        FROM   relateditems
                       WHERE   OPPORTUNITY_ID = v_fundingbodyid);

         IF v_itemcount = 0
         THEN
            DELETE FROM   relateditems
                  WHERE   OPPORTUNITY_ID = v_fundingbodyid;
         END IF;
      ELSEIF p_mode = 4 AND p_insdel = 2
      THEN
         UPDATE   item
            SET   RELTYPE = p_RELTYPE,
                  DESCRIPTION = p_DESCRIPTION,
                  lang = p_lang
          WHERE   ITEM_ID = p_itemid;

         UPDATE   link
            SET   URL = p_URL, LINK_TEXT = p_urltext
          WHERE   ITEM_ID = p_itemid;
      ELSEIF p_mode = 5 AND p_insdel = 0
      THEN
         SELECT   COUNT(*)
           INTO   v_count
           FROM   synopsis
          WHERE   OPPORTUNITY_ID = v_fundingbodyid;

         IF v_count = 0
         THEN
            SELECT   SYNOPSISID_SEQ.NEXTVAL INTO v_value FROM DUAL;

            INSERT INTO synopsis (SYNOPSIS_ID, OPPORTUNITY_ID)
              VALUES   (v_value, v_fundingbodyid);
         ELSE
            SELECT   SYNOPSIS_ID
              INTO   v_value
              FROM   synopsis
             WHERE   OPPORTUNITY_ID = v_fundingbodyid;
         END IF;


         SELECT   item_SEQ.NEXTVAL INTO v_itemid FROM DUAL;



         INSERT INTO item (                                         
                           DESCRIPTION,
                           ITEM_ID,
                           SYNOPSIS_ID,
                           lang)
           VALUES   (                                             
                     p_DESCRIPTION,             
                     v_itemid,
                     v_value,
                     p_lang);

         INSERT INTO link (URL, LINK_TEXT, ITEM_ID)
           VALUES   (p_url, p_urltext, v_itemid);
      
      ELSEIF p_mode = 5 AND p_insdel = 1
      THEN
         DELETE FROM   link
               WHERE   ITEM_ID = p_itemid;

         DELETE FROM   item
               WHERE   ITEM_ID = p_itemid;



         SELECT   COUNT(*)
           INTO   v_itemcount
           FROM   item
          WHERE   SYNOPSIS_ID = (SELECT   SYNOPSIS_ID
                                   FROM   synopsis
                                  WHERE   OPPORTUNITY_ID = v_fundingbodyid);

         IF v_itemcount = 0
         THEN
            DELETE FROM   synopsis
                  WHERE   OPPORTUNITY_ID = v_fundingbodyid;
         END IF;
ELSEIF p_mode = 5 AND p_insdel = 2
      THEN
         UPDATE   item
            SET                                         
               DESCRIPTION = p_DESCRIPTION, lang = p_lang
          WHERE   ITEM_ID = p_itemid;

         UPDATE   link
            SET   URL = p_URL, LINK_TEXT = p_urltext
          WHERE   ITEM_ID = p_itemid;
      ELSEIF p_mode = 6 AND p_insdel = 0
      THEN
         SELECT   COUNT(*)
           INTO   v_count
           FROM   OPP_SUBJECTMATTER
          WHERE   OPPORTUNITY_ID = v_fundingbodyid;

         IF v_count = 0
         THEN
            SELECT   subject_matter_SEQ.NEXTVAL INTO v_value FROM DUAL;

            INSERT INTO OPP_SUBJECTMATTER (SUBJECTMATTER_ID, OPPORTUNITY_ID)
              VALUES   (v_value, v_fundingbodyid);
         ELSE
            SELECT   SUBJECTMATTER_ID
              INTO   v_value
              FROM   OPP_SUBJECTMATTER
             WHERE   OPPORTUNITY_ID = v_fundingbodyid;
         END IF;
         SELECT   item_SEQ.NEXTVAL INTO v_itemid FROM DUAL;

         INSERT INTO item (RELTYPE,
                           DESCRIPTION,
                           ITEM_ID,
                           SUBJECTMATTER_ID,
                           lang)
           VALUES   (p_RELTYPE,
                     p_DESCRIPTION,
                     v_itemid,
                     v_value,
                     p_lang);

         INSERT INTO link (URL, LINK_TEXT, ITEM_ID)
           VALUES   (p_url, p_urltext, v_itemid);
      ELSEIF p_mode = 6 AND p_insdel = 1
      THEN
         DELETE FROM   link
               WHERE   ITEM_ID = p_itemid;

         DELETE FROM   item
               WHERE   ITEM_ID = p_itemid;       

         SELECT   COUNT(*)
           INTO   v_itemcount
           FROM   item
          WHERE   SUBJECTMATTER_ID =
                     (SELECT   SUBJECTMATTER_ID
                        FROM   OPP_SUBJECTMATTER
                       WHERE   OPPORTUNITY_ID = v_fundingbodyid);

         IF v_itemcount = 0
         THEN
            DELETE FROM   OPP_SUBJECTMATTER
                  WHERE   OPPORTUNITY_ID = v_fundingbodyid;
         END IF;
      ELSEIF p_mode = 6 AND p_insdel = 2
      THEN
         UPDATE   item
            SET   RELTYPE = p_RELTYPE,
                  DESCRIPTION = p_DESCRIPTION,
                  lang = p_lang  
          WHERE   ITEM_ID = p_itemid;

         UPDATE   link
            SET   URL = p_URL, LINK_TEXT = p_urltext
          WHERE   ITEM_ID = p_itemid;
      

      ELSEIF p_mode = 8 AND p_insdel = 0
      THEN
         SELECT   COUNT(*)
           INTO   v_count
           FROM   eligibilitydescription
          WHERE   OPPORTUNITY_ID = v_fundingbodyid;

         IF v_count = 0
         THEN
            SELECT   eligibilitydescription_id_SEQ.NEXTVAL
              INTO   l_eligibilitydescription_id
              FROM   DUAL;

            INSERT INTO eligibilitydescription (
                                                   ELIGIBILITYDESCRIPTION_ID,
                                                   OPPORTUNITY_ID
                       )
              VALUES   (l_eligibilitydescription_id, v_fundingbodyid);

            SELECT   item_SEQ.NEXTVAL INTO v_itemid_SEQ FROM DUAL;

            INSERT INTO item (RELTYPE,
                              DESCRIPTION,
                              ITEM_ID,
                              lang,
                              ELIGIBILITYDESCRIPTION_ID)
              VALUES   ('about',
                        p_DESCRIPTION,
                        v_itemid_SEQ,
                        p_lang,
                        l_eligibilitydescription_id);

            INSERT INTO LINK (URL, LINK_TEXT, ITEM_ID)
              VALUES   (p_url, p_urltext, v_itemid_SEQ);
         ELSEIF v_count > 0                                                  -- 
         THEN
            SELECT   item_SEQ.NEXTVAL INTO v_itemid_SEQ FROM DUAL;



            SELECT   ELIGIBILITYDESCRIPTION_ID
              INTO   v_ELIGIBILITYDESCRIPTION_ID
              FROM   eligibilitydescription
             WHERE   OPPORTUNITY_ID = v_fundingbodyid;

            INSERT INTO item (RELTYPE,
                              DESCRIPTION,
                              ITEM_ID,
                              lang,
                              ELIGIBILITYDESCRIPTION_ID)
              VALUES   ('about',
                        p_DESCRIPTION,
                        v_itemid_SEQ,
                        p_lang,
                        v_ELIGIBILITYDESCRIPTION_ID);

            INSERT INTO LINK (URL, LINK_TEXT, ITEM_ID)
              VALUES   (p_url, p_urltext, v_itemid_SEQ);
         END IF;
      -- 
      ELSEIF p_mode = 8 AND p_insdel = 1
      THEN
         DELETE FROM  link
          WHERE   ITEM_ID = p_itemid;


         DELETE  FROM item
          WHERE   ITEM_ID = p_itemid;
      ELSEIF p_mode = 8 AND p_insdel = 2
      THEN
         UPDATE   ITEM
            SET   DESCRIPTION = p_DESCRIPTION, lang = p_lang
          WHERE   ITEM_ID = p_itemid;

         UPDATE   LINK
            SET   URL = p_url, LINK_TEXT = p_urltext
          WHERE   ITEM_ID = p_itemid;
      ELSEIF p_mode = 9 AND p_insdel = 0
      THEN
         SELECT   COUNT(*)
           INTO   v_count
           FROM   LIMITEDSUBMISSIONDESCRIPTION
          WHERE   OPPORTUNITY_ID = v_fundingbodyid;

         IF v_count = 0
         THEN
            SELECT   limitedsubmissiondesc_id_SEQ.NEXTVAL
              INTO   l_limitedsubmissiondesc_id_seq
              FROM   DUAL;

            INSERT INTO LIMITEDSUBMISSIONDESCRIPTION (
                                                         LIMITEDSUBMISSIONDESC_ID,
                                                         OPPORTUNITY_ID
                       )
              VALUES   (l_limitedsubmissiondesc_id_seq, v_fundingbodyid);

            SELECT   item_SEQ.NEXTVAL INTO v_itemid_SEQ FROM DUAL;

            INSERT INTO item (RELTYPE,
                              DESCRIPTION,
                              ITEM_ID,
                              lang,
                              LIMITEDSUBMISSIONDESC_ID)
              VALUES   ('about',
                        p_DESCRIPTION,
                        v_itemid_SEQ,
                        p_lang,
                        l_limitedsubmissiondesc_id_seq);

            INSERT INTO LINK (URL, LINK_TEXT, ITEM_ID)
              VALUES   (p_url, p_urltext, v_itemid_SEQ);
         ELSEIF v_count > 0
         THEN
            SELECT   LIMITEDSUBMISSIONDESC_ID
              INTO   v_ELIGIBILITYDESCRIPTION_ID
              FROM   LIMITEDSUBMISSIONDESCRIPTION
             WHERE   OPPORTUNITY_ID = v_fundingbodyid;



            SELECT   item_SEQ.NEXTVAL INTO v_itemid_SEQ FROM DUAL;

            INSERT INTO item (RELTYPE,
                              DESCRIPTION,
                              ITEM_ID,
                              lang,
                              LIMITEDSUBMISSIONDESC_ID)
              VALUES   ('about',
                        p_DESCRIPTION,
                        v_itemid_SEQ,
                        p_lang,
                        v_ELIGIBILITYDESCRIPTION_ID);

            INSERT INTO LINK (URL, LINK_TEXT, ITEM_ID)
              VALUES   (p_url, p_urltext, v_itemid_SEQ);
         END IF;
      ELSEIF p_mode = 9 AND p_insdel = 1
      THEN
         DELETE FROM link
          WHERE   ITEM_ID = p_itemid;


         DELETE  FROM item
          WHERE   ITEM_ID = p_itemid;
          Update OPPORTUNITY set limitedsubmissiondescription='' WHERE   OPPORTUNITY_ID = v_fundingbodyid;
      ELSEIF p_mode = 9 AND p_insdel = 2
      THEN
         UPDATE   ITEM
            SET   DESCRIPTION = p_DESCRIPTION, lang = p_lang
          WHERE   ITEM_ID = p_itemid;

         UPDATE   LINK
            SET   URL = p_url, LINK_TEXT = p_urltext
          WHERE   ITEM_ID = p_itemid;
      
      ELSEIF p_mode = 10 AND p_insdel = 0
      THEN
         SELECT   COUNT(*)
           INTO   v_count
           FROM   estimatedAmountDescription
          WHERE   OPPORTUNITY_ID = v_fundingbodyid;

         IF v_count = 0
         THEN
             SELECT   estimatedamountdesc_id_SEQ.NEXTVAL
                 INTO   l_estimatedamountdesc_id
                 FROM   DUAL;

            INSERT INTO estimatedAmountDescription (
                                                         ESTIMATEDAMOUNTDESCRIPTION_ID,
                                                         OPPORTUNITY_ID
                       )
              VALUES   (l_estimatedamountdesc_id, v_fundingbodyid);

            SELECT   item_SEQ.NEXTVAL INTO v_itemid_SEQ FROM DUAL;

            INSERT INTO item (RELTYPE,
                              DESCRIPTION,
                              ITEM_ID,
                              lang,
                              ESTIMATEDAMOUNTDESCRIPTION_ID)
              VALUES   ('about',
                        p_DESCRIPTION,
                        v_itemid_SEQ,
                        p_lang,
                        l_estimatedamountdesc_id);

            INSERT INTO LINK (URL, LINK_TEXT, ITEM_ID)
              VALUES   (p_url, p_urltext, v_itemid_SEQ);
         ELSEIF v_count > 0
         THEN
            SELECT   ESTIMATEDAMOUNTDESCRIPTION_ID
              INTO   l_estimatedamountdesc_id
              FROM   estimatedAmountDescription
             WHERE   OPPORTUNITY_ID = v_fundingbodyid;



            SELECT   item_SEQ.NEXTVAL INTO v_itemid_SEQ FROM DUAL;

            INSERT INTO item (RELTYPE,
                              DESCRIPTION,
                              ITEM_ID,
                              lang,
                              ESTIMATEDAMOUNTDESCRIPTION_ID)
              VALUES   ('about',
                        p_DESCRIPTION,
                        v_itemid_SEQ,
                        p_lang,
                        l_estimatedamountdesc_id);

            INSERT INTO LINK (URL, LINK_TEXT, ITEM_ID)
              VALUES   (p_url, p_urltext, v_itemid_SEQ);
         END IF;
      ELSEIF p_mode = 10 AND p_insdel = 1
      THEN
         DELETE  FROM link
          WHERE   ITEM_ID = p_itemid;


         DELETE FROM  item
          WHERE   ITEM_ID = p_itemid;
            
      ELSEIF p_mode = 10 AND p_insdel = 2
      THEN
         UPDATE   ITEM
            SET   DESCRIPTION = p_DESCRIPTION, lang = p_lang
          WHERE   ITEM_ID = p_itemid;

         UPDATE   LINK
            SET   URL = p_url, LINK_TEXT = p_urltext
          WHERE   ITEM_ID = p_itemid;
      
      END IF;

      COMMIT;

      IF p_mode = 2
      THEN
      --   OPEN p_mRELATION FOR
            SELECT   it.RELTYPE,
                     it.description,
                     it.ITEM_ID,
                     it.APPINFO_ID id,
                     it.lang,
                     ch.OPPORTUNITY_ID,
                     l.URL,
                     l.LINK_TEXT,
                     AWARDSTATISTICS_ID
              FROM   item it, appinfo ch, link l
             WHERE       it.APPINFO_ID = ch.APPINFO_ID
                     AND l.item_id = it.item_id
                     AND ch.OPPORTUNITY_ID = v_fundingbodyid;
      ELSEIF p_mode = 4
      THEN
        -- OPEN p_mRELATION FOR
            SELECT   it.RELTYPE,
                     it.description,
                     it.ITEM_ID,
                     it.RELATEDITEMS_ID id,
                     it.lang,
                     ch.OPPORTUNITY_ID,
                     l.URL,
                     l.LINK_TEXT,
                     AWARDSTATISTICS_ID
              FROM   item it, relateditems ch, link l
             WHERE       it.RELATEDITEMS_ID = ch.RELATEDITEMS_ID
                     AND l.item_id = it.item_id
                     AND ch.OPPORTUNITY_ID = v_fundingbodyid;
      ELSEIF p_mode = 5
      THEN
        -- OPEN p_mRELATION FOR
            SELECT  
            it .description,
                     it.ITEM_ID,
                     it.RELATEDITEMS_ID id,
                     it.lang,
                    ch.OPPORTUNITY_ID,
                     l.URL,
                     l.LINK_TEXT,
                     AWARDSTATISTICS_ID
              FROM   item it, synopsis ch, link l
             WHERE       it.SYNOPSIS_ID = ch.SYNOPSIS_ID
                     AND l.item_id = it.item_id
                     AND ch.OPPORTUNITY_ID = v_fundingbodyid;
      ELSEIF p_mode = 7
      THEN
         -- OPEN p_mRELATION FOR
            SELECT   REGION_TEXT, lang, FUNDINGBODY_ID
              FROM   region
             WHERE   FUNDINGBODY_ID = v_fundingbodyid;
      ELSEIF p_mode = 8
      THEN
         -- OPEN p_mRELATION FOR
            SELECT   description,
                     it.item_id,
                     it.lang,
                     opportunity_id,
                     url,
                     link_text reltype
              FROM   eligibilitydescription ED, ITEM IT, LINK LI
             WHERE   OPPORTUNITY_ID = v_fundingbodyid
                     AND ed.eligibilitydescription_id =
                           it.eligibilitydescription_id
                     AND it.item_id = li.item_id;
      ELSEIF p_mode = 9
      THEN
         -- OPEN p_mRELATION FOR
            SELECT   description,
                     it.item_id,
                     it.lang,
                     opportunity_id,
                     url,
                     link_text reltype
              FROM   LIMITEDSUBMISSIONDESCRIPTION ED, ITEM IT, LINK LI
             WHERE   OPPORTUNITY_ID = v_fundingbodyid
                     AND ed.limitedsubmissiondesc_id =
                           it.limitedsubmissiondesc_id
                     AND it.item_id = li.item_id;
                     
                       ELSEIF p_mode = 10
      THEN
         -- OPEN p_mRELATION FOR
            SELECT   description,
                     it.item_id,
                     it.lang,
                     opportunity_id,
                     url,
                     link_text reltype
              FROM   estimatedAmountDescription ED, ITEM IT, LINK LI
             WHERE   OPPORTUNITY_ID = v_fundingbodyid
                     AND ed.estimatedAmountDescription_id =
                           it.estimatedAmountDescription_id
                     AND it.item_id = li.item_id;
      END IF;

      
   ELSEIF v_moduleid = 4
   THEN
      IF p_mode = 4 AND p_insdel = 0
      THEN
         SELECT   COUNT(*)
           INTO   v_count
           FROM   relateditems
          WHERE   AWARD_ID = v_fundingbodyid;

         IF v_count = 0
         THEN
            SELECT   relateditems_SEQ.NEXTVAL INTO v_value FROM DUAL;

            INSERT INTO relateditems (RELATEDITEMS_ID, AWARD_ID)
              VALUES   (v_value, v_fundingbodyid);
         ELSE
            SELECT   RELATEDITEMS_ID
              INTO   v_value
              FROM   relateditems
             WHERE   AWARD_ID = v_fundingbodyid;
         END IF;


         SELECT   item_SEQ.NEXTVAL INTO v_itemid FROM DUAL;



         INSERT INTO item (RELTYPE,
                           DESCRIPTION,
                           ITEM_ID,
                           RELATEDITEMS_ID,
                           lang)
           VALUES   (p_RELTYPE,
                     p_DESCRIPTION,
                     v_itemid,
                     v_value,
                     p_lang);

         INSERT INTO link (URL, LINK_TEXT, ITEM_ID)
           VALUES   (p_url, p_urltext, v_itemid);
      ELSEIF p_mode = 4 AND p_insdel = 1
      THEN
         DELETE FROM   link
               WHERE   ITEM_ID = p_itemid;

         DELETE FROM   item
               WHERE   ITEM_ID = p_itemid;   
		SELECT   COUNT(*)
           INTO   v_itemcount
           FROM   item
          WHERE   RELATEDITEMS_ID = (SELECT   RELATEDITEMS_ID
                                       FROM   relateditems
                                      WHERE   AWARD_ID = v_fundingbodyid);

         IF v_itemcount = 0
         THEN
            DELETE FROM   relateditems
                  WHERE   AWARD_ID = v_fundingbodyid;
         END IF;
      ELSEIF p_mode = 4 AND p_insdel = 2
      THEN
         UPDATE   item
            SET   RELTYPE = p_RELTYPE,
                  DESCRIPTION = p_DESCRIPTION,
                  lang = p_lang
          WHERE   ITEM_ID = p_itemid;

         UPDATE   link
            SET   URL = p_URL, LINK_TEXT = p_urltext
          WHERE   ITEM_ID = p_itemid;
      END IF;

      COMMIT;


      IF p_mode = 4
      THEN
      --   OPEN p_mRELATION FOR
            SELECT   it.RELTYPE,
                     it.description,
                     it.ITEM_ID,
                     it.lang,
                     it.RELATEDITEMS_ID id,
                     ch.AWARD_ID,
                     l.URL,
                     l.LINK_TEXT,
                     AWARDSTATISTICS_ID
              FROM   item it, relateditems ch, link l
             WHERE       it.RELATEDITEMS_ID = ch.RELATEDITEMS_ID
                     AND l.item_id = it.item_id
                     AND ch.AWARD_ID = v_fundingbodyid;
      END IF;
end if;
END