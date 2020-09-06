CREATE PROCEDURE `SCI_OP_ECLASSIFICATIONLIST`(
   P_WORKFLOWID       INTEGER
)
BEGIN
   DECLARE V_ID         INTEGER;
   DECLARE V_MODULEID   INTEGER;
   
   SELECT   ID, MODULEID
     INTO   V_ID, V_MODULEID
     FROM   SCI_WORKFLOW
    WHERE   WORKFLOWID = P_WORKFLOWID;

   IF V_MODULEID = 3
   THEN
        SELECT   sty.TYPE display,
                  el.ID,
                  ELIGIBILITYCLASSIFICATION_TEXT,
                  OPPORTUNITY_ID,
                  el.TYPE TYPE,
                  COUNTRY_CODE,
                  STATE,
                  CITY,
                  ers.ID REGION_SPECIFIC_ID
           FROM   ELIGIBILITYCLASSIFICATION el,
                  SCI_TYPEIDVALUES sty,
                  elgcls_regionspecific ers
          WHERE       el.TYPE = sty.VALUE
                  AND OPPORTUNITY_ID = V_ID
                  AND IFNULL(ers.ID,0) = IFNULL(el.REGION_SPECIFIC_ID,0);

         SELECT   a.VALUE,
                  a.TYPE,
                  a.ELIGIBILITYCLASSIFICATION,
                  b.TYPE display
           FROM   SCI_CLASSIFICATIONTYPEIDVALUES a, SCI_TYPEIDVALUES b
          WHERE   A.TYPE = b.VALUE
         UNION
         SELECT   lcode AS VALUE,
                  'citizenship' AS TYPE,
                  name AS ELIGIBILITYCLASSIFICATION,
                  'Citizenship/Residency Requirements' AS display
           FROM   sci_countrycodes
         ORDER BY   ELIGIBILITYCLASSIFICATION;

     SELECT   * FROM SCI_TYPEIDVALUES;

          SELECT   COUNTRYID,
                    NAME,
                    LCODE,
                    SCODE
             FROM   SCI_COUNTRYCODES
         ORDER BY   NAME;
   END IF;
END;
