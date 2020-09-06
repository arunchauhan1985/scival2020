CREATE PROCEDURE `sci_op_eclassificationins`(
p_workflowid                        INTEGER,
   p_TYPE                              VARCHAR(4000),
   p_ID                                VARCHAR(4000),
   p_ELIGIBLECLASSIFICATION_TEXT       VARCHAR(4000),
   n_TYPE                              VARCHAR(4000),
   n_ID                                VARCHAR(4000),
   n_ELIGIBLECLASSIFICATION_TEXT       VARCHAR(4000),
   P_COUNTRY_CODE                      VARCHAR(4000) /* DEFAULT NULL */ ,
   P_STATE                             VARCHAR(4000) /* DEFAULT NULL */ ,
   P_CITY                              VARCHAR(4000) /* DEFAULT NULL */ ,
   p_region_specific_id              INTEGER /* DEFAULT NULL */ ,
   p_mode                              INTEGER -- 0 = insert ,1 = update 2 = delete
)
BEGIN
   DECLARE v_moduleid             INTEGER;
   DECLARE v_id                   INTEGER;
   DECLARE V_COUNT                INTEGER;
   DECLARE x_count                INTEGER  DEFAULT  0;
   DECLARE xx_count1              INTEGER;
   DECLARE del_count              INTEGER;
   DECLARE l_regionspecific_seq   INTEGER;
   DECLARE l_region_specific_id   INTEGER;

   SELECT   moduleid, id
     INTO   v_moduleid, v_id
     FROM   sci_workflow
    WHERE   workflowid = p_workflowid;

   IF v_moduleid = 3
   THEN
      IF p_mode = 0
      THEN
         IF P_COUNTRY_CODE IS NOT NULL
         THEN
            SELECT   elgcls_regionspecific_seq.NEXTVAL
              INTO   l_regionspecific_seq
              FROM   DUAL;

            INSERT INTO elgcls_regionspecific (ID,
                                               COUNTRY_CODE,
                                               STATE,
                                               CITY)
              VALUES   (l_regionspecific_seq,
                        P_COUNTRY_CODE,
                        P_STATE,
                        P_CITY);
         END IF;
         
		set @delim = ',';
		set @firstPiece = 1 + ((length(p_ELIGIBLECLASSIFICATION_TEXT) - length(replace(p_ELIGIBLECLASSIFICATION_TEXT, @delim, ''))) / length(@delim));
		set @i = 1;
        set @secondPiece = 1 + ((length(P_id) - length(replace(P_id, @delim, ''))) / length(@delim));
		set @j = 1;

		while @i <= @firstPiece do
			set @piece1 = string_splitter(p_ELIGIBLECLASSIFICATION_TEXT, @delim, @i);

			while @j <= @secondPiece do
				set @piece2 = string_splitter(p_ELIGIBLECLASSIFICATION_TEXT, @delim, @j);

				INSERT INTO eligibilityclassification (
                                                            TYPE,
                                                            ID,
                                                            ELIGIBILITYCLASSIFICATION_TEXT,
                                                            OPPORTUNITY_ID,
                                                            region_specific_id
                             )
                    VALUES   (
                                 p_type,
                                 piece2,
                                 piece1,
                                 v_id,
                                 CASE
                                    WHEN P_COUNTRY_CODE IS NULL THEN NULL
                                    ELSE l_regionspecific_seq
                                 END
                             );
				set @j = @j + 1;
			end while;
            
			set @i = @i + 1;
		end while;
      ELSEIF p_mode = 1
      THEN
         UPDATE   elgcls_regionspecific
            SET   COUNTRY_CODE = P_COUNTRY_CODE,
                  STATE = P_STATE,
                  CITY = P_CITY
          WHERE   ID = p_region_specific_id;

         UPDATE   eligibilityclassification
            SET   TYPE = n_TYPE,
                  ID = n_ID,
                  ELIGIBILITYCLASSIFICATION_TEXT =
                     TRIM (n_ELIGIBLECLASSIFICATION_TEXT)
          WHERE   IFNULL (TYPE, 'yy1234243xxyy576567') =
                     IFNULL (p_TYPE, 'yy1234243xxyy576567')
                  AND IFNULL (TRIM (ID), 'yy1234243xxyy576567') =
                        IFNULL (TRIM (p_ID), 'yy1234243xxyy576567')
                  AND IFNULL (TRIM (ELIGIBILITYCLASSIFICATION_TEXT),
                           'yy1234243xxyy576567') =
                        IFNULL (TRIM (p_ELIGIBLECLASSIFICATION_TEXT),
                             'yy1234243xxyy576567')
                  AND OPPORTUNITY_ID = v_id;
      ELSEIF p_mode = 2
      THEN
         DELETE FROM   eligibilityclassification
               WHERE   TYPE = p_TYPE AND TRIM (ID) = TRIM (p_ID)
                       AND TRIM (ELIGIBILITYCLASSIFICATION_TEXT) =
                             TRIM (p_ELIGIBLECLASSIFICATION_TEXT)
                       AND OPPORTUNITY_ID = v_ID;

         DELETE FROM   elgcls_regionspecific
               WHERE   ID = p_region_specific_id;

      END IF;
   END IF;

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
END;
