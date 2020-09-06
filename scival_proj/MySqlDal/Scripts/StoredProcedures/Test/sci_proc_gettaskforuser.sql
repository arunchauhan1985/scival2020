CREATE PROCEDURE `sci_proc_gettaskforuser`(
	IN USER_ID BIGINT
)
BEGIN
	SELECT MODULENAME, MA.MODULEID, FUNDINGBODYNAME, MA.ID, TASKNAME, MA.TASKID, ASSIGNBY, ASSIGNDATE, MA.CYCLE,
    WORKFLOWID, STATUSID, NULL AS OPPORTUNITYNAME, NULL AS OPURL
    FROM 
    (SELECT MODULEID, ID, CYCLE, TASKID, ASSIGNBY, ASSIGNDATE, WORKFLOWID, STATUSID
	FROM sci_workflow WHERE STARTBY = USER_ID AND COMPLETEDDATE IS NULL AND STATUSID IN (7, 9)) MA, 
    sci_modules SM, sci_tasks ST, fundingbody_master FM
    WHERE
    MA.TASKID = ST.TASKID AND MA.MODULEID = SM.MODULEID AND MA.MODULEID = 2
    AND IFNULL(FM.STATUSCODE, 0) <> 4 AND FM.FUNDINGBODY_ID = MA.ID
    UNION ALL
    SELECT MODULENAME, MA.MODULEID, FUNDINGBODYNAME, MA.ID, TASKNAME, MA.TASKID, ASSIGNBY, ASSIGNDATE, MA.CYCLE,
    WORKFLOWID, STATUSID, OPPORTUNITYNAME, OP.URL AS OPURL
    FROM 
    (SELECT MODULEID, ID, CYCLE, TASKID, ASSIGNBY, ASSIGNDATE, WORKFLOWID, STATUSID
	FROM sci_workflow WHERE STARTBY = USER_ID AND COMPLETEDDATE IS NULL AND STATUSID IN (7, 9)) MA, 
    sci_modules SM, sci_tasks ST, fundingbody_master FM, opportunity_master OP
    WHERE
    MA.TASKID = ST.TASKID AND MA.MODULEID = SM.MODULEID AND MA.MODULEID = 3
    AND IFNULL(FM.STATUSCODE, 0) <> 4 AND FM.FUNDINGBODY_ID = OP.FUNDINGBODYID AND OPPORTUNITYID = MA.ID
    UNION ALL
    SELECT MODULENAME, MA.MODULEID, FUNDINGBODYNAME, MA.ID, TASKNAME, MA.TASKID, ASSIGNBY, ASSIGNDATE, MA.CYCLE,
    WORKFLOWID, STATUSID, AWARDNAME, AM.URL AS OPURL
    FROM 
    (SELECT MODULEID, ID, CYCLE, TASKID, ASSIGNBY, ASSIGNDATE, WORKFLOWID, STATUSID
	FROM sci_workflow WHERE STARTBY = USER_ID AND COMPLETEDDATE IS NULL AND STATUSID IN (7, 9)) MA, 
    sci_modules SM, sci_tasks ST, fundingbody_master FM, award_master AM
    WHERE
    MA.TASKID = ST.TASKID AND MA.MODULEID = SM.MODULEID AND MA.MODULEID = 4
    AND IFNULL(FM.STATUSCODE, 0) <> 4 AND FM.FUNDINGBODY_ID = AM.FUNDINGBODYID AND AWARDID = MA.ID;
END;