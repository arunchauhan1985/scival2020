CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_language_detail_dml_prc`(
    tran_id_in           integer,  /* Use -meta option sci_language_detail.tran_id%TYPE */
    scival_id_in         integer,  /* Use -meta option sci_language_detail.scival_id%TYPE */
    column_desc_in       VARCHAR(4000),        /* Use -meta option sci_language_detail.column_desc%TYPE */
    column_id_in         integer,  /* Use -meta option sci_language_detail.column_id%TYPE */
    moduleid_in          VARCHAR(4000),           /* Use -meta option sci_language_detail.moduleid%TYPE */
    language_id_in       integer,  /* Use -meta option sci_language_detail.language_id%TYPE */
    tran_type_id_in      integer,  /* Use -meta option sci_language_detail.tran_type_id%TYPE */
    flag_in               INTEGER
)
BEGIN
   DECLARE l_tran_id integer;    /* Use -meta option sci_language_detail.tran_id%TYPE */
   DECLARE v_count     INTEGER;
 
   BEGIN
     
      IF CHAR_LENGTH(COLUMN_DESC_IN) < 4000 THEN
       SELECT COUNT(*) INTO V_COUNT FROM SCI_LANGUAGE_DETAIL WHERE RTRIM(LTRIM(TO_CHAR(COLUMN_DESC)))= RTRIM(LTRIM(TO_CHAR(COLUMN_DESC_IN)))
          AND SCIVAL_ID= scival_id_in AND COLUMN_ID=column_id_in AND LANGUAGE_ID=language_id_in;         
       
      END IF;
     END;


   IF flag_in = 1
   THEN
      
      IF column_id_in IN (2)
      THEN
         SELECT   COUNT (1)
           INTO   v_count
           FROM   SCI_LANGUAGE_DETAIL
          WHERE       COLUMN_ID = column_id_in
                  AND SCIVAL_ID = scival_id_in
                  AND LANGUAGE_ID = language_id_in;
         
      END IF;

      SET l_tran_id = sci_language_dtl_tran_id_seq.NEXTVAL;

      INSERT INTO scival.sci_language_detail (tran_id,
                                              scival_id,
                                              column_desc,
                                              column_id,
                                              moduleid,
                                              language_id,
                                              tran_type_id)
        VALUES   (l_tran_id,
                  scival_id_in,
                  column_desc_in,
                  column_id_in,
                  moduleid_in,
                  language_id_in,
                  tran_type_id_in);
   ELSEIF flag_in = 2
   THEN
      UPDATE   sci_language_detail
         SET   column_desc = column_desc_in,
               language_id = IFNULL (language_id_in, language_id),
               tran_type_id = IFNULL (tran_type_id_in, tran_type_id)
       WHERE   tran_id = tran_id_in;
   ELSEIF flag_in = 3
   THEN
      DELETE FROM   sci_language_detail
            WHERE   tran_id = tran_id_in;
   END IF;

END ;;
