CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_aw_QCcheckxml50_4`(
   p_tran_type_id              INTEGER, 
   p_workflowid                INTEGER
)
BEGIN
   DECLARE v_id      INTEGER;
   DECLARE l_count   INTEGER;
 
   SELECT   ID
     INTO   v_id
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;

      SELECT   fundingbodyawardid,
               TYPE grantType,
               sci_get_funderSchemeType(TYPE) funderSchemeType, 
               trusting,
               collectioncode,
               hidden,
               'false' defunct,
               recordsource,
               startdate,
               lastamendeddate modifiedDate,
               enddate,
               publishedDate,
               award_id grantAwardId,
               
               ID,
               case when awardNoticeDate is not null then
               concat(ifnull(truncate(awardNoticeDate, 0), '')
                , 'T'
               , IFNULL(DATE_FORMAT (awardNoticeDate, '%H:%i:%s'), ''))
               else 
               null
               End
               noticeDate
        FROM   award
       WHERE   award_id = v_id;

      SELECT   sl_mast.language_code lang,
               sl_dtl.column_desc name_text,
               sl_dtl.scival_id award_id
        FROM   sci_language_detail sl_dtl, sci_language_master sl_mast
       WHERE       sl_dtl.language_id = sl_mast.language_id
               AND sl_dtl.moduleid = 4
               AND sl_dtl.column_id = 5
               AND sl_dtl.scival_id = v_id
               AND sl_dtl.tran_type_id = p_tran_type_id; -- v.0.3 modification (expression added)

      SELECT   sl_mast.language_code lang,
               sl_dtl.column_desc abstract_text,
               sl_dtl.scival_id award_id
        FROM   sci_language_detail sl_dtl, sci_language_master sl_mast
       WHERE       sl_dtl.language_id = sl_mast.language_id
               AND sl_dtl.moduleid = 4
               AND sl_dtl.column_id = 6
               AND sl_dtl.scival_id = v_id
               AND sl_dtl.tran_type_id = p_tran_type_id;
      SELECT   *
        FROM   revisionhistory
       WHERE   award_id = v_id;


   
      SELECT   CURRENCY, AMOUNT_TEXT, AWARDEE_ID
        FROM   amount
       WHERE   award_id = v_id
       AND  AWARDEE_ID IS NOT NULL;


   
      SELECT   *
        FROM   classificationgroup
       WHERE   award_id = v_id;


   
     SELECT  k.keyword_column value,  k.LANG, AWARD_ID
        FROM   keywords ks, keyword k
       WHERE   ks.award_id = v_id
       and K.KEYWORDS_ID=KS.KEYWORDS_ID;


   
      SELECT   *
        FROM   awardees
       WHERE   award_id = v_id;


   
      SELECT   *
        FROM   awardmanagers
       WHERE   award_id = v_id;


   
      SELECT   *
        FROM   relatedprograms
       WHERE   award_id = v_id;

   
   SELECT   COUNT(*)
     INTO   l_count
     FROM   relatedorgs
    WHERE   award_id = v_id AND HIERARCHY = 'lead';

   IF l_count > 0
   THEN
     SELECT  ro.hierarchy, o.ORGDBID fundingBodyId,o.RELTYPE,o.ORG_TEXT,ro.award_id
        FROM   org o,relatedorgs ro
       WHERE   o.RELATEDORGS_ID =ro.RELATEDORGS_ID and ro.award_id=v_id;

   END IF;
   SELECT   RELATEDITEMS_ID,AWARD_ID
        FROM   relateditems
       WHERE   award_id = v_id;

      SELECT   VERSION,
                  CONCAT(IFNULL(DATE_FORMAT (reviseddate_text, '%Y-%m-%d'), '')
               , 'T'
               , IFNULL(DATE_FORMAT (reviseddate_text, '%H:%i:%s'), ''))
                  reviseddate_text,
               revisionhistory_id
        FROM   reviseddate
       WHERE   revisionhistory_id IN (SELECT   revisionhistory_id
                                        FROM   revisionhistory
                                       WHERE   award_id = v_id);


      SELECT   VERSION,
                  CONCAT(IFNULL(DATE_FORMAT (createddate_text, '%Y-%m-%d'), '')
               , 'T'
               , IFNULL(DATE_FORMAT (createddate_text, '%H:%i:%s'), ''))
                  createddate_text,
               revisionhistory_id
        FROM   createddate
       WHERE   revisionhistory_id IN (SELECT   revisionhistory_id
                                        FROM   revisionhistory
                                       WHERE   award_id = v_id);


      SELECT   *
        FROM   classifications
       WHERE   classificationgroup_id IN (SELECT   classificationgroup_id
                                            FROM   classificationgroup
                                           WHERE   award_id = v_id);


         SELECT  clss.type, code value,sci_getpreferredLabel(code) preferredLabel,'Classification' cls_Type, clsG.award_id,cls.orgSpecificClassification
        FROM   classification cls,classifications clss ,classificationgroup clsG
       WHERE   cls.classifications_id=clss.classifications_id
       and clss.classificationgroup_id= clsG.classificationgroup_id
       and clsG.award_id=v_id;

     SELECT  'en' LANG,
               ORG orgtest_text,
               AFFILIATION_ID AFFILIATION_ID 
                        FROM   affiliation
                       WHERE   AWARDEEINSTITUTION_ID IN
                                     (SELECT   AWARDEEINSTITUTION_ID
                                        FROM   AWARDEEINSTITUTION
                                       WHERE   AWARDEE_ID IN
                                                     (SELECT   AWARDEE_ID
                                                        FROM   awardee
                                                       WHERE   AWARDEES_ID IN
                                                                     (SELECT   AWARDEES_ID
                                                                        FROM   awardees
                                                                       WHERE   award_id =
                                                                                  v_id)));
                                                                               
     SELECT  'en' LANG,
               dept dept_text,
               AFFILIATION_ID AFFILIATION_ID 
                        FROM   affiliation
                       WHERE   AWARDEEINSTITUTION_ID IN
                                     (SELECT   AWARDEEINSTITUTION_ID
                                        FROM   AWARDEEINSTITUTION
                                       WHERE   AWARDEE_ID IN
                                                     (SELECT   AWARDEE_ID
                                                        FROM   awardee
                                                       WHERE   AWARDEES_ID IN
                                                                     (SELECT   AWARDEES_ID
                                                                        FROM   awardees
                                                                       WHERE   award_id =
                                                                                  v_id)));
      SELECT   AWADEENAME_ID AWARDEENAME_ID, AWARDEE_ID
        FROM   awardeename
       WHERE    
       awardee_id IN
                     (SELECT   awardee_id
                        FROM   awardee
                       WHERE   awardees_id IN (SELECT   awardees_id
                                                 FROM   awardees
                                                WHERE   award_id = v_id));

      SELECT   LANG,
               INDEXEDNAME INDEXEDNAME_text,
               AWADEENAME_ID AWARDEENAME_ID
        FROM   awardeename
       WHERE   awardee_id IN
                     (SELECT   awardee_id
                        FROM   awardee
                       WHERE   awardees_id IN (SELECT   awardees_id
                                                 FROM   awardees
                                                WHERE   award_id = v_id));


      SELECT   LANG, GIVENNAME GIVENNAME_text, AWADEENAME_ID AWARDEENAME_ID
        FROM   awardeename
       WHERE  GIVENNAME is not null and awardee_id IN
                     (SELECT   awardee_id
                        FROM   awardee
                       WHERE   awardees_id IN (SELECT   awardees_id
                                                 FROM   awardees
                                                WHERE   award_id = v_id));


      SELECT   LANG, INITIALS INITIALS_text, AWADEENAME_ID AWARDEENAME_ID
        FROM   awardeename
       WHERE   INITIALS is not null and awardee_id IN
                     (SELECT   awardee_id
                        FROM   awardee
                       WHERE   awardees_id IN (SELECT   awardees_id
                                                 FROM   awardees
                                                WHERE   award_id = v_id));


      SELECT   LANG, SURNAME SURNAME_text, AWADEENAME_ID AWARDEENAME_ID
        FROM   awardeename
       WHERE   SURNAME is not null and awardee_id IN
                     (SELECT   awardee_id
                        FROM   awardee
                       WHERE   awardees_id IN (SELECT   awardees_id
                                                 FROM   awardees
                                                WHERE   award_id = v_id));




      SELECT   *
        FROM   awardeeinstitution
       WHERE   awardee_id IN
                     (SELECT   awardee_id
                        FROM   awardee
                       WHERE   awardees_id IN (SELECT   awardees_id
                                                 FROM   awardees
                                                WHERE   award_id = v_id));



      SELECT   *
        FROM   awardwebsite
       WHERE   awardmanager_id IN
                     (SELECT   awardmanager_id
                        FROM   awardmanager
                       WHERE   awardmanagers_id IN (SELECT   awardmanagers_id
                                                      FROM   awardmanagers
                                                     WHERE   award_id = v_id));



      SELECT   *
        FROM   awardcontactname
       WHERE   awardmanager_id IN
                     (SELECT   awardmanager_id
                        FROM   awardmanager
                       WHERE   awardmanagers_id IN (SELECT   awardmanagers_id
                                                      FROM   awardmanagers
                                                     WHERE   award_id = v_id));


      SELECT  
               COUNTRYTEST as COUNTRYTEST,
               ROOM,
               STREET,
               CITY,
               STATE,
               POSTALCODE,
               get_country_name(COUNTRYTEST) country,
               AWARDMANAGER_ID
        FROM   address
       WHERE   awardmanager_id IN
                     (SELECT   awardmanager_id
                        FROM   awardmanager
                       WHERE   awardmanagers_id IN (SELECT   awardmanagers_id
                                                      FROM   awardmanagers
                                                     WHERE   award_id = v_id));


      SELECT   RELTYPE,DESCRIPTION,ITEM_ID,RELATEDITEMS_ID,LANG
        FROM   item l
       WHERE   EXISTS
                  (SELECT   *
                     FROM   relateditems
                    WHERE   award_id = v_id
                            AND l.RELATEDITEMS_ID = RELATEDITEMS_ID);


      SELECT   url, link_text, item_id
        FROM   LINK
       WHERE   item_id IN
                     (SELECT   item_id
                        FROM   item l
                       WHERE   EXISTS
                                  (SELECT   *
                                     FROM   relateditems
                                    WHERE   award_id = v_id
                                            AND l.RELATEDITEMS_ID =
                                                  RELATEDITEMS_ID));


      SELECT   SCOPUSINSTITUTIONID,
              externalaffiliationidentifier,
               STARTDATE,
               ENDDATE,
               EMAIL,
               WEBPAGE,
               AFFILIATION_ID,
               AWARDEEINSTITUTION_ID
        FROM   affiliation
       WHERE   awardeeinstitution_id IN
                     (SELECT   awardeeinstitution_id
                        FROM   awardeeinstitution
                       WHERE   awardee_id IN
                                     (SELECT   awardee_id
                                        FROM   awardee
                                       WHERE   awardees_id IN
                                                     (SELECT   awardees_id
                                                        FROM   awardees
                                                       WHERE   award_id =
                                                                  v_id)));


      SELECT   *
        FROM   telephone
       WHERE   affiliation_id IN
                     (SELECT   affiliation_id
                        FROM   affiliation
                       WHERE   awardeeinstitution_id IN
                                     (SELECT   awardeeinstitution_id
                                        FROM   awardeeinstitution
                                       WHERE   awardee_id IN
                                                     (SELECT   awardee_id
                                                        FROM   awardee
                                                       WHERE   awardees_id IN
                                                                     (SELECT   awardees_id
                                                                        FROM   awardees
                                                                       WHERE   award_id =
                                                                                  v_id))));

      SELECT   *
        FROM   fax
       WHERE   affiliation_id IN
                     (SELECT   affiliation_id
                        FROM   affiliation
                       WHERE   awardeeinstitution_id IN
                                     (SELECT   awardeeinstitution_id
                                        FROM   awardeeinstitution
                                       WHERE   awardee_id IN
                                                     (SELECT   awardee_id
                                                        FROM   awardee
                                                       WHERE   awardees_id IN
                                                                     (SELECT   awardees_id
                                                                        FROM   awardees
                                                                       WHERE   award_id =
                                                                                  v_id))));

      SELECT   a.COUNTRYTEST,a.ROOM,a.STREET,a.CITY,a.STATE, a.POSTALCODE, get_country_name(a.COUNTRYTEST) country, a.AFFILIATION_ID
        FROM   address a, SCI_COUNTRYCODES
       WHERE   LCODE = a.COUNTRYTEST
               AND affiliation_id IN
                        (SELECT   affiliation_id
                           FROM   affiliation
                          WHERE   awardeeinstitution_id IN
                                        (SELECT   awardeeinstitution_id
                                           FROM   awardeeinstitution
                                          WHERE   awardee_id IN
                                                        (SELECT   awardee_id
                                                           FROM   awardee
                                                          WHERE   awardees_id IN
                                                                        (SELECT   awardees_id
                                                                           FROM   awardees
                                                                          WHERE   award_id =
                                                                                     v_id))));

      SELECT        RELTYPE,
                    TYPE,
                    DOI,
                    PUBMEDID,
                    PMCID,
                    MEDLINEID,
                    SCOPUSID,
                    ITEMTEST_ID,
                    IT.RESEARCHOUTCOME_ID
                    FROM   researchoutcome ro, itemtest it
            WHERE       RO.AWARD_ID = v_id
                    AND RO.RESEARCHOUTCOME_ID = it.RESEARCHOUTCOME_ID;
                   



   SELECT          ITD.ITEMTEST_ID,
                    ITEMID_COLUMN
             FROM   researchoutcome ro, itemtest it, itemid itd
            WHERE       RO.AWARD_ID = v_id
                    AND RO.RESEARCHOUTCOME_ID = it.RESEARCHOUTCOME_ID
                    AND it.ITEMTEST_ID = itd.ITEMTEST_ID;
      SELECT   COUNTRYTEST,
               ROOM,
               STREET,
               CITY,
               STATE,
               POSTALCODE,
               get_country_name(COUNTRY) country,
               AWARD_ID
        FROM   award_location
       WHERE   award_id = v_id;

      SELECT   INSTALLMENTANDAMOUNT_ID, AWARD_ID
        FROM   instalmentAndAmount
       WHERE   award_id = v_id;

              SELECT  ia.award_id,
              case when inst.AMOUNT >0 Then
               DATE_FORMAT (INSTALLMENTSTART_DATE, '%Y')
               else
               '-100'
               End
                   financialYear ,
               case when inst.AMOUNT >0 Then
               inst.AMOUNT
               else
               -100
               End
               AMOUNT,
              case when inst.AMOUNT >0 Then
               inst.CURRENCY
               else
               ''
               End
               inst_CURRENCY,
               case when ta.AMOUNT >0 Then
               ta.CURRENCY
               else
               ''
               End
               CURRENCY,
                case when ta.AMOUNT >0 Then
               ta.AMOUNT
               else
               -100
               End
                totalAmount,
                 case when inst.AMOUNT >0 Then
               1
               else
               -100
               End
                index_txt
               
        FROM   instalmentAndAmount ia, installment inst,totalAmount ta
       WHERE   IA.INSTALLMENTANDAMOUNT_ID = INST.INSTALLMENTANDAMOUNT_ID
       and ia.INSTALLMENTANDAMOUNT_ID = ta.INSTALLMENTANDAMOUNT_ID
       and  INST.INSTALLMENTANDAMOUNT_ID = ta.INSTALLMENTANDAMOUNT_ID
        AND award_id = v_id;

      SELECT   ta.AMOUNT, ta.CURRENCY, ta.INSTALLMENTANDAMOUNT_ID
        FROM   totalAmount ta, instalmentAndAmount ia
       WHERE   award_id = v_id
               AND ia.INSTALLMENTANDAMOUNT_ID = ta.INSTALLMENTANDAMOUNT_ID;

      SELECT   fba.AMOUNT, fba.CURRENCY , fba.RELEATEDFUNDINGBODIES_ID RELATEDFUNDINGBODIES_ID
        FROM   fundingBodyAmount fba, RELATEDFUNDINGBODIES rfb
       WHERE   fba.RELEATEDFUNDINGBODIES_ID = rfb.RELEATEDFUNDINGBODIES_ID
               AND AWARD_ID = V_ID;
               
           SELECT   r.Related_OPP_ID AS grantOpportunityId,o.fundingBodyOpportunityId,o.recordSource,om.opportunityname,'en' lang 
              
        FROM   sci_related_opportunity r,opportunity  o,opportunity_master om where 
        r.Related_OPP_ID=o.opportunity_id
        and o.opportunity_id=om.opportunityid
         and r.award_ID = v_id;
         
               SELECT PUBLICATION_ID,
  AWARD_ID,
  INGESTIONID,
  PUBLICATIONOUTPUTID,
  case when PUBLISHEDDATE is not null then
               CONCAT(IFNULL(DATE_FORMAT(PUBLISHEDDATE,'%Y-%m-%d'), '')
                , 'T'
               , IFNULL(DATE_FORMAT(PUBLISHEDDATE, '%H:%i:%s'), ''))
               else 
               null
               End
  PUBLISHEDDATE,
  CREATEDON,
  LASTUPDATEON,
  PUBLICATION_AUTHOR,
  JOURNAL_IDENTIFIER,
  PUBLICATION_URL,
  FUNDINGBODYPROJECTID,
  PUB_DESCRIPTION
FROM PUBLICATIONDATA where award_id=v_id;
          
           Select pt.publication_id,pd.award_id,pt.title,pt.lang from   
           Publication_Title Pt,Publicationdata Pd Where Pd.Publication_Id=Pt.Publication_Id And Pd.Award_Id=V_Id; 
           
           
           Select pd.publication_id,pd.award_id,it.title IDENTIFIER_TITLE,it.lang,pd.journal_identifier,'ISSN' type 
           from   IDENTIFIERTITLE IT,PublicationData PD where it.publication_id=pd.publication_id and PD.award_id=v_id; 
           
            -- SELECT list FROM table ((Select sci_getcsvtoLIST(publication_author) from PublicationData where award_id=v_id and 1=2 ));
            
                Select Doi,Medlineid Medline,Pubmedid Pubmed,Pmcid Pmc,Scopusid Scopuseid,Pd.Publication_Id ,ro.Award_Id 
                       From Researchoutcome Ro, Itemtest It,Publicationdata Pd
            Where       Ro.Award_Id = v_id And
                     Ro.Researchoutcome_Id = It.Researchoutcome_Id And Pd.Publication_Id = Ro.Publication_Id; 
            
            Select pd.pub_description description,a.fundingBodyAwardId,pd.fundingBodyProjectId,a.award_id grantAwardId 
           from PublicationData PD, award a where pd.award_id=a.award_id and pd.award_id = v_id;
           
                              SELECT   A.AWARD_ID,
                              'COORDINATOR' TYPE,
                             'Research organization' activityType,
                              awardeeAffiliationId,
                              departmentName,
                              B.FBORGANIZATIONID,
                              B.link,
                             awd.org,
                              'en' Language,
                              B.ROR,
                              B.DUNS,
                              B.Vatnumber,
                              B.Wikidata,
                              Currency,
                              Amount_Text,
                              B.Awardee_Id,
                              B.Awardees_Id
                   
                       FROM   AWARDEES A,
                              AWARDEE B,
                              SCI_AWARDEETYPETYPE d,  amount am,
                             ( Select aff.org,adwinst.Awardee_ID from awardeeinstitution adwinst,affiliation aff 
                             where  adwinst.awardeeinstitution_id=aff.awardeeinstitution_id) awd
                      WHERE       A.AWARDEES_ID = B.AWARDEES_ID
                              AND d.CODE = B.TYPE
                              and AM.AWARDEE_ID= B.AWARDEE_ID
                              and B.TYPE not in ('coPI')
                              and B.AWARDEE_ID =awd.AWARDEE_ID
                              AND a.AWARD_ID = v_id;
                              
                               SELECT   
                           B.EMAIL,
                           B.AWARDEEPERSONID,
                           B.FAMILYNAME,
                           B.FUNDINGBODYPERSONID,
                           B.GIVENNAME,
                           B.ORCID,
                          B.Initials,
                          B.Role,
                         B.Name,
                         A.Awardeeinstitution_Id,
                         A.Awardee_Id,
                         B.Affiliation_Id
                           
                    FROM   AWARDEEINSTITUTION A, AFFILIATION B
                   WHERE   A.AWARDEEINSTITUTION_ID = B.AWARDEEINSTITUTION_ID
                           AND AWARDEE_ID in (Select awardee_id from awardee where awardees_id in 
                           (Select awardees_id from awardees where award_id =v_id));
                           
                                         SELECT   A.COUNTRYTEST,
               C.NAME COUNTRYNAME,
               A.ROOM,
               A.STREET,
               A.CITY,
               A.STATE,
               A.POSTALCODE,
               A.AFFILIATION_ID
        FROM   ADDRESS A, SCI_COUNTRYCODES C
       WHERE   C.LCODE = A.COUNTRYTEST AND AFFILIATION_ID in (Select AFFILIATION_ID from affiliation where  AWARDEEINSTITUTION_ID in 
       (Select AWARDEEINSTITUTION_ID from  AWARDEEINSTITUTION where AWARDEE_ID in (Select awardee_id from awardee where awardees_id in 
                           (Select awardees_id from awardees where award_id =v_id)) ) );
                           
                            SELECT 'ROR' Identifier_text, B.ROR value_text FROM   AWARDEES A,AWARDEE B  
                             WHERE A.AWARDEES_ID = B.AWARDEES_ID AND a.AWARD_ID = v_id
                              
                              Union
                              SELECT 'DUNS' Identifier_text,  B.DUNS value_text FROM   AWARDEES A,AWARDEE B  
                             WHERE A.AWARDEES_ID = B.AWARDEES_ID AND a.AWARD_ID = v_id
                               
                              Union
                              
                              SELECT 'WIKIDATA' Identifier_text, B.WIKIDATA value_text FROM   AWARDEES A,AWARDEE B  
                             WHERE A.AWARDEES_ID = B.AWARDEES_ID AND a.AWARD_ID = v_id;
                             
                             
                             
                             SELECT FUND_ID,
                          AWARD_ID,
                         ACRONYM,
                        BUDGET_AMOUNT AMOUNT,
                        BUDGET_CURRENCY  CURRENCY,
                        ENDDATE,
                        FUNDINGBODYPROJECTID,
                        STARTDATE,
                        STATUS,
                        LINK,
                        COUNTRY,
                        LOCALITY,
                        POSTALCODE,
                        REGION,
                        STREET
                        
                        FROM FUNDEDPROJECTDETAIL where AWARD_ID =v_id ;
                        
                             
                            SELECT SUBFUND_ID,
                           FUND_ID,
                           AWARD_ID,
                           FUNDINGBODYPROJECTID,
                           AMOUNT,
                           CURRENCY
                           FROM FUNDEDSUBPROJECT  where AWARD_ID =v_id ;
                           
                         Select column_desc value_text,get_language_code_byid(language_id)  language  
                         from sci_language_detail where scival_id= v_id and moduleid=4 and column_id=8;
END