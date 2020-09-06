CREATE PROCEDURE `sci_opaw_tasklist`(
	IN pUserId		BIGINT,
    IN pModuleId	BIGINT,
    IN pTaskId		BIGINT,
    IN pAllocation	BIGINT
)
BEGIN
	DECLARE	userAssignmentCount INT DEFAULT 0;    
    SELECT COUNT(1) INTO userAssignmentCount FROM userassignment WHERE userid = pUserId AND moduleid = pModuleId;

	IF pAllocation = 1 AND pModuleId = 4
    THEN
		IF userAssignmentCount = 0
        THEN
			SELECT DISTINCT fm.fundingbody_id, fm.fundingbodyname, fm.duedate
            FROM ( 
				SELECT moduleid, id, cycle, taskid FROM sci_workflow sw
				WHERE taskid = pTaskId AND moduleid = pModuleId AND assignby IS NULL
                AND completeddate IS NULL AND cycle = 0 AND IFNULL(sw.statusid, 0) NOT IN(7, 9, 11)
                AND NOT EXISTS(
					SELECT 1 FROM sci_workflow 
                    WHERE id = sw.id AND cycle = sq.cycle AND sequence < sw.sequence AND completeddate IS NULL
			)) ma,
            sci_modules sm, sci_tasks st, fundingbody_master fm, award aw
            WHERE ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND IFNULL(fm.statuscode, 0) != 4
            AND fm.fundingbody_id = aw.fundingbody_id AND hidden_flag = 0 AND aw.award_id = ma.id
            ORDER BY duedate, fundingbodyname;
		ELSE
			SELECT DISTINCT fm.fundingbody_id, fm.fundingbodyname, fm.duedate
            FROM fundingbody_master fm, award_master am, userassignment ua
            WHERE fm.fundingbody_id = am.fundingbodyid AND hidden_flag = 0 AND aw.awardid = ua.awardid
            AND ua.userid = pUserId AND aw.cycle = 0 AND aw.taskid = pTaskId
            ORDER BY duedate, fundingbodyname;
        END IF;
	ELSEIF pAllocation = 1 AND pModuleId = 3
    THEN
		IF userAssignmentCount = 0
        THEN
			SELECT DISTINCT fm.fundingbody_id, fm.fundingbodyname, fm.duedate
            FROM ( 
				SELECT moduleid, id, cycle, taskid FROM sci_workflow sw
				WHERE taskid = pTaskId AND moduleid = pModuleId AND assignby IS NULL
                AND completeddate IS NULL AND cycle = 0 AND IFNULL(sw.statusid, 0) NOT IN (7, 9)
                AND NOT EXISTS(
					SELECT 1 FROM sci_workflow 
                    WHERE id = sw.id AND cycle = sq.cycle AND sequence < sw.sequence AND completeddate IS NULL
			)) ma,
            sci_modules sm, sci_tasks st, fundingbody_master fm, opportunity op
            WHERE ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND IFNULL(fm.statuscode, 0) != 4
            AND fm.fundingbody_id = op.fundingbody_id AND hidden_flag = 0 AND op.opportunity_id = ma.id
            ORDER BY duedate, fundingbodyname;
		ELSE
			SELECT DISTINCT fm.fundingbody_id, fm.fundingbodyname, fm.duedate
            FROM fundingbody_master fm, opportunity_master om, userassignment ua
            WHERE fm.fundingbody_id = om.fundingbodyid AND hidden_flag = 0 AND om.opportunityid = ua.awardid
            AND ua.userid = pUserId AND aw.cycle = 0 AND aw.taskid = pTaskId
            ORDER BY duedate, fundingbodyname;
		END IF;    
    END IF;
END;
