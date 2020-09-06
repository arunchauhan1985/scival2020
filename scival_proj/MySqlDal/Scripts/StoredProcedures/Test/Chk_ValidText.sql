DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `Chk_ValidText`(
  p_Text LONGTEXT,
  p_TextN LONGTEXT
  )
BEGIN
   DECLARE v_text   LONGTEXT;
   DECLARE v_textN   LONGTEXT;
   SET v_text=p_Text;   
        Select v_text as Text  from  dual;
END$$
DELIMITER ;
