CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_awardstatisticsinsert`(
   p_workflowid              INTEGER,
   p_insdel                  INTEGER,      -- 0 * insert ,2* update ,1* delete
   p_currency                VARCHAR(4000),
   p_totalfunding_text       VARCHAR(4000), 
   p_url                     VARCHAR(4000),
   p_link_text               VARCHAR(4000)
)
sp_lbl:

BEGIN
   DECLARE v_fundingbodyid      INTEGER;
   DECLARE v_value              INTEGER;
   DECLARE v_count              INTEGER;
   DECLARE aw_count             INTEGER;
   DECLARE awcnt                INTEGER;
   DECLARE lv_totalfunding_text varchar(2000);
 
   IF LOCATE('.', p_totalfunding_text,1)>0 
   THEN
      
      leave sp_lbl;
   ELSE
      SET lv_totalfunding_text = TRIM(p_totalfunding_text);-- TO_NUMBER(TRIM(p_totalfunding_text));
   END IF;
  -- -----------------------------------------------------------------------------------------------------------
SELECT 
    ID
INTO v_fundingbodyid FROM
    sci_workflow
WHERE
    workflowid = p_workflowid;

   IF p_insdel = 0
   THEN
      SELECT COUNT(*)
        INTO aw_count
        FROM awardstatistics
       WHERE fundingbody_id = v_fundingbodyid;

SELECT awardstatistics_seq.NEXTVAL INTO v_value FROM DUAL;

      INSERT INTO awardstatistics
                  (awardstatistics_id, fundingbody_id
                  )
           VALUES (v_value, v_fundingbodyid
                  );

      IF p_currency IS NOT NULL
      THEN
         INSERT INTO totalfunding
                     (currency, totalfunding_text, awardstatistics_id
                     )
              VALUES (p_currency, lv_totalfunding_text, v_value
                     );
      END IF;

      IF p_url IS NOT NULL
      THEN
         INSERT INTO LINK
                     (url, link_text, awardstatistics_id
                     )
              VALUES (p_url, p_link_text, v_value
                     );
      END IF;

   ELSEIF p_insdel = 1
   THEN
      DELETE FROM LINK
            WHERE awardstatistics_id IN (
                                       SELECT awardstatistics_id
                                         FROM awardstatistics
                                        WHERE fundingbody_id =
                                                              v_fundingbodyid);

DELETE FROM totalfunding 
WHERE
    awardstatistics_id IN (SELECT 
        awardstatistics_id
    FROM
        awardstatistics
    
    WHERE
        fundingbody_id = v_fundingbodyid);

DELETE FROM awardstatistics 
WHERE
    fundingbody_id = v_fundingbodyid;
   ELSEIF p_insdel = 2
   THEN
      SELECT COUNT (`*`)
        INTO v_count
        FROM LINK
       WHERE 
         awardstatistics_id IN (SELECT awardstatistics_id
                                      FROM awardstatistics
                                     WHERE fundingbody_id = v_fundingbodyid);

      IF v_count > 0
      THEN
         IF p_url IS NULL
         THEN
            DELETE FROM LINK
                  WHERE awardstatistics_id IN (
                                       SELECT awardstatistics_id
                                         FROM awardstatistics
                                        WHERE fundingbody_id =
                                                              v_fundingbodyid);
         ELSE
            UPDATE LINK
               SET url = p_url,
                   link_text = p_link_text
             WHERE awardstatistics_id IN (
                                        SELECT awardstatistics_id
                                          FROM awardstatistics
                                         WHERE fundingbody_id =
                                                               v_fundingbodyid);
         END IF;
      ELSEIF p_url IS NOT NULL
      THEN
         SELECT awardstatistics_id
           INTO v_value
           FROM awardstatistics
          WHERE fundingbody_id = v_fundingbodyid;

         INSERT INTO LINK
                     (url, link_text, awardstatistics_id
                     )
              VALUES (p_url, p_link_text, v_value
                     );
      END IF;

SELECT 
    COUNT(*)
INTO awcnt FROM
    totalfunding
WHERE
    awardstatistics_id IN (SELECT 
            awardstatistics_id
        FROM
            awardstatistics
        WHERE
            fundingbody_id = v_fundingbodyid);

      IF (awcnt > 0)
      THEN
         UPDATE totalfunding
            SET currency = p_currency,
                totalfunding_text = lv_totalfunding_text
          WHERE awardstatistics_id IN (SELECT awardstatistics_id
                                         FROM awardstatistics
                                        WHERE fundingbody_id = v_fundingbodyid);
      ELSEIF p_currency IS NOT NULL
      THEN
         SELECT awardstatistics_id
           INTO v_value
           FROM awardstatistics
          WHERE fundingbody_id = v_fundingbodyid;

         INSERT INTO totalfunding
                     (currency, totalfunding_text, awardstatistics_id
                     )
              VALUES (p_currency, lv_totalfunding_text, v_value
                     );
      END IF;

SELECT 
    COUNT(*)
INTO v_count FROM
    LINK
WHERE
    url IS NULL AND link_text IS NULL
        AND awardstatistics_id IN (SELECT 
            awardstatistics_id
        FROM
            awardstatistics
        WHERE
            fundingbody_id = v_fundingbodyid);

      IF v_count > 0
      THEN
         DELETE FROM LINK
               WHERE awardstatistics_id IN (
                                       SELECT awardstatistics_id
                                         FROM awardstatistics
                                        WHERE fundingbody_id =
                                                              v_fundingbodyid);
      END IF;

SELECT 
    COUNT(*)
INTO v_count FROM
    totalfunding
WHERE
    currency IS NULL
        AND totalfunding_text IS NULL
        AND awardstatistics_id IN (SELECT 
            awardstatistics_id
        FROM
            awardstatistics
        WHERE
            fundingbody_id = v_fundingbodyid);

      IF v_count > 0
      THEN
         DELETE FROM totalfunding
               WHERE awardstatistics_id IN (
                                       SELECT awardstatistics_id
                                         FROM awardstatistics
                                        WHERE fundingbody_id =
                                                              v_fundingbodyid);
		END IF;   
	END IF;   
END ;;
