CREATE DEFINER=`root`@`localhost` PROCEDURE `SCI_OP_RELATEDORGLIST_y`(
	p_workflowid       integer  
)
BEGIN
	DECLARE v_OPPORTUNITYID   integer;
	DECLARE v_moduleid        integer;
	DECLARE v_fbid            integer;
	DECLARE v_count           integer;
	DECLARE v_relatedorgs_id  integer;
	DECLARE v_fbname          varchar(4000);
 
	SELECT   ID, moduleid INTO   v_OPPORTUNITYID, v_moduleid FROM   sci_workflow WHERE   WORKFLOWID = p_workflowid;

	SELECT   * FROM SCI_BINARYRELATIONTYPE;
	SELECT   * FROM sci_hierarchymaster;

	IF v_moduleid = 3
	THEN
		SELECT COUNT(*) INTO v_count FROM RELATEDORGS WHERE OPPORTUNITY_ID= v_OPPORTUNITYID and HIERARCHY='lead';
		IF v_count > 0 
        THEN
			SELECT RELATEDORGS_ID INTO v_relatedorgs_id FROM RELATEDORGS WHERE OPPORTUNITY_ID= v_OPPORTUNITYID and HIERARCHY='lead';
		ELSE
			SELECT RELATEDORGS_SEQ.NEXTVAL INTO v_relatedorgs_id FROM DUAL;
			INSERT INTO relatedorgs (HIERARCHY,RELATEDORGS_ID,OPPORTUNITY_ID)
			VALUES('lead', v_relatedorgs_id, v_OPPORTUNITYID);
		END IF;
     
		SELECT FM.FUNDINGBODY_ID,FM.FUNDINGBODYNAME INTO v_fbid, v_fbname 
        FROM OPPORTUNITY_MASTER OM, FUNDINGBODY_MASTER FM
		WHERE FM.FUNDINGBODY_ID = OM.FUNDINGBODYID AND OM.OPPORTUNITYID = v_opportunityid;

		IF v_fbid IS NOT NULL AND v_fbname IS NOT NULL 
        THEN 
			SELECT COUNT(*) INTO v_count FROM ORG WHERE RELATEDORGS_ID=v_relatedorgs_id;

			IF v_count < 1 
            THEN
				IF v_fbid IS NOT NULL 
                THEN 
					INSERT INTO ORG(orgdbid,reltype,org_text,relatedorgs_id) VALUES(v_fbid,'fundedBy',v_fbname,v_relatedorgs_id);
				END IF;
			ELSE
				UPDATE ORG SET orgdbid=v_fbid,reltype='fundedBy',org_text=v_fbname where  RELATEDORGS_ID=v_relatedorgs_id;
			END IF;
		END IF;
		
        SELECT   fb.ORGDBID, FUNDINGBODYNAME
        FROM   fundingbody fb, fundingbody_master fm
		WHERE   fb.FUNDINGBODY_ID = fm.FUNDINGBODY_ID
               AND IFNULL (STATUSCODE, 1) <> 3
               AND IFNULL (STATUSCODE, 1) <> 45
               AND NOT EXISTS
                     (SELECT   *
                        FROM   org
                       WHERE   RELATEDORGS_ID IN
                                     (SELECT   RELATEDORGS_ID
                                        FROM   relatedorgs
                                       WHERE   OPPORTUNITY_ID =
                                                  v_OPPORTUNITYID)
                               AND ORGDBID = fb.FUNDINGBODY_ID)
                               
        UNION
                               
		SELECT vendor_id ORGDBID, vendor_fundingbody_name FUNDINGBODYNAME FROM   sci_related_orgs_vendor;

		SELECT   FUNDINGBODYNAME,
               rd.HIERARCHY,
               rd.RELATEDORGS_ID,
               fm.FUNDINGBODY_ID,
               o.ORGDBID,
               o.RELTYPE,
               (CASE WHEN fm.STATUSCODE = 3 THEN 1 ELSE 0 END) flag
        FROM   relatedorgs rd,
               org o,
               fundingbody fb,
               fundingbody_master fm
		WHERE  O.RELATEDORGS_ID= RD.RELATEDORGS_ID and
              FB.FUNDINGBODY_ID=O.ORGDBID and
              FM.FUNDINGBODY_ID= FB.FUNDINGBODY_ID 
              AND IFNULL(FM.STATUSCODE, 1) <> 45
				AND FB.FUNDINGBODY_ID = fm.FUNDINGBODY_ID
               AND rd.OPPORTUNITY_ID = v_OPPORTUNITYID
               
		UNION
        
        SELECT   vendor_fundingbody_name FUNDINGBODYNAME,
                  rd.HIERARCHY,
                  rd.RELATEDORGS_ID,
                  rov.vendor_id FUNDINGBODY_ID,
                  o.ORGDBID,
                  o.RELTYPE,
                  0 flag
        FROM   relatedorgs rd,
                  org o,
                  sci_related_orgs_vendor rov
        WHERE       rd.RELATEDORGS_ID = o.RELATEDORGS_ID
                  AND O.ORGDBID = rov.vendor_id
                 AND rd.OPPORTUNITY_ID =v_OPPORTUNITYID;
             
	ELSEIF  v_moduleid = 4 
    THEN
		SELECT   fb.ORGDBID, FUNDINGBODYNAME
		FROM   fundingbody fb, fundingbody_master fm
		WHERE   fb.FUNDINGBODY_ID = fm.FUNDINGBODY_ID
               AND IFNULL (STATUSCODE, 1) <> 3
               AND IFNULL (STATUSCODE, 1) <> 45
               AND NOT EXISTS
                     (SELECT   *
                        FROM   org
                       WHERE   RELATEDORGS_ID IN
                                     (SELECT   RELATEDORGS_ID
                                        FROM   relatedorgs
                                       WHERE   AWARD_ID =
                                                  v_OPPORTUNITYID)
                               AND ORGDBID = fb.FUNDINGBODY_ID)
                               UNION
                                SELECT vendor_id ORGDBID, vendor_fundingbody_name FUNDINGBODYNAME
			FROM   sci_related_orgs_vendor;

		SELECT   FUNDINGBODYNAME,
               rd.HIERARCHY,
               rd.RELATEDORGS_ID,
               rd.FUNDINGBODY_ID,
               o.ORGDBID,
               o.RELTYPE,
                FBA.AMOUNT, 
                FBA.CURRENCY,          
               (CASE WHEN fm.STATUSCODE = 3 THEN 1 ELSE 0 END) flag
		FROM   	relatedorgs rd,
				org o,
				fundingbody fb,
				fundingbody_master fm, 
				fundingBodyAmount FBA 
                LEFT JOIN relatedFundingBodies RFB
                ON FBA.RELEATEDFUNDINGBODIES_ID=RFB.RELEATEDFUNDINGBODIES_ID
		WHERE  rd.RELATEDORGS_ID = o.RELATEDORGS_ID
               AND O.ORGDBID = fb.ORGDBID
               AND IFNULL (fm.STATUSCODE, 1) <> 45
               AND FB.FUNDINGBODY_ID = fm.FUNDINGBODY_ID
               AND RFB.RELATEDORGSSS_ID= RD.RELATEDORGS_ID
               AND rd.AWARD_ID = v_OPPORTUNITYID;	
   END IF;
END ;;
