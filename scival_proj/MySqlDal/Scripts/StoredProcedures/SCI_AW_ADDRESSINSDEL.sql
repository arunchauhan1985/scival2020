CREATE DEFINER=`root`@`localhost` PROCEDURE `SCI_AW_ADDRESSINSDEL`(
   P_AFFILIATION_ID_IN        INTEGER,
   P_FLAG_IN                 INTEGER,
   P_COUNTRY_IN VARCHAR(4000),
   P_ROOM_IN VARCHAR(4000),
   P_STREET_IN VARCHAR(4000),
   P_CITY_IN VARCHAR(4000)  ,
   P_STATE_IN VARCHAR(4000) ,
   P_POSTALCODE_IN VARCHAR(4000) ,
   P_OLD_COUNTRY_IN VARCHAR(4000) ,
   P_OLD_ROOM_IN VARCHAR(4000)  ,
   P_OLD_STREET_IN VARCHAR(4000),
   P_OLD_CITY_IN VARCHAR(4000) ,
   P_OLD_STATE_IN VARCHAR(4000),
   P_OLD_POSTALCODE_IN VARCHAR(4000),
   OUT P_STATUS_OUT          INTEGER,
   OUT P_ERROR_OUT           VARCHAR(4000)
)
BEGIN
   /********************************************************************************
     Modified By    :  Julfcar Ali
     Modified Date  :  20-Feb-2013
     Module         :  SciVal.
     Enhancements   :  Address table is being used for address detail instead of awardaddress.
     -------------------------------------------------------------------------------
     Code Location  :  http://192.168.212.28:9870/PowerManage/branches/Dev/Source/Back End/Objects/Procedures/
     Please Note    :
                      1. P_FLAG_IN -  0 - Insert Record
                                      1 - Delete Record
                                      2 - Update Record
                      2. P_OLD_ denotes that the parameter contains the old  address value.
                      3. There is one observation, that a address id column should be added in address table to uniquely identify the address record
                         to retrieve, update and delete.
    ********************************************************************************/
   DECLARE L_V_NVL VARCHAR (2) default '-0';
      INSERT INTO FROG VALUES(CONCAT(IFNULL(P_AFFILIATION_ID_IN, ''),' - ',IFNULL(P_STATE_IN, '')),SYSDATE());
      
   IF P_FLAG_IN = 0
   THEN
      INSERT INTO ADDRESS (COUNTRYTEST,
                           ROOM,
                           STREET,
                           CITY,
                           STATE,
                           POSTALCODE,
                           AFFILIATION_ID)
        VALUES   (P_COUNTRY_IN,
                  P_ROOM_IN,
                  P_STREET_IN,
                  P_CITY_IN,
                  P_STATE_IN,
                  P_POSTALCODE_IN,
                  P_AFFILIATION_ID_IN);
   ELSEIF P_FLAG_IN = 1
   THEN
      DELETE FROM   ADDRESS
            WHERE       AFFILIATION_ID = P_AFFILIATION_ID_IN
                    AND IFNULL (COUNTRYTEST, L_V_NVL) = IFNULL (P_COUNTRY_IN, L_V_NVL)
                    AND IFNULL (ROOM, L_V_NVL) = IFNULL (P_ROOM_IN, L_V_NVL)
                    AND IFNULL (STREET, L_V_NVL) = IFNULL (P_STREET_IN, L_V_NVL)
                    AND IFNULL (CITY, L_V_NVL) = IFNULL (P_CITY_IN, L_V_NVL)
                    AND IFNULL (STATE, L_V_NVL) = IFNULL (P_STATE_IN, L_V_NVL)
                    AND IFNULL (POSTALCODE, L_V_NVL) =
                          IFNULL (P_POSTALCODE_IN, L_V_NVL);
   ELSEIF P_FLAG_IN = 2
   THEN
      UPDATE   ADDRESS
         SET   COUNTRYTEST = P_COUNTRY_IN,
               ROOM = P_ROOM_IN,
               STREET = P_STREET_IN,
               CITY = P_CITY_IN,
               STATE = P_STATE_IN,
               POSTALCODE = P_POSTALCODE_IN
       WHERE       AFFILIATION_ID = P_AFFILIATION_ID_IN
               AND IFNULL (COUNTRYTEST, L_V_NVL) = IFNULL (P_OLD_COUNTRY_IN, L_V_NVL)
               AND IFNULL (ROOM, L_V_NVL) = IFNULL (P_OLD_ROOM_IN, L_V_NVL)
               AND IFNULL (STREET, L_V_NVL) = IFNULL (P_OLD_STREET_IN, L_V_NVL)
               AND IFNULL (CITY, L_V_NVL) = IFNULL (P_OLD_CITY_IN, L_V_NVL)
               AND IFNULL (STATE, L_V_NVL) = IFNULL (P_OLD_STATE_IN, L_V_NVL)
               AND IFNULL (POSTALCODE, L_V_NVL) =
                     IFNULL (P_OLD_POSTALCODE_IN, L_V_NVL);
END IF;
      SELECT   A.COUNTRYTEST,
               C.NAME COUNTRYNAME,
               A.ROOM,
               A.STREET,
               A.CITY,
               A.STATE,
               A.POSTALCODE,
               A.AFFILIATION_ID
        FROM   ADDRESS A, SCI_COUNTRYCODES C
       WHERE   C.LCODE = A.COUNTRYTEST AND AFFILIATION_ID = P_AFFILIATION_ID_IN;
   END