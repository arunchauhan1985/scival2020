CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_aw_awradins50`(
   p_workflowid               INTEGER,
   p_FUNDINGBODYAWARDID       VARCHAR(4000),
   p_TYPE                     VARCHAR(4000),
   p_TRUSTING                 VARCHAR(4000),
   p_COLLECTIONCODE           VARCHAR(4000),
   p_HIDDEN                   VARCHAR(4000),
   p_NAME                     VARCHAR(4000) ,
   p_STARTDATE                DATETIME,
   p_LASTAMENDEDDATE          DATETIME,
   p_ENDDATE                  DATETIME,
   p_ABSTRACT                 VARCHAR(4000) ,
   p_url                      VARCHAR(4000),
   p_mode                     INTEGER, 
   p_recordsource VARCHAR(4000) ,
   p_AwardNoticeDate           datetime,
   p_PublishedDate           datetime 
   )
BEGIN
   DECLARE V_MODULEID             INTEGER;
   DECLARE V_ID                   INTEGER;
   DECLARE V_FUNDINGBODYID        INTEGER;
   DECLARE v_REVISIONHISTORYID    INTEGER;
   --  DECLARE E_ERROR exception;
   DECLARE V_TRUSTING             VARCHAR (200);
   DECLARE v_preferedorgname      LONGTEXT;
   DECLARE v_type                 VARCHAR (2000);
   DECLARE v_RELATEDORGS_ID       INTEGER;
   DECLARE v_cnt                  INTEGER;
   DECLARE l_fundingbodyawardid   INTEGER;
   DECLARE l_awardid              INTEGER;
   DECLARE l_tran_id              decimal;
   -- DECLARE l_col_mast_ot          sci_language_common_ot  DEFAULT  sci_language_common_ot ();
   -- DECLARE l_col_mast_ntt         sci_language_common_ntt DEFAULT  sci_language_common_ntt () ;

    SELECT   * FROM   award WHERE   AWARD_ID = V_ID;
   INSERT INTO FROG VALUES(CONCAT(IFNULL(P_WORKFLOWID, '') , ' - ' , IFNULL(P_TYPE, '') , ' - ' , IFNULL(P_HIDDEN, '') , ' - ' , IFNULL(P_STARTDATE, '')), SYSDATE());

   SELECT   MODULEID, ID INTO   V_MODULEID, V_ID FROM   SCI_WORKFLOW WHERE   WORKFLOWID = P_WORKFLOWID;

   IF V_MODULEID = 4
   THEN
      SELECT   FUNDINGBODYID  INTO   V_FUNDINGBODYID FROM   award_MASTER WHERE   AWARDID = V_ID;
      SELECT   TRUSTING INTO   V_TRUSTING FROM   FUNDINGBODY WHERE   FUNDINGBODY_ID = V_FUNDINGBODYID;
   END IF;
   /*SET l_col_mast_ot.column_id = 6;
   SET l_col_mast_ot.column_desc = p_abstract;
   l_col_mast_ntt.EXTEND;
   l_col_mast_ntt (1) := l_col_mast_ot;
   SET l_col_mast_ot.column_id = 5;
   SET l_col_mast_ot.column_desc = p_name;
   l_col_mast_ntt.EXTEND;
   l_col_mast_ntt (2) := l_col_mast_ot; */
   IF p_mode = 0
   THEN
   SELECT   COUNT(*) INTO   l_fundingbodyawardid FROM   award a, award_master am WHERE       a.FUNDINGBODY_ID = AM.FUNDINGBODYID AND A.AWARD_ID = AM.AWARDID AND a.fundingbody_id = V_FUNDINGBODYID AND Upper(FUNDINGBODYAWARDID) = Upper(p_FUNDINGBODYAWARDID) 
            and not exists(select 1 from SCI_XMLDELIVERYDETAIL  where id=am.awardid and  am.statuscode=3 ) AND Upper(FUNDINGBODYAWARDID) <> Upper('Not Available');
      IF l_fundingbodyawardid < 0  
      THEN
         INSERT INTO award (FUNDINGBODY_ID, FUNDINGBODYAWARDID, TYPE, TRUSTING, COLLECTIONCODE, HIDDEN, NAME, STARTDATE, LASTAMENDEDDATE, ENDDATE, ABSTRACT, AWARD_ID, ID, RECORDSOURCE, AwardNoticeDate, PUBLISHEDDATE) VALUES   (v_FUNDINGBODYID, p_FUNDINGBODYAWARDID, p_TYPE, V_TRUSTING, p_COLLECTIONCODE, p_HIDDEN, p_NAME, p_STARTDATE,
                     p_LASTAMENDEDDATE, p_ENDDATE, p_ABSTRACT, v_id, v_id, P_RECORDSOURCE, p_AwardNoticeDate, p_PublishedDate);
         IF v_TYPE = 'gov'
         THEN
            SELECT   RELATEDORGS_SEQ.NEXTVAL INTO v_RELATEDORGS_ID FROM DUAL;

            INSERT INTO relatedorgs (HIERARCHY, RELATEDORGS_ID, award_ID)
              VALUES   ('lead', v_RELATEDORGS_ID, V_ID);

            INSERT INTO org (ORGDBID,  RELTYPE, ORG_TEXT, RELATEDORGS_ID)
              VALUES   (V_FUNDINGBODYID,  'fundedBy',  v_preferedorgname, v_RELATEDORGS_ID);

            SELECT   COUNT(1) INTO   v_cnt  FROM   org WHERE   RELATEDORGS_ID IN (SELECT   RELATEDORGS_ID FROM   relatedorgs WHERE   HIERARCHY = 'component' AND award_ID = V_ID);

            IF v_cnt > 0
            THEN
               SELECT   RELATEDORGS_SEQ.NEXTVAL INTO v_RELATEDORGS_ID FROM DUAL;

               INSERT INTO relatedorgs (  HIERARCHY, RELATEDORGS_ID, OPPORTUNITY_ID )
                 VALUES   ('component', v_RELATEDORGS_ID, V_ID);

               INSERT INTO org (ORGDBID, RELTYPE, ORG_TEXT, RELATEDORGS_ID)
                  SELECT   ORGDBID, RELTYPE,  ORG_TEXT, v_RELATEDORGS_ID FROM   org WHERE   RELATEDORGS_ID IN
                                 (SELECT   RELATEDORGS_ID FROM   relatedorgs WHERE   HIERARCHY = 'component' AND award_ID = V_ID);
            END IF;
         END IF;
     UPDATE   award_master SET   AWARDNAME = P_NAME, URL = p_url WHERE   AWARDID = V_ID;
     SELECT   REVISIONHISTORYID_SEQ.NEXTVAL INTO   v_REVISIONHISTORYID FROM   DUAL;

     INSERT INTO REVISIONHISTORY (STATUS, REVISIONHISTORY_ID, AWARD_ID) VALUES   ('new', v_REVISIONHISTORYID, V_ID);
     INSERT INTO CREATEDDATE ( VERSION, CREATEDDATE_TEXT,  REVISIONHISTORY_ID ) VALUES   (0, SYSDATE(), v_REVISIONHISTORYID);
   END IF;
   ELSEIF p_mode = 1
   THEN
      SELECT   COUNT(*)  INTO   l_awardid FROM   award WHERE fundingbody_id = V_FUNDINGBODYID
               AND AWARD_ID <> v_id AND FUNDINGBODYAWARDID = p_FUNDINGBODYAWARDID AND Upper(FUNDINGBODYAWARDID) <> Upper('Not Available');
      IF l_awardid = 0
      THEN
         UPDATE award SET   FUNDINGBODY_ID = v_FUNDINGBODYID, FUNDINGBODYAWARDID = p_FUNDINGBODYAWARDID, TYPE = p_TYPE, COLLECTIONCODE = p_COLLECTIONCODE,
                  HIDDEN = p_HIDDEN, NAME = p_NAME,  STARTDATE = p_STARTDATE, LASTAMENDEDDATE = p_LASTAMENDEDDATE, ENDDATE = p_ENDDATE,RECORDSOURCE = P_RECORDSOURCE,
                  AwardNoticeDate=p_AwardNoticeDate, PUBLISHEDDATE=p_PublishedDate WHERE   AWARD_ID = v_id;
		UPDATE   award_master  SET   AWARDNAME = P_NAME, URL = p_url WHERE   AWARDID = V_ID;
	END IF;
END IF;

      SELECT  * FROM   award WHERE   AWARD_ID = V_ID;
   END