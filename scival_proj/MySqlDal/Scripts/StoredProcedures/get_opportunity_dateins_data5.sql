CREATE DEFINER=`root`@`localhost` PROCEDURE `get_opportunity_dateins_data5`(
   P_WORKFLOWID                     INTEGER
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
   
	SELECT   MODULEID, ID, CYCLE INTO   V_MODULEID, V_ID, V_CYCLE FROM   SCI_WORKFLOW WHERE   WORKFLOWID = P_WORKFLOWID;
      
	IF V_MODULEID = 3
	THEN		
		SELECT   FUNDINGBODYID INTO   V_FUNDINGBODYID FROM   OPPORTUNITY_MASTER WHERE   OPPORTUNITYID = V_ID;

		SELECT   TRUSTING, PREFERREDORGNAME, TYPE INTO   V_TRUSTING, v_preferedorgname, v_type FROM   FUNDINGBODY
		WHERE   FUNDINGBODY_ID = V_FUNDINGBODYID;        

		SELECT COUNT(1) INTO   v_subtype FROM   SUBTYPE
		WHERE   SUBTYPE_TEXT IN ('federal', 'Federal/National Government') AND FUNDINGBODY_ID = V_FUNDINGBODYID;	
	END IF;
      
	SELECT   opportunity_id,
               DATE_FORMAT (loi_due_date, '%d-%m-%Y') loi_DATE,
               sequence_id,
               DATE_REMARKS,
               (CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
    FROM   sci_opp_loi_duedate_detail
    WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 1;
   
    SELECT   opportunity_id,
               (CASE
                   WHEN loi_due_date IS NULL THEN (DATE_REMARKS)
                   ELSE TO_CLOB (DATE_FORMAT (loi_due_date, '%d-%m-%Y'))
                END)
                  DUE_DATE,
               sequence_id,
               DATE_REMARKS,
               (CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
    FROM   sci_opp_loi_duedate_detail
    WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 2;
   
	SELECT   opportunity_id,
               DATE_FORMAT (loi_due_date, '%d-%m-%Y') expiration_date,
               sequence_id,
               DATE_REMARKS,
               (CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
    FROM   sci_opp_loi_duedate_detail
    WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 3;
   
	SELECT   opportunity_id,
               DATE_FORMAT (loi_due_date, '%d-%m-%Y') firstpost_date,
               sequence_id,
               DATE_REMARKS,
               (CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
    FROM   sci_opp_loi_duedate_detail
    WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 4;
   
	SELECT   opportunity_id,
               DATE_FORMAT (loi_due_date, '%d-%m-%Y') lastmodifedpost_date,
               sequence_id,
               DATE_REMARKS,
               (CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
    FROM   sci_opp_loi_duedate_detail
    WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 5;
   
	SELECT   opportunity_id,
               DATE_FORMAT (loi_due_date, '%d-%m-%Y') open_date,
               sequence_id,
               DATE_REMARKS,
               (CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
    FROM   sci_opp_loi_duedate_detail
    WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 6;
   
	SELECT   OPPORTUNITYSTATUS FROM   opportunity WHERE   OPPORTUNITY_ID = V_ID;
 
	SELECT   opportunity_id,
               DATE_FORMAT (loi_due_date, '%d-%m-%Y') PreProposalDate,
               sequence_id,
               DATE_REMARKS,
               (CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
    FROM   sci_opp_loi_duedate_detail
    WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 7;
         
	SELECT   opportunity_id,
               DATE_FORMAT (loi_due_date, '%d-%m-%Y') DecisionDate,
               sequence_id,
               DATE_REMARKS,
               (CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
    FROM   sci_opp_loi_duedate_detail
    WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 8;	
END ;;
