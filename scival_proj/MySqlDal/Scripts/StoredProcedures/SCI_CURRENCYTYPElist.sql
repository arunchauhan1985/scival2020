CREATE DEFINER=`root`@`localhost` PROCEDURE `SCI_CURRENCYTYPElist`(
)
BEGIN   
   
        SELECT   *
          FROM   SCI_CURRENCYTYPE
      ORDER BY   2;
   
END ;;