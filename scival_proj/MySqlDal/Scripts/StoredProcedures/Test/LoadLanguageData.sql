CREATE PROCEDURE `LoadLanguageData`()
BEGIN
SELECT   sl_dtl.tran_id,
               sl_dtl.scival_id,
               sl_dtl.column_desc,
               sl_dtl.column_id,
               (SELECT   column_name
                  FROM   sci_column_master
                 WHERE   column_id = sl_dtl.column_id)
                  column_name,
               sl_dtl.moduleid,
               (SELECT   modulename
                  FROM   sci_modules
                 WHERE   moduleid = sl_dtl.moduleid)
                  modulename,
               sl_mast.language_id,
               sl_mast.language_group_id,
               sl_mast.language_name,
               sl_mast.language_code,
               sl_mast.code_length,
               sl_mast.priority,
               sl_dtl.tran_type_id,
               (SELECT   tran_name
                  FROM   sci_tran_type_master
                 WHERE   tran_type_id = sl_dtl.tran_type_id)
                  tran_name
        FROM   sci_language_detail sl_dtl, sci_language_master sl_mast
       WHERE       sl_dtl.language_id = sl_mast.language_id
      || (CASE WHEN tran_id_in IS NOT NULL THEN ' AND sl_dtl.tran_id = ' END)
      || tran_id_in
      || (CASE
             WHEN scival_id_in IS NOT NULL THEN ' AND sl_dtl.scival_id = '
          END)
      || scival_id_in
      || (CASE
             WHEN column_id_in IS NOT NULL THEN ' AND sl_dtl.column_id = '
          END)
      || column_id_in
      || (CASE
             WHEN moduleid_in IS NOT NULL THEN ' AND sl_dtl.moduleid = '
          END)
      || moduleid_in
      || (CASE
             WHEN language_id_in IS NOT NULL
             THEN
                ' AND sl_dtl.language_id = '
          END)
      || language_id_in
      || (CASE
             WHEN tran_type_id_in IS NOT NULL
             THEN
                ' AND sl_dtl.tran_type_id = '
          END)
      || tran_type_id_in;
END;
