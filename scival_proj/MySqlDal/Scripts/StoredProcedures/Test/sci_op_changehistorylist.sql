CREATE PROCEDURE `sci_op_changehistorylist`(
	p_workflowid       INTEGER
)
BEGIN
	DECLARE v_id                 INTEGER;
	DECLARE v_moduleid           INTEGER;
	DECLARE v_CHANGEHISTORY_ID   INTEGER;
	DECLARE l_status_code        INTEGER;
	DECLARE v_count              INTEGER;
    
    SELECT   id, moduleid INTO  v_id, v_moduleid FROM   sci_workflow WHERE   workflowid = p_workflowid;

	SELECT   IFNULL(STATUSCODE, 0) INTO   l_status_code FROM   opportunity_master WHERE   OPPORTUNITYID = v_id;
    
    IF l_status_code = 1
	THEN
		UPDATE   createddate SET   CREATEDDATE_TEXT = SYSDATE() WHERE REVISIONHISTORY_ID=(select max(REVISIONHISTORY_ID) from revisionhistory   where  OPPORTUNITY_ID = v_id);
	END IF;
    
    IF v_moduleid = 3
	THEN
		SELECT   COUNT(CHANGEHISTORY_ID) INTO   v_count FROM   changehistory WHERE   OPPORTUNITY_ID = v_id;

		IF v_count = 0
		THEN
			SET v_CHANGEHISTORY_ID = NULL;
		ELSE
			SELECT   CHANGEHISTORY_ID 
			INTO   v_CHANGEHISTORY_ID
			FROM   changehistory
			WHERE   OPPORTUNITY_ID = v_id;
		END IF;

        SELECT  *
		FROM   `CHANGE` WHERE   CHANGEHISTORY_ID = v_CHANGEHISTORY_ID AND version = (SELECT  MIN(version) FROM   `CHANGE` WHERE   CHANGEHISTORY_ID = v_CHANGEHISTORY_ID)
		UNION
		SELECT *
        FROM   `CHANGE` WHERE   CHANGEHISTORY_ID = v_CHANGEHISTORY_ID AND version = (SELECT   MAX(version) FROM   `CHANGE` WHERE   CHANGEHISTORY_ID = v_CHANGEHISTORY_ID);

		SELECT   POSTDATE_IS_SAVE 
		FROM   `CHANGE` WHERE   CHANGEHISTORY_ID = v_CHANGEHISTORY_ID AND version = (SELECT   MAX(version) FROM   `CHANGE` WHERE   CHANGEHISTORY_ID = v_CHANGEHISTORY_ID);
   END IF;
END;
