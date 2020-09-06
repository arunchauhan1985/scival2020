CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_op_relatedprogeam`(
p_workflowid       integer
)
BEGIN
   DECLARE v_id         integer;
   DECLARE v_moduleid   integer;
 
   SELECT   ID, moduleid
     INTO   v_id, v_moduleid
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;

--   OPEN p_mrel FOR
        SELECT   *
          FROM   SCI_BINARYRELATIONTYPE
      ORDER BY   VALUE;

  -- OPEN p_mhie FOR
        SELECT   *
          FROM   sci_hierarchymaster
      ORDER BY   HIERARCHY;

   IF v_moduleid = 3
   THEN
    --  OPEN mlist FOR
         SELECT   *
           FROM   relatedprograms
          WHERE   OPPORTUNITY_ID = v_id;

      -- OPEN mdata FOR
         SELECT   rs.HIERARCHY,
                  rs.RELATEDPROGRAMS_ID,
                  rm.id,
                  rm.RELTYPE,
                  rm.RELATEDPROGRAM_TEXT
           FROM   relatedprograms rs, relatedprogram rm
          WHERE   rs.RELATEDPROGRAMS_ID = rm.RELATEDPROGRAMS_ID
                  AND OPPORTUNITY_ID = v_id;

      elseif v_moduleid=4
      then
--      OPEN mlist FOR
         SELECT   *
           FROM   relatedprograms
          WHERE   AWARD_ID = v_id;

  --    OPEN mdata FOR
         SELECT   rs.HIERARCHY,
                  rs.RELATEDPROGRAMS_ID,
                  rm.id,
                  rm.RELTYPE,
                  rm.RELATEDPROGRAM_TEXT
           FROM   relatedprograms rs, relatedprogram rm
          WHERE   rs.RELATEDPROGRAMS_ID = rm.RELATEDPROGRAMS_ID
                  AND AWARD_ID = v_id;
   END IF;
END