CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_op_QCcheck_final_S50_Final`(
   p_tran_type_id                       INTEGER,
   p_workflowid                         INTEGER
)
BEGIN
   DECLARE v_id           INTEGER;
   DECLARE v_moduleid     INTEGER;
   DECLARE l_count        INTEGER;
   DECLARE v_count2       INTEGER;
   DECLARE v_statuscode   INTEGER;
   DECLARE v_desc         LONGTEXT;
   DECLARE v_lsub         VARCHAR (100);
   DECLARE v_ldecid       INTEGER;
   DECLARE v_count3       INTEGER;
   DECLARE v_reltype      VARCHAR (2000);
   DECLARE V_TF           VARCHAR (50);
   DECLARE v_ppf          VARCHAR (50);
 
   SELECT   ID, MODULEID
     INTO   v_id, v_moduleid
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;
  IF v_moduleid = 3 AND v_id IS NOT NULL
   THEN
      SELECT   statuscode
        INTO   v_statuscode
        FROM   opportunity_master
       WHERE   opportunityid = v_id;
      IF v_statuscode IN (1, 2)
      THEN
         SELECT   COUNT(1)
           INTO   v_count2
           FROM   x  -- change
          WHERE   CHANGEHISTORY_ID = (SELECT   CHANGEHISTORY_ID
                                        FROM   changehistory
                                       WHERE   opportunity_id = v_id);

         IF v_count2 < 1
         THEN
            SELECT   COUNT(1)
              INTO   l_count
              FROM   sci_opp_loi_duedate_detail
             WHERE   OPPORTUNITY_ID = v_id AND date_type IN (4, 5);
         END IF;
      END IF;
   END IF;
 
 SELECT   COUNT(*)
     INTO   v_count2
     FROM   Opp_SubjectMatter
    WHERE   opportunity_id = v_id;
 
 SELECT   COUNT(*)
     INTO   v_count2
     FROM   item
    WHERE   SUBJECTMATTER_ID IN (SELECT   SUBJECTMATTER_ID
                                   FROM   Opp_SubjectMatter
                                  WHERE   opportunity_id = v_id);

   IF v_count2 > 1
   THEN      
      SELECT   DESCRIPTION
        INTO   v_desc
        FROM   item
       WHERE   SUBJECTMATTER_ID IN (SELECT   SUBJECTMATTER_ID
                                      FROM   Opp_SubjectMatter
                                     WHERE   opportunity_id = v_id)
               LIMIT 1;     
   END IF;
   
SELECT   IFNULL (LIMITEDSUBMISSION, 'false')
     INTO   v_lsub
     FROM   OPPORTUNITY
    WHERE   opportunity_id = v_id;
   
   IF v_lsub = 'true'
   THEN
      SELECT   COUNT(*)
        INTO   v_count2
        FROM   LIMITEDSUBMISSIONDESCRIPTION
       WHERE   OPPORTUNITY_ID = v_id;
       
      IF v_count2 > 1
      THEN       
         SELECT   LIMITEDSUBMISSIONDESC_ID
           INTO   v_ldecid
           FROM   LIMITEDSUBMISSIONDESCRIPTION
          WHERE   OPPORTUNITY_ID = v_id 
         LIMIT 1;

         SELECT   COUNT(*)
           INTO   v_count2
           FROM   item
          WHERE   LIMITEDSUBMISSIONDESC_ID = v_ldecid;

         IF v_count3 > 1
         THEN
            SELECT   reltype
              INTO   v_reltype
              FROM   item
             WHERE   LIMITEDSUBMISSIONDESC_ID = v_ldecid
            LIMIT 1;
		 END IF;
      END IF;
   END IF;
SELECT   COUNT(1)
     INTO   v_count2
     FROM   SCI_OPP_LOI_DUEDATE_DETAIL
    WHERE       DATE_TYPE = (SELECT   TYPE_ID
                               FROM   MST_DATE_TYPE
                              WHERE   name = 'DueDate')
            AND LOI_DUE_DATE IS NOT NULL
            AND opportunity_id = v_id;           
      SELECT   ID,
               CASE
                  WHEN FUNDINGBODYOPPORTUNITYID = 'Not Available'
                  THEN
                     'NAOPP'
                  ELSE
                     FUNDINGBODYOPPORTUNITYID
               END
                  FUNDINGBODYOPPORTUNITYID,
               LIMITEDSUBMISSION,
               TRUSTING,
               COLLECTIONCODE,
               HIDDEN,
               RECORDSOURCE,
               LOI_MANDATORY LOIMANDATORY,
               ELIGIBILITYCATEGORY,
               LINKTOFULLTEXT,
               LOWER (OPPORTUNITYSTATUS) OPPORTUNITYSTATUS,
               NUMBEROFAWARDS,
               DURATION,
               o.OPPORTUNITY_ID,
               REPEATINGOPPORTUNITY,
               PREPROPOSALMANDATORY
        FROM   opportunity o, ESTIMATEDFUNDING esf
       WHERE   o.OPPORTUNITY_ID = esf.opportunity_id
               AND o.opportunity_id = v_id;

      SELECT   sl_mast.language_code lang,
               sl_dtl.column_desc name_text,
               sl_dtl.scival_id opportunity_id
        FROM   sci_language_detail sl_dtl, sci_language_master sl_mast
       WHERE       sl_dtl.language_id = sl_mast.language_id
               AND sl_dtl.moduleid = 3
               AND sl_dtl.column_id = 5
               AND sl_dtl.scival_id = v_id;

      SELECT   STATUS, REVISIONHISTORY_ID, OPPORTUNITY_ID
        FROM   revisionhistory
       WHERE   opportunity_id = v_id;


      SELECT   VERSION,
                  CONCAT(IFNULL(DATE_FORMAT (reviseddate_text, '%Y-%m-%d'), '')
               , 'T'
               , IFNULL(DATE_FORMAT (reviseddate_text, '%H:%i:%s'), ''))
                  reviseddate_text,
               revisionhistory_id
        FROM   reviseddate
       WHERE   revisionhistory_id IN (SELECT   revisionhistory_id
                                        FROM   revisionhistory
                                       WHERE   OPPORTUNITY_ID = v_id);

      SELECT   VERSION,
                  CONCAT(IFNULL(DATE_FORMAT (createddate_text, '%Y-%m-%d'), '')
               , 'T'
               , IFNULL(DATE_FORMAT (createddate_text, '%H:%i:%s'), ''))
                  createddate_text,
               revisionhistory_id
        FROM   createddate
       WHERE   revisionhistory_id IN (SELECT   revisionhistory_id
                                        FROM   revisionhistory
                                       WHERE   OPPORTUNITY_ID = v_id);

   
      SELECT   ID, TYPE_TEXT, OPPORTUNITY_ID
        FROM   TYPE
       WHERE   opportunity_id = v_id;

     SELECT   ELIGIBILITYCLASSIFICATION_ID, OPPORTUNITY_ID
        FROM   eligibilityclassification ecls, elgcls_regionspecific eclsrs
       WHERE       ecls.REGION_SPECIFIC_ID = eclsrs.ID
               AND ELIGIBILITYCLASSIFICATION_ID IS NOT NULL
               AND opportunity_id = v_id;

    SELECT   CURRENCY, ESTIMATEDFUNDING_TEXT, OPPORTUNITY_ID
        FROM   estimatedfunding
       WHERE   opportunity_id = v_id and ESTIMATEDFUNDING_TEXT <> '0';

   SELECT   CURRENCY, AWARDCEILING_TEXT, OPPORTUNITY_ID
        FROM   awardceiling
       WHERE   opportunity_id = v_id and AWARDCEILING_TEXT > 0;

   SELECT   lnk.url,
               CASE WHEN item.LANG IS NULL THEN 'en' ELSE lang END lang,
               item.ITEM_ID,
               item.RELATEDITEMS_ID,
               item.SYNOPSIS_ID,
               item.DESCRIPTION
   FROM      item
               LEFT JOIN
                  link lnk
               ON lnk.item_id = item.item_id
       WHERE   SYNOPSIS_ID IN (SELECT   SYNOPSIS_ID
                                 FROM   synopsis
                                WHERE   opportunity_id = v_id);

      SELECT   currency,
               estimatedfunding_text awardfloor_text,
               opportunity_id
        FROM   awardfloor
       WHERE   opportunity_id = v_id and estimatedfunding_text <>'0';

      SELECT   *
        FROM   relateditems
       WHERE   opportunity_id = v_id;

      SELECT   *
        FROM   classificationgroup
       WHERE   opportunity_id = v_id;

      SELECT   *
        FROM   keywords
       WHERE   opportunity_id = v_id;

      SELECT   ID AS relatedOpportunities_Id,
               OPPORTUNITY_ID AS opportunity_Id
        FROM   sci_related_opportunity
       WHERE   OPPORTUNITY_ID = v_id;

      SELECT   *
        FROM   relatedprograms
       WHERE   opportunity_id = v_id;

   SELECT   COUNT(*)
     INTO   l_count
     FROM   relatedorgs
    WHERE   opportunity_id = v_id AND HIERARCHY = 'lead';

   IF l_count > 0
   THEN
      SELECT   HIERARCHY,
                  relatedorgs_id relatedfundingbodies_id,
                  opportunity_id,
                  (SELECT   FUNDINGBODY_ID
                     FROM   opportunity
                    WHERE   opportunity_id = v_id) as
                     FUNDINGBODY_ID
           FROM   relatedorgs
          WHERE   opportunity_id = v_id;
   END IF;
        SELECT   *
        FROM   changehistory
       WHERE   opportunity_id = v_id;

      SELECT   NULL RELTYPE,
               CASE WHEN LANG IS NULL THEN 'en' ELSE lang END lang,
               ITEM_ID,
               RELATEDITEMS_ID,
               SYNOPSIS_ID,
               DESCRIPTION,
               SUBJECTMATTER_ID,
               ELIGIBILITYDESCRIPTION_ID,
               ESTIMATEDAMOUNTDESCRIPTION_ID,
               LIMITEDSUBMISSIONDESC_ID          -- added by avi on14-JUNt-2018
        FROM   item
       WHERE   RELATEDITEMS_ID IN (SELECT   RELATEDITEMS_ID
                                     FROM   relateditems
                                    WHERE   opportunity_id = v_id)
      UNION ALL      
      SELECT   NULL RELTYPE,
               CASE WHEN LANG IS NULL THEN 'en' ELSE lang END lang,
               ITEM_ID,
               RELATEDITEMS_ID,
               SYNOPSIS_ID,
               DESCRIPTION,
               SUBJECTMATTER_ID,
               ELIGIBILITYDESCRIPTION_ID,
               ESTIMATEDAMOUNTDESCRIPTION_ID,
               LIMITEDSUBMISSIONDESC_ID
        FROM   item
       WHERE   SYNOPSIS_ID IN (SELECT   SYNOPSIS_ID
                                 FROM   synopsis
                                WHERE   opportunity_id = v_id)
      UNION ALL
      SELECT   RELTYPE,
               CASE WHEN LANG IS NULL THEN 'en' ELSE lang END lang,
               ITEM_ID,
               RELATEDITEMS_ID,
               SYNOPSIS_ID,
               DESCRIPTION,
               SUBJECTMATTER_ID,
               ELIGIBILITYDESCRIPTION_ID,
               ESTIMATEDAMOUNTDESCRIPTION_ID,
               LIMITEDSUBMISSIONDESC_ID
        FROM   item
       WHERE   SUBJECTMATTER_ID IN (SELECT   SUBJECTMATTER_ID
                                      FROM   OPP_SUBJECTMATTER
                                     WHERE   opportunity_id = v_id)
      UNION ALL
      SELECT   RELTYPE,
               CASE WHEN LANG IS NULL THEN 'en' ELSE lang END lang,
               ITEM_ID,
               RELATEDITEMS_ID,
               SYNOPSIS_ID,
               DESCRIPTION,
               SUBJECTMATTER_ID,
               ELIGIBILITYDESCRIPTION_ID,
               ESTIMATEDAMOUNTDESCRIPTION_ID,
               LIMITEDSUBMISSIONDESC_ID
        FROM   item
       WHERE   ELIGIBILITYDESCRIPTION_ID IN
                     (SELECT   ELIGIBILITYDESCRIPTION_ID
                        FROM   ELIGIBILITYDESCRIPTION
                       WHERE   OPPORTUNITY_ID = v_id)
      UNION ALL
      SELECT   RELTYPE,
               CASE WHEN LANG IS NULL THEN 'en' ELSE lang END lang,
               ITEM_ID,
               RELATEDITEMS_ID,
               SYNOPSIS_ID,
               DESCRIPTION,
               SUBJECTMATTER_ID,
               ELIGIBILITYDESCRIPTION_ID,
               ESTIMATEDAMOUNTDESCRIPTION_ID,
               LIMITEDSUBMISSIONDESC_ID
        FROM   item
       WHERE   ESTIMATEDAMOUNTDESCRIPTION_ID IN
                     (SELECT   ESTIMATEDAMOUNTDESCRIPTION_ID
                        FROM   ESTIMATEDAMOUNTDESCRIPTION
                       WHERE   OPPORTUNITY_ID = v_id)
      UNION ALL
      SELECT   RELTYPE,
               CASE WHEN LANG IS NULL THEN 'en' ELSE lang END lang,
               ITEM_ID,
               RELATEDITEMS_ID,
               SYNOPSIS_ID,
               DESCRIPTION,
               SUBJECTMATTER_ID,
               ELIGIBILITYDESCRIPTION_ID,
               ESTIMATEDAMOUNTDESCRIPTION_ID,
               LIMITEDSUBMISSIONDESC_ID
        FROM   item
       WHERE   LIMITEDSUBMISSIONDESC_ID IN
                     (SELECT   LIMITEDSUBMISSIONDESC_ID
                        FROM   LIMITEDSUBMISSIONDESCRIPTION
                       WHERE   OPPORTUNITY_ID = v_id);

      SELECT   url, link_text, item_id
        FROM   LINK
       WHERE   item_id IN
                     (SELECT   ITEM_ID
                        FROM   (SELECT   *
                                  FROM   item
                                 WHERE   RELATEDITEMS_ID IN
                                               (SELECT   RELATEDITEMS_ID
                                                  FROM   relateditems
                                                 WHERE   opportunity_id =
                                                            v_id)
                                UNION ALL
                                SELECT   *
                                  FROM   item
                                 WHERE   SYNOPSIS_ID IN
                                               (SELECT   SYNOPSIS_ID
                                                  FROM   synopsis
                                                 WHERE   opportunity_id =
                                                            v_id)
                                UNION ALL
                                  SELECT   *
                                  FROM   item
                                 WHERE   ESTIMATEDAMOUNTDESCRIPTION_ID IN
                                               (SELECT   ESTIMATEDAMOUNTDESCRIPTION_ID
                                                  FROM   ESTIMATEDAMOUNTDESCRIPTION
                                                 WHERE   opportunity_id =
                                                            v_id)
                                UNION ALL
                                 SELECT   *
                                  FROM   item
                                 WHERE   ELIGIBILITYDESCRIPTION_ID IN
                                               (SELECT   ELIGIBILITYDESCRIPTION_ID
                                                  FROM   ELIGIBILITYDESCRIPTION
                                                 WHERE   opportunity_id =
                                                            v_id)
                                UNION ALL
                                SELECT   *
                                  FROM   item
                                 WHERE   LIMITEDSUBMISSIONDESC_ID IN
                                               (SELECT   LIMITEDSUBMISSIONDESC_ID
                                                  FROM   LIMITEDSUBMISSIONDESCRIPTION
                                                 WHERE   opportunity_id =
                                                            v_id)
                                UNION ALL
                                SELECT   *
                                  FROM   item
                                 WHERE   SUBJECTMATTER_ID IN
                                               (SELECT   SUBJECTMATTER_ID
                                                  FROM   OPP_SUBJECTMATTER
                                                 WHERE   opportunity_id =
                                                            v_id)) T );

    SELECT   *
        FROM   classifications
       WHERE   classificationgroup_id IN (SELECT   classificationgroup_id
                                            FROM   classificationgroup
                                           WHERE   opportunity_id = v_id);
     SELECT   *
        FROM   classification
       WHERE   classifications_id IN
                     (SELECT   classifications_id
                        FROM   classifications
                       WHERE   classificationgroup_id IN
                                     (SELECT   classificationgroup_id
                                        FROM   classificationgroup
                                       WHERE   opportunity_id = v_id));

      SELECT   KEYWORD_COLUMN,
               CASE WHEN LANG IS NULL THEN 'en' ELSE lang END LANG
        FROM   keyword
       WHERE   keywords_id IN (SELECT   keywords_id
                                 FROM   keywords
                                WHERE   opportunity_id = v_id);

     SELECT   *
        FROM   relatedprogram
       WHERE   relatedprograms_id IN (SELECT   relatedprograms_id
                                        FROM   relatedprograms
                                       WHERE   opportunity_id = v_id);

    SELECT   RELATED_OPP_ID AS ID,
               RELAION_NAME AS relType,
               OPPORTUNITYNAME AS relatedOpportunity_text,
               ID AS relatedOpportunities_Id
        FROM   sci_related_opportunity
       WHERE   OPPORTUNITY_ID = v_id;

   SELECT   orgdbid,
               reltype,
               org_text,
               relatedorgs_id relatedfundingbodies_id
        FROM   org
       WHERE   relatedorgs_id IN (SELECT   relatedorgs_id
                                    FROM   relatedorgs
                                   WHERE   opportunity_id = v_id);

   SELECT   TYPE ,
               DATE_FORMAT (STR_TO_DATE (POSTDATE, '%d-%m-%Y'), '%d-%m-%Y')
                  POSTDATE,
               VERSION,
               CHANGE_TEXT,
               CHANGEHISTORY_ID
                FROM   X -- change by neha
       WHERE   CHANGEHISTORY_ID IN (SELECT   changehistory_id
                                      FROM   changehistory
                                     WHERE   opportunity_id = v_id)
               AND version =
                     (SELECT   MIN (version)
                        FROM   X -- change by neha
                       WHERE   CHANGEHISTORY_ID IN
                                     (SELECT   changehistory_id
                                        FROM   changehistory
                                       WHERE   opportunity_id = v_id))
      UNION
      SELECT   TYPE,
               DATE_FORMAT (STR_TO_DATE (POSTDATE, '%d-%m-%Y'), '%d-%m-%Y')
                  POSTDATE,
               VERSION,
               CHANGE_TEXT,
               CHANGEHISTORY_ID
          FROM   X -- change by neha
       WHERE   CHANGEHISTORY_ID IN (SELECT   changehistory_id
                                      FROM   changehistory
                                     WHERE   opportunity_id = v_id)
               AND version =
                     (SELECT   MAX (version)
                        FROM   X -- change by neha
                       WHERE   CHANGEHISTORY_ID IN
                                     (SELECT   changehistory_id
                                        FROM   changehistory
                                       WHERE   opportunity_id = v_id));
 --  below line commented by neha
/* OPEN p_contactinfo FOR
     SELECT   *
        FROM   contactinfo
       WHERE   opportunity_id = v_id;

   OPEN p_contact FOR
      SELECT   *
        FROM   contact
       WHERE   contactinfo_id IN (SELECT   contactinfo_id
                                    FROM   contactinfo
                                   WHERE   opportunity_id = v_id); 

   OPEN p_contactname FOR
      SELECT   cn.CONTACT_ID,
               cn.prefix,
               cn.givenname,
               cn.middlename,
               cn.surname,
               cn.suffix,
               c.email
        FROM      contactname cn
               LEFT JOIN
                  contact c
               ON c.contact_id = cn.contact_id
       WHERE   cn.contact_id IN
                     (SELECT   contact_id
                        FROM   contact
                       WHERE   contactinfo_id IN
                                     (SELECT   contactinfo_id
                                        FROM   contactinfo
                                       WHERE   opportunity_id = v_id));


   OPEN p_website FOR
      SELECT   url, website_text, contact_id
        FROM   website
       WHERE   contact_id IN
                     (SELECT   contact_id
                        FROM   contact
                       WHERE   contactinfo_id IN
                                     (SELECT   contactinfo_id
                                        FROM   contactinfo
                                       WHERE   opportunity_id = v_id));

   OPEN p_address FOR
      SELECT   countrytest country,
               room,
               street,
               city,
               state,
               postalcode,
               countrytest,
               contact_id
        FROM   address
       WHERE   contact_id IN
                     (SELECT   contact_id
                        FROM   contact
                       WHERE   contactinfo_id IN
                                     (SELECT   contactinfo_id
                                        FROM   contactinfo
                                       WHERE   opportunity_id = v_id));

   OPEN p_opportunityDates FOR
        SELECT   0 opportunityDates_Id, OPPORTUNITY_ID
          FROM   SCI_OPP_LOI_DUEDATE_DETAIL
         WHERE   opportunity_id = v_id
      ORDER BY   SEQUENCE_ID;


   OPEN p_opportunityDate FOR
        SELECT   DATE_REMARKS description,
                 O_ID opportunityDate_Id,
                 0 opportunityDates_Id
          FROM   SCI_OPP_LOI_DUEDATE_DETAIL
         WHERE   opportunity_id = v_id
      ORDER BY   SEQUENCE_ID;

   OPEN p_date FOR
        SELECT   sci_get_date_name (DATE_TYPE) TYPE,
                 DATE_FORMAT (LOI_DUE_DATE, '%Y-%m-%d') date_text,
                 O_ID opportunityDate_Id
          FROM   SCI_OPP_LOI_DUEDATE_DETAIL
         WHERE   opportunity_id = v_id
      ORDER BY   SEQUENCE_ID;

   OPEN p_opportunityLocation FOR
      SELECT   country countrytest,
               room,
               street,
               city,
               state,
               postalcode,
               country,
               opportunity_id
        FROM   opportunity_location
       WHERE   opportunity_id = v_id;


   OPEN p_subjectMatter FOR
      SELECT   lnk.url,
               CASE WHEN item.LANG IS NULL THEN 'en' ELSE lang END lang,
               item.ITEM_ID,
			   item.RELATEDITEMS_ID,
               item.DESCRIPTION,
               SUBJECTMATTER_ID
        FROM      item
               LEFT JOIN
                  link lnk
               ON lnk.item_id = item.item_id
       WHERE   SUBJECTMATTER_ID IN (SELECT   SUBJECTMATTER_ID
                                      FROM   OPP_SUBJECTMATTER
                                     WHERE   opportunity_id = v_id);

   OPEN p_eligibility_desc FOR
      SELECT   lnk.url,
               CASE WHEN item.LANG IS NULL THEN 'en' ELSE lang END lang,
               item.ITEM_ID,
               item.RELATEDITEMS_ID,
               item.DESCRIPTION,
               ELIGIBILITYDESCRIPTION_ID
        FROM      item
               LEFT JOIN
                  link lnk
               ON lnk.item_id = item.item_id
       WHERE   ELIGIBILITYDESCRIPTION_ID IN
                     (SELECT   ELIGIBILITYDESCRIPTION_ID
                        FROM   ELIGIBILITYDESCRIPTION
                       WHERE   opportunity_id = v_id);

   OPEN P_individualEligibility FOR
      SELECT   
      (case when lower(not_specified) ='true' then 'LIMITED' else 'NOTSPECIFIED' end) limitation,
               degree degreeRequirement,
               graduate applicantType,
               individualeligibility_id,
               ec.eligibilityclassification_id
        FROM   individualEligibility IE, ELIGIBILITYCLASSIFICATION EC
       WHERE   ec.eligibilityclassification_id =
                  ie.eligibilityclassification_id
               AND EC.OPPORTUNITY_ID = v_id;

   OPEN P_citizenship FOR
      SELECT   CASE
                  WHEN norestriction IS NULL THEN 'NOTSPECIFIED'
                  WHEN lower(norestriction) ='false' THEN 'NOTSPECIFIED'
                  WHEN country is null THEN 'NOTSPECIFIED'
                  WHEN lower(norestriction) ='true' THEN 'LIMITED'
                  ELSE norestriction
               END
                  limitation,
               CASE
                  WHEN country IS NULL THEN null
                  ELSE country
               END
                  country,
               cz.individualeligibility_id
        FROM   citizenship CZ,
               individualEligibility IE,
               ELIGIBILITYCLASSIFICATION EC
       WHERE   ec.eligibilityclassification_id =
                  ie.eligibilityclassification_id
               AND ie.individualeligibility_id = cz.individualeligibility_id
               AND EC.OPPORTUNITY_ID = v_id;

   OPEN P_organizationEligibility FOR
      SELECT   not_specified limitation,
               academic applicantType,
               organizationeligibility_id,
               EC.eligibilityclassification_id
        FROM   organizationEligibility OE, ELIGIBILITYCLASSIFICATION EC
       WHERE   ec.eligibilityclassification_id =
                  oe.eligibilityclassification_id
               AND ec.opportunity_id = v_id;

   OPEN P_regionspecific FOR
      SELECT 
      case when country is not null then 'LIMITED'
      else 'NOTSPECIFIED' end  limitation,
               city,
               state,
               country,
               OE.organizationeligibility_id
        FROM   regionspecific RS,
               organizationEligibility OE,
               ELIGIBILITYCLASSIFICATION EC
       WHERE   ec.eligibilityclassification_id =
                  oe.eligibilityclassification_id
               AND ec.opportunity_id = v_id
               AND OE.ORGANIZATIONELIGIBILITY_ID =
                     RS.ORGANIZATIONELIGIBILITY_ID;

   OPEN P_restrictions FOR
      SELECT   not_specified limitation,
               disabilities restriction,
               restrictions_id,
               ec.eligibilityclassification_id
        FROM   restrictions RS, ELIGIBILITYCLASSIFICATION EC
       WHERE   ec.eligibilityclassification_id =
                  RS.eligibilityclassification_id
               AND EC.OPPORTUNITY_ID = V_ID;

   OPEN P_limitedsubmission FOR
      SELECT   numberofapplicantsallowed numberOfApplications,
               'Limited' limitation,
               CASE WHEN LD.description IS NULL THEN 'Not Available_X' ELSE to_char(LD.description) END description,
               case when  LD.lang is null then 'Not Available_X' else  LD.lang end lang,
               case when  LD.URL is null then 'Not Available_X' else  LD.URL end URL,
               ls.restriction_id restrictions_id
        FROM   limitedsubmission LS,
               restrictions RS,
               ELIGIBILITYCLASSIFICATION EC,
               (SELECT   lsd.limitedsubmissiondesc_id,
                         opportunity_id,
                         it.description,
                         it.lang,
                         l.url
                  FROM   limitedsubmissionDescription LSD, ITEM IT, link l
                 WHERE   it.LIMITEDSUBMISSIONDESC_ID =
                            LSD.LIMITEDSUBMISSIONDESC_ID
                         AND l.item_id = it.item_id
                         AND LSD.opportunity_id = V_ID) LD
       WHERE   ec.eligibilityclassification_id =
                  RS.eligibilityclassification_id
               AND EC.OPPORTUNITY_ID = V_ID
               AND RS.RESTRICTIONS_ID = LS.RESTRICTION_ID
               AND LD.opportunity_id = EC.OPPORTUNITY_ID;

   OPEN p_estimatedamountdescription FOR
      SELECT   lnk.url,
               CASE WHEN item.LANG IS NULL THEN 'en' ELSE lang END lang,
               item.ITEM_ID,
               item.RELATEDITEMS_ID,
               item.estimatedamountdescription_id,
               item.DESCRIPTION
        FROM      item
               LEFT JOIN
                  link lnk
               ON lnk.item_id = item.item_id
       WHERE   estimatedamountdescription_id IN
                     (SELECT   estimatedamountdescription_id
                        FROM   estimatedAmountDescription
                       WHERE   opportunity_id = v_id);

   OPEN p_limitedsubmissionDescription FOR
      SELECT   lsd.limitedsubmissiondesc_id, opportunity_id
        FROM   limitedsubmissionDescription LSD, ITEM IT
       WHERE   it.LIMITEDSUBMISSIONDESC_ID = LSD.LIMITEDSUBMISSIONDESC_ID
               AND LSD.opportunity_id = v_id;

   OPEN P_Duration FOR
      SELECT   it.RELTYPE,
               it.description,
               it.ITEM_ID,
               it.lang,
               it.RELATEDITEMS_ID id,
               it.duration_id,
               d.OPPORTUNITY_ID,
               l.URL,
               l.LINK_TEXT,
               AWARDSTATISTICS_ID,
               o.DURATION
        FROM   item it,
               duration d,
               link l,
               OPPORTUNITY o
       WHERE       it.duration_ID = d.duration_ID
               AND d.OPPORTUNITY_ID = o.OPPORTUNITY_ID
               AND l.item_id = it.item_id
               AND d.OPPORTUNITY_ID = v_id;

   OPEN P_expirationDateDetail FOR
      SELECT   CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     'NOTSPECIFIED'
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     'ONGOING'
                  ELSE
                        CONCAT(IFNULL(DATE_FORMAT (LOI_DUE_DATE, '%Y-%m-%d'), '')
                     , 'T'
                     , IFNULL(DATE_FORMAT (LOI_DUE_DATE, '%H:%i:%s'), ''))
               END
                  date_text,
               URL URL_expirationDate,
               CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     ''
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900' THEN ''
                  WHEN to_char(Date_Remarks) is null   THEN ''
                  ELSE
                     lang
               END
               lang,
               Date_Remarks description,
               opportunity_id,
               CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     'NOTSPECIFIED'
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     'ONGOING'
                  ELSE
                     'SPECIFIED'
               END
                  limitation,
               CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900' THEN 'false'
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900' THEN 'false'
                  ELSE 'true'
               END
                  required
        FROM   SCI_OPP_LOI_DUEDATE_DETAIL
       WHERE   opportunity_id = v_id AND date_type = 3;

   OPEN P_decision FOR
      SELECT   CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     ''
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     ''
                  ELSE
                        CONCAT(IFNULL(DATE_FORMAT (LOI_DUE_DATE, '%Y-%m-%d'), '')
                     , 'T'
                     , IFNULL(DATE_FORMAT (LOI_DUE_DATE, '%H:%i:%s'), ''))
               END
                  date_text,
               URL URL_decisionDate,
                CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     ''
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     ''
                     WHEN to_char(Date_Remarks) is null   THEN ''
                  ELSE
                     lang
               END
               lang,
               Date_Remarks description,
               opportunity_id,
               CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     'NOTSPECIFIED'
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     'ONGOING'
                  ELSE
                     'SPECIFIED'
               END
                  limitation,
               CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900' THEN 'false'
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900' THEN 'false'
                  ELSE 'true'
               END
                  required
        FROM   SCI_OPP_LOI_DUEDATE_DETAIL
       WHERE   opportunity_id = v_id AND date_type = 8;


   OPEN P_letterOfIntent FOR
      SELECT   CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     ''
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     ''
                  ELSE
                        CONCAT(IFNULL(DATE_FORMAT (LOI_DUE_DATE, '%Y-%m-%d'), '')
                     , 'T'
                     , IFNULL(DATE_FORMAT (LOI_DUE_DATE, '%H:%i:%s'), ''))
               END
                  date_text,
               URL URL_LOIDATE,
                CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     ''
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     ''
                     WHEN to_char(Date_Remarks) is null   THEN ''
                  ELSE
                     lang
               END
               lang,
               Date_Remarks description,
               opportunity_id,
               CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     'NOTSPECIFIED'
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     'ONGOING'
                  ELSE
                     'SPECIFIED'
               END
                  limitation,
               CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900' THEN 'false'
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900' THEN 'false'
                  ELSE 'true'
               END
                  required
        FROM   SCI_OPP_LOI_DUEDATE_DETAIL
       WHERE   opportunity_id = v_id AND date_type = 1;

   OPEN P_preproposal FOR
      SELECT   CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     ''
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     ''
                  ELSE
                        CONCAT(IFNULL(DATE_FORMAT (LOI_DUE_DATE, '%Y-%m-%d'), '')
                     , 'T'
                     , IFNULL(DATE_FORMAT (LOI_DUE_DATE, '%H:%i:%s'), ''))
               END
                  date_text,
               URL URL_preproposalDate,
                CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     ''
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     ''
                     WHEN to_char(Date_Remarks) is null   THEN ''
                  ELSE
                     lang
               END
               lang,
               Date_Remarks description,
               opportunity_id,
               CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     'NOTSPECIFIED'
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     'ONGOING'
                  ELSE
                     'SPECIFIED'
               END
                  limitation,
               CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900' THEN 'false'
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900' THEN 'false'
                  ELSE 'true'
               END
                  required
        FROM   SCI_OPP_LOI_DUEDATE_DETAIL
       WHERE   opportunity_id = v_id AND date_type = 7;

   OPEN P_startDateDetail FOR
      SELECT   CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     ''
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     ''
                  ELSE
                        CONCAT(IFNULL(DATE_FORMAT (LOI_DUE_DATE, '%Y-%m-%d'), '')
                     , 'T'
                     , IFNULL(DATE_FORMAT (LOI_DUE_DATE, '%H:%i:%s'), ''))
               END
                  date_text,
               URL URL_startDate,
                CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     ''
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     ''
                     WHEN to_char(Date_Remarks) is null   THEN ''
                  ELSE
                     lang
               END
               lang,
               Date_Remarks description,
               opportunity_id,
               CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     'NOTSPECIFIED'
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     'ONGOING'
                  ELSE
                     'SPECIFIED'
               END
                  limitation,
                CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900' THEN 'false'
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900' THEN 'false'
                  ELSE 'true'
               END
                  required
        FROM   SCI_OPP_LOI_DUEDATE_DETAIL
       WHERE   opportunity_id = v_id AND date_type = 6;

   OPEN P_proposal FOR
      SELECT   CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     ''
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     ''
                  ELSE
                        CONCAT(IFNULL(DATE_FORMAT (LOI_DUE_DATE+3000, '%Y-%m-%d'), '')
                     , 'T'
                     , IFNULL(DATE_FORMAT (LOI_DUE_DATE, '%H:%i:%s'), ''))
               END
                  date_text,
               URL URL_proposalDate,
                CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     ''
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     ''
                     WHEN to_char(Date_Remarks) is null   THEN ''
                  ELSE
                     lang
               END
               lang,
               Date_Remarks description,
               opportunity_id,
               CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     'NOTSPECIFIED'
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     'ONGOING'
                  ELSE
                     'SPECIFIED'
               END
                  limitation,
               CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900' THEN 'false'
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900' THEN 'false'
                  ELSE 'true'
               END
                  required
        FROM   SCI_OPP_LOI_DUEDATE_DETAIL
       WHERE   opportunity_id = v_id AND date_type = 2;

   OPEN P_endDateDetail FOR
      SELECT   CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     ''
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     ''
                  ELSE
                        CONCAT(IFNULL(DATE_FORMAT (LOI_DUE_DATE, '%Y-%m-%d'), '')
                     , 'T'
                     , IFNULL(DATE_FORMAT (LOI_DUE_DATE, '%H:%i:%s'), ''))
               END
                  date_text,
               URL URL_endDate,
                CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     ''
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     ''
                     WHEN to_char(Date_Remarks) =''   THEN ''
                  ELSE
                     lang
               END
               lang,
               Date_Remarks description,
               opportunity_id,
               CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                     'NOTSPECIFIED'
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                     'ONGOING'
                  ELSE
                     'SPECIFIED'
               END
                  limitation,
               CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900' THEN 'false'
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900' THEN 'false'
                  ELSE 'true'
               END
                  required
        FROM   SCI_OPP_LOI_DUEDATE_DETAIL
       WHERE   opportunity_id = v_id AND date_type = 2;

   OPEN P__Cycle FOR
      SELECT   2 AS cycle,
               0 AS indexValue,
                  CONCAT(DATE_FORMAT (SYSDATE(), '%bth')
               , DATE_FORMAT (SYSDATE(), '%Y')
               , ' '
               , 'round')
                  AS label
        FROM   DUAL;

   OPEN P_homePage FOR
      SELECT   recordsource link,
               (SELECT     CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                    null
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                    null
                  ELSE
                        CONCAT(IFNULL(DATE_FORMAT (LOI_DUE_DATE, '%Y-%m-%d'), '')
                     , 'T'
                     , IFNULL(DATE_FORMAT (LOI_DUE_DATE, '%H:%i:%s'), ''))
               END LOI_DUE_DATE
                  FROM   SCI_OPP_LOI_DUEDATE_DETAIL DD
                 WHERE   o.opportunity_id = DD.opportunity_id
                         AND dd.date_type = 4)
                  publishedDate,
               (SELECT    CASE
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-01-1900'
                  THEN
                    null
                  WHEN date_format (LOI_DUE_DATE,'%d-%m-%Y') = '01-02-1900'
                  THEN
                    null
                  ELSE
                        CONCAT(IFNULL(DATE_FORMAT (LOI_DUE_DATE, '%Y-%m-%d'), '')
                     , 'T'
                     , IFNULL(DATE_FORMAT (LOI_DUE_DATE, '%H:%i:%s'), ''))
               END LOI_DUE_DATE
                  FROM   SCI_OPP_LOI_DUEDATE_DETAIL DD
                 WHERE   o.opportunity_id = DD.opportunity_id
                         AND dd.date_type = 5)
                  modifiedDate
        FROM   opportunity o
       WHERE   o.opportunity_id = v_id;

   OPEN p_LICENSEINFORMATION FOR
      SELECT   lnk.url,
               CASE WHEN item.LANG IS NULL THEN 'en' ELSE lang END lang,
               item.ITEM_ID,
               item.RELATEDITEMS_ID,
               item.DESCRIPTION,
               LICENSEINFORMATION_ID
        FROM      item
               LEFT JOIN
                  link lnk
               ON lnk.item_id = item.item_id
       WHERE   LICENSEINFORMATION_ID IN (SELECT   LICENSEINFORMATION_ID
                                           FROM   LICENSEINFORMATION
                                          WHERE   opportunity_id = v_id);

   OPEN p_instruction FOR
      SELECT   lnk.url,
               CASE WHEN item.LANG IS NULL THEN 'en' ELSE lang END lang,
               item.ITEM_ID,
               item.RELATEDITEMS_ID,
               item.DESCRIPTION,
               INSTRUCTION_ID
        FROM      item
               LEFT JOIN
                  link lnk
               ON lnk.item_id = item.item_id
       WHERE   INSTRUCTION_ID IN (SELECT   INSTRUCTION_ID
                                    FROM   INSTRUCTION
                                   WHERE   opportunity_id = v_id);

   OPEN p_relatedTo FOR
      SELECT   relid,
               ort.relation_name,
               RELATED_OPP_ID,
               sro.OPPORTUNITYNAME,
               sld.LANGUAGE_ID,
               slm.language_code,
               sro.description
        FROM   sci_related_opportunity sro,
               opportunity_master opm,
               SCI_OPPORTUNITY_RELATION_TYPE ort,
               sci_language_detail sld,
               sci_language_master slm
       WHERE       sro.RELATED_OPP_ID = opm.OPPORTUNITYID
               AND SRO.REL_OPP_ID = ORT.RELID
               AND REL_OPP_ID = 1
               AND sld.SCIVAL_ID = opm.OPPORTUNITYID
               AND slm.LANGUAGE_ID = sld.LANGUAGE_ID
               AND IFNULL (opm.STATUSCODE, 1) <> 3
               AND SRO.OPPORTUNITY_ID = v_id;

   OPEN p_replacedBy FOR
      SELECT   relid,
               ort.relation_name,
               RELATED_OPP_ID,
               sro.OPPORTUNITYNAME,
               sld.LANGUAGE_ID,
               slm.language_code,
               sro.description
        FROM   sci_related_opportunity sro,
               opportunity_master opm,
               SCI_OPPORTUNITY_RELATION_TYPE ort,
               sci_language_detail sld,
               sci_language_master slm
       WHERE       sro.RELATED_OPP_ID = opm.OPPORTUNITYID
               AND SRO.REL_OPP_ID = ORT.RELID
               AND REL_OPP_ID = 2
               AND sld.SCIVAL_ID = opm.OPPORTUNITYID
               AND slm.LANGUAGE_ID = sld.LANGUAGE_ID
               AND IFNULL (opm.STATUSCODE, 1) <> 3
               AND SRO.OPPORTUNITY_ID = v_id;


   OPEN p_replaces FOR
      SELECT   relid,
               ort.relation_name,
               RELATED_OPP_ID,
               sro.OPPORTUNITYNAME,
               sld.LANGUAGE_ID,
               slm.language_code,
               sro.description
        FROM   sci_related_opportunity sro,
               opportunity_master opm,
               SCI_OPPORTUNITY_RELATION_TYPE ort,
               sci_language_detail sld,
               sci_language_master slm
       WHERE       sro.RELATED_OPP_ID = opm.OPPORTUNITYID
               AND SRO.REL_OPP_ID = ORT.RELID
               AND REL_OPP_ID = 3
               AND sld.SCIVAL_ID = opm.OPPORTUNITYID
               AND slm.LANGUAGE_ID = sld.LANGUAGE_ID
               AND IFNULL (opm.STATUSCODE, 1) <> 3
               AND SRO.OPPORTUNITY_ID = v_id; */
END