CREATE DEFINER=`root`@`localhost` PROCEDURE `save_funding_prc3`(
   P_fundingBodyProjectId_f       VARCHAR(4000),
   P_Amount_f                     DOUBLE,
   P_ddlCurrency_f                VARCHAR(4000),
   P_fundingBodyProjectId_h       VARCHAR(4000),
   P_Amount_h                     DOUBLE,
   P_ddlCurrency_h                VARCHAR(4000),
   P_acronym                      VARCHAR(4000),
   P_SrtDate                      DATETIME,
   P_EndDateDate                  DATETIME,
   P_Status                       VARCHAR(4000),
   P_link                         VARCHAR(4000),
   P_postofficeboxn               VARCHAR(4000),
   P_Street                       VARCHAR(4000),
   P_locality                     VARCHAR(4000),
   P_region                       VARCHAR(4000),
   P_PostalCode                   VARCHAR(4000),
   P_COUNTRY                      VARCHAR(4000),
   P_COLUMN_DESC                  VARCHAR(4000),
   P_LANGUAGE_NAME                VARCHAR(4000),
   P_COLUMN_ID                    INTEGER,
   P_TRAN_TYPE_ID                 INTEGER,
   P_WFID                         INTEGER,
   P_pagemode                     INTEGER,
   P_mode                         INTEGER,
   P_SEQUENCE_ID                  INTEGER
)
BEGIN
   DECLARE v_award_id       INTEGER;
   DECLARE v_value          INTEGER;
   DECLARE v_moduleid       INTEGER;
   DECLARE v_LANGUAGE_ID    INTEGER;
   DECLARE v_count          INTEGER;
   DECLARE v_count1         INTEGER;
   DECLARE v_count2         INTEGER;
   DECLARE v_COLUMN_ID      INTEGER;
   DECLARE v_TRAN_TYPE_ID   INTEGER;
 
   BEGIN
      SELECT   ID, moduleid
        INTO   v_award_id, v_moduleid
        FROM   sci_workflow
       WHERE   workflowid = P_WFID;
     END;

   

   IF v_moduleid = 4
   THEN
      IF P_mode = 0
      THEN
         SELECT   COUNT(*)
           INTO   v_count
           FROM   FUNDEDPROJECTDETAIL
          WHERE   AWARD_ID = v_award_id;

         IF v_count > 0
         THEN
            UPDATE   FUNDEDPROJECTDETAIL
               SET   ACRONYM = P_acronym,
                     BUDGET_AMOUNT = P_Amount_f,
                     BUDGET_CURRENCY = P_ddlCurrency_f,
                     ENDDATE = P_EndDateDate,
                     FUNDINGBODYPROJECTID = P_fundingBodyProjectId_f,
                     STARTDATE = P_SrtDate,
                     STATUS = P_Status,
                     COUNTRY = P_COUNTRY,
                     LOCALITY = P_locality,
                     POSTALCODE = P_PostalCode,
                     REGION = P_region,
                     STREET = P_Street,
                     POSTOFFICEBOXNUMBER = P_postofficeboxn,
                     link = P_link
             WHERE   AWARD_ID = v_award_id;
         ELSE
            INSERT INTO SCIVAL.FUNDEDPROJECTDETAIL (FUND_ID,
                                                    AWARD_ID,
                                                    ACRONYM,
                                                    BUDGET_AMOUNT,
                                                    BUDGET_CURRENCY,
                                                    ENDDATE,
                                                    FUNDINGBODYPROJECTID,
                                                    STARTDATE,
                                                    STATUS,
                                                    COUNTRY,
                                                    LOCALITY,
                                                    POSTALCODE,
                                                    REGION,
                                                    STREET,
                                                    POSTOFFICEBOXNUMBER)
              VALUES   (FUND_ID.NEXTVAL,
                        v_award_id,
                        P_acronym,
                        P_Amount_f,
                        P_ddlCurrency_f,
                        P_EndDateDate,
                        P_fundingBodyProjectId_f,
                        P_SrtDate,
                        P_Status,
                        P_COUNTRY,
                        P_locality,
                        P_PostalCode,
                        P_region,
                        P_Street,
                        P_postofficeboxn);
         END IF;

         SELECT   COUNT(*)
           INTO   v_count1
           FROM   FUNDEDSUBPROJECT
          WHERE   AWARD_ID = v_award_id;

         IF v_count1 > 0
         THEN
            UPDATE   FUNDEDSUBPROJECT
               SET   FUNDINGBODYPROJECTID = P_fundingBodyProjectId_h,
                     AMOUNT = P_Amount_h,
                     CURRENCY = P_ddlCurrency_h
             WHERE   AWARD_ID = v_award_id;
         ELSE
            INSERT INTO SCIVAL.FUNDEDSUBPROJECT (SUBFUND_ID,
                                                 FUND_ID,
                                                 AWARD_ID,
                                                 FUNDINGBODYPROJECTID,
                                                 AMOUNT,
                                                 CURRENCY)
              VALUES   (SUBFUND_ID.NEXTVAL,
                        FUND_ID.CURRVAL,
                        v_award_id,
                        P_fundingBodyProjectId_h,
                        P_Amount_h,
                        P_ddlCurrency_h);
         END IF;
      END IF;
      COMMIT;
   END IF;

 --  OPEN P_dataout FOR
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

   -- OPEN p_dataout1 FOR
      SELECT   TRAN_ID,
               SCIVAL_ID,
               COLUMN_DESC,
               COLUMN_ID,
               MODULEID,
               LANGUAGE_ID,
               TRAN_TYPE_ID,
               CREATED_DATE
        FROM   SCI_LANGUAGE_DETAIL
       WHERE       SCIVAL_ID = v_award_id
               AND COLUMN_ID = v_COLUMN_ID
               AND MODULEID = v_moduleid;
END