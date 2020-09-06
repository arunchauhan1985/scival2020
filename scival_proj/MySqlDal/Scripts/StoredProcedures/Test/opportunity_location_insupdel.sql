CREATE DEFINER=`root`@`localhost` PROCEDURE `opportunity_location_insupdel`(
	p_workflowid        INTEGER,
	p_insdel            INTEGER,
	p_country           VARCHAR(4000) /* DEFAULT NULL */ ,
	p_room              VARCHAR(4000) /* DEFAULT NULL */ ,
	p_street            VARCHAR(4000) /* DEFAULT NULL */ ,
	p_city              VARCHAR(4000) /* DEFAULT NULL */ ,
	p_state             VARCHAR(4000) /* DEFAULT NULL */ ,
	p_postalcode        VARCHAR(4000) /* DEFAULT NULL */ ,
	p_location_id       INTEGER /* DEFAULT NULL */  
)
BEGIN
	DECLARE v_opportunity_id   INTEGER;
	DECLARE v_value            INTEGER;
	DECLARE v_moduleid         INTEGER;
	DECLARE v_count            INTEGER;
    
	SELECT   ID, moduleid INTO   v_opportunity_id, v_moduleid FROM   sci_workflow
    WHERE   workflowid = p_workflowid;

	IF v_moduleid = 3  
    THEN
		IF p_insdel = 0
		THEN
			Select count(*) into v_count FROM opportunity_location WHERE COUNTRY=p_country AND opportunity_id=v_opportunity_id;
			
			SELECT   opportunity_location_SEQ.NEXTVAL INTO v_value FROM DUAL;

			INSERT INTO opportunity_location (location_id,
                                           country,
                                           room,
                                           street,
                                           city,
                                           state,
                                           postalcode,
                                           opportunity_id)
			VALUES   (v_value,
                     p_country,
                     p_room,
                     p_street,
                     p_city,
                     p_state,
                     p_postalcode,
                     v_opportunity_id);
         
         
      
		ELSEIF p_insdel = 2
		THEN
			UPDATE   opportunity_location
            SET   country = p_country,
                  room = p_room,
                  street = p_street,
                  city = p_city,
                  state = p_state,
                  postalcode = p_postalcode
			WHERE   location_id = p_location_id;			

		ELSEIF p_insdel = 1
		THEN
			DELETE FROM   opportunity_location WHERE   location_id = p_location_id;         
		END IF;
      
		SELECT   a.LOCATION_ID,
                  a.country countrycode,
                  cc.name countryname,
                  a.room,
                  a.street,
                  a.city,
                  IFNULL (a.state, a.state) statecode,
                  IFNULL (sc.name, a.state) statename,
                  a.postalcode,
                  OPPORTUNITY_ID
        FROM   opportunity_location a
        LEFT JOIN SCI_STATECODES sc
        ON sc.code = a.STATE
        LEFT JOIN SCI_COUNTRYCODES cc
        ON cc.LCODE = a.COUNTRY                  
        WHERE a.OPPORTUNITY_ID = v_opportunity_id;
	END IF;   
END ;;
