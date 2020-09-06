CREATE PROCEDURE `opprotunity_loi_status`(P_WORKFLOWID                 INTEGER)
BEGIN
   DECLARE v_cnt           INTEGER;
   DECLARE V_MODULEID      INTEGER;
   DECLARE V_ID            INTEGER;
   DECLARE V_CYCLE         INTEGER;
   DECLARE l_status        VARCHAR (500);
   DECLARE l_status1       VARCHAR (500);
   DECLARE l_status2       VARCHAR (500);
   DECLARE l_name_status   VARCHAR (1000);
 
   SELECT   MODULEID, ID, CYCLE
     INTO   V_MODULEID, V_ID, V_CYCLE
     FROM   SCI_WORKFLOW
    WHERE   WORKFLOWID = P_WORKFLOWID;

   SELECT   LOI_MANDATORY
     INTO   l_status
     FROM   opportunity
    WHERE   OPPORTUNITY_ID = V_ID;

   SELECT   REPEATINGOPPORTUNITY
     INTO   l_status1
     FROM   opportunity
    WHERE   OPPORTUNITY_ID = V_ID;


   SELECT   PREPROPOSALMANDATORY
     INTO   l_status2
     FROM   opportunity
    WHERE   OPPORTUNITY_ID = V_ID;
    
    SELECT l_status, l_status1, l_status1;

END;
