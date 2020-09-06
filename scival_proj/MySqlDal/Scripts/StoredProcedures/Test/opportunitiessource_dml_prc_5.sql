CREATE DEFINER=`root`@`localhost` PROCEDURE `opportunitiessource_dml_prc_5`(
   p_url                  VARCHAR(4000) /* DEFAULT NULL */ ,
   p_status               VARCHAR(4000) /* DEFAULT NULL */ ,
   p_Workflow_id          INTEGER /* DEFAULT NULL */ ,
   p_lang                 VARCHAR(4000) /* DEFAULT NULL */ ,
   p_name                 LONGTEXT /* DEFAULT NULL */ ,
   p_Frequency            Varchar(4000) /* DEFAULT NULL */ ,
   p_date_captureStart    DATETIME /* DEFAULT NULL */,
   p_date_captureEnd      DATETIME /* DEFAULT NULL */,
   p_Comment              LONGTEXT /* DEFAULT NULL */ ,
   p_mode                 INTEGER, -- 1 for insert, 2 for update ,3 for display , 4 for deletion of data
   p_opp_source_id        INTEGER /* DEFAULT NULL */
)
BEGIN
   DECLARE v_moduleid      INTEGER;
   DECLARE v_batch         INTEGER;
   DECLARE v_cnt           INTEGER;
   DECLARE v_opp_sequnce   INTEGER;
   DECLARE v_id            integer;
    
SELECT 
    id
INTO v_id FROM
    sci_workflow
WHERE
    WORKFLOWID = p_Workflow_id;
   IF p_mode = 1
   THEN
               SELECT   opportunitiessource_seq.NEXTVAL INTO v_opp_sequnce FROM DUAL;

         INSERT INTO OPPORTUNITIESSOURCE (OPP_SOURCE_ID,
                                          URL,
                                          STATUS,
                                          FUNDINGBODY_ID,
                                          LANG,
                                          NAME,
                                          FREQUENCY,
                                          CAPTURESTART,
                                          CAPTUREEND,
                                          OPP_COMMENT)
           VALUES   (v_opp_sequnce,
                     p_url,
                     p_status,
                     v_id,
                     p_lang,
                     p_name,
                     p_Frequency,
                     truncate(p_date_captureStart, 0),
                    truncate(p_date_captureEnd, 0), 
                    p_Comment);         

SELECT 
    NULL OPP_SOURCE_ID,
    NULL URL,
    NULL STATUS,
    NULL LASTVISITED,
    NULL FUNDINGBODY_ID,
    NULL LANG
FROM DUAL;
         ELSEIF p_mode = 2
   THEN
      UPDATE   OPPORTUNITIESSOURCE
         SET   URL = p_url, STATUS = p_status, LANG=p_lang,
          NAME=p_name,
                                          FREQUENCY=p_Frequency,
                                          CAPTURESTART=p_date_captureStart,
                                          CAPTUREEND=p_date_captureEnd,
                                          OPP_COMMENT=p_Comment
       WHERE   OPP_SOURCE_ID = p_opp_source_id;      
	
SELECT 
    NULL OPP_SOURCE_ID,
    NULL URL,
    NULL STATUS,
    NULL LASTVISITED,
    NULL FUNDINGBODY_ID,
    NULL LANG
FROM DUAL;
   ELSEIF p_mode = 3
   THEN
   
         SELECT   OPP_SOURCE_ID,
                  URL,
                  STATUS,
                  LASTVISITED,
                  FUNDINGBODY_ID,
                  LANG,
                  NAME,
                  FREQUENCY,  
                  CAPTURESTART,
                  CAPTUREEND,
                  OPP_COMMENT
           FROM   OPPORTUNITIESSOURCE
          WHERE   FUNDINGBODY_ID = v_id;
      
    ELSEIF p_mode = 4
   THEN   
     delete from OPPORTUNITIESSOURCE where OPP_SOURCE_ID=p_opp_source_id;
     
   END IF;
END ;;
