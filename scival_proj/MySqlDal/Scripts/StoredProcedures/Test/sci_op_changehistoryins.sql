CREATE PROCEDURE `sci_op_changehistoryins`(
	p_workflowid           INTEGER /* DEFAULT NULL */ ,
   p_opportunity_id       INTEGER /* DEFAULT NULL */ ,
   p_TYPE                 VARCHAR(4000) /* DEFAULT NULL */ ,
   p_POSTDATE             DATETIME /* DEFAULT NULL */ ,
   p_CHANGE_TEXT          VARCHAR(4000) /* DEFAULT NULL */ ,
   p_mode                 INTEGER, -- 0 = insert, 1 = insert with condition, 2 = update 3 delete
   p_change_id            INTEGER /* DEFAULT NULL */
)
BEGIN
	DECLARE v_moduleid          INTEGER;
	DECLARE v_id                INTEGER;
	DECLARE v_count             INTEGER;
	DECLARE v_value             INTEGER;
	DECLARE v_x                 INTEGER;
	DECLARE l_count             INTEGER;
	DECLARE l_version           INTEGER;
	DECLARE l_change_hist_cnt   INTEGER;
    
    IF p_workflowid IS NOT NULL
   THEN
      SELECT   moduleid, id
        INTO   v_moduleid, v_id
        FROM   sci_workflow
       WHERE   workflowid = p_workflowid;
   END IF;

   IF p_opportunity_id IS NOT NULL
   THEN
      SET v_id = p_opportunity_id;
      SET v_moduleid = 3;
   END IF;
   
   SELECT   COUNT( * )
     INTO   V_COUNT
     FROM   changehistory
    WHERE   OPPORTUNITY_ID = v_id;

IF V_COUNT = 0
   THEN
      SET V_VALUE = NULL;
   ELSE
      SELECT   CHANGEHISTORY_ID
        INTO   V_VALUE
        FROM   changehistory
       WHERE   OPPORTUNITY_ID = v_id;
   END IF;
   
   IF v_moduleid = 3
   THEN
      IF p_mode = 0
      THEN
         SELECT   COUNT( * )
           INTO   v_count
           FROM   changehistory
          WHERE   OPPORTUNITY_ID = v_id;



         IF v_count = 0
         THEN
            SELECT   changehistory_SEQ.NEXTVAL INTO v_value FROM DUAL;

            INSERT INTO CHANGEHISTORY (CHANGEHISTORY_ID, OPPORTUNITY_ID)
              VALUES   (v_value, V_ID);

            INSERT INTO `change` (TYPE,
                                POSTDATE,
                                VERSION,
                                CHANGE_TEXT,
                                CHANGEHISTORY_ID,
                                POSTDATE_IS_SAVE,
                                change_id)
              VALUES   (p_TYPE,
                        STR_TO_DATE (p_POSTDATE, '%d/%m/%Y'),
                        1,
                        p_CHANGE_TEXT,
                        v_value,
                        1,
                        change_id_SEQ.NEXTVAL);
         END IF;
      ELSEIF p_mode = 1
      THEN
         SELECT   COUNT( * )
           INTO   V_COUNT
           FROM   changehistory
          WHERE   OPPORTUNITY_ID = v_id;


         IF V_COUNT > 0
         THEN
            SELECT   COUNT( * )
              INTO   l_count
              FROM   `CHANGE`
             WHERE   CHANGEHISTORY_ID = (SELECT   CHANGEHISTORY_ID
                                           FROM   changehistory
                                          WHERE   OPPORTUNITY_ID = v_id)
                     AND TYPE = 'New';


            IF l_count > 0
            THEN
               SELECT   MAX (version)
                 INTO   l_version
                 FROM   `CHANGE`
                WHERE   CHANGEHISTORY_ID IN (SELECT   CHANGEHISTORY_ID
                                               FROM   changehistory
                                              WHERE   OPPORTUNITY_ID = v_id);

               INSERT INTO `change` (TYPE,
                                   POSTDATE,
                                   VERSION,
                                   CHANGE_TEXT,
                                   CHANGEHISTORY_ID,
                                   POSTDATE_IS_SAVE,
                                   change_id)
                  SELECT   'Update' TYPE,
                           POSTDATE,
                           l_version + 1,
                           CHANGE_TEXT,
                           CHANGEHISTORY_ID,
                           0 POSTDATE_IS_SAVE,
                           change_id_SEQ.NEXTVAL
                    FROM   `CHANGE`
                   WHERE   CHANGEHISTORY_ID =
                              (SELECT   CHANGEHISTORY_ID
                                 FROM   changehistory
                                WHERE   OPPORTUNITY_ID = v_id)
                           AND TYPE = 'New';
            ELSE
               SELECT   MAX(version)
                 INTO   l_version
                 FROM   `CHANGE`
                WHERE   CHANGEHISTORY_ID IN (SELECT   CHANGEHISTORY_ID
                                               FROM   changehistory
                                              WHERE   OPPORTUNITY_ID = v_id);

               INSERT INTO `change` (TYPE,
                                   POSTDATE,
                                   VERSION,
                                   CHANGE_TEXT,
                                   CHANGEHISTORY_ID,
                                   POSTDATE_IS_SAVE,
                                   change_id)
                  SELECT 'Update' TYPE,
                           POSTDATE,
                           l_version + 1,
                           CHANGE_TEXT,
                           CHANGEHISTORY_ID,
                           0 POSTDATE_IS_SAVE,
                           change_id_SEQ.NEXTVAL
                    FROM   `CHANGE`
                   WHERE   CHANGEHISTORY_ID =
                              (SELECT   CHANGEHISTORY_ID
                                 FROM   changehistory
                                WHERE   OPPORTUNITY_ID = v_id)
                           AND
                  ROWNUM <= 1;
            END IF;
         ELSE
            SELECT   changehistory_SEQ.NEXTVAL INTO v_value FROM DUAL;

            INSERT INTO CHANGEHISTORY (CHANGEHISTORY_ID, OPPORTUNITY_ID)
              VALUES   (v_value, V_ID);


            SELECT   MAX (version)
              INTO   l_version
              FROM   `CHANGE`
             WHERE   CHANGEHISTORY_ID IN (SELECT   CHANGEHISTORY_ID
                                            FROM   changehistory
                                           WHERE   OPPORTUNITY_ID = v_id);

            INSERT INTO `change` (TYPE,
                                POSTDATE,
                                VERSION,
                                CHANGE_TEXT,
                                CHANGEHISTORY_ID,
                                POSTDATE_IS_SAVE,
                                change_id)
              VALUES   ('Update',
                        NULL,
                        l_version + 1,
                        NULL,
                        v_value,
                        0,
                        change_id_SEQ.NEXTVAL);
         END IF;
      -- END IF;
      ELSEIF p_mode = 2
      THEN
         UPDATE   `change`
            SET   POSTDATE = STR_TO_DATE (p_POSTDATE, '%d-MON-%Y'),
                  CHANGE_TEXT = p_CHANGE_TEXT,
                  POSTDATE_IS_SAVE = 1
          WHERE   CHANGE_ID = p_change_id;

      ELSEIF p_mode = 3
      THEN
         SELECT   COUNT(CHANGEHISTORY_ID)
           INTO   l_change_hist_cnt
           FROM   `change`
          WHERE   CHANGEHISTORY_ID IN (SELECT   CHANGEHISTORY_ID
                                         FROM   `change`
                                        WHERE   CHANGE_ID = p_change_id);

         IF l_change_hist_cnt = 1
         THEN
            DELETE FROM   changehistory
                  WHERE   CHANGEHISTORY_ID IN
                                (SELECT   CHANGEHISTORY_ID
                                   FROM   `change`
                                  WHERE   CHANGE_ID = p_change_id);

            DELETE FROM   `change`
                  WHERE   CHANGE_ID = p_change_id;
                  
         ELSE
            DELETE FROM   `change`
                  WHERE   CHANGE_ID = p_change_id;
         END IF;
      END IF;

         SELECT   *
           FROM   `change`
          WHERE   CHANGEHISTORY_ID = v_value
                  AND version = (SELECT   MIN (version)
                                   FROM   `change`
                                  WHERE   CHANGEHISTORY_ID = v_value)
         UNION
         SELECT  *
           FROM   `change`
          WHERE   CHANGEHISTORY_ID = v_value
                  AND version = (SELECT   MAX (version)
                                   FROM   `change`
                                  WHERE   CHANGEHISTORY_ID = v_value);
   END IF;
END;
