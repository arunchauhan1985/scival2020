CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_GetLanguageDetails`(
	p_langLength INTEGER
)
BEGIN

	SELECT * FROM sci_language_master 
    WHERE code_length = p_langLength  
    ORDER BY LANGUAGE_NAME;

END ;;