DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `award_location_insupdel`(
   p_workflowid        INTEGER,
   p_insdel            INTEGER,
   p_countrytest       VARCHAR(4000) ,
   p_country           VARCHAR(4000) ,
   p_room              VARCHAR(4000) ,
   p_street            VARCHAR(4000) ,
   p_city              VARCHAR(4000) ,
   p_state             VARCHAR(4000) ,
   p_postalcode        VARCHAR(4000) ,
   p_location_id       INTEGER
   )
BEGIN
 DECLARE v_award_id   INTEGER;
 DECLARE v_value            INTEGER;
 DECLARE v_moduleid         INTEGER;
      SELECT   ID, moduleid INTO   v_award_id, v_moduleid FROM   sci_workflow WHERE   workflowid = p_workflowid;

   IF v_moduleid = 4
   THEN
      IF p_insdel = 0
      THEN
         SELECT   award_location_seq.NEXTVAL INTO v_value FROM DUAL;
         INSERT INTO award_location (location_id,countrytest,country,room, street, city, state,postalcode,AWARD_ID)
           VALUES   (v_value,p_countrytest,p_country, p_room,p_street,p_city,p_state,p_postalcode,v_award_id);
      ELSEIF p_insdel = 2
      THEN
         UPDATE   award_location SET   countrytest =p_countrytest,country = p_country, room = p_room,street = p_street,city = p_city,
                  state = p_state,postalcode = p_postalcode WHERE   location_id = p_location_id;
      ELSEIF p_insdel = 1
      THEN
         DELETE FROM   award_location WHERE   location_id = p_location_id;
      END IF; 

         SELECT   a.LOCATION_ID, A.COUNTRYTEST,a.country countrycode,cc.name countryname,
                  a.room, a.street,a.city,IFNULL (a.state, a.state) statecode,IFNULL (sc.name, a.state) statename,
                  a.postalcode,award_id FROM   award_location a,SCI_COUNTRYCODES cc,SCI_STATECODES sc
          WHERE   sc.code = a.STATE AND cc.LCODE = a.COUNTRY AND a.AWARD_ID = v_award_id;
   END IF;

END$$
DELIMITER ;
