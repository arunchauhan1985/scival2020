CREATE PROCEDURE `SCI_OP_OPPORTUNITYINS`(
P_WORKFLOWID                         INTEGER,
   P_FUNDINGBODYOPPORTUNITYID           VARCHAR(4000),
   P_LIMITEDSUBMISSION                  VARCHAR(4000),
   P_TRUSTING                           VARCHAR(4000),
   P_COLLECTIONCODE                     VARCHAR(4000),
   P_HIDDEN                             VARCHAR(4000),
   P_NAME                               VARCHAR(4000),
   P_DUEDATEDESCRIPTION                 LONGTEXT,
   P_ELIGIBILITYDESCRIPTION             LONGTEXT,
   P_ELIGIBILITYCATEGORY                VARCHAR(4000),
   P_LINKTOFULLTEXT                     VARCHAR(4000),
   P_OPPORTUNITYSTATUS                  VARCHAR(4000),
   P_NUMBEROFAWARDS                     INTEGER,
   P_DURATION                           VARCHAR(4000),
   P_LIMITEDSUBMISSIONDESCRIPTION       VARCHAR(4000),
   P_RAWTEXT                            LONGTEXT, 
   p_id                                 VARCHAR(4000),
   p_url                                VARCHAR(4000),
   p_mode                               INTEGER, -- 0 for insert ,1 for update
   p_recordsource VARCHAR(4000)                       /* Use -meta option opportunity.recordsource%TYPE */, -- -- Added by Julfcar on dt- 20130218
   p_loi_mandatory VARCHAR(4000)                       /* Use -meta option opportunity.LOI_MANDATORY %TYPE */, 
    p_preProposalMandatory              VARCHAR(4000),-- -- Added by avinash on dt- 220180619
   p_repeatingOpportunity               VARCHAR(4000),
   p_langRSource                        VARCHAR(4000)
)
BEGIN
   DECLARE V_MODULEID               INTEGER;
   DECLARE Pv_RAWTEXT               VARCHAR (100);
   DECLARE v_subtype                INTEGER;
   DECLARE V_ID                     INTEGER;
   DECLARE V_CYCLE                  INTEGER;
   DECLARE V_FUNDINGBODYID          INTEGER;
   DECLARE v_value                  VARCHAR (2000);
   DECLARE v_REVISIONHISTORYID      INTEGER;
   DECLARE V_TRUSTING               VARCHAR (200);
   DECLARE v_preferedorgname        LONGTEXT;
   DECLARE v_type                   VARCHAR (2000);
   DECLARE v_RELATEDORGS_ID         INTEGER;
   DECLARE v_cnt                    INTEGER;  
   DECLARE l_opportunity_name_chk   INTEGER;
   DECLARE l_count  INTEGER;
   DECLARE l_o_id   INTEGER;
   
   SET Pv_RAWTEXT = SUBSTR (P_RAWTEXT, 1, 50);

   SELECT   MODULEID, ID, CYCLE
     INTO   V_MODULEID, V_ID, V_CYCLE
     FROM   SCI_WORKFLOW
    WHERE   WORKFLOWID = P_WORKFLOWID;

   IF V_MODULEID = 3
   THEN
      SELECT   FUNDINGBODYID
        INTO   V_FUNDINGBODYID
        FROM   OPPORTUNITY_MASTER
       WHERE   OPPORTUNITYID = V_ID;

      SELECT   TRUSTING, PREFERREDORGNAME, TYPE
        INTO   V_TRUSTING, v_preferedorgname, v_type
        FROM   FUNDINGBODY
       WHERE   FUNDINGBODY_ID = V_FUNDINGBODYID;

      SELECT   COUNT (1)
        INTO   v_subtype
        FROM   SUBTYPE
       WHERE   SUBTYPE_TEXT IN ('federal', 'Federal/National Government')
               AND FUNDINGBODY_ID = V_FUNDINGBODYID;
   END IF;

   IF p_mode = 0
   THEN
      SELECT   COUNT( * )
        INTO   l_opportunity_name_chk
        FROM   sci_language_detail sl_dtl
       WHERE   EXISTS
                  (SELECT   1
                     FROM   opportunity
                    WHERE   fundingbody_id = v_fundingbodyid
                            AND opportunity_id = sl_dtl.scival_id)
               AND sl_dtl.moduleid = 3
               AND column_id = 5
               and dbms_lob.instr(column_desc, to_clob(p_name))>=1;
      IF l_opportunity_name_chk > 0
      THEN
         ROLLBACK;
      END IF;

      INSERT INTO OPPORTUNITY (FUNDINGBODY_ID,
                               FUNDINGBODYOPPORTUNITYID,
                               LIMITEDSUBMISSION,
                               TRUSTING,
                               COLLECTIONCODE,
                               HIDDEN,
                             DUEDATEDESCRIPTION,
                               ELIGIBILITYDESCRIPTION,
                               ELIGIBILITYCATEGORY,
                               LINKTOFULLTEXT,
                              OPPORTUNITYSTATUS,
                               NUMBEROFAWARDS,
                               DURATION,
                               LIMITEDSUBMISSIONDESCRIPTION,
                               OPPORTUNITY_ID,                           -- ,ID
                               RECORDSOURCE,
                               loi_mandatory,
                               REPEATINGOPPORTUNITY,
                               PREPROPOSALMANDATORY,
                               Lang
                               )
        VALUES   (V_FUNDINGBODYID,
                  P_FUNDINGBODYOPPORTUNITYID,
                  P_LIMITEDSUBMISSION,
                  V_TRUSTING,
                  P_COLLECTIONCODE,
                  P_HIDDEN,
                  P_DUEDATEDESCRIPTION,
                  P_ELIGIBILITYDESCRIPTION,
                  P_ELIGIBILITYCATEGORY,
                  P_LINKTOFULLTEXT,
                  P_OPPORTUNITYSTATUS,
                  P_NUMBEROFAWARDS,
                  P_DURATION,
                  P_LIMITEDSUBMISSIONDESCRIPTION,
                  V_ID,
                  P_RECORDSOURCE,
                  p_loi_mandatory,
                  p_repeatingOpportunity,
                  p_preProposalMandatory,
                  p_langRSource
                                  );

      -- -------------------------------------------------------------------------------------------------------------------------
      IF p_id IS NOT NULL
      THEN
         SELECT   TYPE
           INTO   v_value
           FROM   SCI_FUNDINGTYPEIDSTYPE
          WHERE   VALUE = p_id;

         INSERT INTO TYPE (ID, TYPE_TEXT, OPPORTUNITY_ID)
           VALUES   (p_id, v_value, V_ID);
      END IF;

      UPDATE   OPPORTUNITY_master
         SET   OPPORTUNITYNAME = P_NAME, URL = p_url, CREATEDDATE = SYSDATE()
       WHERE   OPPORTUNITYID = V_ID;

      SELECT   REVISIONHISTORYID_SEQ.NEXTVAL INTO v_REVISIONHISTORYID FROM DUAL;

      INSERT INTO REVISIONHISTORY (
                                      STATUS,
                                      REVISIONHISTORY_ID,
                                      OPPORTUNITY_ID
                 )
        VALUES   ('new', v_REVISIONHISTORYID, V_ID);

      INSERT INTO CREATEDDATE (VERSION, CREATEDDATE_TEXT, REVISIONHISTORY_ID)
        VALUES   (0, SYSDATE(), v_REVISIONHISTORYID);
     
   ELSEIF p_mode = 1
   THEN
      -- v.0.2 modification start
      SELECT   COUNT( * )
        INTO   l_opportunity_name_chk
        FROM   sci_language_detail sl_dtl
       WHERE   EXISTS
                  (SELECT   1
                     FROM   opportunity
                    WHERE   fundingbody_id = v_fundingbodyid
                            AND opportunity_id = sl_dtl.scival_id)
               AND sl_dtl.moduleid = 3
               AND column_id = 5
               AND sl_dtl.scival_id <> v_id
             AND TO_CHAR (column_desc) = p_name;

      IF l_opportunity_name_chk > 0
      THEN
         ROLLBACK;
      END IF;

      UPDATE   OPPORTUNITY
         SET   FUNDINGBODYOPPORTUNITYID = P_FUNDINGBODYOPPORTUNITYID,
               LIMITEDSUBMISSION = P_LIMITEDSUBMISSION,
               COLLECTIONCODE = P_COLLECTIONCODE,
               HIDDEN = P_HIDDEN,
                 DUEDATEDESCRIPTION = P_DUEDATEDESCRIPTION,
               ELIGIBILITYDESCRIPTION = P_ELIGIBILITYDESCRIPTION,
               ELIGIBILITYCATEGORY = P_ELIGIBILITYCATEGORY,
               LINKTOFULLTEXT = P_LINKTOFULLTEXT,
                 OPPORTUNITYSTATUS = P_OPPORTUNITYSTATUS,
               NUMBEROFAWARDS = P_NUMBEROFAWARDS,
               DURATION = P_DURATION,
               LIMITEDSUBMISSIONDESCRIPTION = P_LIMITEDSUBMISSIONDESCRIPTION,
               RECORDSOURCE = P_RECORDSOURCE,
               loi_mandatory=p_loi_mandatory,
               REPEATINGOPPORTUNITY= p_repeatingOpportunity,
                PREPROPOSALMANDATORY=  p_preProposalMandatory,
                Lang=p_langRSource
       WHERE   OPPORTUNITY_ID = V_ID;

      -- -------------------------------------------------------------------------------------------------------------------------
      UPDATE   OPPORTUNITY_master
         SET   OPPORTUNITYNAME = P_NAME
       WHERE   OPPORTUNITYID = V_ID;


      IF p_id IS NOT NULL
      THEN
         SELECT   TYPE
           INTO   v_value
           FROM   SCI_FUNDINGTYPEIDSTYPE
          WHERE   VALUE = p_id;

         SELECT   COUNT (1)
           INTO   v_cnt
           FROM   TYPE
          WHERE   OPPORTUNITY_ID = V_ID;

         IF v_cnt = 0
         THEN
            INSERT INTO TYPE
              VALUES   (p_id, v_value, v_id);
         ELSE
            UPDATE   TYPE
               SET   ID = p_id, TYPE_TEXT = v_value
             WHERE   OPPORTUNITY_ID = V_ID;
         END IF;
      END IF;
      
   END IF;

      SELECT   fundingbody_id,
               fundingbodyopportunityid,
               limitedsubmission,
               trusting,
               collectioncode,
               hidden,
               eligibilitycategory,
               linktofulltext,
               firstpostdate,
               lastmodifiedpostdate,
               opportunitystatus,
               numberofawards,
               duration,
               limitedsubmissiondescription,
               opportunity_id,
               eligibilitydescription,
               duedatedescription,
               id,
               recordsource,
               expirationdate,
               loi_mandatory,
               REPEATINGOPPORTUNITY,
               PREPROPOSALMANDATORY,
               (CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
        FROM   OPPORTUNITY
       WHERE   OPPORTUNITY_ID = V_ID;

      SELECT   *
        FROM   TYPE
       WHERE   OPPORTUNITY_ID = V_ID;

      SELECT   opportunity_id,
               date_format(loi_due_date,'%m-%d-%Y') loi_DATE,
               sequence_id
           FROM   sci_opp_loi_duedate_detail
       WHERE   OPPORTUNITY_ID = V_ID
        AND DATE_TYPE=1;
        
      SELECT   opportunity_id,
                (CASE WHEN loi_due_date IS NULL THEN DATE_REMARKS ELSE to_clob(DATE_FORMAT(loi_due_date,'%m-%d-%Y')) END) DUE_DATE,
               sequence_id
           FROM   sci_opp_loi_duedate_detail
       WHERE   OPPORTUNITY_ID = V_ID
        AND DATE_TYPE=2;

         SELECT   *
           FROM   OPPORTUNITY
          WHERE   OPPORTUNITY_ID = V_ID;

         SELECT   *
           FROM   TYPE
          WHERE   OPPORTUNITY_ID = V_ID;
END;
