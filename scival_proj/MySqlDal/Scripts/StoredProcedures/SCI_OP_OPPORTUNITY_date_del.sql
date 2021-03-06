CREATE DEFINER=`root`@`localhost` PROCEDURE `SCI_OP_OPPORTUNITY_date_del`(
   P_WORKFLOWID       INTEGER
)
sp_lbl:

BEGIN
	DECLARE l_count      INTEGER;
	DECLARE V_ID         INTEGER;
	DECLARE V_MODULEID   INTEGER;
	DECLARE V_CYCLE      INTEGER;
    
	SELECT   MODULEID, ID, CYCLE INTO   V_MODULEID, V_ID, V_CYCLE FROM   SCI_WORKFLOW WHERE   WORKFLOWID = P_WORKFLOWID;     

	SELECT   COUNT(*) INTO   l_count FROM   sci_opp_loi_duedate_detail	WHERE   OPPORTUNITY_ID = V_ID;

	IF l_count > 0
	THEN
		DELETE FROM   sci_opp_loi_duedate_detail WHERE   OPPORTUNITY_ID = V_ID;				
	END IF;      
END ;;
