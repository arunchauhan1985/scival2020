CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_op_delete_date_prc`(
	P_WORKFLOWID        INTEGER,
	p_date_type         INTEGER,
	p_sequence_id       INTEGER,
	p_mode              INTEGER                    
)
BEGIN
   
	DECLARE V_MODULEID               INTEGER;
	DECLARE V_ID                     INTEGER;
	DECLARE V_CYCLE                  INTEGER;
	DECLARE V_FUNDINGBODYID          INTEGER;
	DECLARE v_value                  VARCHAR (2000);   
	DECLARE v_RELATEDORGS_ID         INTEGER;
	DECLARE v_cnt                    INTEGER;
	DECLARE l_count 		INTEGER;
	BEGIN
		SELECT   MODULEID, ID, CYCLE INTO   V_MODULEID, V_ID, V_CYCLE FROM   SCI_WORKFLOW WHERE   WORKFLOWID = P_WORKFLOWID;

		IF V_MODULEID = 3
		THEN
			SELECT   FUNDINGBODYID INTO   V_FUNDINGBODYID FROM   OPPORTUNITY_MASTER WHERE   OPPORTUNITYID = V_ID;
		END IF;

		IF p_mode = 0
		THEN
			SELECT  COUNT(*) INTO   l_count FROM   sci_opp_loi_duedate_detail 
            WHERE OPPORTUNITY_ID = V_ID AND DATE_TYPE = p_date_type AND SEQUENCE_ID = p_sequence_id;

		IF l_count > 0
		THEN
			DELETE FROM   sci_opp_loi_duedate_detail WHERE OPPORTUNITY_ID = V_ID AND DATE_TYPE = p_date_type
            AND SEQUENCE_ID = p_sequence_id;
		 -- RETURNING p_sequence_id INTO l_sequence_id;

			UPDATE sci_opp_loi_duedate_detail SET   sequence_id = sequence_id - 1
			WHERE opportunity_id = v_id AND date_type = p_date_type AND sequence_id > l_sequence_id;
		END IF;
	END IF;

	IF p_date_type = 1
	THEN      
		SELECT   opportunity_id,
                   DATE_FORMAT (loi_due_date, '%d-%m-%Y') loi_DATE,
                  sequence_id,
                  DATE_REMARKS
        FROM   sci_opp_loi_duedate_detail
        WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 1;
	ELSEIF p_date_type = 2
	THEN      
		SELECT   opportunity_id,
                  DATE_FORMAT (loi_due_date, '%d-%m-%Y') DUE_DATE,
                  sequence_id,
                  DATE_REMARKS
        FROM   sci_opp_loi_duedate_detail
        WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 2;
	ELSEIF p_date_type = 3
	THEN    
        SELECT   opportunity_id,
                    DATE_FORMAT (loi_due_date, '%d-%m-%Y') expiration_date,
                  sequence_id,
                  DATE_REMARKS
        FROM   sci_opp_loi_duedate_detail
        WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 3;
	ELSEIF p_date_type = 4
	THEN    
        SELECT   opportunity_id,
                   DATE_FORMAT (loi_due_date, '%d-%m-%Y') firstpost_date,
                  sequence_id,
                  DATE_REMARKS
        FROM   sci_opp_loi_duedate_detail
        WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 4;
	ELSEIF p_date_type = 5
	THEN      
        SELECT   opportunity_id,
                    DATE_FORMAT (loi_due_date, '%d-%m-%Y') lastmodifedpost_date,
                  sequence_id,
                  DATE_REMARKS
        FROM   sci_opp_loi_duedate_detail
        WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 5;
	ELSEIF p_date_type = 6
	THEN
		SELECT   opportunity_id,
                  DATE_FORMAT (loi_due_date, '%d-%m-%Y') open_date,
                  sequence_id,
                  DATE_REMARKS
        FROM   sci_opp_loi_duedate_detail
        WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 6;
	END IF;	
   END;
END ;;
