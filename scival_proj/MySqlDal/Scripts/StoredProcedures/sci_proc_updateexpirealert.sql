CREATE PROCEDURE `sci_proc_updateexpirealert`()
BEGIN
	DECLARE	alertCount INT DEFAULT 0;
    
    SELECT COUNT(OPPORTUNITYID) INTO alertCount FROM sci_expire_alert WHERE DATE(CREATEDDATE) = CURDATE();
    
    IF alertCount = 0
    THEN
		INSERT INTO sci_expire_alert(OPPORTUNITYID, DUEDATE, CREATEDDATE, FLAG)
        SELECT od.OPPORTUNITY_ID, DUEDATE, CREATEDDATE, FLAG FROM 
        (
			SELECT DISTINCT soldd.OPPORTUNITY_ID,
            CASE
				WHEN 
                (
					SELECT YEAR(LOI_DUE_DATE) FROM sci_opp_loi_duedate_detail sodd WHERE sodd.OPPORTUNITY_ID = soldd.OPPORTUNITY_ID AND sodd.DATE_TYPE = 2
                    AND SEQUENCE_ID = (SELECT MAX(SEQUENCE_ID) FROM sci_opp_loi_duedate_detail soldd2 WHERE soldd2.OPPORTUNITY_ID = sodd.OPPORTUNITY_ID AND soldd2.DATE_TYPE = 2)
				)
				IN ('1900')
                THEN
                (
					SELECT LOI_DUE_DATE FROM sci_opp_loi_duedate_detail sodd WHERE sodd.OPPORTUNITY_ID = soldd.OPPORTUNITY_ID AND sodd.DATE_TYPE = 1
                    AND SEQUENCE_ID = (SELECT MAX(SEQUENCE_ID) FROM sci_opp_loi_duedate_detail soldd2 WHERE soldd2.OPPORTUNITY_ID = sodd.OPPORTUNITY_ID AND soldd2.DATE_TYPE = 1)
                )
                ELSE
                (
					SELECT LOI_DUE_DATE FROM sci_opp_loi_duedate_detail sodd WHERE sodd.OPPORTUNITY_ID = soldd.OPPORTUNITY_ID AND sodd.DATE_TYPE = 2
                    AND SEQUENCE_ID = (SELECT MAX(SEQUENCE_ID) FROM sci_opp_loi_duedate_detail soldd2 WHERE soldd2.OPPORTUNITY_ID = sodd.OPPORTUNITY_ID AND soldd2.DATE_TYPE = 2)
                )
                END DUEDATE, CURDATE() AS CREATEDDATE, 0 AS FLAG
                FROM sci_opp_loi_duedate_detail soldd, oppotunity o
                WHERE o.OPPORTUNITY_ID = soldd.OPPORTUNITY_ID
                AND LTRIM(RTRIM(LOWER(o.OPPORTUNITYSTATUS))) = 'active'
                AND NOT EXISTS (SELECT 1 FROM sci_expire_alert e WHERE e.OPPORTUNITYID = soldd.OPPORTUNITY_ID AND e.FLAG = 0)
		) od
        WHERE DUEDATE IS NOT NULL AND DUEDATE < CURDATE();
    END IF;
END;
