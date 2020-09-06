CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_CheckOppStatus`(
	p_WorkflowID INTEGER
)
BEGIN
	SELECT opportunitystatus FROM opportunity WHERE Opportunity_id IN 
    (SELECT id FROM sci_workflow WHERE WORKFLOWID = p_WorkflowID);
END ;;