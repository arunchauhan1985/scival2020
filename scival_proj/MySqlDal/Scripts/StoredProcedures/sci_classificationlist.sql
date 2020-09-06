CREATE PROCEDURE `sci_classificationlist`(p_workflowid       integer)
BEGIN
   DECLARE v_fundingbodyid   integer;
   DECLARE v_moduleid integer;
 
   SELECT   ID, moduleid
     INTO   v_fundingbodyid,v_moduleid
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;

if v_moduleid=2
then
	SELECT   cgp.CLASSIFICATIONGROUP_ID,
               cgp.FUNDINGBODY_ID,
               cfs.TYPE,
               cfs.CLASSIFICATIONS_ID,
               cf.FREQUENCY,
               cf.CODE,
               cf.CLASSIFICATION_TEXT
        FROM   classificationgroup cgp
               right outer join classifications cfs on cgp.CLASSIFICATIONGROUP_ID = cfs.CLASSIFICATIONGROUP_ID
               right outer join classification cf on cfs.CLASSIFICATIONS_ID = cf.CLASSIFICATIONS_ID
       WHERE  cgp.FUNDINGBODY_ID = v_fundingbodyid;

elseif v_moduleid=3
then
      SELECT   cgp.CLASSIFICATIONGROUP_ID,
               cgp.FUNDINGBODY_ID,
               cfs.TYPE,
               cfs.CLASSIFICATIONS_ID,
               cf.FREQUENCY,
               cf.CODE,
               cf.CLASSIFICATION_TEXT
        FROM   classificationgroup cgp
               right outer join classifications cfs on cgp.CLASSIFICATIONGROUP_ID = cfs.CLASSIFICATIONGROUP_ID
               right outer join classification cf on cfs.CLASSIFICATIONS_ID = cf.CLASSIFICATIONS_ID
       WHERE  cgp.OPPORTUNITY_ID = v_fundingbodyid;
       
elseif v_moduleid=4
then
      SELECT   cgp.CLASSIFICATIONGROUP_ID,
               cgp.FUNDINGBODY_ID,
               cfs.TYPE,
               cfs.CLASSIFICATIONS_ID,
               cf.FREQUENCY,
               cf.CODE,
               cf.CLASSIFICATION_TEXT
        FROM   classificationgroup cgp
               right outer join classifications cfs on cgp.CLASSIFICATIONGROUP_ID = cfs.CLASSIFICATIONGROUP_ID
               right outer join classification cf on cfs.CLASSIFICATIONS_ID = cf.CLASSIFICATIONS_ID
       WHERE  cgp.AWARD_ID = v_fundingbodyid;
end if;

END;
