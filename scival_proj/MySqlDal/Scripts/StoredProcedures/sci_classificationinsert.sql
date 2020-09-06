CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_classificationinsert`(
   p_workflowid                integer,
   p_type                      varchar(4000),
   p_FREQUENCY                 integer,
   p_CODE                      varchar(4000),
   p_CLASSIFICATION_TEXT       varchar(4000)   
)
sp_lbl:

BEGIN
	DECLARE v_fundingbodyid    integer;
	DECLARE v_value            integer;
	DECLARE v_value1           integer;
	DECLARE v_contype          integer;
	DECLARE v_classification   integer;
	DECLARE v_moduleid         integer;
	DECLARE v_count            integer;
	DECLARE V_COUNT1           integer;
	DECLARE v_classcount       integer;
	DECLARE NOT_FOUND INT DEFAULT 0;

	DECLARE CONTINUE HANDLER FOR NOT FOUND SET NOT_FOUND = 1;
   
	SELECT   ID, moduleid
		INTO   v_fundingbodyid, v_moduleid
		FROM   sci_workflow
	WHERE   WORKFLOWID = p_workflowid;

	IF v_moduleid = 2
	THEN
		SELECT   COUNT(*)
        INTO   v_classification
        FROM   classificationgroup
		WHERE   FUNDINGBODY_ID = v_fundingbodyid;

		IF v_classification = 0
		THEN
			SELECT   CLASSIFICATIONGROUP_SEQ.NEXTVAL INTO v_value FROM DUAL;

			INSERT INTO classificationgroup (CLASSIFICATIONGROUP_ID, FUNDINGBODY_ID)
			VALUES   (v_value, v_fundingbodyid);
		ELSE
			SELECT   CLASSIFICATIONGROUP_ID
			INTO   v_value
			FROM   classificationgroup
			WHERE   FUNDINGBODY_ID = v_fundingbodyid;
		END IF;

		SELECT   COUNT(*) INTO   v_contype
        FROM   classifications WHERE TYPE = p_type
		AND CLASSIFICATIONGROUP_ID 
        IN (SELECT   CLASSIFICATIONGROUP_ID FROM classificationgroup WHERE FUNDINGBODY_ID = v_fundingbodyid);

		IF v_contype = 0
		THEN
			SELECT   CLASSIFICATIONS_SEQ.NEXTVAL INTO v_value1 FROM DUAL;

			INSERT INTO classifications (TYPE, CLASSIFICATIONS_ID, CLASSIFICATIONGROUP_ID)
			VALUES   (p_type, v_value1, v_value);
           
			/*declare i cursor for p_CODE.first.p_CODE loop

            INSERT INTO classification (FREQUENCY, CODE, CLASSIFICATION_TEXT, CLASSIFICATIONS_ID)
			VALUES   (p_FREQUENCY, p_CODE(i), p_CLASSIFICATION_TEXT(i), v_value1);
			 fetch  into;
			 end while;
			 close ;*/
             						
		ELSE
			SELECT   CLASSIFICATIONS_ID INTO v_value1
			FROM   classifications WHERE TYPE = p_type AND CLASSIFICATIONGROUP_ID 
            IN (SELECT CLASSIFICATIONGROUP_ID FROM   classificationgroup WHERE FUNDINGBODY_ID = v_fundingbodyid);
           
			/*declare i cursor for p_CODE.first.p_CODE loop   
			INSERT INTO classification (FREQUENCY,CODE,CLASSIFICATION_TEXT,CLASSIFICATIONS_ID)
			VALUES   (p_FREQUENCY,p_CODE(i),p_CLASSIFICATION_TEXT(i),v_value1);
                     fetch  into;
                     end while;
                     close ;*/
		END IF;
		
        SELECT   cgp.CLASSIFICATIONGROUP_ID,
                  cgp.FUNDINGBODY_ID,
                  cfs.TYPE,
                  cfs.CLASSIFICATIONS_ID,
                  cf.FREQUENCY,
                  cf.CODE,
                  cf.CLASSIFICATION_TEXT
        FROM   classificationgroup cgp,
                  classifications cfs,
                  classification cf
        WHERE   cgp.CLASSIFICATIONGROUP_ID = cfs.CLASSIFICATIONGROUP_ID
                  AND cfs.CLASSIFICATIONS_ID = cf.CLASSIFICATIONS_ID
                  AND cgp.FUNDINGBODY_ID = v_fundingbodyid;
   -- ------------------------------------------------------------
	ELSEIF v_moduleid = 3
	THEN
		SELECT   COUNT(*) INTO   v_classification
        FROM   classificationgroup
		WHERE   OPPORTUNITY_ID = v_fundingbodyid;

		IF v_classification = 0
		THEN
			SELECT   CLASSIFICATIONGROUP_SEQ.NEXTVAL INTO v_value FROM DUAL;

			INSERT INTO classificationgroup (CLASSIFICATIONGROUP_ID,OPPORTUNITY_ID)
			VALUES   (v_value, v_fundingbodyid);
		ELSE
			SELECT   CLASSIFICATIONGROUP_ID INTO   v_value
			FROM   classificationgroup
			WHERE   OPPORTUNITY_ID = v_fundingbodyid;
		END IF;
        
		SELECT   COUNT(*) INTO   v_contype
        FROM   classifications
		WHERE   TYPE = p_type AND CLASSIFICATIONGROUP_ID 
        IN (SELECT   CLASSIFICATIONGROUP_ID FROM   classificationgroup WHERE   OPPORTUNITY_ID = v_fundingbodyid);

		SELECT   COUNT(*) INTO v_count FROM classification where  classifications_id in
		(select classifications_id from classifications where  CLASSIFICATIONGROUP_ID IN
		(SELECT   CLASSIFICATIONGROUP_ID FROM   classificationgroup WHERE   OPPORTUNITY_ID = v_fundingbodyid));

		/*declare i cursor for p_CODE.first.p_CODE loop
		IF v_count > 0
        THEN
			SELECT   COUNT(*) INTO   v_count1 FROM   classification
			WHERE   code = 1000  and classifications_id in 
            (select classifications_id from classifications where  CLASSIFICATIONGROUP_ID IN
			(SELECT   CLASSIFICATIONGROUP_ID FROM   classificationgroup WHERE   OPPORTUNITY_ID = v_fundingbodyid));			
		END IF;
		fetch  into;
		end while;
		close ;*/

		IF v_contype = 0
		THEN
			SELECT   CLASSIFICATIONS_SEQ.NEXTVAL INTO v_value1 FROM DUAL;

			INSERT INTO classifications (TYPE,CLASSIFICATIONS_ID,CLASSIFICATIONGROUP_ID)
			VALUES   (p_type, v_value1, v_value);
           
			/*declare i cursor for p_CODE.first.p_CODE loop
			INSERT INTO classification (FREQUENCY,CODE,CLASSIFICATION_TEXT,CLASSIFICATIONS_ID)
			VALUES   (p_FREQUENCY,p_CODE(i) ,p_CLASSIFICATION_TEXT(i),v_value1);
			 fetch  into;
			 end while;
			 close ;                     */
		ELSE
			SELECT   CLASSIFICATIONS_ID INTO   v_value1
			FROM   classifications
			WHERE   TYPE = p_type AND CLASSIFICATIONGROUP_ID IN
		    (SELECT   CLASSIFICATIONGROUP_ID FROM   classificationgroup WHERE   OPPORTUNITY_ID = v_fundingbodyid);

			/*declare i cursor for p_CODE.first.p_CODE loop
            SELECT   COUNT(*) INTO   v_classcount
			FROM   classification WHERE   FREQUENCY=p_FREQUENCY AND CODE=p_CODE(i) and CLASSIFICATIONS_ID=v_value1;
			if v_classcount > 0
			then
				
			else 
				INSERT INTO classification (FREQUENCY,CODE,CLASSIFICATION_TEXT,CLASSIFICATIONS_ID)
				VALUES   (p_FREQUENCY,p_CODE(i),p_CLASSIFICATION_TEXT(i),v_value1);
			END IF;
			fetch  into;
			end while;
			close ;*/
      
		END IF;		

		SET NOT_FOUND = 0;
		
		SELECT   cgp.CLASSIFICATIONGROUP_ID,
                  cgp.FUNDINGBODY_ID,
                  cfs.TYPE,
                  cfs.CLASSIFICATIONS_ID,
                  cf.FREQUENCY,
                  cf.CODE,
                  cf.CLASSIFICATION_TEXT
        FROM   classificationgroup cgp,
                  classifications cfs,
                  classification cf
		WHERE   cgp.CLASSIFICATIONGROUP_ID = cfs.CLASSIFICATIONGROUP_ID
                  AND cfs.CLASSIFICATIONS_ID = cf.CLASSIFICATIONS_ID
                  AND cgp.OPPORTUNITY_ID = v_fundingbodyid;
   -- ----------------------------------------------------------------------------------
	ELSEIF v_moduleid = 4
	THEN
		SELECT   COUNT(*)
        INTO   v_classification
        FROM   classificationgroup
		WHERE   AWARD_ID = v_fundingbodyid;

		IF v_classification = 0
		THEN
			SELECT   CLASSIFICATIONGROUP_SEQ.NEXTVAL INTO v_value FROM DUAL;

			INSERT INTO classificationgroup (CLASSIFICATIONGROUP_ID, AWARD_ID)
			VALUES   (v_value, v_fundingbodyid);
		ELSE
			SELECT   CLASSIFICATIONGROUP_ID
			INTO   v_value
			FROM   classificationgroup
			WHERE   AWARD_ID = v_fundingbodyid;
		END IF;

		SELECT   COUNT(*) INTO   v_contype
        FROM   classifications
		WHERE   TYPE = p_type AND CLASSIFICATIONGROUP_ID IN
		(SELECT   CLASSIFICATIONGROUP_ID FROM   classificationgroup WHERE   AWARD_ID = v_fundingbodyid);

		SELECT   COUNT(*) INTO v_count FROM classification where  classifications_id in
        (select classifications_id from classifications where  CLASSIFICATIONGROUP_ID IN
		(SELECT   CLASSIFICATIONGROUP_ID FROM   classificationgroup WHERE   AWARD_ID = v_fundingbodyid));
       
		/*declare i cursor for p_CODE.first.p_CODE loop
     
		IF v_count > 0
		THEN
			SELECT   COUNT(*) INTO   v_count1 FROM   classification
			WHERE   code = '1000' and classifications_id in
            (select classifications_id from classifications where  CLASSIFICATIONGROUP_ID IN
			(SELECT   CLASSIFICATIONGROUP_ID FROM   classificationgroup WHERE   AWARD_ID = v_fundingbodyid));
					
		fetch  into;
		end while;
		close ;	*/

		IF v_contype = 0
		THEN
			SELECT   CLASSIFICATIONS_SEQ.NEXTVAL INTO v_value1 FROM DUAL;

			INSERT INTO classifications (TYPE,CLASSIFICATIONS_ID,CLASSIFICATIONGROUP_ID)
			VALUES   (p_type, v_value1, v_value);
           			
			/*declare i cursor for p_CODE.first.p_CODE loop
				INSERT INTO classification (FREQUENCY,CODE,CLASSIFICATION_TEXT,CLASSIFICATIONS_ID)
				VALUES   (p_FREQUENCY,p_CODE(i),p_CLASSIFICATION_TEXT(i),v_value1);
				fetch  into;
                end while;
                close ;*/
		ELSE
			SELECT   CLASSIFICATIONS_ID INTO   v_value1
            FROM   classifications
			WHERE   TYPE = p_type AND CLASSIFICATIONGROUP_ID IN
			(SELECT   CLASSIFICATIONGROUP_ID FROM   classificationgroup WHERE AWARD_ID = v_fundingbodyid);
           
			/*declare i cursor for p_CODE.first.p_CODE loop
				INSERT INTO classification (FREQUENCY,CODE,CLASSIFICATION_TEXT,CLASSIFICATIONS_ID)
				VALUES   (p_FREQUENCY,p_CODE(i),p_CLASSIFICATION_TEXT(i),v_value1);
				fetch  into;
				end while;
				close ;*/
		END IF;
     
			SET NOT_FOUND = 0;
		
			SELECT   cgp.CLASSIFICATIONGROUP_ID,
                  cgp.FUNDINGBODY_ID,
                  cfs.TYPE,
                  cfs.CLASSIFICATIONS_ID,
                  cf.FREQUENCY,
                  cf.CODE,
                  cf.CLASSIFICATION_TEXT
			FROM   classificationgroup cgp,
                  classifications cfs,
                  classification cf
			WHERE   cgp.CLASSIFICATIONGROUP_ID = cfs.CLASSIFICATIONGROUP_ID
                  AND cfs.CLASSIFICATIONS_ID = cf.CLASSIFICATIONS_ID
                  AND cgp.AWARD_ID = v_fundingbodyid;
		
	END IF;

END ;;
