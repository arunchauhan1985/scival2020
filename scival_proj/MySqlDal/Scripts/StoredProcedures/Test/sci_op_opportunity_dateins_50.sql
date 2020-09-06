CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_op_opportunity_dateins_50`(
   P_WORKFLOWID                     INTEGER,
   P_date                           DATETIME /* DEFAULT NULL */ ,
   P_date_type                      INTEGER /* DEFAULT NULL */ ,
   P_date_sequence                  INTEGER /* DEFAULT NULL */ ,
   p_date_remarks                   LONGTEXT /* DEFAULT NULL */ ,
   p_date_Lang                   VARCHAR(4000) /* DEFAULT NULL */ ,
    p_URL                       VARCHAR(4000) /* DEFAULT NULL */ 
)
BEGIN
	DECLARE V_MODULEID               INTEGER;
	DECLARE Pv_RAWTEXT               VARCHAR (100);
	DECLARE v_subtype                INTEGER;
	DECLARE V_ID                     INTEGER;
	DECLARE V_CYCLE                  INTEGER;
	DECLARE V_FUNDINGBODYID          INTEGER;
	DECLARE v_value                  VARCHAR (2000);
	DECLARE v_REVISIONHISTORYID      INTEGER;
	DECLARE V_TRUSTING               VARCHAR (200);
	DECLARE v_preferedorgname        LONGTEXT;
	DECLARE v_type                   VARCHAR (2000);
	DECLARE v_RELATEDORGS_ID         INTEGER;
	DECLARE v_cnt                    INTEGER;	
	DECLARE l_opportunity_name_chk   INTEGER;
	DECLARE l_count                  INTEGER;
	DECLARE l_o_id                   INTEGER;

	SELECT   MODULEID, ID, CYCLE INTO   V_MODULEID, V_ID, V_CYCLE FROM   SCI_WORKFLOW
	WHERE   WORKFLOWID = P_WORKFLOWID;    

	IF V_MODULEID = 3
	THEN		
		SELECT   FUNDINGBODYID INTO   V_FUNDINGBODYID FROM   OPPORTUNITY_MASTER WHERE   OPPORTUNITYID = V_ID;

		SELECT   TRUSTING, PREFERREDORGNAME, TYPE INTO   V_TRUSTING, v_preferedorgname, v_type
		FROM   FUNDINGBODY WHERE   FUNDINGBODY_ID = V_FUNDINGBODYID;
		
		SELECT   COUNT (1) INTO   v_subtype FROM   SUBTYPE 
        WHERE   SUBTYPE_TEXT IN ('federal', 'Federal/National Government') AND FUNDINGBODY_ID = V_FUNDINGBODYID;
	END IF;

	SELECT COUNT(*) INTO V_CNT FROM sci_opp_loi_duedate_detail WHERE OPPORTUNITY_ID=v_id 
    AND DATE_TYPE=P_date_type AND SEQUENCE_ID=P_date_sequence AND LANG=p_date_Lang;
	IF v_cnt > 0 
    THEN
		UPDATE sci_opp_loi_duedate_detail SET DATE_REMARKS=p_date_remarks, LOI_DUE_DATE= p_date_Lang,URL=p_URL
		WHERE OPPORTUNITY_ID=v_id AND DATE_TYPE=P_date_type AND SEQUENCE_ID=P_date_sequence AND LANG=p_date_Lang;
           
		IF P_date_type = 2
		THEN
			UPDATE   OPPORTUNITY SET   DATE_FLAG = 1 WHERE   OPPORTUNITY_ID = v_id;
		END IF;      	
	END IF;  

	SELECT   IFNULL (MAX (O_ID), 0) + 1 INTO   l_o_id
	FROM   sci_opp_loi_duedate_detail;		
   
	INSERT INTO sci_opp_loi_duedate_detail (o_id,
                                           opportunity_id,
                                           loi_due_date,
                                           date_type,
                                           sequence_id,
                                           date_remarks,
                                           lang,
                                           URL)
	VALUES   (l_o_id,
               v_id,
               P_date,
               P_date_type,
               P_date_sequence,
               p_date_remarks,
               p_date_Lang,
               p_URL);

	IF P_date_type = 2
	THEN
		UPDATE   OPPORTUNITY  SET   DATE_FLAG = 1 WHERE   OPPORTUNITY_ID = v_id;
	END IF;	
END ;;
