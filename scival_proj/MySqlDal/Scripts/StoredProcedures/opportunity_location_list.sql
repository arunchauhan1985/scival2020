CREATE DEFINER=`root`@`localhost` PROCEDURE `opportunity_location_list`(
	p_workflowid       INTEGER  
)
BEGIN
	DECLARE v_opportunity_id   INTEGER;
	DECLARE v_moduleid         INTEGER;
 
	SELECT   ID, moduleid INTO   v_opportunity_id, v_moduleid FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;
   
	SELECT   a.location_id,
               a.country countrycode,
               cc.name countryname,
               a.room,
               a.street,
               a.city,
               ifnull (a.state, a.state) statecode,
               ifnull (sc.name, a.state) statename,
               a.postalcode,
               opportunity_id
	FROM   opportunity_location a LEFT JOIN SCI_STATECODES sc
    ON sc.code = a.STATE
    LEFT JOIN SCI_COUNTRYCODES cc
    ON cc.LCODE = a.COUNTRY
	WHERE a.opportunity_id = v_opportunity_id;
   
END ;;