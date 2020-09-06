CREATE DEFINER=`root`@`localhost` PROCEDURE `update_fbdetails_multilanguage`(

   IN p_trans_id       INTEGER,
   IN p_languagecode     VARCHAR(4000),
   IN p_title          VARCHAR(4000))
BEGIN
	DECLARE v_count   INTEGER;
	DECLARE v_languageid integer;
	   
	SELECT   COUNT(*) INTO   v_count FROM   sci_language_detail WHERE   tran_id = p_trans_id;

	SELECT language_id INTO v_languageid from sci_language_master where LANGUAGE_CODE=p_languagecode;

	UPDATE   sci_language_detail SET   column_desc = p_title, language_id = IFNULL (v_languageid, language_id)    
	WHERE   tran_id = p_trans_id;
END ;;
