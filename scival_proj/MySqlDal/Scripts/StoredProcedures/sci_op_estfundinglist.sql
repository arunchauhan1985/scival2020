CREATE PROCEDURE `sci_op_estfundinglist`(
	p_workflowid       INTEGER,
	page_mode          INTEGER   -- 1 = awardceiling, 2 = awardflooring, 3 = estimatefunding, 4 = amount
)
BEGIN
   DECLARE v_id         INTEGER;
   DECLARE v_moduleid   INTEGER;
   DECLARE v_INSTALLMENTANDAMOUNT_ID   INTEGER;
 
   SELECT   ID, moduleid
     INTO   v_id, v_moduleid
     FROM   sci_workflow
    WHERE   workflowid = p_workflowid;

   IF v_moduleid = 3
   THEN
        SELECT   code, CONCAT(IFNULL(VALUE, '') , ' (' , ifnull(code, '') , ')') AS VALUE
           FROM   sci_currencytype;

      IF page_mode = 3
      THEN
           SELECT   *
              FROM   estimatedfunding
             WHERE   opportunity_id = v_id;
             
            SELECT   li.URL,li.LINK_TEXT
              FROM   estimatedAmountDescription ead, item it, link li 
             WHERE   OPPORTUNITY_ID = V_ID
             and EAD.ESTIMATEDAMOUNTDESCRIPTION_ID=IT.ESTIMATEDAMOUNTDESCRIPTION_ID
             and IT.ITEM_ID=LI.ITEM_ID;

      ELSEIF page_mode = 2
      THEN
           SELECT   *
              FROM   awardfloor
             WHERE   opportunity_id = v_id;
             
            SELECT   null URL,null LINK_TEXT
              FROM   dual;
      ELSEIF page_mode = 1
      THEN
            SELECT   currency,
                     awardceiling_text estimatedfunding_text,
                     opportunity_id
              FROM   awardceiling
             WHERE   opportunity_id = v_id;
             
           SELECT   null URL,null LINK_TEXT
              FROM   dual;
      END IF;
   ELSEIF v_moduleid = 4
   THEN
        SELECT   code, CONCAT(IFNULL(VALUE, '') , ' (' , ifnull(code, '') , ')') AS VALUE
           FROM   sci_currencytype;
           
            SELECT   null URL,null LINK_TEXT
              FROM   dual;

      IF page_mode = 4
      THEN
         -- OPEN p_result1 FOR
         
         Select INSTALLMENTANDAMOUNT_ID into 
           v_INSTALLMENTANDAMOUNT_ID from  totalAmount
            where INSTALLMENTANDAMOUNT_ID in 
             (Select INSTALLMENTANDAMOUNT_ID from INSTALMENTANDAMOUNT where award_id=v_id);
         
         IF v_INSTALLMENTANDAMOUNT_ID IS NOT NULL THEN
            SELECT   ta.CURRENCY,
                      ta.AMOUNT AS AMOUNT_TEXT,
                    -- null INSTALLMENTSTART_DATE,
                     -- null INSTALLMENTEND_DATE,
                     ta.AMOUNT AS total_amount
              FROM   -- amount am,
                     -- installment inst,
                     totalAmount ta,
                     instalmentAndAmount iaa
             WHERE   
             iaa.award_id = v_id 
            -- AND am.award_id = iaa.award_id
                     -- AND INST.INSTALLMENTANDAMOUNT_ID = TA.INSTALLMENTANDAMOUNT_ID
                     -- AND IAA.INSTALLMENTANDAMOUNT_ID =  INST.INSTALLMENTANDAMOUNT_ID 
                     AND IAA.INSTALLMENTANDAMOUNT_ID = ta.INSTALLMENTANDAMOUNT_ID;
                    -- and AWARDEE_ID is null
          ELSE          
                    SELECT   am.CURRENCY,
                    0 AMOUNT_TEXT,
                    -- null INSTALLMENTSTART_DATE,
                    -- null INSTALLMENTEND_DATE,
                     am.AMOUNT_TEXT AS total_amount
              FROM   amount am     
             WHERE   am.award_id = v_id;         
          END IF;                 

               SELECT   SEQUENCE_ID,
                  i.INSTALLMENTANDAMOUNT_ID,
            --   TO_CHAR (INSTALLMENTSTART_DATE, 'rrrr') INSTALLMENTSTART_DATE,
              -- TO_CHAR (INSTALLMENTEND_DATE, 'rrrr') INSTALLMENTEND_DATE,
                  INSTALLMENTSTART_DATE,
                  INSTALLMENTEND_DATE,
                  AMOUNT,
                  CURRENCY
           FROM   installment i, INSTALMENTANDAMOUNT im
          WHERE   i.INSTALLMENTANDAMOUNT_ID = im.INSTALLMENTANDAMOUNT_ID
                  AND award_id = v_id;
      END IF;
   END IF;
END;
