CREATE PROCEDURE `sci_itemlist5`(
	p_workflowid       integer,
   p_mode             integer -- 1 * about,2 * appinfo,3 * geoscope,4 * relateditems,5 * synopsis 6* subjectmatter 7* for region8* eligibility description*9 limitedsubmissiondescription
)
BEGIN
   DECLARE v_fundingbodyid   integer;
   DECLARE v_moduleid        integer;
 
   SELECT   ID, moduleid
     INTO   v_fundingbodyid, v_moduleid
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;

   SELECT   * FROM SCI_BINARYRELATIONTYPE;

   IF v_moduleid = 2
   THEN
      IF p_mode = 1
      THEN
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
                     Select Null RELTYPE,DESCRIPTION,FUNDINGDESCRIPTION_ID Item_id,LANG,FUNDINGBODY_ID,SOURCE URL,NULL LINK_TEXT  from FUNDINGDESCRIPTION where  FUNDINGBODY_ID=v_fundingbodyid;   
      ELSEIF p_mode = 4
      THEN        
                      Select Null RELTYPE,DESCRIPTION,AWARDSUCCESSRATE_ID Item_id,LANG,FUNDINGBODY_ID,SOURCE URL,NULL LINK_TEXT  from AWARDSUCCESSRATEDESC where  FUNDINGBODY_ID=v_fundingbodyid;   
                  
             ELSEIF p_mode = 7
      THEN         
              Select Type RELTYPE,Type DESCRIPTION,identifier_id Item_id,'en' LANG,FUNDINGBODY_ID,null URL,value LINK_TEXT 
             from identifier where  FUNDINGBODY_ID=v_fundingbodyid; 
             
              ELSEIF p_mode = 11
      THEN
         SELECT   LANG,
                    subregion_TEXT,
                   FUNDINGBODY_ID 
              FROM   subregion
             WHERE     FUNDINGBODY_ID = v_fundingbodyid; 
             
              ELSEIF p_mode = 12
      THEN
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
                     
                     
       ELSEIF p_mode = 13
      THEN
           SELECT   it.RELTYPE,
                     it.description,
                     it.ITEM_ID,
                     it.lang,
                     it.RELATEDITEMS_ID id,
                     it.duration_id,
                     d.OPPORTUNITY_ID,
                     l.URL,
                     l.LINK_TEXT,
                     AWARDSTATISTICS_ID,
                     o.DURATION
              FROM   item it,
                     duration d,
                     link l,
                     OPPORTUNITY o
             WHERE       it.duration_ID = d.duration_ID
                     AND d.OPPORTUNITY_ID = o.OPPORTUNITY_ID
                     AND l.item_id = it.item_id
                     AND d.OPPORTUNITY_ID = v_fundingbodyid;
                     
                        ELSEIF p_mode = 14
      THEN
            SELECT   it.RELTYPE,
                     it.description,
                     it.ITEM_ID,
                     it.lang,
                     it.RELATEDITEMS_ID id,
                     it.instruction_id,
                     i.OPPORTUNITY_ID,
                     l.URL,
                     l.LINK_TEXT
                    
                     
              FROM   item it,
                     instruction i,
                     link l,
                     OPPORTUNITY o
             WHERE   it.instruction_ID = i.instruction_ID
                     AND i.OPPORTUNITY_ID = o.OPPORTUNITY_ID
                     AND l.item_id = it.item_id
                     AND i.OPPORTUNITY_ID = v_fundingbodyid;
                     
                      ELSEIF p_mode = 15
      THEN
            SELECT   it.RELTYPE,
                     it.description,
                     it.ITEM_ID,
                     it.lang,
                     it.RELATEDITEMS_ID id,
                     it.licenseInformation_id,
                     i.OPPORTUNITY_ID,
                     l.URL,
                     l.LINK_TEXT
                    
                     
              FROM   item it,
                     licenseInformation i,
                     link l,
                     OPPORTUNITY o
             WHERE   it.licenseInformation_ID = i.licenseInformation_ID
                     AND i.OPPORTUNITY_ID = o.OPPORTUNITY_ID
                     AND l.item_id = it.item_id
                     AND i.OPPORTUNITY_ID = v_fundingbodyid;
      END IF;
   ELSEIF v_moduleid = 4
   THEN
      IF p_mode = 4
      THEN
           SELECT   it.RELTYPE,
                     it.description,
                     it.ITEM_ID,
                     -- it.GEOSCOPE_ID,
                     -- it.APPINFO_ID,
                     -- it.ABOUT_ID,
                     it.RELATEDITEMS_ID id,
                     it.LANG,
                     -- it.SYNOPSIS_ID,
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
END;
