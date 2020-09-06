DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `award_location_list`(
   p_workflowid       INTEGER
)
BEGIN
   DECLARE v_award_id   INTEGER;
   DECLARE v_moduleid    INTEGER;    
   SELECT   ID, moduleid INTO   v_award_id, v_moduleid FROM   sci_workflow WHERE   WORKFLOWID = p_workflowid;

   SELECT   a.location_id,a.countrytest, a.country countrycode,cc.name countryname,a.room,a.street,
			a.city,ifnull (a.state, a.state) statecode, ifnull (sc.name, a.state) statename,a.postalcode,award_id
	FROM   award_location a, SCI_COUNTRYCODES cc, SCI_STATECODES sc WHERE  sc.code = a.STATE
           AND cc.LCODE = a.COUNTRY AND a.award_id = v_award_id;   
END$$
DELIMITER ;
