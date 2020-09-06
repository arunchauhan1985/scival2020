CREATE PROCEDURE `dummy_tasklist_new`(
	pUserId     INT,
	pModuleId	INT,
	pTaskId		INT,
	pCycle		INT,
	pAllocation	INT
)
BEGIN
	DECLARE lUserAssignmentCount	INT;
    
    SELECT COUNT(1) INTO lUserAssignmentCount FROM userassignment WHERE userid = pUserId AND moduleid = pModuleId;
    
    IF pModuleId = 2
    THEN
		IF pAllocation = 0
        THEN
			IF lUserAssignmentCount = 0
            THEN
				SELECT * FROM 
					(SELECT   modulename, ma.moduleid, fundingbodyname, ma.ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (duedate, 'DD-Mon-YYYY') duedate, duedate due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM 
						sci_workflow sw WHERE taskid = pTaskId AND moduleid = pModuleId AND assignby IS NULL AND completeddate IS NULL AND CYCLE <> 0 
						AND IFNULL(sw.statusid, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma,  sci_modules sm, sci_tasks st, fundingbody_master fm  
						WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND fm.fundingbody_id = ma.ID ORDER BY due, fundingbodyname) abc
					WHERE ROWNUM < 2;
            ELSE
				SELECT * FROM 
					(SELECT modulename, ma.moduleid, fundingbodyname, ma.ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (duedate, 'DD-Mon-YYYY') duedate, duedate due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM 
						sci_workflow sw WHERE taskid = pTaskId AND moduleid = pModuleId AND assignby IS NULL AND completeddate IS NULL AND CYCLE <> 0 
						AND IFNULL(sw.statusid, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm 
						WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND fm.fundingbody_id = ma.ID and fm.fundingbody_id IN 
							(SELECT FUNDINGBODYID FROM userassignment WHERE userid = pUserId and moduleid=pModuleId)
						ORDER BY due, fundingbodyname) abc;
            END IF;
        ELSE
			IF lUserAssignmentCount = 0
            THEN
				SELECT * FROM 
					(SELECT modulename, ma.moduleid, fundingbodyname, ma.ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (duedate, 'DD-Mon-YYYY' ) duedate, duedate due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM 
						sci_workflow sw WHERE taskid = p_taskid AND moduleid = p_moduleid AND assignby IS NULL AND completeddate IS NULL AND CYCLE <> 0
						AND IFNULL(sw.statusid, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm 
						WHERE HIDDEN_FLAG = 0 AND  ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND fm.fundingbody_id = ma.ID ORDER BY due, fundingbodyname) abc;
            ELSE
				SELECT * FROM 
					(SELECT modulename, ma.moduleid, fundingbodyname, ma.ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (duedate, 'DD-Mon-YYYY') duedate, duedate due FROM 
						(SELECT moduleid, ID, CYCLE, taskid 
						FROM sci_workflow sw WHERE taskid = p_taskid AND moduleid = p_moduleid AND assignby IS NULL AND completeddate IS NULL AND CYCLE <> 0 
						AND IFNULL(sw.statusid, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm 
						WHERE HIDDEN_FLAG = 0 AND  ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND fm.fundingbody_id = ma.ID and FM.fundingbody_id IN 
							(SELECT FUNDINGBODYID FROM userassignment WHERE userid = p_userid and moduleid = p_moduleid) 
						ORDER BY due, fundingbodyname) abc;
            END IF;
        END IF;
    END IF;
    
    IF pModuleId = 3
    THEN
		IF pAllocation = 0
        THEN
			IF lUserAssignmentCount = 0
            THEN
				SELECT * FROM 
					(SELECT modulename, ma.moduleid, fundingbodyname, fm.fundingbody_id ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (duedate, 'DD-Mon-YYYY') duedate, duedate due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM 
						sci_workflow sw WHERE taskid = 2 AND moduleid = 3 AND assignby IS NULL AND completeddate IS NULL AND CYCLE <> 0
						AND IFNULL(sw.statusid, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm, opportunity_master om 
						WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND fm.fundingbody_id = om.fundingbodyid AND om.opportunityid = ma.ID
						ORDER BY due, fundingbodyname) abc
				WHERE  ROWNUM < 2;
            ELSE
				SELECT * FROM 
					(SELECT modulename, ma.moduleid, fundingbodyname, fm.fundingbody_id ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (duedate, 'DD-Mon-YYYY') duedate, duedate due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM 
						sci_workflow sw WHERE taskid = 2 AND moduleid = 3 AND assignby IS NULL AND completeddate IS NULL AND CYCLE <> 0
						AND IFNULL(sw.statusid, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm, opportunity_master om 
						WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND fm.fundingbody_id = om.fundingbodyid AND om.opportunityid = ma.ID
						AND fm.fundingbody_id IN 
							(SELECT FUNDINGBODYID FROM userassignment WHERE userid = pUserId and moduleid = pModuleId)
						ORDER BY due, fundingbodyname) abc
					WHERE  ROWNUM < 2;
            END IF;
        ELSE
			IF lUserAssignmentCount = 0
			THEN
				SELECT * FROM 
					(SELECT modulename, ma.moduleid, fundingbodyname, fm.fundingbody_id ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (duedate, 'DD-Mon-YYYY') duedate, duedate due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM 
						sci_workflow sw WHERE taskid = 2 AND moduleid = 3 AND assignby IS NULL AND completeddate IS NULL AND CYCLE <> 0 
						AND IFNULL(sw.statusid, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm, opportunity_master om
						WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND fm.fundingbody_id = om.fundingbodyid AND om.opportunityid = ma.ID
						ORDER BY due, fundingbodyname) abc;
			ELSE
				SELECT * FROM 
					(SELECT modulename, ma.moduleid, fundingbodyname, fm.fundingbody_id ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (duedate, 'DD-Mon-YYYY') duedate, duedate due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM 
						sci_workflow sw WHERE taskid = 2 AND moduleid = 3 AND assignby IS NULL AND completeddate IS NULL AND CYCLE <> 0 
						AND IFNULL(sw.statusid, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm, opportunity_master om 
						WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND fm.fundingbody_id = om.fundingbodyid AND om.opportunityid = ma.ID
						AND om.opportunityid IN 
							(SELECT OPPORTUNITYID FROM userassignment WHERE userid = pUserId AND moduleid = pModuleId) 
						ORDER BY due, fundingbodyname) abc;
			END IF;
        END if;
    END IF;
    
    IF pModuleId = 4
    THEN
		IF pAllocation = 0
        THEN
			IF lUserAssignmentCount = 0
            THEN
				SELECT * FROM 
					(SELECT modulename, ma.moduleid, fundingbodyname, fm.fundingbody_id ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR(duedate, 'DD-Mon-YYYY') duedate, duedate due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM 
						sci_workflow sw WHERE taskid = 2 AND moduleid = 4 AND assignby IS NULL AND completeddate IS NULL AND CYCLE <> 0 
						AND IFNULL(sw.statusid, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm, award_master om
					WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND fm.fundingbody_id = om.fundingbodyid AND om.awardid = ma.ID 
					ORDER BY due, fundingbodyname) abc
				WHERE  ROWNUM < 2;
            ELSE
				SELECT * FROM 
					(SELECT modulename, ma.moduleid, fundingbodyname, fm.fundingbody_id ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR(duedate, 'DD-Mon-YYYY') duedate, duedate due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM 
						sci_workflow sw WHERE taskid = 2 AND moduleid = 4 AND assignby IS NULL AND completeddate IS NULL AND CYCLE <> 0 
						AND IFNULL(sw.statusid, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm, award_master om  
						WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND fm.fundingbody_id = om.fundingbodyid AND om.awardid = ma.ID AND fm.fundingbody_id IN 
							(SELECT FUNDINGBODYID FROM userassignment WHERE userid = pUserId AND moduleid = pModuleId)
						ORDER BY due, fundingbodyname) abc
				WHERE  ROWNUM < 2;
            END IF;
        ELSE
			IF lUserAssignmentCount = 0
			THEN
				SELECT * FROM 
					(SELECT modulename, ma.moduleid, fundingbodyname, fm.fundingbody_id ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR(duedate, 'DD-Mon-YYYY') duedate, duedate due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM 
						sci_workflow sw WHERE taskid = 2 AND moduleid = 4 AND assignby IS NULL AND completeddate IS NULL AND CYCLE <> 0 
						AND IFNULL(sw.statusid, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm, award_master om  
						WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND fm.fundingbody_id = om.fundingbodyid and not exists 
							(select 1 from userassignment where moduleid = 4 and awardid = om.awardid)
						AND om.awardid = ma.ID 
						ORDER BY due, fundingbodyname) abc;
			ELSE
				SELECT * FROM 
					(SELECT modulename, ma.moduleid, fundingbodyname, fm.fundingbody_id ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR(duedate, 'DD-Mon-YYYY') duedate, duedate due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM 
						sci_workflow sw WHERE taskid = 2 AND moduleid = 4 AND assignby IS NULL AND completeddate IS NULL AND CYCLE <> 0 
						AND IFNULL(sw.statusid, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm, award_master om  
					WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND fm.fundingbody_id = om.fundingbodyid AND om.awardid = ma.ID and om.awardid IN 
						(SELECT awardid FROM userassignment WHERE userid = pUserId and moduleid = pModuleId) ORDER BY due, fundingbodyname) abc;
			END IF;
        END if;
    END IF;
END;
