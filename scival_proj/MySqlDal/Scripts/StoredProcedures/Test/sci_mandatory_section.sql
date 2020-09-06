CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_mandatory_section`(
   p_workflowid       INTEGER -- ,
   -- OUT flag           INTEGER,
   -- OUT msg            VARCHAR(4000)
)
BEGIN
	DECLARE v_id         INTEGER;
	DECLARE v_moduleid   INTEGER;
	DECLARE v_name       VARCHAR (2000);
	DECLARE v_count      INTEGER;
	DECLARE NOT_FOUND INT DEFAULT 0;
   
   INSERT INTO frog VALUES   ('start', SYSDATE());

   COMMIT;
 
   SELECT ID, moduleid INTO   v_id, v_moduleid FROM   sci_workflow WHERE   workflowid = p_workflowid;

   IF v_moduleid = 2
   THEN
		-- DECLARE fb CURSOR FOR 
        SELECT   NAME FROM   fundingbodytables WHERE   flag = 1;
		BEGIN
			/*OPEN fb;

			loop_label:
			LOOP
            FETCH fb INTO   v_name;

			IF NOT_FOUND = 1 THEN LEAVE loop_label; END IF;

            EXECUTE IMMEDIATE   'select count (*) from '
                             || concat(ifnull(v_name, '')
                             , ' where fundingbody_id= '
                             , ifnull(v_id, ''))
               INTO   v_count;

            IF v_count = 0
            THEN
				SET flag = 0;
				SET msg = CONCAT('No data found for ' , ifnull(v_name, ''));
				IF flag = THEN LEAVE label; END IF 0;
            ELSE
				SET flag = 1;
				SET msg = 'Data found';
            END IF;
			END LOOP;

			CLOSE fb;            */
		END;                
	ELSEIF v_moduleid = 3
	THEN
		-- DECLARE op CURSOR FOR
            SELECT   MAPTABLE FROM   opportunity_tables WHERE   flag = 1;
			BEGIN
				SET NOT_FOUND = 0;
				/*OPEN op;

				loop_label2:
				LOOP
				FETCH op INTO   v_name;

				IF NOT_FOUND = 1 THEN LEAVE loop_label2; END IF;

				EXECUTE IMMEDIATE   'select count (*) from '
                             || concat(ifnull(v_name, '')
                             , ' where opportunity_id= '
                             , ifnull(v_id, ''))
				INTO   v_count;

				IF v_count = 0
				THEN
					SET flag = 0;
					SET msg = CONCAT('No data found for ' , ifnull(v_name, ''));
					IF flag = THEN LEAVE label; END IF 0;
					ELSE
						SET flag = 1;
						SET msg = 'Data found';
					END IF;
			END LOOP;

			CLOSE op;*/
		END;
	ELSEIF v_moduleid = 4
	THEN
        -- DECLARE aw CURSOR FOR
            SELECT   MAPTABLE FROM   award_tables WHERE   flag = 1;
      
			-- DECLARE CONTINUE HANDLER FOR NOT FOUND SET NOT_FOUND = 1;
            BEGIN
				SET NOT_FOUND = 0;
				/*OPEN aw;

				loop_label3:
				LOOP
				FETCH aw INTO   v_name;

				IF NOT_FOUND = 1 THEN LEAVE loop_label3; END IF;

				EXECUTE IMMEDIATE   'select count (*) from '
                             || concat(ifnull(v_name, '')
                             , ' where award_id= '
                             , ifnull(v_id, ''))
               INTO   v_count;

				IF v_count = 0
				THEN
				   SET flag = 0;
				   SET msg = CONCAT('No data found for ' , ifnull(v_name, ''));
				   IF flag = THEN LEAVE label; END IF 0;
				ELSE
				   SET flag = 1;
				   SET msg = 'Data found';
				END IF;
			END LOOP;

			CLOSE aw;*/
		END;
	END IF;
END ;;
