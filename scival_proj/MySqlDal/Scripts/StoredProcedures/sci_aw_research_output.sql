CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_aw_research_output`(
   p_workflowid       INTEGER
)
BEGIN
   DECLARE v_id         INTEGER;
   DECLARE v_moduleid   INTEGER;
    
   SELECT   ID, moduleid
     INTO   v_id, v_moduleid
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;

 --  OPEN p_soit FOR
        SELECT   ID, NAME
          FROM   SCI_SCHOLAR_OUTPUT_ITEM_TYPE
      ORDER BY   ID;

   -- OPEN p_soitt FOR
        SELECT   ID, NAME
          FROM   SCI_SCHOLAR_ITEM_TYPE_type
      ORDER BY   ID;

   -- OPEN p_sort FOR
        SELECT   ID, NAME
          FROM   SCI_output_relation_type
      ORDER BY   ID;

   IF v_moduleid = 4
   THEN
      -- OPEN mdata FOR
            SELECT   ID,
                    RELTYPE,
                    TYPE,
                    DOI,
                    PUBMEDID,
                    PMCID,
                    MEDLINEID,
                    SCOPUSID,
                    listagg (itd.ITEMID_COLUMN, ',')
                       -- WITHIN GROUP (ORDER BY itd.ITEMTEST_ID)
                       -- ITEMID_COLUMN
             FROM   researchoutcome ro, itemtest it, itemid itd
            WHERE       RO.AWARD_ID = v_id
                    AND RO.RESEARCHOUTCOME_ID = it.RESEARCHOUTCOME_ID
                    AND it.ITEMTEST_ID = itd.ITEMTEST_ID
         GROUP BY   ID,
                    RELTYPE,
                    TYPE,
                    DOI,
                    PUBMEDID,
                    PMCID,
                    MEDLINEID,
                    SCOPUSID;
   END IF;
END