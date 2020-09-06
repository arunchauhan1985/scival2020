CREATE DEFINER=`root`@`localhost` PROCEDURE `update_award_title`(
   IN p_trans_id       INTEGER,
   IN p_title          VARCHAR(4000)
   )
BEGIN
   DECLARE v_count   INTEGER;
   -- DECLARE excp EXCEPTION;
 
   
   SELECT   COUNT(*)
     INTO   v_count
     FROM   sci_language_detail
    WHERE   tran_id = p_trans_id;

   
   UPDATE   sci_language_detail
      SET   column_desc = p_title
    WHERE   tran_id = p_trans_id;
END