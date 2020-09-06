CREATE PROCEDURE `sci_proc_dummy_tasklist`(
	IN pUserId		BIGINT,
    IN pModuleId	BIGINT,
    IN pTaskId		BIGINT,
    IN pAllocation	BIGINT,
    IN pCycle		BIGINT
)
BEGIN
	DECLARE	userAssignmentCount INT DEFAULT 0;    
    SELECT COUNT(1) INTO userAssignmentCount FROM userassignment WHERE userid = pUserId AND moduleid = pModuleId;
    
    IF pModuleId = 2 AND pAllocation != 0
    THEN
		IF userAssignmentCount = 0
        THEN
			SELECT DISTINCT modulename, ma.moduleid, fundingbodyname, ma.id, taskname, ma.taskid, ma.cycle, duedate
            FROM (
				SELECT moduleid, id, cycle, taskid FROM sci_workflow sw 
				WHERE taskid = pTaskId and moduleid = pModuleId AND assignby IS NULL and completeddate IS NULL
                AND CYCLE != 0 AND IFNULL(sw.statusid, 0) NOT IN (7, 9)
                AND NOT EXISTS (
					SELECT 1 FROM sci_workflow WHERE id = sw.id AND cycle = sw.cycle 
					AND sequence < sw.sequence AND completeddate IS NULL)) ma,
			sci_modules sm, sci_tasks st, fundingbody_master fm
            WHERE hidden_flag = 0 AND ma.taskid = st.taskid and ma.moduleid = sm.moduleid
            AND fm.fundingbody_id = ma.id
            ORDER BY duedate, fundingbodyname;
        ELSE
			SELECT COUNT(1) INTO userAssignmentCount FROM userassignment WHERE userid = pUserId AND moduleid = pModuleId;
        END IF;
    END IF;
    
    IF pModuleId = 3 AND pAllocation != 0
    THEN
		IF userAssignmentCount = 0
        THEN
			SELECT COUNT(1) INTO userAssignmentCount FROM userassignment WHERE userid = pUserId AND moduleid = pModuleId;
        ELSE
			SELECT COUNT(1) INTO userAssignmentCount FROM userassignment WHERE userid = pUserId AND moduleid = pModuleId;
        END IF;
    END IF;
    
    IF pModuleId = 4 AND pAllocation != 0
    THEN
		IF userAssignmentCount = 0
        THEN
			SELECT COUNT(1) INTO userAssignmentCount FROM userassignment WHERE userid = pUserId AND moduleid = pModuleId;
        ELSE
			SELECT COUNT(1) INTO userAssignmentCount FROM userassignment WHERE userid = pUserId AND moduleid = pModuleId;
        END IF;
    END IF;
END;
