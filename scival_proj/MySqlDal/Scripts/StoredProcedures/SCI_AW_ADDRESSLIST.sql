CREATE DEFINER=`root`@`localhost` PROCEDURE `SCI_AW_ADDRESSLIST`(
   P_AFFILIATION_ID_IN       INTEGER
)
BEGIN
     SELECT   A.COUNTRYTEST country,
               C.NAME COUNTRYNAME,
               A.ROOM,
               A.STREET,
               A.CITY,
               IFNULL(SC.NAME,A.STATE) STATE,
               A.POSTALCODE,
               A.AFFILIATION_ID,
               A.STATE STATECODE
        FROM   ADDRESS A, SCI_COUNTRYCODES C, SCI_STATECODES SC
       WHERE   C.LCODE = A.COUNTRYTEST AND SC.CODE= A.STATE AND AFFILIATION_ID = P_AFFILIATION_ID_IN;

END