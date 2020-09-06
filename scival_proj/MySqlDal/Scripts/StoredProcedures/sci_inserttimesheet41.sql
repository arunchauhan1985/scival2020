CREATE PROCEDURE `sci_inserttimesheet41`(
p_workflowid           INTEGER,
   p_userid               INTEGER
   )
BEGIN
   DECLARE v_moduleid   INTEGER;
   DECLARE v_id         INTEGER;
   DECLARE v_cycleid    INTEGER;
   DECLARE v_taskid     INTEGER;
   DECLARE p_TRANSITIONALID INTEGER;
   DECLARE p_murl	NVARCHAR(200);
   DECLARE p_fname NVARCHAR(200);

   SELECT   moduleid,
            ID,
            CYCLE,
            taskid
     INTO   v_moduleid,
            v_id,
            v_cycleid,
            v_taskid
     FROM   sci_workflow
    WHERE   workflowid = p_workflowid;


   UPDATE   sci_timesheet
      SET   ENDDATE = SYSDATE()
    WHERE   workflowid = p_workflowid AND ENDDATE IS NULL;

   SELECT   IFNULL (MAX (TRANSITIONALID), 0)
     INTO   p_TRANSITIONALID
     FROM   sci_timesheet
    WHERE       workflowid = p_workflowid
            AND STATUSCODE = 7
            AND userid = p_userid;

   IF p_TRANSITIONALID <> 0
   THEN
      UPDATE   sci_timesheet
         SET   IDLETIME = SYSDATE() - IFNULL (ENDDATE, SYSDATE()), enddate = NULL
       WHERE   TRANSITIONALID = p_TRANSITIONALID;
   ELSE
      SELECT   timesheetid_seq.NEXTVAL INTO p_TRANSITIONALID FROM DUAL;

      INSERT INTO sci_timesheet (transitionalid,
                                 workflowid,
                                 moduleid,
                                 ID,
                                 taskid,
                                 userid,
                                 cycleid,
                                 statuscode,
                                 startdate)
        VALUES   (p_TRANSITIONALID,
                  p_workflowid,
                  v_moduleid,
                  v_id,
                  v_taskid,
                  p_userid,
                  v_cycleid,
                  7,
                  SYSDATE());
   END IF;

   IF v_moduleid = 2
   THEN
      SELECT   url, fundingbodyname
        INTO   p_murl, p_fname
        FROM   fundingbody_master
       WHERE   fundingbody_id = v_id;

         SELECT   fb.FUNDINGBODY_ID,
                  ORGDBID,
                  TYPE,
                  TRUSTING,
                  COUNTRY,
                  STATE,
                  COLLECTIONCODE,
                  HIDDEN,
                  ELIGIBILITYDESCRIPTION,
                  ID subtypeid,
                  SUBTYPE_TEXT,
                  recordsource,
                  awardsuccesrate,
                  comment_desc,                          -- v.0.2 modification
                  DEFUNCT, -- added by Rakesh  on  10-dec-2012  for  v.2 .0 modification
                  CROSSREFID, -- -- added by Rakesh  on  10-dec-2012  for  v.2 .0 modification
                  EXTENDEDRECORD,
                  captureawards, -- ---modified  by  AVINASH    on  29-MAY-2018  for modeficaiton of   v.40
                  captureopportunities, -- ---modified  by  AVINASH    on  29-MAY-2018  for modeficaiton of   v.40
                  tierinfo, -- ---modified  by  AVINASH    on  29-MAY-2018  for modeficaiton of   v.40
                  awardssupplier, -- ---modified  by  AVINASH    on  29-MAY-2018  for modeficaiton of   v.40
                  opportunitiessupplier, -- ---modified  by  AVINASH    on  29-MAY-2018  for modeficaiton of   v.40
                  profit,
                 opportunitiesfrequency,
                awardsfrequency
           FROM   fundingbody fb, subtype st
          WHERE   fb.FUNDINGBODY_ID = st.FUNDINGBODY_ID
                  AND fb.fundingbody_id = v_id;

      -- --------------------------------------------------------------------------------
SELECT   * FROM sci_countrycodes;

         SELECT   sc.lcode countrycode, ss.NAME, ss.code
           FROM   sci_countrycodes sc, sci_statecodes ss
          WHERE   sc.countryid = ss.countryid;
          
SELECT   * FROM sci_fundingbodytypeidstype;

SELECT   * FROM sci_fundingbodysubtypeidstype order by value;

          SELECT   NULL award_KEYWORDS,
                    NULL KEYWORDS_ID,
                    NULL LANG,
                    NULL AWARD_ID
             FROM   keyword k, keywords ks
            WHERE   k.KEYWORDS_ID = ks.KEYWORDS_ID AND FUNDINGBODY_ID = V_ID
         GROUP BY   k.KEYWORDS_ID, K.LANG, ks.AWARD_ID;
   ELSEIF v_moduleid = 3
   THEN
      SELECT   url
        INTO   p_murl
        FROM   FUNDINGBODY_MASTER
       WHERE   FUNDINGBODY_ID = (SELECT   FUNDINGBODYID
                                   FROM   OPPORTUNITY_MASTER
                                  WHERE   OPPORTUNITYID = V_ID);

        SELECT   fundingbody_id,
                  fundingbodyopportunityid,
                  limitedsubmission,
                  trusting,
                  -- newinvestigator,  ---v.2.0 commented by avinash
                  collectioncode,
                  hidden,
                  -- name,-- v.0.2 modification
                  --   loidate,
                  -- -   duedate,
                  eligibilitycategory,
                  linktofulltext,
                  firstpostdate,
                  lastmodifiedpostdate,
                  opportunitystatus,
                  numberofawards,
                  duration,
                  limitedsubmissiondescription,
                  opportunity_id,
                  -- - rawtext,   ---v.2.0 commented by avinash
                  eligibilitydescription,
                  duedatedescription,
                  id,
                  recordsource,
                  expirationdate,
               (CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
           FROM   OPPORTUNITY
          WHERE   OPPORTUNITY_ID = V_ID;

         SELECT   *
           FROM   TYPE
          WHERE   OPPORTUNITY_ID = V_ID;

 SELECT   * FROM sci_countrycodes;

          SELECT   sc.lcode countrycode, ss.NAME, ss.code
           FROM   sci_countrycodes sc, sci_statecodes ss
          WHERE   sc.countryid = ss.countryid;

         SELECT   opportunity_id, loi_due_date loi_DATE, sequence_id
           FROM   sci_opp_loi_duedate_detail
          WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 1;

         SELECT   opportunity_id,
                  (CASE
                      WHEN loi_due_date IS NULL THEN DATE_REMARKS
                      ELSE TO_CLOB (DATE_FORMAT (loi_due_date, '%m-%d-%Y'))
                   END)
                     DUE_DATE,
                  sequence_id
           FROM   sci_opp_loi_duedate_detail
          WHERE   OPPORTUNITY_ID = V_ID AND DATE_TYPE = 2;

 SELECT   * FROM sci_FundingTypeIDsType;

          SELECT k.KEYWORD_COLUMN    opportunity_KEYWORDS,
                    k.KEYWORDS_ID,
                    K.LANG,
                    ks.opportunity_id
             FROM   keyword k, keywords ks
            WHERE   k.KEYWORDS_ID = ks.KEYWORDS_ID AND ks.opportunity_id = V_ID
         GROUP BY   k.KEYWORDS_ID, K.LANG, ks.opportunity_id;
   ELSEIF v_moduleid = 4
   THEN
      SELECT   url
        INTO   p_murl
        FROM   FUNDINGBODY_MASTER
       WHERE   FUNDINGBODY_ID = (SELECT   FUNDINGBODYID
                                   FROM   award_MASTER
                                  WHERE   AWARDID = V_ID);

         SELECT   fundingbody_id,
                  fundingbodyawardid,
                  TYPE,
                  trusting,
                  collectioncode,
                  hidden,
                  -- name,-- v.0.2 modification
                  startdate,
                  lastamendeddate,
                  enddate,
                  award_id,
                  id,
                  -- abstract,-- v.0.2 modification
                  recordsource,
                  AWARDNOTICEDATE,
                  PUBLISHEDDATE-- ADDED ON 7-JUN-2018 FOR SCIVAL SCHEMA VERSION 4.0
                  
           FROM   award
          WHERE   AWARD_ID = V_ID;

         SELECT   *
           FROM   DUAL
          WHERE   1 = 2;

 SELECT   * FROM sci_countrycodes;

         SELECT   sc.lcode countrycode, ss.NAME, ss.code
           FROM   sci_countrycodes sc, sci_statecodes ss
          WHERE   sc.countryid = ss.countryid;

   SELECT   * FROM SCI_FUNDEDPROGRAMSTYPELIST;

      SELECT  k.KEYWORD_COLUMN
                       award_KEYWORDS,
                    k.KEYWORDS_ID,
                    K.LANG,
                    ks.AWARD_ID
             FROM   keyword k, keywords ks
            WHERE   k.KEYWORDS_ID = ks.KEYWORDS_ID AND AWARD_ID = V_ID
         GROUP BY   k.KEYWORDS_ID, K.LANG, ks.AWARD_ID;
   END IF;

   COMMIT;
END;
