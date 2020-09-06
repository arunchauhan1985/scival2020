CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_relatedorglist`(
   p_workflowid       integer
)
BEGIN
   DECLARE v_fundingbodyid   integer;
   DECLARE v_moduleid        integer;
    
   SELECT   ID, moduleid
     INTO   v_fundingbodyid, v_moduleid
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;
   
   SELECT * FROM SCI_BINARYRELATIONTYPE
   where value in (
   'partOf', 
   'parentOf',
   'hasPart',
   'CHANGE',
   'affiliatedWith', 
   'renamedAs', 
   'continuationOf', 
   'mergedWith', 
   'mergerOf', 
   'incrorporatedInto', 
   'incorporates', 
   'splitInto', 
   'splitFrom', 
   'isReplacedBy', 
   'replaces') ;
   
	SELECT   * FROM sci_hierarchymaster;

   IF v_moduleid = 2
   THEN
            
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
 WHERE       rd.RELATEDORGS_ID = o.RELATEDORGS_ID
         AND O.ORGDBID = fb.ORGDBID
         AND FB.FUNDINGBODY_ID = fm.FUNDINGBODY_ID
         AND rd.FUNDINGBODY_ID = v_fundingbodyid
UNION
SELECT   VENDOR_FUNDINGBODY_NAME FUNDINGBODYNAME,
         rd.HIERARCHY,
         rd.RELATEDORGS_ID,
         rd.FUNDINGBODY_ID,
         o.ORGDBID,
         o.RELTYPE,
         0 flag
  FROM   sci_related_orgs_vendor rov, relatedorgs rd, org o
 WHERE   rd.RELATEDORGS_ID = o.RELATEDORGS_ID AND rov.vendor_id = o.ORGDBID
          AND rd.FUNDINGBODY_ID = v_fundingbodyid
   ORDER BY   FUNDINGBODYNAME;
      
      SELECT   fb.ORGDBID, FUNDINGBODYNAME
           FROM   fundingbody fb, fundingbody_master fm
          WHERE       fb.FUNDINGBODY_ID = fm.FUNDINGBODY_ID
                  AND IFNULL (STATUSCODE, 1) <> 3
                  AND NOT EXISTS
                        (SELECT   *
                           FROM   org
                          WHERE   RELATEDORGS_ID IN
                                        (SELECT   RELATEDORGS_ID
                                           FROM   relatedorgs
                                          WHERE   FUNDINGBODY_ID =
                                                     v_fundingbodyid)
                                  AND ORGDBID = fb.FUNDINGBODY_ID);
      
   END IF;
END ;;
