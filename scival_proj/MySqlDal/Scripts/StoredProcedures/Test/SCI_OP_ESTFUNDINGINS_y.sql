CREATE DEFINER=`root`@`localhost` PROCEDURE `SCI_OP_ESTFUNDINGINS_y`(
   p_workflowid               INTEGER,
   p_currency                 VARCHAR(4000),
   p_amount                   VARCHAR(4000),
   p_amount_description       VARCHAR(4000) ,
   p_total_amount             DOUBLE ,
   p_startdate                DATETIME ,
   p_enddate                  DATETIME ,
   p_url                      VARCHAR(4000),
   p_url_text                 VARCHAR(4000),
   p_mode                     INTEGER,     
   PAGE_MODE                  INTEGER,
   p_SEQUENCE_ID              integer
)
BEGIN
DECLARE v_moduleid                    INTEGER;
DECLARE v_id                          INTEGER;
DECLARE v_count                       INTEGER;
DECLARE l_inst_Amount_Id_seq          INTEGER;
DECLARE l_count                       INTEGER;
DECLARE l_installmentandamount_id     INTEGER;
DECLARE v_itemid                      INTEGER;
DECLARE l_estimatedamountdesc_id      INTEGER;
DECLARE l_limitedsubmissiondesc_id    INTEGER;
DECLARE l_eligibilitydescription_id   INTEGER;
DECLARE l_INST_AMOUNT_l_SEQUENCE_ID   integer;
DECLARE lv_amount                     INTEGER;
DECLARE v_aggregate_amount            INTEGER;
DECLARE v_lead_amount                 INTEGER;

 
   DECLARE EXIT HANDLER FOR SQLEXCEPTION
   /*BEGIN
      INSERT INTO frog (VALUE, ENTRYDATE)
        VALUES   (p_o_error, SYSDATE());
      COMMIT;
   END;
   IF LOCATE ('.', p_amount, 1) > 0
   THEN
      SET p_o_status = 1;
      SET p_o_error = CONCAT('Amount can not be decimal - ' , ifnull(p_amount, ''));
      LEAVE sp_lbl;
   ELSE */
      SET lv_amount = TO_NUMBER (TRIM (p_amount));
   /*END IF;*/

   SELECT   moduleid, id  INTO   v_moduleid, v_id FROM   sci_workflow WHERE   workflowid = p_workflowid;

   IF v_moduleid = 3
   THEN
      IF PAGE_MODE = 3                                  -- 0 = estimatefunding,
      THEN
         IF p_mode = 0
         THEN
            SELECT  COUNT(*)  INTO   v_count FROM   ESTIMATEDFUNDING WHERE   OPPORTUNITY_ID = v_id;
            IF v_count = 0
            THEN
               INSERT INTO ESTIMATEDFUNDING (CURRENCY,ESTIMATEDFUNDING_TEXT, OPPORTUNITY_ID,amount_description)
                 VALUES   (p_CURRENCY,lv_amount,v_id,p_amount_description);

               SELECT   item_SEQ.NEXTVAL INTO v_itemid FROM DUAL;
               SELECT   estimatedamountdesc_id_SEQ.NEXTVAL INTO   l_estimatedamountdesc_id FROM   DUAL;               
            ELSE
               UPDATE   ESTIMATEDFUNDING SET   CURRENCY = p_CURRENCY,ESTIMATEDFUNDING_TEXT = lv_amount,amount_description = p_amount_description
                WHERE   OPPORTUNITY_ID = v_id;
            END IF;
         ELSEIF p_mode = 1
         THEN
            UPDATE   ESTIMATEDFUNDING SET   CURRENCY = p_CURRENCY,ESTIMATEDFUNDING_TEXT = lv_amount,amount_description = p_amount_description
             WHERE   OPPORTUNITY_ID = v_id;
         ELSEIF p_mode = 2
         THEN
            DELETE FROM   ESTIMATEDFUNDING WHERE CURRENCY = p_CURRENCY  AND ESTIMATEDFUNDING_TEXT = lv_amount AND amount_description = p_amount_description
                          AND OPPORTUNITY_ID = v_ID;         
         END IF;
            SELECT   * FROM   ESTIMATEDFUNDING WHERE   OPPORTUNITY_ID = V_ID;
            SELECT   li.URL, li.LINK_TEXT FROM   estimatedAmountDescription ead, item it, link li
             WHERE   OPPORTUNITY_ID = V_ID AND EAD.ESTIMATEDAMOUNTDESCRIPTION_ID = IT.ESTIMATEDAMOUNTDESCRIPTION_ID
                     AND IT.ITEM_ID = LI.ITEM_ID;
      ELSEIF page_mode = 2                                  -- 1 = awardflooring
      THEN
         IF p_mode = 0
         THEN
            SELECT   COUNT(*) INTO   v_count FROM   awardfloor WHERE   OPPORTUNITY_ID = v_id;
            IF v_count = 0
            THEN
               INSERT INTO awardfloor (CURRENCY,ESTIMATEDFUNDING_TEXT,OPPORTUNITY_ID)
                 VALUES   (p_CURRENCY, lv_amount, v_id);
            ELSE
               UPDATE   awardfloor  SET   CURRENCY = p_CURRENCY, ESTIMATEDFUNDING_TEXT = lv_amount
                WHERE   OPPORTUNITY_ID = v_id;
            END IF;
         ELSEIF p_mode = 1
         THEN
            UPDATE   awardfloor
               SET   CURRENCY = p_CURRENCY, ESTIMATEDFUNDING_TEXT = lv_amount
             WHERE   OPPORTUNITY_ID = v_id;
         ELSEIF p_mode = 2
         THEN
            DELETE FROM   awardfloor WHERE CURRENCY = p_CURRENCY AND ESTIMATEDFUNDING_TEXT = lv_amount AND OPPORTUNITY_ID = v_ID;
         END IF;
            SELECT   *  FROM   awardfloor WHERE   OPPORTUNITY_ID = V_ID;
			SELECT   NULL URL, NULL LINK_TEXT FROM DUAL;
      ELSEIF page_mode = 1                                   -- 2 = awardceiling
      THEN
         IF p_mode = 0
         THEN
            SELECT   COUNT(*) INTO   v_count FROM   AWARDCEILING WHERE   OPPORTUNITY_ID = v_id;
            IF v_count = 0
            THEN
               INSERT INTO AWARDCEILING (CURRENCY, AWARDCEILING_TEXT,OPPORTUNITY_ID)
                 VALUES   (p_CURRENCY, lv_amount, v_id);
			ELSE
               UPDATE   AWARDCEILING
                  SET   CURRENCY = p_CURRENCY, AWARDCEILING_TEXT = lv_amount
                WHERE   OPPORTUNITY_ID = v_id;
            END IF;
         ELSEIF p_mode = 1
         THEN
            UPDATE   AWARDCEILING SET   CURRENCY = p_CURRENCY, AWARDCEILING_TEXT = lv_amount
             WHERE   OPPORTUNITY_ID = v_id;
         ELSEIF p_mode = 2
         THEN
            DELETE FROM  AWARDCEILING WHERE CURRENCY = p_CURRENCY  AND AWARDCEILING_TEXT = lv_amount AND OPPORTUNITY_ID = v_ID;
         END IF;
            SELECT   CURRENCY,AWARDCEILING_TEXT ESTIMATEDFUNDING_TEXT, OPPORTUNITY_ID FROM   AWARDCEILING WHERE   OPPORTUNITY_ID = V_ID;
             SELECT   NULL URL, NULL LINK_TEXT FROM DUAL;
      END IF;
      ELSEIF v_moduleid = 4 THEN
      IF page_mode = 4              -- - Modify amount only
      THEN
         IF p_mode = 0
         THEN
            SELECT   COUNT(*) INTO   v_count FROM amount WHERE   AWARD_ID = v_id;
            IF v_count = 0
            THEN
               INSERT INTO amount (CURRENCY, AMOUNT_TEXT, AWARD_ID) VALUES   (p_CURRENCY, lv_amount, v_id);
            ELSE
               UPDATE   amount SET   CURRENCY = p_CURRENCY, AMOUNT_TEXT = lv_amount WHERE   AWARD_ID = v_id;
            END IF;
         ELSEIF p_mode = 1
         THEN
            UPDATE amount SET   CURRENCY = p_CURRENCY, AMOUNT_TEXT = lv_amount WHERE   AWARD_ID = v_id;
         ELSEIF p_mode = 2
         THEN
            DELETE FROM amount WHERE CURRENCY = p_CURRENCY AND AMOUNT_TEXT = lv_amount AND AWARD_ID = v_ID;
	     END IF;
            SELECT   * FROM   amount WHERE   award_ID = V_ID;
      ELSEIF PAGE_MODE = 5
      THEN
         IF p_mode = 0          -- 0 = insert, 1 = update 2 = delete
         THEN
            SELECT COUNT(*) INTO v_count FROM   amount WHERE   AWARD_ID = v_id AND AWARDEE_ID IS NULL;
            IF v_count = 0
            THEN
               INSERT INTO amount (CURRENCY, AMOUNT_TEXT, AWARD_ID) VALUES   (p_CURRENCY, p_total_amount, v_id);
            ELSE
               UPDATE  amount SET   CURRENCY = p_CURRENCY, AMOUNT_TEXT = p_total_amount WHERE   AWARD_ID = v_id AND AWARDEE_ID IS NULL;
            END IF;
            SELECT   COUNT(*) INTO   l_count FROM   instalmentAndAmount WHERE   award_id = v_id;
            SELECT   INST_AMOUNT_l_SEQUENCE_ID.NEXTVAL INTO   l_INST_AMOUNT_l_SEQUENCE_ID FROM   DUAL;
            IF l_count = 0
            THEN
               SELECT   INST_AMOUNT_ID_SEQ.NEXTVAL
                 INTO   l_inst_Amount_Id_seq
                 FROM   DUAL;
               INSERT INTO instalmentAndAmount (INSTALLMENTANDAMOUNT_ID,award_id) VALUES   (l_inst_Amount_Id_seq, v_id);

               INSERT INTO installment (INSTALLMENTANDAMOUNT_ID,INSTALLMENTSTART_DATE, INSTALLMENTEND_DATE,AMOUNT,CURRENCY, SEQUENCE_ID)
                 VALUES   (l_inst_Amount_Id_seq,p_startdate,p_enddate,p_amount,p_currency,
                           l_INST_AMOUNT_l_SEQUENCE_ID);

               INSERT INTO totalAmount (INSTALLMENTANDAMOUNT_ID,AMOUNT, CURRENCY)
                 VALUES   (l_inst_Amount_Id_seq, p_total_amount, p_currency);
            ELSE
            SELECT   INSTALLMENTANDAMOUNT_ID INTO   l_installmentandamount_id FROM   instalmentAndAmount WHERE   award_id = v_id;

               INSERT INTO installment (INSTALLMENTANDAMOUNT_ID, INSTALLMENTSTART_DATE, INSTALLMENTEND_DATE, AMOUNT, CURRENCY,SEQUENCE_ID)
                 VALUES   (l_installmentandamount_id, p_startdate, p_enddate, p_amount, p_currency, l_INST_AMOUNT_l_SEQUENCE_ID);
                /*sci_Lead_Amount_Update(v_id,p_total_amount,p_currency,p_o_status, p_o_error); */
                 -- ------------------------------------------ End -----------------------------------------------
            END IF;
         ELSEIF p_mode = 1      -- Update Total Amount
         THEN
             -- ---- Below section added on 24 Jan 2019 by S.S.Shah to tackle auto reducing  LEAD amount ------
             SELECT SUM(Amount) INTO v_aggregate_amount FROM fundingbodyamount fba LEFT JOIN relatedfundingbodies rfb ON 
             FBA.RELEATEDFUNDINGBODIES_ID=RFB.RELEATEDFUNDINGBODIES_ID WHERE RFB.HIERARCHY<>'lead' AND  RFB.AWARD_ID = v_id;
             
             Select count(*) INTO v_lead_amount from FundingBodyAmount where RELEATEDFUNDINGBODIES_ID=(Select RELEATEDFUNDINGBODIES_ID from relatedfundingbodies where HIERARCHY='lead' and AWARD_ID=v_id);
             IF v_lead_amount > 0 THEN
               Select Amount INTO v_lead_amount from FundingBodyAmount where RELEATEDFUNDINGBODIES_ID= (Select RELEATEDFUNDINGBODIES_ID from relatedfundingbodies where HIERARCHY='lead' and AWARD_ID=v_id);
             END IF;
             
            SELECT   COUNT(*) INTO   v_count FROM   amount WHERE   AWARD_ID = v_id AND AWARDEE_ID IS NULL;
            IF v_count = 0
            THEN
               INSERT INTO amount (CURRENCY, AMOUNT_TEXT, AWARD_ID) VALUES (p_CURRENCY, p_total_amount, v_id);
            ELSE
               UPDATE   amount SET   CURRENCY = p_CURRENCY, AMOUNT_TEXT = p_total_amount WHERE   AWARD_ID = v_id AND AWARDEE_ID IS NULL;
            END IF;
            SELECT   COUNT(*) INTO l_count FROM   instalmentAndAmount  WHERE   award_id = v_id;

			SELECT   INST_AMOUNT_l_SEQUENCE_ID.NEXTVAL INTO   l_INST_AMOUNT_l_SEQUENCE_ID FROM   DUAL;

            IF l_count = 0
            THEN 
            SELECT   INST_AMOUNT_ID_SEQ.NEXTVAL INTO   l_inst_Amount_Id_seq FROM   DUAL;
            
            INSERT INTO instalmentAndAmount ( INSTALLMENTANDAMOUNT_ID, award_id ) VALUES (l_inst_Amount_Id_seq, v_id);
			if(p_SEQUENCE_ID=0)
             then
               INSERT INTO installment (INSTALLMENTANDAMOUNT_ID,INSTALLMENTSTART_DATE,INSTALLMENTEND_DATE,AMOUNT,CURRENCY,SEQUENCE_ID)
                 VALUES   (l_inst_Amount_Id_seq, p_startdate,p_enddate, p_amount, p_currency, l_INST_AMOUNT_l_SEQUENCE_ID);
			end if;
               INSERT INTO totalAmount (INSTALLMENTANDAMOUNT_ID,AMOUNT, CURRENCY )VALUES (l_inst_Amount_Id_seq, p_total_amount, p_currency);
            ELSE
               SELECT   INSTALLMENTANDAMOUNT_ID INTO   l_installmentandamount_id FROM   instalmentAndAmount WHERE   award_id = v_id;

               if(p_SEQUENCE_ID=0) then
                 INSERT INTO installment (INSTALLMENTANDAMOUNT_ID, INSTALLMENTSTART_DATE, INSTALLMENTEND_DATE, AMOUNT, CURRENCY, SEQUENCE_ID)
                 VALUES   (l_installmentandamount_id, p_startdate, p_enddate, p_amount, p_currency, l_INST_AMOUNT_l_SEQUENCE_ID);
               else
                 UPDATE   installment  SET   INSTALLMENTSTART_DATE = p_startdate,  INSTALLMENTEND_DATE = p_enddate, AMOUNT = p_amount, CURRENCY = p_currency
                 WHERE   INSTALLMENTANDAMOUNT_ID = l_INSTALLMENTANDAMOUNT_ID  AND SEQUENCE_ID = p_SEQUENCE_ID;
               end if;

               UPDATE   totalAmount SET   AMOUNT = p_total_amount, CURRENCY = p_currency  WHERE   INSTALLMENTANDAMOUNT_ID = l_INSTALLMENTANDAMOUNT_ID;

               UPDATE   amount SET   CURRENCY = p_CURRENCY, AMOUNT_TEXT = p_total_amount WHERE   AWARD_ID = v_id AND AWARDEE_ID IS NULL;
       -- ----------------------------------------------------------------------------------------------
               --           Below section added by S.S.Shah on 15 Jan 2019 to update FundingBodyAmount
               --                         when total amount is updated for an award
               --   Updated on 24 Jan 2019 to incorporate auto reducing LEAD amount functionality.
               -- ------------------------------------------ Start ---------------------------------------------
              /* sci_Lead_Amount_Update(v_id,(p_total_amount-v_aggregate_amount),p_currency,p_o_status, p_o_error); */
               -- ------------------------------------------- End ----------------------------------------------
            END IF;
            ELSEIF p_mode = 3 and p_SEQUENCE_ID>0
         THEN
           delete from installment where SEQUENCE_ID=p_SEQUENCE_ID;
		END IF;
      END IF;
         SELECT   SEQUENCE_ID, i.INSTALLMENTANDAMOUNT_ID, INSTALLMENTSTART_DATE, INSTALLMENTEND_DATE, AMOUNT, CURRENCY
           FROM   installment i, INSTALMENTANDAMOUNT im  WHERE   i.INSTALLMENTANDAMOUNT_ID = im.INSTALLMENTANDAMOUNT_ID AND award_id = v_id;
   END IF;

END