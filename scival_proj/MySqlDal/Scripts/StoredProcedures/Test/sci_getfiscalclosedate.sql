CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_getfiscalclosedate`(
   IN p_workflowid            integer
)
BEGIN
   DECLARE v_FUNDINGBODY_ID   integer;
   DECLARE v_moduleid         integer;
    
   SELECT   moduleid, id
     INTO   v_moduleid, v_FUNDINGBODY_ID
     FROM   sci_workflow
    WHERE   workflowid = p_workflowid;

   IF v_moduleid = 2
   THEN
 
         SELECT   *
           FROM   fiscalclosedate
          WHERE   FINANCIALINFO_ID IN
                        (SELECT   FINANCIALINFO_ID
                           FROM   FINANCIALINFO
                          WHERE   FUNDINGBODY_ID = v_FUNDINGBODY_ID);
      
   END IF;
END ;;