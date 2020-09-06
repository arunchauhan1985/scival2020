CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_op_relatedprogeamins`(
   p_workflowid               integer,
   p_insdel                   integer,
   p_id                    integer,
   p_HIERARCHY                varchar(4000),
   p_RELATEDPROGRAMTEXT       varchar(4000),
   p_RELTYPE                  varchar(4000),
   x_id                    integer,
   x_RELTYPE                varchar(4000),
   x_RELATEDPROGRAMTEXT      varchar(4000)
)
BEGIN
   DECLARE v_id         INTEGER;
   DECLARE v_MODULEID   INTEGER;
   DECLARE v_value      integer;
   DECLARE v_count  integer;
   DECLARE d_count   integer;
 
   SELECT   ID, MODULEID
     INTO   v_id, v_MODULEID
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;

if v_moduleid=3
then

   IF p_insdel = 0
   THEN
      SELECT   COUNT(*)
        INTO   v_count
        FROM   relatedprograms
       WHERE   OPPORTUNITY_ID = v_id AND HIERARCHY = p_HIERARCHY;


      IF v_count > 0
      THEN
         SELECT   RELATEDPROGRAMS_ID
           INTO   v_value
           FROM   relatedprograms
          WHERE   OPPORTUNITY_ID = v_id AND HIERARCHY = p_HIERARCHY;
      ELSE
         SELECT   RELATEDPROGRAMSID_SEQ.NEXTVAL INTO v_value FROM DUAL;

         INSERT INTO relatedprograms (
                                         HIERARCHY,
                                         RELATEDPROGRAMS_ID,
                                         OPPORTUNITY_ID
                    )
           VALUES   (p_HIERARCHY, v_value, v_id);
      END IF;

      INSERT INTO relatedprogram (ID,
                                  RELTYPE,
                                  RELATEDPROGRAM_TEXT,
                                  RELATEDPROGRAMS_ID)
        VALUES   (p_id,
                  p_RELTYPE,
                  p_RELATEDPROGRAMTEXT,
                  v_value);

   ELSEIF p_insdel = 1
   THEN
      SELECT   RELATEDPROGRAMS_ID
        INTO   v_value
        FROM   relatedprograms
       WHERE   OPPORTUNITY_ID = v_id AND HIERARCHY = p_HIERARCHY;

      DELETE FROM   relatedprogram
            WHERE       RELATEDPROGRAMS_ID = v_value
                    AND ID = p_id
                    AND RELTYPE = p_RELTYPE
                    AND RELATEDPROGRAM_TEXT = p_RELATEDPROGRAMTEXT;
                    
                    
        select count(*) into d_count from  relatedprogram where  RELATEDPROGRAMS_ID= v_value; 
        
        if d_count=0
        then
        delete from  relatedprograms where  RELATEDPROGRAMS_ID= v_value; 
        end if;       
   ELSEIF p_insdel = 2
   THEN
      SELECT   RELATEDPROGRAMS_ID
        INTO   v_value
        FROM   relatedprograms
       WHERE   OPPORTUNITY_ID = v_id AND HIERARCHY = p_HIERARCHY;

      UPDATE   relatedprogram
         SET   ID = p_id,
               RELTYPE = p_RELTYPE,
               RELATEDPROGRAM_TEXT = p_RELATEDPROGRAMTEXT
       WHERE       RELATEDPROGRAMS_ID = v_value
               AND ID = x_id
               AND RELTYPE = x_RELTYPE
               AND RELATEDPROGRAM_TEXT = x_RELATEDPROGRAMTEXT;
   END IF;

--   OPEN mlist FOR
      SELECT   *
        FROM   relatedprograms
       WHERE   OPPORTUNITY_ID = v_id;

  -- OPEN mdata FOR
      SELECT   rs.HIERARCHY,
               rs.RELATEDPROGRAMS_ID,
               rm.id,
               rm.RELTYPE,
               rm.RELATEDPROGRAM_TEXT
        FROM   relatedprograms rs, relatedprogram rm
       WHERE   rs.RELATEDPROGRAMS_ID = rm.RELATEDPROGRAMS_ID
               AND OPPORTUNITY_ID = v_id;

elseif  v_moduleid=4
then
IF p_insdel = 0
   THEN
      SELECT   COUNT(*)
        INTO   v_count
        FROM   relatedprograms
       WHERE   AWARD_ID = v_id AND HIERARCHY = p_HIERARCHY;


      IF v_count > 0
      THEN
         SELECT   RELATEDPROGRAMS_ID
           INTO   v_value
           FROM   relatedprograms
          WHERE   AWARD_ID = v_id AND HIERARCHY = p_HIERARCHY;
      ELSE
         SELECT   RELATEDPROGRAMSID_SEQ.NEXTVAL INTO v_value FROM DUAL;

         INSERT INTO relatedprograms (
                                         HIERARCHY,
                                         RELATEDPROGRAMS_ID,
                                         AWARD_ID
                    )
           VALUES   (p_HIERARCHY, v_value, v_id);
      END IF;

      INSERT INTO relatedprogram (ID,
                                  RELTYPE,
                                  RELATEDPROGRAM_TEXT,
                                  RELATEDPROGRAMS_ID)
        VALUES   (p_id,
                  p_RELTYPE,
                  p_RELATEDPROGRAMTEXT,
                  v_value);

   ELSEIF p_insdel = 1
   THEN
      SELECT   RELATEDPROGRAMS_ID
        INTO   v_value
        FROM   relatedprograms
       WHERE   AWARD_ID = v_id AND HIERARCHY = p_HIERARCHY;

      DELETE FROM   relatedprogram
            WHERE       RELATEDPROGRAMS_ID = v_value
                    AND ID = p_id
                    AND RELTYPE = p_RELTYPE
                    AND RELATEDPROGRAM_TEXT = p_RELATEDPROGRAMTEXT;
                    
                    
        select count(*) into d_count from  relatedprogram where  RELATEDPROGRAMS_ID= v_value; 
        
        if d_count=0
        then
        delete from  relatedprograms where  RELATEDPROGRAMS_ID= v_value; 
        end if;       
   ELSEIF p_insdel = 2
   THEN
      SELECT   RELATEDPROGRAMS_ID
        INTO   v_value
        FROM   relatedprograms
       WHERE   AWARD_ID = v_id AND HIERARCHY = p_HIERARCHY;

      UPDATE   relatedprogram
         SET   ID = p_id,
               RELTYPE = p_RELTYPE,
               RELATEDPROGRAM_TEXT = p_RELATEDPROGRAMTEXT
       WHERE       RELATEDPROGRAMS_ID = v_value
               AND ID = x_id
               AND RELTYPE = x_RELTYPE
               AND RELATEDPROGRAM_TEXT = x_RELATEDPROGRAMTEXT;
   END IF;

--   OPEN mlist FOR
      SELECT   *
        FROM   relatedprograms
       WHERE   AWARD_ID = v_id;

  -- OPEN mdata FOR
      SELECT   rs.HIERARCHY,
               rs.RELATEDPROGRAMS_ID,
               rm.id,
               rm.RELTYPE,
               rm.RELATEDPROGRAM_TEXT
        FROM   relatedprograms rs, relatedprogram rm
       WHERE   rs.RELATEDPROGRAMS_ID = rm.RELATEDPROGRAMS_ID
               AND AWARD_ID = v_id;
        
   end if;            
END