CREATE DEFINER=`root`@`localhost` PROCEDURE `SCI_GETOPPORTUNITYDATES_5`(
   P_WORKFLOWID                     INTEGER
)
BEGIN
   DECLARE V_MODULEID   INTEGER;
   DECLARE V_ID         INTEGER;
   DECLARE V_CYCLE      INTEGER;
    
   SELECT   MODULEID, ID, CYCLE INTO   V_MODULEID, V_ID, V_CYCLE FROM   SCI_WORKFLOW
   WHERE   WORKFLOWID = P_WORKFLOWID;
   
      SELECT   opportunity_id,
               DATE_FORMAT (loi_due_date, '%d-%m-%Y') loi_DATE,
               sequence_id,
               DATE_REMARKS,
               (CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
        FROM   sci_opp_loi_duedate_detail
       WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 1;

      SELECT   opportunity_id,
               (CASE
                   WHEN loi_due_date IS NULL THEN (DATE_REMARKS)
                   ELSE to_clob(DATE_FORMAT (loi_due_date, '%d-%m-%Y'))
                END)
                  DUE_DATE,
               sequence_id,
               DATE_REMARKS,
               (CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
        FROM   sci_opp_loi_duedate_detail
       WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 2;

      SELECT   opportunity_id,
               DATE_FORMAT (loi_due_date, '%d-%m-%Y') expiration_date,
               sequence_id,
               DATE_REMARKS,
               (CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
        FROM   sci_opp_loi_duedate_detail
       WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 3;

      SELECT   opportunity_id,
               DATE_FORMAT (loi_due_date, '%d-%m-%Y') firstpost_date,
               sequence_id,
               DATE_REMARKS,
               (CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
        FROM   sci_opp_loi_duedate_detail
       WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 4;

      SELECT   opportunity_id,
               DATE_FORMAT (loi_due_date, '%d-%m-%Y') lastmodifedpost_date,
               sequence_id,
               DATE_REMARKS,
               (CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
        FROM   sci_opp_loi_duedate_detail
       WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 5;

      SELECT   opportunity_id,
               DATE_FORMAT (loi_due_date, '%d-%m-%Y') open_date,
               sequence_id,
               DATE_REMARKS,
               (CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
        FROM   sci_opp_loi_duedate_detail
       WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 6;
       	
       SELECT   opportunity_id,
               DATE_FORMAT (loi_due_date, '%d-%m-%Y') PreProposal_Date,
               sequence_id,
               DATE_REMARKS,
               (CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
        FROM   sci_opp_loi_duedate_detail
       WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 7;
            
       SELECT   opportunity_id,
               DATE_FORMAT (loi_due_date, '%d-%m-%Y') Decision_Date,
               sequence_id,
               DATE_REMARKS,
               (CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
        FROM   sci_opp_loi_duedate_detail
       WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 8;
   
      SELECT   OPPORTUNITYSTATUS
        FROM   opportunity
       WHERE   OPPORTUNITY_ID = V_ID;       
END ;;
