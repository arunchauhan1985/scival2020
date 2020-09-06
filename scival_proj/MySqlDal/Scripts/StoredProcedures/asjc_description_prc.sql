CREATE DEFINER=`root`@`localhost` PROCEDURE `asjc_description_prc`(
		
)
BEGIN
	DECLARE v_amount   INTEGER;
 
	SELECT   code,Detail as description, code as sub_level_code,description as sub_level_description  
	FROM SCI_ASJCDESCRIPTION 
       
	ORDER BY code;
   
END ;;