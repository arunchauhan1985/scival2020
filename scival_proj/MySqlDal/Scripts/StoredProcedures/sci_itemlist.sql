CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_itemlist`(
   p_workflowid       integer,
   p_mode             integer 
)
BEGIN
   DECLARE v_fundingbodyid   integer;
   DECLARE v_moduleid        integer;
 
   DECLARE EXIT HANDLER FOR SQLEXCEPTION
   BEGIN
      -- CLOSE p_mlist;
      -- CLOSE p_mRELATION;
END;
   SELECT   ID, moduleid
     INTO   v_fundingbodyid, v_moduleid
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;

--   OPEN p_mlist FOR SELECT   * FROM SCI_BINARYRELATIONTYPE;

   IF v_moduleid = 2
   THEN
      IF p_mode = 1
      THEN
   --       OPEN p_mRELATION FOR
            SELECT   it.RELTYPE,
                     it.description,
                     it.ITEM_ID,
                     it.ABOUT_ID id,
                     it.lang,
                     ch.FUNDINGBODY_ID,
                     l.URL,
                     l.LINK_TEXT,
                     AWARDSTATISTICS_ID
              FROM   item it, about ch, link l
             WHERE       it.item_id = l.item_id
                     AND it.about_id = ch.about_id
                     AND ch.FUNDINGBODY_ID = v_fundingbodyid;
      ELSEIF p_mode = 2
      THEN
         -- OPEN p_mRELATION FOR
            SELECT   it.RELTYPE,
                     it.description,
                     it.ITEM_ID,
                     it.APPINFO_ID id,
                     it.lang,
                     ch.FUNDINGBODY_ID,
                     l.URL,
                     l.LINK_TEXT,
                     AWARDSTATISTICS_ID
              FROM   item it, fundingpolicy ch, link l
             WHERE       it.item_id = l.item_id
                     AND it.FUNDINGPOLICY_ID = ch.FUNDINGPOLICY_ID
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
             WHERE       it.item_id = l.item_id
                     AND it.geoscope_id = ch.geoscope_id
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
             WHERE       it.item_id = l.item_id
                     AND it.RELATEDITEMS_ID = ch.RELATEDITEMS_ID
                     AND ch.FUNDINGBODY_ID = v_fundingbodyid;
                  
             ELSEIF p_mode = 7
      THEN
         -- OPEN p_mRELATION FOR
            SELECT   LANG,
                    REGION_TEXT,
                   FUNDINGBODY_ID 
              FROM   region
             WHERE     FUNDINGBODY_ID = v_fundingbodyid;  
             
              ELSEIF p_mode = 11
      THEN
         -- OPEN p_mRELATION FOR
            SELECT   LANG,
                    subregion_TEXT,
                   FUNDINGBODY_ID 
              FROM   subregion
             WHERE     FUNDINGBODY_ID = v_fundingbodyid; 
             
              ELSEIF p_mode = 12
      THEN
         -- OPEN p_mRELATION FOR
            SELECT   LANG,
                    fundergroup_TEXT,
                   FUNDINGBODY_ID 
              FROM   fundergroup
             WHERE     FUNDINGBODY_ID = v_fundingbodyid; 
                     
      END IF;
   ELSEIF v_moduleid = 3
   THEN
      IF p_mode = 2
      THEN
         -- OPEN p_mRELATION FOR
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
             WHERE       it.item_id = l.item_id
                     AND it.APPINFO_ID = ch.APPINFO_ID
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
             WHERE       it.item_id = l.item_id
                     AND it.RELATEDITEMS_ID = ch.RELATEDITEMS_ID
                     AND ch.OPPORTUNITY_ID = v_fundingbodyid;
      ELSEIF p_mode = 5
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
              FROM   item it, synopsis ch, link l
             WHERE       it.item_id = l.item_id
                     AND it.SYNOPSIS_ID = ch.SYNOPSIS_ID
                     AND ch.OPPORTUNITY_ID = v_fundingbodyid;
           
         ELSEIF p_mode = 6
      THEN
         -- OPEN p_mRELATION FOR
            SELECT   it.RELTYPE,
                     it.description,  
                     it.ITEM_ID,
                     it.SUBJECTMATTER_ID id,
                     it.lang,
                     ch.OPPORTUNITY_ID,
                     l.URL,
                     l.LINK_TEXT,
                     AWARDSTATISTICS_ID
              FROM   item it, opp_subjectmatter ch, link l
             WHERE       it.item_id = l.item_id
                     AND it.SUBJECTMATTER_ID = ch.SUBJECTMATTER_ID
                     AND ch.OPPORTUNITY_ID = v_fundingbodyid;
                     
           ELSEIF p_mode = 8
      THEN
         -- OPEN p_mRELATION FOR
            SELECT   description,
                     it.item_id,it.lang,
                     opportunity_id,
                     url,
                     link_text ,
                     reltype
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
                     link_text,
                      reltype
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
                     link_text,
                     reltype
              FROM   estimatedAmountDescription ED, ITEM IT, LINK LI
             WHERE   OPPORTUNITY_ID = v_fundingbodyid
                     AND ed.estimatedAmountDescription_id =
                           it.estimatedAmountDescription_id
                     AND it.item_id = li.item_id;
                     
                     
                     
      END IF;
   ELSEIF v_moduleid = 4
   THEN
      IF p_mode = 4
      THEN
         -- OPEN p_mRELATION FOR
            SELECT   it.RELTYPE,
                     it.description,
                     it.ITEM_ID,
                     it.RELATEDITEMS_ID id,
                     it.LANG,
                     ch.AWARD_ID,
                     l.URL,
                     l.LINK_TEXT,
                     AWARDSTATISTICS_ID
              FROM   item it, relateditems ch, link l
             WHERE       it.item_id = l.item_id
                     AND it.RELATEDITEMS_ID = ch.RELATEDITEMS_ID
                     AND ch.AWARD_ID = v_fundingbodyid;
      END IF;
   END IF;
END