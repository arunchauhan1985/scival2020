CREATE DEFINER=`root`@`localhost` PROCEDURE `awardssource_dml_prc`(
   p_url                   VARCHAR(4000) /* DEFAULT NULL */ ,
   p_status                VARCHAR(4000) /* DEFAULT NULL */ ,
   p_lastvisited           DATETIME /* DEFAULT NULL */ ,
   p_Workflow_id           INTEGER /* DEFAULT NULL */ ,
   p_lang                  VARCHAR(4000) /* DEFAULT NULL */ ,
   p_mode                  INTEGER, -- 1 for insert, 2 for update ,3 for display, 4 for deleteion of award source
   p_award_source_id       INTEGER /* DEFAULT NULL */   
)
BEGIN
   DECLARE v_moduleid        INTEGER;
   DECLARE v_batch           INTEGER;
   DECLARE v_cnt             INTEGER;
   DECLARE v_award_sequnce   INTEGER;
   DECLARE v_id              INTEGER;

SELECT 
    id
INTO v_id FROM
    sci_workflow
WHERE
    WORKFLOWID = p_Workflow_id;

   IF p_mode = 1
   THEN
      SELECT   awardssource_seq.NEXTVAL INTO v_award_sequnce FROM DUAL;

      INSERT INTO awardssource (AWARD_SOURCE_ID,
                                URL,
                                STATUS,
                                LASTVISITED,
                                FUNDINGBODY_ID,
                                LANG)
        VALUES   (v_award_sequnce,
                  p_url,
                  p_status,
                  p_lastvisited,
                  v_id,
                  p_lang);
      
SELECT 
    NULL AWARD_SOURCE_ID,
    NULL URL,
    NULL STATUS,
    NULL LASTVISITED,
    NULL FUNDINGBODY_ID,
    NULL lang
FROM DUAL;
   ELSEIF p_mode = 2
   THEN
      UPDATE   awardssource
         SET   URL = p_url, STATUS = p_status, LASTVISITED = p_lastvisited, lang=p_lang
       WHERE   AWARD_SOURCE_ID = p_award_source_id;
      
SELECT 
    NULL AWARD_SOURCE_ID,
    NULL URL,
    NULL STATUS,
    NULL LASTVISITED,
    NULL FUNDINGBODY_ID,
    NULL lang
FROM DUAL;
   ELSEIF p_mode = 3
   THEN
      
         SELECT   AWARD_SOURCE_ID,
                  URL,
                  STATUS,
                  LASTVISITED,
                  FUNDINGBODY_ID,
                  LANG
           FROM   awardssource
          WHERE   FUNDINGBODY_ID = v_id;
      
   ELSEIF p_mode = 4
   THEN
      DELETE FROM   awardssource
            WHERE   AWARD_SOURCE_ID = p_award_source_id;

   END IF;
END ;;