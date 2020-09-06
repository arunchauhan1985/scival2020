CREATE DEFINER=`root`@`localhost` PROCEDURE `SCI_AW_FAXINSDEL`(
   P_AFFILIATIONID       INTEGER,
   P_INSDEL              INTEGER,  
   P_FAX                 VARCHAR(4000),
   X_FAX                 VARCHAR(4000)
)
BEGIN
   IF P_INSDEL = 0
   THEN
      INSERT INTO FAX (FAX_COLUMN, AFFILIATION_ID)
        VALUES   (P_FAX, P_AFFILIATIONID);
   ELSEIF P_INSDEL = 1
   THEN
      DELETE FROM   FAX
            WHERE   AFFILIATION_ID = P_AFFILIATIONID
                    AND IFNULL (FAX_COLUMN, '1xzsed6d5fe8g45bhj') =
                          IFNULL (P_FAX, '1xzsed6d5fe8g45bhj');
     ELSEIF P_INSDEL = 2
   THEN
      UPDATE   FAX
         SET   FAX_COLUMN = P_FAX
       WHERE   AFFILIATION_ID = P_AFFILIATIONID
               AND IFNULL (FAX_COLUMN, '1xzsed6d5fe8g45bhj') =
                     IFNULL (X_FAX, '1xzsed6d5fe8g45bhj');
   END IF;
      SELECT   *
        FROM   FAX
       WHERE   AFFILIATION_ID = P_AFFILIATIONID;
END