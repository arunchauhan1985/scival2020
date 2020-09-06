CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_establishmentlist`(
   p_workflowid       integer
)
BEGIN
   DECLARE v_fundingbodyid   integer;
    
   SELECT   ID
     INTO   v_fundingbodyid
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;
   
      SELECT   COUNTRYID,
               NAME,
               LOWER (LCODE) LCODE,
               LOWER (SCODE) SCODE
        FROM   sci_countrycodes;
   
      SELECT   sc.lcode countrycode, ss.NAME, ss.code
        FROM   sci_countrycodes sc, sci_statecodes ss
       WHERE   sc.countryid = ss.countryid;
   
      SELECT   *
        FROM   establishmentinfo
       WHERE   FUNDINGBODY_ID = v_fundingbodyid;

END ;;