CREATE DEFINER=`root`@`localhost` PROCEDURE `SCI_PROC_AWARDBASE`(
p_workflowid  INTEGER
)
begin
   declare V_ID         integer;
   declare V_MODULEID   integer;
  
   SELECT   ID, MODULEID INTO   V_ID, V_MODULEID FROM   SCI_WORKFLOW WHERE   WORKFLOWID = P_WORKFLOWID;
   IF v_moduleid = 4
   THEN
         SELECT *  FROM   AWARD WHERE   AWARD_ID = v_ID;	   
   END IF;

END