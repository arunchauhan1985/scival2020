CREATE PROCEDURE `sci_fundingbodyurlinsert`(
	pOrgDbId	INT,
    pUserId		INT
)
BEGIN
	DECLARE lFundingBodyCount	INT;
    DECLARE lBatch				INT;
    DECLARE lUrlMasterCount		INT;
    
	SELECT COUNT(1) INTO lFundingBodyCount From fundingbody WHERE ORGDBID = pOrgDbId;
    
    IF lFundingBodyCount > 0
    THEN
		SELECT IFNULL(MAX(batch), 0) + 1 INTO lBatch FROM sci_urlmaster WHERE ORGDBID = pOrgDbId;
        
        CREATE TEMPORARY TABLE distinct_url SELECT DISTINCT URL FROM temp_scivalurl;
        
        SELECT COUNT(1) INTO lUrlMasterCount FROM sci_urlmaster WHERE ORGDBID = pOrgDbId AND ROWNUM = 1;
        
        IF lUrlMasterCount > 0
        THEN
			INSERT INTO sci_urlmaster (ORGDBID, URLID, URL, status, CREATEDBY, CREATEDDATE,batch)
            SELECT pOrgDbId, SCI_URLSEQ.NEXTVAL, distinct_url.*, 0, pUserId, SYSDATE, v_batch FROM distinct_url;
        ELSE
			INSERT INTO sci_urlmaster (ORGDBID, URLID, URL, status, CREATEDBY, CREATEDDATE,batch)
            SELECT pOrgDbId, SCI_URLSEQ.NEXTVAL, URL, 0, pUserId, SYSDATE, v_batch 
            FROM (SELECT T1.URL FROM (SELECT COLUMN_VALUE URL FROM distinct_url) t1 
            LEFT JOIN (SELECT url FROM SCI_URLMASTER WHERE orgdbid = p_orgdbid) T2 ON TRIM (UPPER (t1.URL)) = TRIM (UPPER (t2.url)) WHERE T2.URL IS NULL) T;
        END IF;
    END IF;
END;
