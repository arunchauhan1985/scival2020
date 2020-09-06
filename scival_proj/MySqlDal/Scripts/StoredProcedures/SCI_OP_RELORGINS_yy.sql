CREATE DEFINER=`root`@`localhost` PROCEDURE `SCI_OP_RELORGINS_yy`(
   p_workflowid          INTEGER,
   p_amount              DOUBLE /* DEFAULT NULL */ ,  -- - changes version 4.0 schema
   p_currency            VARCHAR(4000) /* DEFAULT NULL */ ,
   p_insdel              INTEGER,
   p_HIERARCHY           VARCHAR(4000),            -- - can be 'lead' or 'component'
   p_orgdbid             VARCHAR(4000),
   p_RELTYPE             VARCHAR(4000),
   p_relatedorgsid       VARCHAR(4000) /* DEFAULT NULL */  
)
sp_lbl:

BEGIN
	DECLARE v_count                  INTEGER;
	DECLARE v_count2                 INTEGER;
	DECLARE v_count3                 INTEGER;
	DECLARE v_OPPORTUNITYID          INTEGER;
	DECLARE v_value                  INTEGER;
	DECLARE v_counter1               INTEGER  DEFAULT  0;
	DECLARE v_counter                INTEGER  DEFAULT  0;
	DECLARE v_orgcunt                INTEGER;
	DECLARE v_ORG_TEXT               VARCHAR (2000);
	DECLARE v_MODULEID               INTEGER;
	DECLARE l_relatedorgs_count      INTEGER;
	DECLARE v_validation             INTEGER  DEFAULT  0;
	DECLARE l_relatedfunbodies_seq   INTEGER;

	DECLARE v_totalamount            INTEGER;
	DECLARE v_totalcurrency          VARCHAR (200);
	DECLARE v_type                   VARCHAR (20);
	DECLARE v_leadcount              INTEGER  DEFAULT  0;
	DECLARE v_del_amount             INTEGER;

	DECLARE v_lead_currency          VARCHAR (200);
	DECLARE v_lead_amount            INTEGER;

	DECLARE v_leadiszero             INTEGER  DEFAULT  0;
	DECLARE NOT_FOUND INT DEFAULT 0;

	DECLARE CONTINUE HANDLER FOR NOT FOUND SET NOT_FOUND = 1; 
   
	SELECT   ID, MODULEID INTO   v_OPPORTUNITYID, v_MODULEID FROM   sci_workflow WHERE   WORKFLOWID = p_workflowid;

	IF v_moduleid = 3
	THEN
		IF p_insdel = 0
		THEN         
			IF p_HIERARCHY = 'component'
			THEN
				SELECT COUNT(*) INTO   l_relatedorgs_count FROM   relatedorgs
				WHERE   OPPORTUNITY_ID = v_OPPORTUNITYID AND HIERARCHY = 'lead';

				IF l_relatedorgs_count = 0
				THEN
					SET v_validation = 1;
				END IF;
				ELSEIF p_HIERARCHY = 'lead'
				THEN
					SELECT   COUNT(*) INTO   l_relatedorgs_count FROM   relatedorgs
					WHERE   OPPORTUNITY_ID = v_OPPORTUNITYID AND HIERARCHY = 'lead';

					IF l_relatedorgs_count > 0
					THEN
						SET v_validation = 1;
					END IF;
				END IF;
				
                IF v_validation = 0
				THEN
					/*DECLARE i CURSOR FOR SELECT   list FROM table (sci_getcsvtoLIST (p_orgdbid));
					OPEN i;
					FETCH i INTO;
					WHILE NOT_FOUND=0
					DO  */          
						IF p_HIERARCHY = 'lead'
						THEN
							IF v_leadcount > 0
							THEN
								SET v_validation = 1;
								ROLLBACK;
								LEAVE sp_lbl;
							END IF;
							SET v_leadcount = v_leadcount + 1;
						END IF;

						SELECT   RELATEDORGS_SEQ.NEXTVAL INTO v_value FROM DUAL;

						INSERT INTO relatedorgs (
                                           HIERARCHY,
                                           RELATEDORGS_ID,
                                           OPPORTUNITY_ID)
						VALUES   (p_HIERARCHY, v_value, v_OPPORTUNITYID);

/*						
                        SELECT   FUNDINGBODYNAME INTO v_ORG_TEXT FROM (
						SELECT   FUNDINGBODYNAME
						FROM   fundingbody_master
						WHERE  StatusCode <> 45 and  FUNDINGBODY_ID = (
						SELECT FUNDINGBODY_ID FROM fundingbody WHERE ORGDBID = i.list)
                UNION   
						SELECT  vendor_fundingbody_name FUNDINGBODYNAME
						FROM   sci_related_orgs_vendor where vendor_id = i.list
						) LIMIT 1;

						INSERT INTO org (ORGDBID,
                                RELTYPE,
                                ORG_TEXT,
                                RELATEDORGS_ID)
						VALUES   (i.list,
							   p_RELTYPE,
							   v_ORG_TEXT,
							   v_value);
				FETCH  INTO;
				END WHILE;
				CLOSE ;*/
				END IF;
			ELSEIF p_insdel = 1
			THEN
				SELECT   HIERARCHY
				INTO   v_type
				FROM   relatedorgs
				WHERE   RELATEDORGS_ID = p_relatedorgsid;

				DELETE FROM   org
				WHERE   RELATEDORGS_ID = p_relatedorgsid;
				
                DELETE FROM   relatedorgs
				WHERE   RELATEDORGS_ID = p_relatedorgsid;               
			END IF;

			SELECT   fb.ORGDBID, FUNDINGBODYNAME
			FROM   fundingbody fb, fundingbody_master fm
			WHERE   fb.FUNDINGBODY_ID = fm.FUNDINGBODY_ID
                  AND IFNULL (STATUSCODE, 1) <> 3
                  AND NOT EXISTS
                        (SELECT   *
                           FROM   org
                          WHERE   RELATEDORGS_ID IN
                                        (SELECT   RELATEDORGS_ID
                                           FROM   relatedorgs
                                          WHERE   OPPORTUNITY_ID =
                                                     v_OPPORTUNITYID)
                                  AND ORGDBID = fb.FUNDINGBODY_ID)
            UNION							
				SELECT vendor_id ORGDBID, vendor_fundingbody_name FUNDINGBODYNAME FROM   sci_related_orgs_vendor;

				SELECT   FUNDINGBODYNAME,
                  rd.HIERARCHY,
                  rd.RELATEDORGS_ID,
                  rd.FUNDINGBODY_ID,
                  o.ORGDBID,
                  o.RELTYPE,
                  (CASE WHEN fm.STATUSCODE = 3 THEN 1 ELSE 0 END) flag
				FROM   relatedorgs rd,
                  org o,
                  fundingbody fb,
                  fundingbody_master fm
				WHERE rd.RELATEDORGS_ID = o.RELATEDORGS_ID
                  AND O.ORGDBID = fb.ORGDBID
                  AND FB.FUNDINGBODY_ID = fm.FUNDINGBODY_ID
                  AND rd.OPPORTUNITY_ID = v_OPPORTUNITYID
                  And fm.StatusCode<>45
			UNION
                 SELECT   vendor_fundingbody_name FUNDINGBODYNAME,
                  rd.HIERARCHY,
                  rd.RELATEDORGS_ID,
                  rd.FUNDINGBODY_ID,
                  o.ORGDBID,
                  o.RELTYPE,
                  0 flag
				FROM   relatedorgs rd,
                  org o,
                  sci_related_orgs_vendor rov
				WHERE  rd.RELATEDORGS_ID = o.RELATEDORGS_ID
					AND O.ORGDBID = rov.vendor_id
					AND rd.OPPORTUNITY_ID =v_OPPORTUNITYID;
                  
		ELSEIF v_moduleid = 4
		THEN
			IF p_insdel = 0
			THEN
				IF p_HIERARCHY = 'component'
				THEN
					SELECT   COUNT(*)
					INTO   l_relatedorgs_count
					FROM   relatedorgs
					WHERE   AWARD_ID = v_OPPORTUNITYID AND HIERARCHY = 'lead';

				IF l_relatedorgs_count = 0
				THEN
					SET v_validation = 1;					
				END IF;
			ELSEIF p_HIERARCHY = 'lead'
			THEN
				SELECT   COUNT(*)
				INTO   l_relatedorgs_count
				FROM   relatedorgs
				WHERE   AWARD_ID = v_OPPORTUNITYID AND HIERARCHY = 'lead';

				IF l_relatedorgs_count > 0
				THEN
					SET v_validation = 1;					
				END IF;
			END IF;
         
			SET v_totalcurrency = NULL;
            
            SELECT   Currency
            INTO   v_totalcurrency
            FROM   TotalAmount ta, instalmentandamount ia
            WHERE   IA.INSTALLMENTANDAMOUNT_ID = TA.INSTALLMENTANDAMOUNT_ID
                     AND IA.AWARD_ID = v_OPPORTUNITYID;            

			SET v_lead_currency = NULL;
			SELECT   FBA.CURRENCY
            INTO   v_lead_currency
            FROM   FUNDINGBODYAMOUNT FBA, RELATEDFUNDINGBODIES RFB
            WHERE   RFB.RELEATEDFUNDINGBODIES_ID =
                        FBA.RELEATEDFUNDINGBODIES_ID
                     AND RFB.AWARD_ID = v_OPPORTUNITYID
                     AND RFB.HIERARCHY = 'lead';
            
			IF v_lead_currency IS NOT NULL
			THEN
				IF p_currency <> v_lead_currency
				THEN
					SET v_validation = 1;
				END IF;
			END IF;

			IF v_validation = 0
			THEN            
				/*DECLARE i CURSOR FOR SELECT   list FROM table (sci_getcsvtoLIST (p_orgdbid));
				OPEN i;
				FETCH i INTO;
				WHILE NOT_FOUND=0
				DO
				IF p_HIERARCHY = 'lead'
				THEN
					IF v_leadcount > 0
					THEN						 
						 SET v_validation = 1;						 
					END IF;

					SET v_leadcount = v_leadcount + 1;
			   END IF;*/

				SELECT   RELATEDORGS_SEQ.NEXTVAL INTO v_value FROM DUAL;

				INSERT INTO relatedorgs (HIERARCHY, RELATEDORGS_ID, AWARD_ID)
				VALUES   (p_HIERARCHY, v_value, v_OPPORTUNITYID);

				/*SELECT   FUNDINGBODYNAME INTO v_ORG_TEXT FROM ( SELECT   FUNDINGBODYNAME
					FROM   fundingbody_master
					WHERE  StatusCode <> 45 and  FUNDINGBODY_ID = (
					SELECT FUNDINGBODY_ID FROM fundingbody WHERE ORGDBID = i.list)
                    
                union  
                
				SELECT  vendor_fundingbody_name FUNDINGBODYNAME
					FROM   sci_related_orgs_vendor where vendor_id = i.list
                ) LIMIT 1;

				INSERT INTO org (ORGDBID,
                                RELTYPE,
                                ORG_TEXT,
                                RELATEDORGS_ID)
                VALUES   (i.list,
                           p_RELTYPE,
                           v_ORG_TEXT,
                           v_value);

				SELECT   relatedfunbodies_seq.NEXTVAL
                 INTO   l_relatedfunbodies_seq
                 FROM   DUAL;

               INSERT INTO relatedFundingBodies (HIERARCHY,
                                                 RELEATEDFUNDINGBODIES_ID,
                                                 AWARD_ID,
                                                 RELATEDORGSSS_ID)
                 VALUES   (p_HIERARCHY,
                           l_relatedfunbodies_seq,
                           v_OPPORTUNITYID,
                           v_value);

				IF p_HIERARCHY = 'lead'
               THEN
                  SELECT   COUNT(*)
                    INTO   v_count2
                    FROM   TotalAmount
                   WHERE   INSTALLMENTANDAMOUNT_ID IN
                                 (SELECT   INSTALLMENTANDAMOUNT_ID
                                    FROM   InstalmentAndAmount
                                   WHERE   Award_id = v_OPPORTUNITYID);

                  IF v_count2 > 0
                  THEN
                     SELECT   amount, currency
                       INTO   v_totalamount, v_totalcurrency
                       FROM   TotalAmount
                      WHERE   INSTALLMENTANDAMOUNT_ID =
                                 (SELECT   INSTALLMENTANDAMOUNT_ID
                                    FROM   InstalmentAndAmount
                                   WHERE   Award_id = v_OPPORTUNITYID);

                     INSERT INTO fundingBodyAmount (
                                                       AMOUNT,
                                                       CURRENCY,
                                                       RELEATEDFUNDINGBODIES_ID
                                )
                       VALUES   (
                                    v_totalamount,
                                    v_totalcurrency,
                                    l_relatedfunbodies_seq
                                );
                  ELSE
                     SET v_validation = 1;                     
                  END IF;
               ELSE
                  SELECT   Amount, Currency
                    INTO   v_totalamount, v_totalcurrency
                    FROM   fundingbodyamount
                   WHERE   RELEATEDFUNDINGBODIES_ID =
                              (SELECT   RELEATEDFUNDINGBODIES_ID
                                 FROM   relatedfundingbodies
                                WHERE   RELATEDORGSSS_ID =
                                           (SELECT   RELATEDORGS_ID
                                              FROM   relatedorgs
                                             WHERE   award_id =
                                                        v_OPPORTUNITYID
                                                     AND hierarchy = 'lead'));

                  IF v_totalamount IS NULL
                  THEN
                     SET v_totalamount = 0;
                  END IF;

                  IF v_totalcurrency IS NULL
                  THEN
                     SET v_totalcurrency = p_currency;

                     IF v_totalcurrency IS NULL
                     THEN
                        SET v_totalcurrency = 'USD';
                     END IF;
                  END IF;

                  IF p_amount IS NOT NULL
                  THEN
                     IF v_totalamount < p_amount
                     THEN
                        SET v_validation = 1;                     
                     ELSE
                        
                        UPDATE   fundingbodyamount
                           SET   Amount = (v_totalamount - p_amount)
                         WHERE   RELEATEDFUNDINGBODIES_ID =
                                    (SELECT   RELEATEDFUNDINGBODIES_ID
                                       FROM   relatedfundingbodies
                                      WHERE   RELATEDORGSSS_ID =
                                                 (SELECT   RELATEDORGS_ID
                                                    FROM   relatedorgs
                                                   WHERE   award_id =
                                                              v_OPPORTUNITYID
                                                           AND hierarchy =
                                                                 'lead'));

                        INSERT INTO fundingBodyAmount (
                                                          AMOUNT,
                                                          CURRENCY,
                                                          RELEATEDFUNDINGBODIES_ID
                                   )
                          VALUES   (
                                       p_amount,
                                       p_currency,
                                       l_relatedfunbodies_seq
                                   );
                     END IF;
                  ELSE                                   
                     INSERT INTO fundingBodyAmount (
                                                       AMOUNT,
                                                       CURRENCY,
                                                       RELEATEDFUNDINGBODIES_ID
                                )
                       VALUES   (0, v_totalcurrency, l_relatedfunbodies_seq);
                  END IF;
               END IF;
            
            FETCH  INTO;
            END WHILE;
            CLOSE ;*/

            SELECT   Amount, Currency
              INTO   v_totalamount, v_totalcurrency      
              FROM   fundingbodyamount
             WHERE   RELEATEDFUNDINGBODIES_ID =
                        (SELECT   RELEATEDFUNDINGBODIES_ID
                           FROM   relatedfundingbodies
                          WHERE   RELATEDORGSSS_ID =
                                     (SELECT   RELATEDORGS_ID
                                        FROM   relatedorgs
                                       WHERE   award_id = v_OPPORTUNITYID
                                               AND hierarchy = 'lead'));

            IF v_totalamount < 1
            THEN
               SET v_leadiszero = 1;
            END IF;
         END IF;
      
      ELSEIF p_insdel = 1
      THEN                                                           
                  
         SELECT   HIERARCHY
           INTO   v_type
           FROM   relatedorgs
          WHERE   RELATEDORGS_ID = p_relatedorgsid;
         
         SELECT   AMOUNT
           INTO   v_del_amount
           FROM   fundingBodyAmount
          WHERE   RELEATEDFUNDINGBODIES_ID =
                     (SELECT   RELEATEDFUNDINGBODIES_ID
                        FROM   relatedFundingBodies
                       WHERE   RELATEDORGSSS_ID = p_relatedorgsid);

         UPDATE   fundingbodyamount
            SET   Amount = (Amount + v_del_amount)
          WHERE   RELEATEDFUNDINGBODIES_ID =
                     (SELECT   RELEATEDFUNDINGBODIES_ID
                        FROM   relatedfundingbodies
                       WHERE   RELATEDORGSSS_ID =
                                  (SELECT   RELATEDORGS_ID
                                     FROM   relatedorgs
                                    WHERE   award_id = v_OPPORTUNITYID
                                            AND hierarchy = 'lead'));

         DELETE FROM   org
               WHERE   RELATEDORGS_ID = p_relatedorgsid;

         DELETE FROM  relatedFundingBodies
          WHERE   RELATEDORGSSS_ID = p_relatedorgsid;
      
         DELETE FROM   relatedorgs
               WHERE   RELATEDORGS_ID = p_relatedorgsid;

         DELETE FROM  fundingBodyAmount
          WHERE   RELEATEDFUNDINGBODIES_ID IN
                        (SELECT   RELEATEDFUNDINGBODIES_ID
                           FROM   relatedFundingBodies
                          WHERE   RELATEDORGSSS_ID = p_relatedorgsid);
      ELSEIF p_insdel = 2
      THEN
         IF p_relatedorgsid IS NULL
         THEN
            SET v_validation = 1;            
         END IF;

         IF p_HIERARCHY = 'lead'
         THEN
            SET v_validation = 1;            
         END IF;

         SELECT   HIERARCHY
           INTO   v_type
           FROM   relatedorgs
          WHERE   RELATEDORGS_ID = p_relatedorgsid;

         IF v_type = 'lead'
         THEN
            SET v_validation = 1;            
         END IF;

               SET v_totalcurrency = NULL;
            SELECT   Currency
              INTO   v_totalcurrency
              FROM   TotalAmount ta, instalmentandamount ia
             WHERE   IA.INSTALLMENTANDAMOUNT_ID = TA.INSTALLMENTANDAMOUNT_ID
                     AND IA.AWARD_ID = v_OPPORTUNITYID;
        
         
               SET v_lead_currency = NULL;
            SELECT   FBA.CURRENCY
              INTO   v_lead_currency
              FROM   FUNDINGBODYAMOUNT FBA, RELATEDFUNDINGBODIES RFB
             WHERE   RFB.RELEATEDFUNDINGBODIES_ID =
                        FBA.RELEATEDFUNDINGBODIES_ID
                     AND RFB.AWARD_ID = v_OPPORTUNITYID
                     AND RFB.HIERARCHY = 'lead';                 

         IF v_lead_currency IS NOT NULL
         THEN
            IF p_currency <> v_lead_currency
            THEN
               SET v_validation = 1;
            END IF;
         END IF;

         IF v_validation = 0
         THEN
            IF CHAR_LENGTH (LTRIM (RTRIM (p_RELTYPE))) > 0
            THEN
               /*DECLARE i CURSOR
               FOR SELECT   list FROM table (sci_getcsvtoLIST (p_orgdbid));
               OPEN i;
               FETCH i INTO;
               WHILE NOT_FOUND=0
               DO                 
                                              
            SELECT   FUNDINGBODYNAME INTO v_ORG_TEXT FROM (
               SELECT   FUNDINGBODYNAME
                 FROM   fundingbody_master
                WHERE  StatusCode <> 45 and  FUNDINGBODY_ID = (
                  SELECT FUNDINGBODY_ID FROM fundingbody WHERE ORGDBID = i.list)
                union  
                SELECT  vendor_fundingbody_name FUNDINGBODYNAME
                FROM   sci_related_orgs_vendor where vendor_id = i.list
                ) LIMIT 1;

                  IF CHAR_LENGTH (LTRIM (RTRIM (v_ORG_TEXT))) > 0
                  THEN
                     SELECT   COUNT(*)
                       INTO   v_count2
                       FROM   ORG
                      WHERE   RELATEDORGS_ID = p_relatedorgsid
                              AND ORGDBID = i.list;

                     IF v_count2 > 0
                     THEN
                        UPDATE   ORG
                           SET   RELTYPE = p_RELTYPE, ORG_TEXT = v_ORG_TEXT
                         WHERE   RELATEDORGS_ID = p_relatedorgsid
                                 AND ORGDBID = i.list;                     
                     ELSE                        
                        SET v_validation = 1;                        
                     END IF;
                  END IF;*/

                  SELECT   AMOUNT
                    INTO   v_del_amount
                    FROM   fundingBodyAmount
                   WHERE   RELEATEDFUNDINGBODIES_ID =
                              (SELECT   RELEATEDFUNDINGBODIES_ID
                                 FROM   relatedFundingBodies
                                WHERE   RELATEDORGSSS_ID = p_relatedorgsid);

                  IF v_del_amount IS NOT NULL
                  THEN
                     SET v_del_amount = p_amount - v_del_amount;      
                  ELSE
                     SET v_del_amount = p_amount;
                  END IF;
                  
                  SELECT   Amount
                    INTO   v_lead_amount
                    FROM   fundingbodyamount
                   WHERE   RELEATEDFUNDINGBODIES_ID =
                              (SELECT   RELEATEDFUNDINGBODIES_ID
                                 FROM   relatedfundingbodies
                                WHERE   RELATEDORGSSS_ID =
                                           (SELECT   RELATEDORGS_ID
                                              FROM   relatedorgs
                                             WHERE   award_id =
                                                        v_OPPORTUNITYID
                                                     AND hierarchy = 'lead'));

                  IF v_del_amount > v_lead_amount
                  THEN
                     SET v_validation = 1;					
                  END IF;

                  IF v_validation = 0
                  THEN
                     
                           SET l_relatedfunbodies_seq = NULL;
                       
                        SELECT   RELEATEDFUNDINGBODIES_ID
                          INTO   l_relatedfunbodies_seq
                          FROM   RELATEDFUNDINGBODIES
                         WHERE   RELATEDORGSSS_ID = p_relatedorgsid;
                     
                     IF l_relatedfunbodies_seq IS NULL
                     THEN
                        SELECT   relatedfunbodies_seq.NEXTVAL
                          INTO   l_relatedfunbodies_seq
                          FROM   DUAL;

                        INSERT INTO relatedFundingBodies (
                                                             HIERARCHY,
                                                             RELEATEDFUNDINGBODIES_ID,
                                                             AWARD_ID,
                                                             RELATEDORGSSS_ID
                                   )
                          VALUES   (p_HIERARCHY,
                                    l_relatedfunbodies_seq,
                                    v_OPPORTUNITYID,
                                    p_relatedorgsid);
                     END IF;

                     SET v_lead_amount = v_lead_amount - v_del_amount;

                     UPDATE   fundingbodyamount
                        SET   Amount = v_lead_amount
                      WHERE   RELEATEDFUNDINGBODIES_ID =
                                 (SELECT   RELEATEDFUNDINGBODIES_ID
                                    FROM   relatedfundingbodies
                                   WHERE   RELATEDORGSSS_ID =
                                              (SELECT   RELATEDORGS_ID
                                                 FROM   relatedorgs
                                                WHERE   award_id =
                                                           v_OPPORTUNITYID
                                                        AND hierarchy =
                                                              'lead'));
                     
                     SELECT   COUNT(*)
                       INTO   v_count2
                       FROM   fundingBodyAmount
                      WHERE   RELEATEDFUNDINGBODIES_ID =
                                 l_relatedfunbodies_seq;

                     IF v_count2 > 0
                     THEN
                        UPDATE   fundingBodyAmount
                           SET   AMOUNT = p_amount, CURRENCY = p_currency
                         WHERE   RELEATEDFUNDINGBODIES_ID =
                                    l_relatedfunbodies_seq;
                     ELSE
                        INSERT INTO fundingBodyAmount (
                                                          AMOUNT,
                                                          CURRENCY,
                                                          RELEATEDFUNDINGBODIES_ID
                                   )
                          VALUES   (
                                       p_amount,
                                       p_currency,
                                       l_relatedfunbodies_seq
                                   );
                     END IF;
                  END IF;
               /*FETCH  INTO;
               END WHILE;
               CLOSE ;*/
            END IF;
            
            SELECT   Amount, Currency
              INTO   v_totalamount, v_totalcurrency
              FROM   fundingbodyamount
             WHERE   RELEATEDFUNDINGBODIES_ID =
                        (SELECT   RELEATEDFUNDINGBODIES_ID
                           FROM   relatedfundingbodies
                          WHERE   RELATEDORGSSS_ID =
                                     (SELECT   RELATEDORGS_ID
                                        FROM   relatedorgs
                                       WHERE   award_id = v_OPPORTUNITYID
                                               AND hierarchy = 'lead'));

            IF v_totalamount < 1
            THEN
               SET v_leadiszero = 1;
            END IF;
         END IF;                                            
      
      END IF;

         SELECT   fb.ORGDBID, FUNDINGBODYNAME
           FROM   fundingbody fb, fundingbody_master fm
          WHERE   fb.FUNDINGBODY_ID = fm.FUNDINGBODY_ID
                  AND IFNULL (STATUSCODE, 1) <> 3
                  AND NOT EXISTS
                        (SELECT   *
                           FROM   org
                          WHERE   RELATEDORGS_ID IN
                                        (SELECT   RELATEDORGS_ID
                                           FROM   relatedorgs
                                          WHERE   AWARD_ID = v_OPPORTUNITYID)
                                  AND ORGDBID = fb.FUNDINGBODY_ID);

         SELECT   FUNDINGBODYNAME,
                  rd.HIERARCHY,
                  rd.RELATEDORGS_ID,
                  rd.FUNDINGBODY_ID,
                  o.ORGDBID,
                  o.RELTYPE,
                  FBA.AMOUNT,          -- - ADDED FOR VERSION 4.0 ON 7-JUN-2018
                  FBA.CURRENCY,        -- - ADDED FOR VERSION 4.0 ON 7-JUN-2018
                  (CASE WHEN fm.STATUSCODE = 3 THEN 1 ELSE 0 END) flag
           FROM   relatedorgs rd,
                  org o,
                  fundingbody fb,
                  fundingbody_master fm,
                  relatedFundingBodies RFB 
                  LEFT JOIN fundingBodyAmount FBA
                  ON FBA.RELEATEDFUNDINGBODIES_ID = RFB.RELEATEDFUNDINGBODIES_ID
          WHERE       rd.RELATEDORGS_ID = o.RELATEDORGS_ID
                  AND O.ORGDBID = fb.ORGDBID
                  AND FB.FUNDINGBODY_ID = fm.FUNDINGBODY_ID                  
                  AND RFB.RELATEDORGSSS_ID = RD.RELATEDORGS_ID
                  AND rd.AWARD_ID = v_OPPORTUNITYID;
   END IF;   
END ;;
