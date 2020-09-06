CREATE PROCEDURE `sci_aw_awexist`(
	p_fundingbodyid       INTEGER,
	p_taskid              INTEGER,
	p_updateflag          INTEGER,
	p_userid              INTEGER
)
BEGIN
	DECLARE	v_cycle     INTEGER;
	DECLARE l_awardid   INTEGER;
    
    SELECT count(AWARDID) INTO l_awardid FROM USERASSIGNMENT WHERE USERID = p_userid AND MODULEID = 4 AND ROWNUM = 1;
    
    IF p_updateflag = 0
	THEN
		IF l_awardid = 0
		THEN
			SELECT   workflowid,
                     award_id,
                     fundingbodyawardid,
                     NAME,
                     op.id
              FROM   award op, sci_workflow sw,award_master om
             WHERE       fundingbody_id = p_fundingbodyid
                     AND op.award_id = sw.ID
                     And OM.awardID = op.award_id
                     AND moduleid = 4
                     AND sw.CYCLE = 0
                     And om.statuscode in (1,2)
                     AND taskid = p_taskid
                     AND COMPLETEDDATE IS NULL
                     AND NVL (statusid, 0) NOT IN (8, 7, 11,45)
                     AND NOT EXISTS
                           (SELECT   1
                              FROM   sci_workflow
                             WHERE       ID = sw.ID
                                     AND CYCLE = sw.CYCLE
                                     AND SEQUENCE < sw.SEQUENCE
                                     AND completeddate IS NULL)
            UNION ALL
            SELECT   WORKFLOWID,
                     AWARDID,
                     NULL FUNDINGBODYOPPORTUNITYID,
                     AWARDNAME name,
                     NULL id
              FROM   award_master om, sci_workflow sw
             WHERE       OM.awardID = sw.id
                     AND OM.CYCLE = sw.cycle
                     AND sw.taskid = p_taskid
                     AND sw.MODULEID = 4
                     AND FUNDINGBODYID = P_FUNDINGBODYID
                     AND isautomated = 1
                     AND om.cycle = 0
                     AND statuscode = 1
                     AND NVL (STATUSID, 0) NOT IN (7, 8, 11,45)
                     AND COMPLETEDDATE IS NULL
                     AND NOT EXISTS (SELECT   1
                                       FROM   AWARD
                                      WHERE   AWARD_ID = om.AWARDID)
                     AND NOT EXISTS
                           (SELECT   1
                              FROM   sci_workflow
                             WHERE       ID = sw.ID
                                     AND CYCLE = sw.CYCLE
                                     AND SEQUENCE < sw.SEQUENCE
                                     AND completeddate IS NULL)
            ORDER BY   name;
        ELSE
			SELECT   workflowid,
                     award_id,
                     fundingbodyawardid,
                     NAME,
                     op.id
              FROM   award op, sci_workflow sw,award_master om
             WHERE       fundingbody_id = p_fundingbodyid
                     AND op.award_id = sw.ID
                     And OM.awardID = op.award_id
                     AND moduleid = 4
                     AND sw.CYCLE = 0
                     And om.statuscode in (1,2)
                     AND taskid = p_taskid
                     AND op.AWARD_ID IN
                              (SELECT   AWARDID
                                 FROM   USERASSIGNMENT
                                WHERE   USERID = p_userid AND MODULEID = 4)
                     AND COMPLETEDDATE IS NULL
                     AND NVL (statusid, 0) NOT IN (8, 7,45)
                     AND NOT EXISTS
                           (SELECT   1
                              FROM   sci_workflow
                             WHERE       ID = sw.ID
                                     AND CYCLE = sw.CYCLE
                                     AND SEQUENCE < sw.SEQUENCE
                                     AND completeddate IS NULL)
            UNION ALL
            SELECT   WORKFLOWID,
                     AWARDID,
                     NULL FUNDINGBODYOPPORTUNITYID,
                     AWARDNAME name,
                     NULL id
              FROM   award_master om, sci_workflow sw
             WHERE       OM.awardID = sw.id
                     AND OM.CYCLE = sw.cycle
                     AND sw.taskid = p_taskid
                     AND sw.MODULEID = 4
                     AND FUNDINGBODYID = P_FUNDINGBODYID
                     AND isautomated = 1
                     AND om.cycle = 0
                     AND statuscode = 1
                     AND NVL (STATUSID, 0) NOT IN (7, 8,45)
                     AND om.AWARDID IN
                              (SELECT   AWARDID
                                 FROM   USERASSIGNMENT
                                WHERE   USERID = p_userid AND MODULEID = 4)
                     AND COMPLETEDDATE IS NULL
                     AND NOT EXISTS (SELECT   1
                                       FROM   AWARD
                                      WHERE   AWARD_ID = om.AWARDID)
                     AND NOT EXISTS
                           (SELECT   1
                              FROM   sci_workflow
                             WHERE       ID = sw.ID
                                     AND CYCLE = sw.CYCLE
                                     AND SEQUENCE < sw.SEQUENCE
                                     AND completeddate IS NULL)
            ORDER BY   name;
        END IF;
    END IF;
    
    IF p_updateflag = 1
	THEN
		IF l_awardid = 0
		THEN
			SELECT   workflowid,
                    award_id,
                    fundingbodyawardid,
                    NAME,
                    op.id
                FROM   award op, sci_workflow sw,award_master om
                WHERE       fundingbody_id = p_fundingbodyid
                    AND op.award_id = sw.ID
                     And OM.awardID = op.award_id
                    AND moduleid = 4
                    And om.statuscode in (1,2)
                    AND taskid = p_taskid
                    AND sw.CYCLE > 0
                    AND NVL (statusid, 0) NOT IN (8, 7,45)
                    AND NOT EXISTS
                          (SELECT   1
                             FROM   sci_workflow
                            WHERE       ID = sw.ID
                                    AND CYCLE = sw.CYCLE
                                    AND SEQUENCE < sw.SEQUENCE
                                    AND completeddate IS NULL)
                ORDER BY   name;
        ELSE
			SELECT   workflowid,
                    award_id,
                    fundingbodyawardid,
                    NAME,
                    op.id
                FROM   award op, sci_workflow sw,award_master om
                WHERE       fundingbody_id = p_fundingbodyid
                    AND op.award_id = sw.ID
                    AND moduleid = 4
                    And OM.awardID = op.award_id
                    And om.statuscode in (1,2)
                    AND taskid = p_taskid
                    AND sw.CYCLE > 0
                    AND NVL (statusid, 0) NOT IN (8, 7,45)
                    AND op.award_id IN
                              (SELECT   AWARDID
                                 FROM   USERASSIGNMENT
                                WHERE   USERID = p_userid AND MODULEID = 4)
                    AND NOT EXISTS
                          (SELECT   1
                             FROM   sci_workflow
                            WHERE       ID = sw.ID
                                    AND CYCLE = sw.CYCLE
                                    AND SEQUENCE < sw.SEQUENCE
                                    AND completeddate IS NULL)
                ORDER BY   name;
        END IF;
    END IF;
END;
