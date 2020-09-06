CREATE DEFINER=`root`@`localhost` PROCEDURE `get_funding_details`(
   P_WFID           INTEGER
)
BEGIN
   DECLARE v_award_id   INTEGER;
   DECLARE v_count      INTEGER;
   DECLARE v_count2     INTEGER;
   DECLARE v_moduleid   INTEGER;
 
   BEGIN
      SELECT   ID, moduleid
        INTO   v_award_id, v_moduleid
        FROM   sci_workflow
       WHERE   workflowid = P_WFID;
     END;

   SELECT   COUNT(1)
     INTO   v_count
     FROM   FUNDEDPROJECTDETAIL fpd, FUNDEDSUBPROJECT fsp
    WHERE   fpd.AWARD_ID = v_award_id AND FPD.AWARD_ID = FSP.AWARD_ID;

   IF v_count != 0
   THEN
     -- OPEN P_dataout FOR
         SELECT   fpd.FUND_ID,
                  fpd.AWARD_ID,
                  ACRONYM,
                  BUDGET_AMOUNT,
                  BUDGET_CURRENCY,
                  ENDDATE,
                  fpd.FUNDINGBODYPROJECTID,
                  STARTDATE,
                  STATUS,
                  COUNTRY,
                  LOCALITY,
                  POSTALCODE,
                  REGION,
                  STREET,
                  POSTOFFICEBOXNUMBER,
                  LINK,
                  SUBFUND_ID,
                  AMOUNT,
                  CURRENCY
           FROM   FUNDEDPROJECTDETAIL fpd, FUNDEDSUBPROJECT fsp
          WHERE   fpd.AWARD_ID = v_award_id AND FPD.AWARD_ID = FSP.AWARD_ID;
   END IF;

   SELECT   COUNT (1)
     INTO   v_count2
     FROM   SCI_LANGUAGE_DETAIL
    WHERE   SCIVAL_ID = v_award_id AND MODULEID = v_moduleid;

   IF v_count2 != 0
   THEN
     -- OPEN p_dataout1 FOR
         SELECT   sld.TRAN_ID,
                  sld.SCIVAL_ID,
                  sld.COLUMN_DESC,
                  sld.COLUMN_ID,
                  sld.MODULEID,
                  sld.LANGUAGE_ID,
                  slm.LANGUAGE_CODE,
                  sld.TRAN_TYPE_ID,
                  sld.CREATED_DATE
           FROM   SCI_LANGUAGE_DETAIL sld, SCI_LANGUAGE_MASTER slm
          WHERE       SCIVAL_ID = v_award_id
                  AND MODULEID = v_moduleid
                  AND slm.CODE_LENGTH = 2
                  AND SLD.LANGUAGE_ID = SLM.LANGUAGE_ID
                  AND COLUMN_ID = 8;
   END IF;
END