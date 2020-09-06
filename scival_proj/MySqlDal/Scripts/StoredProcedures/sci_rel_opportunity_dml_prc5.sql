CREATE PROCEDURE `sci_rel_opportunity_dml_prc5`(
	p_workflowid       INTEGER,
   p_insdel           INTEGER,                       -- 0 * insert ,1 * delete
   p_oppdbid          VARCHAR(4000),
   p_RELTYPE          INTEGER,
   p_Desc             VARCHAR(4000)
)
BEGIN
   DECLARE v_count            INTEGER;
   DECLARE v_OPPORTUNITYID    INTEGER;
   DECLARE v_value            INTEGER;
   DECLARE v_counter1         INTEGER  DEFAULT  0;
   DECLARE v_counter          INTEGER  DEFAULT  0;
   DECLARE v_orgcunt          INTEGER;
   DECLARE v_opp_text         VARCHAR (2000);
   DECLARE v_MODULEID         INTEGER;
   DECLARE v_fundingbody_id   INTEGER;
   DECLARE v_relname          VARCHAR (1000);
   DECLARE l_count            INTEGER;
   DECLARE l_id_nxt           INTEGER;

   SELECT   ID, MODULEID
     INTO   v_OPPORTUNITYID, v_MODULEID
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;

   IF v_moduleid = 3
   THEN
      IF p_insdel = 0
      THEN
         SELECT   RELATION_NAME
           INTO   v_relname
           FROM   SCI_OPPORTUNITY_RELATION_TYPE
          WHERE   RELID = p_RELTYPE;

	set @delim = ',';
		set @firstPiece = 1 + ((length(p_oppdbid) - length(replace(p_oppdbid, @delim, ''))) / length(@delim));
		set @i = 1;
        
		while @i <= @firstPiece do
			set @piece1 = string_splitter(p_oppdbid, @delim, @i);

			SELECT   OPPORTUNITYNAME
              INTO   v_opp_text
              FROM   opportunity_master
             WHERE   OPPORTUNITYID = (SELECT   OPPORTUNITY_ID
                                        FROM   opportunity
                                       WHERE   OPPORTUNITY_ID = @piece1);

            SELECT   COUNT( * )
              INTO   l_count
              FROM   sci_related_opportunity
             WHERE   OPPORTUNITY_ID = v_OPPORTUNITYID
                     AND RELATED_OPP_ID = piece1;

            IF l_count = 0
            THEN
               SELECT   IFNULL (MAX (ID), 0) + 1
                 INTO   l_id_nxt
                 FROM   sci_related_opportunity;

               INSERT INTO sci_related_opportunity (rel_opp_id,
                                                    opportunity_id,
                                                    related_opp_id,
                                                    opportunityname,
                                                    relaion_name,
                                                    ID,description)
                 VALUES   (p_RELTYPE,
                           v_OPPORTUNITYID,
                           piece1,
                           v_opp_text,
                           v_relname,
                           l_id_nxt,p_Desc);
            END IF;
            
			set @i = @i + 1;
		end while;
      ELSEIF p_insdel = 1
      THEN
         DELETE FROM   sci_related_opportunity
               WHERE   related_opp_id = p_oppdbid
                       AND OPPORTUNITY_ID = v_OPPORTUNITYID;
      END IF;

      SELECT   fundingbodyid
        INTO   v_fundingbody_id
        FROM   opportunity_master
       WHERE   opportunityid = v_OPPORTUNITYID;

         SELECT   opportunityid, opportunityname
           FROM   opportunity_master
          WHERE       fundingbodyid = v_fundingbody_id
                  AND opportunityid <> v_OPPORTUNITYID
                  AND IFNULL (STATUSCODE, 1) <> 3;

         SELECT   relid,
                  ort.relation_name,
                  RELATED_OPP_ID,
                  sro.OPPORTUNITYNAME,
                  sro.description
           FROM   sci_related_opportunity sro,
                  opportunity_master opm,
                  SCI_OPPORTUNITY_RELATION_TYPE ort
          WHERE       sro.RELATED_OPP_ID = opm.OPPORTUNITYID
                  AND SRO.REL_OPP_ID = ORT.RELID
                  AND IFNULL (opm.STATUSCODE, 1) <> 3
                  AND SRO.OPPORTUNITY_ID = v_OPPORTUNITYID;
                  
                  
                  
   ELSEIF v_moduleid = 4
   THEN
      IF p_insdel = 0
      THEN
         SELECT   RELATION_NAME
           INTO   v_relname
           FROM   SCI_OPPORTUNITY_RELATION_TYPE
          WHERE   RELID = p_RELTYPE;

         set @delim = ',';
		set @firstPiece = 1 + ((length(p_oppdbid) - length(replace(p_oppdbid, @delim, ''))) / length(@delim));
		set @i = 1;
        
		while @i <= @firstPiece do
			set @piece1 = string_splitter(p_oppdbid, @delim, @i);

			SELECT   OPPORTUNITYNAME
              INTO   v_opp_text
              FROM   opportunity_master
             WHERE   OPPORTUNITYID = (SELECT   OPPORTUNITY_ID
                                        FROM   opportunity
                                       WHERE   OPPORTUNITY_ID = @piece1);

            SELECT   COUNT( * )
              INTO   l_count
              FROM   sci_related_opportunity
             WHERE   OPPORTUNITY_ID = v_OPPORTUNITYID
                     AND RELATED_OPP_ID = I.LIST;

            IF l_count = 0
            THEN              
               SELECT   IFNULL (MAX (ID), 0) + 1
                 INTO   l_id_nxt
                 FROM   sci_related_opportunity;

               INSERT INTO sci_related_opportunity (rel_opp_id,
                                                    Award_id,
                                                    related_opp_id,
                                                    opportunityname,
                                                    relaion_name,
                                                    ID)
                 VALUES   (p_RELTYPE,
                           v_OPPORTUNITYID,
                          @piece1,
                           v_opp_text,
                           v_relname,
                           l_id_nxt);
            END IF;
            
			set @i = @i + 1;
		end while;
         
      ELSEIF p_insdel = 1
      THEN
         DELETE FROM   sci_related_opportunity
               WHERE   related_opp_id = p_oppdbid
                       AND Award_id = v_OPPORTUNITYID;
      END IF;

      SELECT   fundingbodyid
        INTO   v_fundingbody_id
        FROM   award_master
       WHERE   awardid = v_OPPORTUNITYID;

         SELECT   opportunityid, opportunityname
           FROM   opportunity_master
          WHERE   -- fundingbodyid = v_fundingbody_id and 
                  opportunityid <> v_OPPORTUNITYID
                  AND IFNULL (STATUSCODE, 1) <> 3;

         SELECT   relid,
                  ort.relation_name,
                  RELATED_OPP_ID,
                  sro.OPPORTUNITYNAME
           FROM   sci_related_opportunity sro,
                  opportunity_master opm,
                  SCI_OPPORTUNITY_RELATION_TYPE ort
          WHERE       sro.RELATED_OPP_ID = opm.OPPORTUNITYID
                  AND SRO.REL_OPP_ID = ORT.RELID
                  AND IFNULL (opm.STATUSCODE, 1) <> 3
                  AND SRO.Award_id = v_OPPORTUNITYID;
   END IF;
END;
