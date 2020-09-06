CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_AWARDSTATISTICSlist`(
	p_workflowid integer
)
BEGIN
   DECLARE v_fundingbodyid   integer;
    
SELECT 
    ID
INTO v_fundingbodyid FROM
    sci_workflow
WHERE
    WORKFLOWID = p_workflowid;
   
SELECT 
    ast.AWARDSTATISTICS_ID,
    ast.FUNDINGBODY_ID,
    tf.CURRENCY,
    tf.TOTALFUNDING_TEXT,
    l.URL,
    l.LINK_TEXT
FROM
    AWARDSTATISTICS ast
        LEFT JOIN
    TOTALFUNDING tf ON ast.AWARDSTATISTICS_ID = tf.AWARDSTATISTICS_ID
        LEFT JOIN
    link l ON ast.AWARDSTATISTICS_ID = l.AWARDSTATISTICS_ID
WHERE
    ast.FUNDINGBODY_ID = v_fundingbodyid;
   
END ;;
