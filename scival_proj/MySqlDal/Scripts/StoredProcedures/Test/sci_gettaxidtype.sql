CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_gettaxidtype`()
BEGIN

   SELECT   * FROM SCI_TAXIDTYPETYPE;
   
END ;;