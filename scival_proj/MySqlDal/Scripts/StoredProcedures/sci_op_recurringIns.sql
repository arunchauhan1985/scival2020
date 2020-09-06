CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_op_recurringIns`(
   P_WORKFLOWID                 INTEGER,
   p_recurring                  VARCHAR(4000),
   P_UserID                     INTEGER
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
	DECLARE l_Rec_id                   INTEGER;
	
	SELECT   MODULEID, ID, CYCLE INTO   V_MODULEID, V_ID, V_CYCLE FROM   SCI_WORKFLOW WHERE   WORKFLOWID = P_WORKFLOWID;      	

	IF V_MODULEID = 3
	THEN		
		SELECT   FUNDINGBODYID INTO   V_FUNDINGBODYID FROM   OPPORTUNITY_MASTER WHERE   OPPORTUNITYID = V_ID;

		SELECT   TRUSTING, PREFERREDORGNAME, TYPE INTO   V_TRUSTING, v_preferedorgname, v_type
		FROM   FUNDINGBODY WHERE   FUNDINGBODY_ID = V_FUNDINGBODYID;

		SELECT   COUNT (1) INTO   v_subtype FROM   SUBTYPE
		WHERE   SUBTYPE_TEXT IN ('federal', 'Federal/National Government') AND FUNDINGBODY_ID = V_FUNDINGBODYID;	
	END IF;
	
	SELECT   IFNULL (MAX (REC_ID), 0) + 1 INTO   l_Rec_id FROM   recurring;		
		
	INSERT INTO recurring (REC_ID,
						   OPPORTUNITY_ID,
						   RECURRING_STATUS,
						   USERID,
						   REC_DATE)
	VALUES   (l_Rec_id,
		   V_ID,
		   p_recurring,
		   P_UserID,
		   sysdate()
		  );
END ;;
