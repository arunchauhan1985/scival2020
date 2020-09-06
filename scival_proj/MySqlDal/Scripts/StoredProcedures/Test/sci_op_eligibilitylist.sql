CREATE PROCEDURE `sci_op_eligibilitylist`(
	p_workflowid              INTEGER
)
BEGIN
   DECLARE v_OPPORTUNITYID   INTEGER;
   DECLARE v_moduleid        INTEGER;
 
   SELECT   ID, moduleid
     INTO   v_OPPORTUNITYID, v_moduleid
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;

   IF v_moduleid = 3
   THEN
         SELECT   ec.eligibilityclassification_id,
                  ec.opportunity_id,
                  ie.individualeligibility_ID,
                  ie.degree,
                  ie.graduate,
                  ie.newfaculty,
                  ie.not_specified,
                  ie.undergraduate,
                  cs.citizenship_text,
                  s.name country,
                  cs.norestriction,
                  Case  when ec.LANG is null then 'en' 
                  else ec.Lang End LANG
           FROM   eligibilityclassification ec,
                  individualeligibility ie,
                  citizenship cs,
                  SCI_COUNTRYCODES s
          WHERE   ec.opportunity_id = v_OPPORTUNITYID
                  AND ec.eligibilityclassification_id =
                        ie.eligibilityclassification_id
                  AND ie.individualeligibility_id =
                        cs.individualeligibility_id
                        AND lower(S.LCODE)=lower(cs.country);

         SELECT   ec.eligibilityclassification_id,
                  ec.opportunity_id,
                  oe.organizationeligibility_id,
                  oe.academic,
                  oe.commercial,
                  oe.government,
                  oe.nonprofit,
                  oe.not_specified,
                  oe.sme,
                  rs.city,
                  s.name country,
                  rs.norestriction,
                  rs.regionspecific_text,
                  rs.state,
                  Case  when ec.LANG is null then 'en' 
                  else ec.Lang End LANG
           FROM   eligibilityclassification ec,
                  organizationeligibility oe,
                  regionspecific rs,
                  SCI_COUNTRYCODES s
          WHERE   ec.opportunity_id = v_opportunityid
                  AND ec.eligibilityclassification_id =
                        oe.eligibilityclassification_id
                  AND oe.organizationeligibility_id =
                        rs.organizationeligibility_id
                         AND lower(S.LCODE)=lower(cs.country);

        SELECT   ec.eligibilityclassification_id,
                  ec.opportunity_id,                 
                  rt.disabilities,
                  rt.invitationonly,
                  rt.memberonly,
                  rt.nominationonly,
                  rt.minorties,
                  rt.women,
                  rt.restrictions_id,
                  rt.not_specified,
                  ls.numberofapplicantsallowed,
                  Case  when ec.LANG is null then 'en' 
                  else ec.Lang End LANG
           FROM   eligibilityclassification ec,
                  restrictions rt,
                  limitedsubmission ls
          WHERE   ec.opportunity_id = v_opportunityid
                  AND ec.eligibilityclassification_id =
                        RT.ELIGIBILITYCLASSIFICATION_ID
                  AND RT.RESTRICTIONS_ID = LS.RESTRICTION_ID;
   END IF;
END;
