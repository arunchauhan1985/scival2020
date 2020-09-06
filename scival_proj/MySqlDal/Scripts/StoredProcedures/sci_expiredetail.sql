CREATE PROCEDURE `sci_expiredetail`(
	pUserId	INT
)
BEGIN
	DECLARE lLogDate	DATE;
    
    SELECT logdate INTO lLogDate FROM sci_expired_opplist WHERE ROWNUM = 1 ORDER BY logdate;
    
    IF DATE(lLogDate) <> CURDATE()
    THEN
		DELETE FROM sci_expired_opplist WHERE 1=1;
        
        INSERT INTO sci_expired_opplist (Modulename, moduleid, oppname, fundingbodyname, id, taskname, taskid, cycle, duedate, cid)
		SELECT * FROM
			(SELECT modulename, moduleid, INITCAP(oppname) oppname, fundingbodyname, OPPORTUNITY_ID id, taskname, taskid, cycle, duedate, id cid FROM
				(SELECT DISTINCT O.OPPORTUNITY_ID, o.id id, f_loi_date_return_2 (O.OPPORTUNITY_ID, vmx1.mx, vmx2.mx) duedate, 'Opportunity' modulename, 3 moduleid, 
                OM.OPPORTUNITYNAME oppname, get_opp_fundingbodyname(o.opportunity_id) fundingbodyname, 'Expire' taskname, 7 taskid, 15 cycle FROM
					(SELECT opportunity_id, id, TRUNC (SYSDATE) curr_Date, FUNDINGBODY_ID FROM opportunity) o,
				fundingbody_master fm, Opportunity_Master om right outer join
                (SELECT MAX(sequence_id) mx, OPPORTUNITY_ID FROM sci_opp_loi_duedate_detail WHERE date_type IN (1, 2) GROUP BY opportunity_id, date_type) vmx1 ON OM.OPPORTUNITYID = vmx1.opportunity_id
                WHERE OM.OPPORTUNITYID = O.OPPORTUNITY_ID
                AND FM.FUNDINGBODY_ID = o.fundingbody_id AND om.statuscode IN (1, 2, 5, 6) AND EXISTS 
					(SELECT 1 FROM opportunity WHERE opportunity_id = OM.OPPORTUNITYID AND TRIM(opportunitystatus) = 'Active') AND EXISTS 
						(SELECT 1 FROM sci_xmldeliverydetail WHERE id = O.OPPORTUNITY_ID AND MODULEID = 3)) table2
				WHERE duedate IS NOT NULL AND duedate <= TRUNC(SYSDATE) 
                UNION
                SELECT 'Opportunity' modulename, 3 moduleid, INITCAP (OM.OPPORTUNITYNAME) oppname, fundingbodyname, O.OPPORTUNITY_ID id, 'Expire' taskname, 
                7 taskid, 15 cycle, F_LOI_DATE_RETURN_2 (sciwf.id, vmx1.mx, vmx2.mx) duedate, o.id cid 
                FROM sci_workflow sciwf, opportunity o, fundingbody_master fm, opportunity_master om right outer join
					(SELECT MAX(sequence_id) mx, OPPORTUNITY_ID FROM sci_opp_loi_duedate_detail WHERE date_type IN (1, 2) GROUP BY opportunity_id, date_type) vmx1 ON OM.OPPORTUNITYID = vmx1.opportunity_id
                    WHERE sciwf.id = om.opportunityid
                    AND O.OPPORTUNITY_ID = OM.OPPORTUNITYID AND FM.FUNDINGBODY_ID = OM.FUNDINGBODYID AND sciwf.CYCLE > 0 AND moduleid = 3 AND taskid = 7 
                    AND completeddate IS NULL AND statusid = 6 AND om.statuscode IN (1, 2, 5, 6) AND EXISTS 
						(SELECT 1 FROM sci_xmldeliverydetail WHERE id = OM.OPPORTUNITYID AND MODULEID = 3)) table1
         WHERE TO_CHAR(DUEDATE,'yyyy') <> '1900'                                                
         ORDER BY DUEDATE ASC;
    END IF;
    
    SELECT modulename, moduleid, oppname, fundingbodyname, id, taskname, taskid, cycle, duedate, id cid FROM SCI_expired_opplist WHERE flag = 0;
    
    UPDATE sci_language_detail SET tran_type_id = 1 WHERE scival_id IN (SELECT ID FROM SCI_expired_opplist) AND moduleid = 3 AND column_id = 5 AND tran_type_id = 0;
END;
