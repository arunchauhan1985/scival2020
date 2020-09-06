CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_auto_lead_relorgs_prc`(
   p_id             INTEGER,
   p_moduleid       INTEGER,
   p_flag           INTEGER   
)
sp_lbl:

BEGIN
	DECLARE L_COUNT               INTEGER;
	DECLARE l_relatedorgs_count   INTEGER;
	DECLARE v_sci_id              INTEGER;
	DECLARE v_fbid VARCHAR(4000);
   
	SELECT   ID INTO   v_sci_id FROM   SCI_WORKFLOW a WHERE   A.WORKFLOWID = p_id;
      
	IF p_moduleid = 3
    THEN
		SELECT   FUNDINGBODYID INTO   v_fbid FROM   OPPORTUNITY_MASTER WHERE   OPPORTUNITYID = v_sci_id;
	ELSE
		SELECT   FUNDINGBODYID INTO   v_fbid FROM   award_MASTER WHERE   AWARDID = v_sci_id;
	END IF;     

	SELECT COUNT(*) INTO l_count FROM auto_lead_relatedorgs
    WHERE FUNDINGBODYID = v_fbid and 1=2;

	IF p_flag = 0
	THEN
		IF l_count > 0
		THEN
			SELECT   LEAD_FUNDINGBODYID,
                     COMPONENT_FUNDINGBODYID,
                     component_fundingbodyid_other
            FROM   auto_lead_relatedorgs
            WHERE   FUNDINGBODYID =v_fbid; -- p_id;		
		ELSE         
			SELECT   NULL LEAD_FUNDINGBODYID,
                     NULL COMPONENT_FUNDINGBODYID,
                     NULL component_fundingbodyid_other
            FROM   DUAL;		
		END IF;
	ELSE
		IF p_moduleid = 3
		THEN
			SELECT COUNT(*) INTO   l_relatedorgs_count FROM   relatedorgs WHERE   OPPORTUNITY_ID = v_sci_id;           

			IF l_relatedorgs_count > 0
			THEN				
				SELECT   RELO.HIERARCHY,
                        ORGS.ORGDBID,
                        ORGS.RELTYPE,
                        ORGS.ORG_TEXT,
                        ORGS.RELATEDORGS_ID
                FROM   relatedorgs RELO, ORG ORGS
                WHERE   ORGS.RELATEDORGS_ID = RELO.RELATEDORGS_ID
                        AND OPPORTUNITY_ID = v_sci_id;            
			ELSE
				SELECT   NULL LEAD_FUNDINGBODYID,
                        NULL COMPONENT_FUNDINGBODYID,
                        NULL component_fundingbodyid_other
                 FROM   DUAL;
			END IF;
		ELSE
			SELECT   COUNT(*) INTO   l_relatedorgs_count FROM   relatedorgs WHERE   award_id = v_sci_id;    

			IF l_relatedorgs_count > 0
			THEN				
				SELECT   RELO.HIERARCHY,
                        ORGS.ORGDBID,
                        ORGS.RELTYPE,
                        ORGS.ORG_TEXT,
                        ORGS.RELATEDORGS_ID
                FROM   relatedorgs RELO, ORG ORGS
                WHERE   ORGS.RELATEDORGS_ID = RELO.RELATEDORGS_ID
                        AND award_id = v_sci_id;                                  
			ELSE            
				SELECT   NULL LEAD_FUNDINGBODYID,
                        NULL COMPONENT_FUNDINGBODYID,
                        NULL component_fundingbodyid_other
                FROM   DUAL;            
			END IF;
		END IF;
	END IF;
END ;;
