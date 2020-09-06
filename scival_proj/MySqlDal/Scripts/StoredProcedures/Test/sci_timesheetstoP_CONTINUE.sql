CREATE PROCEDURE `sci_timesheetstoP_CONTINUE`(
p_workflowid           INTEGER,
   p_userid               INTEGER,
   p_transitionalid       INTEGER,
   p_remarks              VARCHAR(4000)
)
BEGIN
	DECLARE V_mode                INTEGER  DEFAULT  8;
	DECLARE v_check               INTEGER;
	DECLARE v_moduleid            INTEGER;
	DECLARE v_id                  INTEGER;
	DECLARE v_cycle               INTEGER;
	DECLARE v_sequence            INTEGER;
	DECLARE vm_sequence           INTEGER;
	DECLARE v_taskid              INTEGER;
	DECLARE x_revisionhistoryid   INTEGER;
	DECLARE xx_count              INTEGER;
	DECLARE v_count               INTEGER;
	DECLARE v_transitionalid      INTEGER  DEFAULT  0;
	DECLARE v_awardid             INTEGER;
	DECLARE v_opportunityid       INTEGER;
	DECLARE x_WORKFLOWID1      INTEGER ;
	DECLARE P_O_STATUS1 INTEGER ;
	DECLARE P_O_ERROR1 VARCHAR(500) ;
	DECLARE v_fb integer;

    SELECT   moduleid,
            ID,
            CYCLE,
            SEQUENCE,
            taskid
     INTO   v_moduleid,
            v_id,
            v_cycle,
            v_sequence,
            v_taskid
     FROM   sci_workflow
    WHERE   workflowid = p_workflowid;
    
    if  v_moduleid=4  then
    select FUNDINGBODY_ID into v_fb from award where AWARD_ID=v_id ;
   end if;
   
   if  v_moduleid=3  then
    select FUNDINGBODY_ID into v_fb from OPPORTUNITY where OPPORTUNITY_ID=v_id ;
    end if;
    
    CALL sci_timesheetstop (p_workflowid,
                      p_userid,
                      p_transitionalid,
                      8,
                      p_remarks,
                      p_o_status,
                      p_o_error);
   
    
   
   IF v_moduleid = 4
   THEN
      CALL SCI_AW_AWNEW (v_fb,
                    p_userid,
                    x_WFLOWID,
                    P_O_STATUS1,
                    P_O_ERROR1);
   END IF;

   IF v_moduleid = 3
   THEN
      CALL SCI_OP_OPNEW (v_fb,
                    p_userid,
                    x_WFLOWID,
                    P_O_STATUS1,
                    P_O_ERROR1);
   END IF;

   COMMIT;
END;
