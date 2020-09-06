CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_Publication_out_insupdel`(
   p_workflowid             INTEGER,
   p_insdel                 INTEGER,
   p_publication_ID         INTEGER ,
   p_FundingBodyProjectId   VARCHAR(4000) ,
   p_PublishedDate          Datetime,
   p_PublicationURL         VARCHAR(4000),
   p_PublicationOutputId    INTEGER,
   p_IngestionId            VARCHAR(4000),
   p_JournalTitle           VARCHAR(4000),
   p_Journalidentifier      VARCHAR(4000),
   p_Authors                VARCHAR(4000),
   p_Description            LONGTEXT
)
BEGIN
   DECLARE v_id                     INTEGER;
   DECLARE v_MODULEID               INTEGER;
   DECLARE v_value                  INTEGER;
   DECLARE v_count                  INTEGER;
   DECLARE d_count                  INTEGER;
   DECLARE v_count1                 INTEGER;
   DECLARE l_scolarly_id            INTEGER;
   DECLARE l_output_item_type       INTEGER;
   DECLARE l_publication_SEQ        INTEGER;
   DECLARE l_IDENTIFIERTITLE_SEQ    INTEGER;
   DECLARE L_ID                     INTEGER;
   DECLARE L_ITEMTEST_ID            INTEGER;
   DECLARE l_researchoutcome_id     INTEGER;
   DECLARE l_count                  INTEGER;
 
   SELECT   ID, MODULEID
     INTO   v_id, v_MODULEID
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;


   IF v_moduleid = 4
   THEN
      IF p_insdel = 0
      THEN
         SELECT   COUNT(*)
           INTO   v_count
           FROM   publicationdata
          WHERE   AWARD_ID = v_id;

         IF v_count > 0
         THEN
         UPDATE PUBLICATIONDATA
SET 
 PUBLICATIONOUTPUTID  = p_PublicationOutputId,
 PUBLISHEDDATE        = p_PublishedDate,
 LASTUPDATEON         = sysdate(),
 PUBLICATION_AUTHOR   = p_Authors,
 JOURNAL_IDENTIFIER   = p_Journalidentifier,
 PUBLICATION_URL      = p_PublicationURL,
 FUNDINGBODYPROJECTID = p_FundingBodyProjectId,
 PUB_DESCRIPTION      = p_Description
WHERE award_id=v_id;

   UPDATE IDENTIFIERTITLE
   SET TITLE=  p_JournalTitle
WHERE PUBLICATION_ID in (Select PUBLICATION_ID from PUBLICATIONDATA where award_id=v_id);
           -- OPEN p_PublicationData FOR
            SELECT PD.PUBLICATION_ID,
  AWARD_ID,
  INGESTIONID,
  PUBLICATIONOUTPUTID,
  PUBLISHEDDATE,
  CREATEDON,
  LASTUPDATEON,
  PUBLICATION_AUTHOR,
  JOURNAL_IDENTIFIER,
  PUBLICATION_URL,
  FUNDINGBODYPROJECTID,
  PUB_DESCRIPTION,
  Title
    FROM PUBLICATIONDATA PD , IDENTIFIERTITLE  IT where PD.publication_id=IT.publication_id and  award_id=v_id;
            
         ELSE
            SELECT  PUBLICATION_SEQ.NEXTVAL
              INTO   l_publication_SEQ
              FROM   DUAL;
              
              SELECT  IDENTIFIERTITLE_SEQ.NEXTVAL
              INTO   l_IDENTIFIERTITLE_SEQ
              FROM   DUAL;
              


 INSERT
INTO PUBLICATIONDATA
  (
    PUBLICATION_ID,
    AWARD_ID,
    INGESTIONID,
    PUBLICATIONOUTPUTID,
    PUBLISHEDDATE,
    CREATEDON,
    LASTUPDATEON,
    PUBLICATION_AUTHOR,
    JOURNAL_IDENTIFIER,
    PUBLICATION_URL,
    FUNDINGBODYPROJECTID,
    PUB_DESCRIPTION
  )
  VALUES
  (
   l_publication_SEQ,
    v_id,
    p_IngestionId,
    p_PublicationOutputId,
    p_PublishedDate,
    sysdate(),
    sysdate(),
    p_Authors,
    p_Journalidentifier,
    p_PublicationURL,
    p_FundingBodyProjectId,
    p_Description
  );   
  
  INSERT
INTO IDENTIFIERTITLE
  (
    ID,
    PUBLICATION_ID,
    TITLE,
    LANG,
    CREATEDDATE
  )
  VALUES
  (
    l_IDENTIFIERTITLE_SEQ,
    l_publication_SEQ,
    p_JournalTitle,
    'en',
    sysdate()
  );
            END IF;
         END IF;
         
      ELSEIF P_INSDEL = 1
      THEN
         IF p_publication_ID <> 0
         THEN
          UPDATE PUBLICATIONDATA
SET 
 PUBLICATIONOUTPUTID  = p_PublicationOutputId,
 PUBLISHEDDATE        = p_PublishedDate,
 LASTUPDATEON         = sysdate(),
 PUBLICATION_AUTHOR   = p_Authors,
 JOURNAL_IDENTIFIER   = p_Journalidentifier,
 PUBLICATION_URL      = p_PublicationURL,
 FUNDINGBODYPROJECTID = p_FundingBodyProjectId,
 PUB_DESCRIPTION      = p_Description
WHERE award_id=v_id;

   UPDATE IDENTIFIERTITLE
   SET TITLE=  p_JournalTitle
WHERE PUBLICATION_ID in (Select PUBLICATION_ID from PUBLICATIONDATA where award_id=v_id); 
         END IF;
          ELSEIF P_INSDEL = 3
         THEN
         Delete FROM publicationdata where award_id=v_id;
         
          --     OPEN p_PublicationData FOR
     
      
           SELECT PD.PUBLICATION_ID,
  AWARD_ID,
  INGESTIONID,
  PUBLICATIONOUTPUTID,
  PUBLISHEDDATE,
  CREATEDON,
  LASTUPDATEON,
  PUBLICATION_AUTHOR,
  JOURNAL_IDENTIFIER,
  PUBLICATION_URL,
  FUNDINGBODYPROJECTID,
  PUB_DESCRIPTION,
  Title
    FROM PUBLICATIONDATA PD , IDENTIFIERTITLE  IT where PD.publication_id=IT.publication_id and  award_id=v_id;
     
      END IF;

      -- OPEN p_PublicationData FOR
           SELECT PD.PUBLICATION_ID,
  AWARD_ID,
  INGESTIONID,
  PUBLICATIONOUTPUTID,
  PUBLISHEDDATE,
  CREATEDON,
  LASTUPDATEON,
  PUBLICATION_AUTHOR,
  JOURNAL_IDENTIFIER,
  PUBLICATION_URL,
  FUNDINGBODYPROJECTID,
  PUB_DESCRIPTION,
  Title
    FROM PUBLICATIONDATA PD , IDENTIFIERTITLE  IT where PD.publication_id=IT.publication_id and  award_id=v_id;
         
END