CREATE PROCEDURE `sci_proc_tasklist_new`(
	p_userid       	INTEGER,
	P_moduleid      INTEGER,
	P_taskid        INTEGER,
	P_CYCLE         INTEGER,
	P_ALLOCATION    INTEGER
)
BEGIN
	DECLARE lCount	INTEGER;
    
    SELECT COUNT(1) INTO lCount FROM userassignment WHERE userid = p_userid AND moduleid = p_moduleid; 
    
    IF P_taskid = 1
	THEN
		IF P_ALLOCATION = 0
		THEN
			IF lCount = 0 
            THEN
				SELECT modulename, moduleid, fundingbodyname, ID, taskname, taskid, CYCLE, DUEDATE FROM
					(SELECT modulename, ma.moduleid, fundingbodyname, ma.ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR(DUEDATE, 'DD-Mon-YYYY') DUEDATE, DUEDATE due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM sci_workflow sw 
						WHERE taskid = P_taskid AND moduleid = P_moduleid AND ASSIGNBY IS NULL AND completeddate IS NULL AND CYCLE = p_CYCLE 
						AND IFNULL(sw.STATUSID, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm
					WHERE fm.HIDDEN_FLAG = 0 and ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND IFNULL(fm.STATUSCODE, 0) <> 4 AND fm.fundingbody_id = ma.ID 
					ORDER BY DUE, fundingbodyname) table1
				WHERE ROWNUM < 2;
            ELSE
				SELECT modulename, moduleid, fundingbodyname, ID, taskname, taskid, CYCLE, DUEDATE FROM
					(SELECT modulename, ma.moduleid, fundingbodyname, ma.ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (DUEDATE, 'DD-Mon-YYYY') DUEDATE, DUEDATE due FROM
						(SELECT moduleid, ID, CYCLE, taskid FROM sci_workflow sw 
						WHERE taskid = P_taskid AND moduleid = P_moduleid AND ASSIGNBY IS NULL AND completeddate IS NULL AND CYCLE = p_CYCLE 
                        AND IFNULL(sw.STATUSID, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm  
					WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND IFNULL(fm.STATUSCODE, 0) <> 4 AND fm.fundingbody_id = ma.ID 
                    AND fm.fundingbody_id IN 
						(SELECT FUNDINGBODYID FROM userassignment WHERE userid = p_userid and moduleid = p_moduleid)
					ORDER BY DUE, fundingbodyname) table1
				WHERE ROWNUM < 2;
            END IF;
        ELSEIF P_ALLOCATION = 1
        THEN
			IF lCount = 0 
            THEN
				SELECT modulename, moduleid, fundingbodyname, ID, taskname, taskid, CYCLE, DUEDATE FROM
					(SELECT modulename, ma.moduleid, fundingbodyname, ma.ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (DUEDATE, 'DD-Mon-YYYY') DUEDATE, DUEDATE due FROM
						(SELECT moduleid, ID, CYCLE, taskid FROM sci_workflow sw 
						WHERE taskid = P_taskid AND moduleid = P_moduleid AND ASSIGNBY IS NULL AND completeddate IS NULL AND CYCLE = p_CYCLE
                        AND IFNULL(sw.STATUSID, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm 
					WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND IFNULL(fm.STATUSCODE, 0) <> 4 AND fm.fundingbody_id = ma.ID 
                    ORDER BY DUE, fundingbodyname) table1;
            ELSE
				SELECT modulename, moduleid, fundingbodyname, ID, taskname, taskid, CYCLE, DUEDATE FROM
					(SELECT modulename, ma.moduleid, fundingbodyname, ma.ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (DUEDATE, 'DD-Mon-YYYY') DUEDATE, DUEDATE due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM sci_workflow sw 
                        WHERE taskid = P_taskid AND moduleid = P_moduleid AND ASSIGNBY IS NULL AND completeddate IS NULL AND CYCLE = p_CYCLE 
                        AND IFNULL(sw.STATUSID, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm 
					WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND IFNULL(fm.STATUSCODE, 0) <> 4 AND fm.fundingbody_id = ma.ID
                    and fm.fundingbody_id IN 
						(SELECT FUNDINGBODYID FROM userassignment WHERE userid = p_userid and moduleid=p_moduleid)
					ORDER BY DUE, fundingbodyname) table1;
            END IF;
        END IF;
    ELSEIF P_taskid = 2
    THEN
		IF P_ALLOCATION = 0
		THEN
			IF lCount = 0 
            THEN
				SELECT modulename, moduleid, fundingbodyname, ID, taskname, taskid, CYCLE, DUEDATE FROM
					(SELECT modulename, ma.moduleid, fundingbodyname, ma.ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (DUEDATE, 'DD-Mon-YYYY') DUEDATE, DUEDATE due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM sci_workflow sw 
						WHERE taskid = P_taskid AND moduleid = P_moduleid AND ASSIGNBY IS NULL AND completeddate IS NULL AND CYCLE = 0 
                        AND IFNULL(sw.STATUSID, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm 
					WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND IFNULL(fm.STATUSCODE, 0) <> 4 AND fm.fundingbody_id = ma.ID 
                    ORDER BY due, fundingbodyname) table1
				WHERE ROWNUM < 2;
            ELSE
				SELECT modulename, moduleid, fundingbodyname, ID, taskname, taskid, CYCLE, DUEDATE FROM
					(SELECT modulename, ma.moduleid, fundingbodyname, ma.ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (DUEDATE, 'DD-Mon-YYYY') DUEDATE, DUEDATE due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM sci_workflow sw  
                        WHERE taskid = P_taskid AND moduleid = P_moduleid AND ASSIGNBY IS NULL AND completeddate IS NULL AND CYCLE = 0 
                        AND IFNULL(sw.STATUSID, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm 
                        WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND IFNULL(fm.STATUSCODE, 0) <> 4 AND fm.fundingbody_id = ma.ID 
                        and fm.fundingbody_id IN 
							(SELECT FUNDINGBODYID FROM userassignment WHERE userid = p_userid and moduleid = p_moduleid)
						ORDER BY   due, fundingbodyname) table1
					WHERE ROWNUM < 2;
            END IF;
        ELSEIF P_ALLOCATION = 1
        THEN
			IF lCount = 0 
            THEN
				SELECT modulename, moduleid, fundingbodyname, ID, taskname, taskid, CYCLE, DUEDATE FROM
					(SELECT modulename, ma.moduleid, fundingbodyname, ma.ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (DUEDATE, 'DD-Mon-YYYY') DUEDATE, DUEDATE due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM sci_workflow sw  
                        WHERE taskid = P_taskid AND moduleid = P_moduleid AND ASSIGNBY IS NULL AND completeddate IS NULL AND CYCLE = 0 
                        AND IFNULL(sw.STATUSID, 0) NOT IN  (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm  
					WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND IFNULL(fm.STATUSCODE, 0) <> 4 AND fm.fundingbody_id = ma.ID 
                    ORDER BY due, fundingbodyname) table1;
            ELSE
				SELECT modulename, moduleid, fundingbodyname, ID, taskname, taskid, CYCLE, DUEDATE FROM
					(SELECT modulename, ma.moduleid, fundingbodyname, ma.ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (DUEDATE, 'DD-Mon-YYYY') DUEDATE, DUEDATE due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM sci_workflow sw 
						WHERE taskid = P_taskid AND moduleid = P_moduleid AND ASSIGNBY IS NULL AND completeddate IS NULL AND CYCLE = 0 AND 
                        IFNULL(sw.STATUSID, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm 
                        WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND IFNULL(fm.STATUSCODE, 0) <> 4 AND fm.fundingbody_id = ma.ID 
                        and fm.fundingbody_id IN 
							(SELECT FUNDINGBODYID FROM userassignment WHERE userid = p_userid and moduleid=p_moduleid)
						ORDER BY due, fundingbodyname) table1;
            END IF;
        END IF;
	ELSE
		IF P_ALLOCATION = 0
		THEN
			IF lCount = 0 
            THEN
				SELECT modulename, ma.moduleid, fundingbodyname, ma.ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (DUEDATE, 'DD-Mon-YYYY') DUEDATE, DUEDATE due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM sci_workflow sw  
                        WHERE taskid = P_taskid AND moduleid = P_moduleid AND ASSIGNBY IS NULL AND completeddate IS NULL 
                        AND IFNULL(sw.STATUSID, 0) NOT IN (7, 9) AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm  
				WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND fm.fundingbody_id = ma.ID AND ROWNUM < 2;
            ELSE
				SELECT modulename, ma.moduleid, fundingbodyname, ma.ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (DUEDATE, 'DD-Mon-YYYY') DUEDATE, DUEDATE due FROM 
						(SELECT moduleid, ID, CYCLE, taskid FROM sci_workflow sw 
                        WHERE taskid = P_taskid AND moduleid = P_moduleid AND ASSIGNBY IS NULL AND completeddate IS NULL AND IFNULL(sw.STATUSID, 0) NOT IN (7, 9) 
                        AND NOT EXISTS 
							(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
						) ma, sci_modules sm, sci_tasks st, fundingbody_master fm 
					WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND fm.fundingbody_id = ma.ID and fm.fundingbody_id IN 
						(SELECT FUNDINGBODYID FROM userassignment WHERE userid = p_userid and moduleid=p_moduleid)
                     AND ROWNUM < 2;
            END IF;
        ELSEIF P_ALLOCATION = 1
        THEN
			IF lCount = 0 
            THEN
				SELECT modulename, ma.moduleid, fundingbodyname, ma.ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (DUEDATE, 'DD-Mon-YYYY') DUEDATE, DUEDATE due FROM 
					(SELECT moduleid, ID, CYCLE, taskid FROM sci_workflow sw 
                    WHERE taskid = P_taskid AND moduleid = P_moduleid AND ASSIGNBY IS NULL AND completeddate IS NULL AND IFNULL(sw.STATUSID, 0) NOT IN (7, 9) 
                    AND NOT EXISTS
						(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
					) ma, sci_modules sm, sci_tasks st, fundingbody_master fm 
				WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND fm.fundingbody_id = ma.ID;
            ELSE
				SELECT modulename, ma.moduleid, fundingbodyname, ma.ID, taskname, ma.taskid, ma.CYCLE, TO_CHAR (DUEDATE, 'DD-Mon-YYYY') DUEDATE, DUEDATE due FROM 
					(SELECT moduleid, ID, CYCLE, taskid FROM sci_workflow sw 
                    WHERE taskid = P_taskid AND moduleid = P_moduleid AND ASSIGNBY IS NULL AND completeddate IS NULL AND IFNULL(sw.STATUSID, 0) NOT IN (7, 9)
                    AND NOT EXISTS
						(SELECT 1 FROM sci_workflow WHERE ID = sw.ID AND CYCLE = sw.CYCLE AND SEQUENCE < sw.SEQUENCE AND completeddate IS NULL)
					) ma, sci_modules sm, sci_tasks st, fundingbody_master fm
				WHERE HIDDEN_FLAG = 0 AND ma.taskid = st.taskid AND ma.moduleid = sm.moduleid AND fm.fundingbody_id = ma.ID and fm.fundingbody_id IN 
					(SELECT FUNDINGBODYID FROM userassignment WHERE userid = p_userid and moduleid=p_moduleid);
            END IF;
        END IF;
    END IF;
END;
