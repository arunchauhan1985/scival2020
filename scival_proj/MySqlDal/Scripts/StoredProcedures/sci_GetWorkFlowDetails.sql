CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_GetWorkFlowDetails`(
	p_WorkflowId INTEGER
)
BEGIN
	SELECT sw.ID, sw.MODULEID, om.fundingbodyid 
    FROM sci_workflow sw, opportunity_master om  
    WHERE om.opportunityid=sw.id AND workflowid=p_WorkflowId;
END ;;