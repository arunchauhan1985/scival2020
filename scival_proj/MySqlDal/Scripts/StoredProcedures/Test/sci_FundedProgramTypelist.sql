CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_FundedProgramTypelist`(
   p_workflowid       integer
)
BEGIN
   DECLARE v_fundingbodyid   integer;
    
   SELECT   ID
     INTO   v_fundingbodyid
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;

   SELECT   * FROM sci_fundedprogramstypelist;

      SELECT   FUNDINGBODY_ID,
               fps.FUNDEDPROGRAMSTYPES_ID,
               ID,
               FUNDEDPROGRAMSTYPE_TEXT
        FROM   FUNDEDPROGRAMSTYPES fps, FUNDEDPROGRAMSTYPE fp
       WHERE   fp.FUNDEDPROGRAMSTYPES_ID = fps.FUNDEDPROGRAMSTYPES_ID
               AND fps.FUNDINGBODY_ID = v_fundingbodyid;
   
END ;;