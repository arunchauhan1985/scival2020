CREATE PROCEDURE `sci_op_relatedopplist_prc`(
	p_workflowid       INTEGER
)
BEGIN
	DECLARE v_OPPORTUNITYID    INTEGER;
	DECLARE v_moduleid         INTEGER;
	DECLARE v_fundingbody_id   INTEGER;

   SELECT   ID, moduleid
     INTO   v_OPPORTUNITYID, v_moduleid
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;

	SELECT   * FROM SCI_OPPORTUNITY_RELATION_TYPE;

   IF v_moduleid = 3
   THEN
   
      SELECT   fundingbodyid
        INTO   v_fundingbody_id
        FROM   opportunity_master
       WHERE   opportunityid = v_OPPORTUNITYID;
       
        SELECT   opportunityid, opportunityname
           FROM   opportunity_master
          WHERE   --fundingbodyid = v_fundingbody_id and
                   opportunityid <> v_OPPORTUNITYID
                  AND NVL (STATUSCODE, 1) <> 3;

          SELECT   relid ,ort.relation_name,RELATED_OPP_ID, sro.OPPORTUNITYNAME,sro.description
           FROM   sci_related_opportunity sro, opportunity_master opm,SCI_OPPORTUNITY_RELATION_TYPE ort
          WHERE   sro.RELATED_OPP_ID = opm.OPPORTUNITYID
                  and SRO.REL_OPP_ID=ORT.RELID
                  AND NVL (opm.STATUSCODE, 1) <> 3
                  and SRO.OPPORTUNITY_ID = v_OPPORTUNITYID;
     
   ELSE IF v_moduleid = 4 THEN
     SELECT   fundingbodyid
        INTO   v_fundingbody_id
        FROM   award_master
       WHERE   awardid = v_OPPORTUNITYID;
       
        SELECT   opportunityid, opportunityname
           FROM   opportunity_master
          WHERE   --fundingbodyid = v_fundingbody_id and 
                  opportunityid <> v_OPPORTUNITYID
                  AND NVL (STATUSCODE, 1) <> 3;

          SELECT   relid ,ort.relation_name,RELATED_OPP_ID, sro.OPPORTUNITYNAME
           FROM   sci_related_opportunity sro, opportunity_master opm,SCI_OPPORTUNITY_RELATION_TYPE ort
          WHERE   sro.RELATED_OPP_ID = opm.OPPORTUNITYID
                  and SRO.REL_OPP_ID=ORT.RELID
                  AND NVL (opm.STATUSCODE, 1) <> 3
                  and SRO.Award_ID = v_OPPORTUNITYID;
      END IF;
     
   END IF;
END;
