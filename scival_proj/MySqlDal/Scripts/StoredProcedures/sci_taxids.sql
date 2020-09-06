CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_taxids`(p_workflowid            integer,
                             p_insdel                integer, 
                             p_type                  varchar(4000),
                             p_taxids_text           varchar(4000),
                             p_userid                integer,
                             p_FINANCIALINFO_ID       integer)
BEGIN
   DECLARE v_fundingbody_id     integer;
   DECLARE v_financialinfo_id   integer;
   DECLARE v_count              integer;
   DECLARE v_count1             integer;
   DECLARE v_count2             integer;
  
   SELECT   ID
     INTO   v_fundingbody_id
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;
   
   IF p_insdel = 0
   THEN
      SELECT   COUNT(*)
        INTO   v_count
        FROM   financialinfo
       WHERE   fundingbody_id = v_fundingbody_id;
             
      IF (v_count = 0)
      THEN
         SELECT   financial_seq.NEXTVAL INTO v_financialinfo_id FROM DUAL;
         
         insert into financialinfo values(v_financialinfo_id,v_fundingbody_id);

         INSERT INTO taxids (TYPE, taxids_text, financialinfo_id)
           VALUES   (p_type, p_taxids_text, v_financialinfo_id);
        
      ELSE        
      
         SELECT   FINANCIALINFO_ID
           INTO   v_FINANCIALINFO_ID
           FROM   financialinfo
          WHERE   fundingbody_id = v_fundingbody_id;
          
          select count(*) into v_count  from taxids where financialinfo_id=v_financialinfo_id and type=p_type;
          if(v_count=0)
           then
         INSERT INTO taxids (TYPE, taxids_text, financialinfo_id)
           VALUES   (p_type, p_taxids_text, v_financialinfo_id);
                    
         end if;
         
      END IF;
   
   ELSEIF p_insdel = 2
   THEN
      SELECT   COUNT(*)
        INTO   v_count1
        FROM   taxids
       WHERE   FINANCIALINFO_ID = p_FINANCIALINFO_ID;
       
       SET v_financialinfo_id=p_FINANCIALINFO_ID;

      IF (v_count1 >0)
      THEN
         UPDATE   taxids
            SET   TYPE = p_type, taxids_text = p_taxids_text
          WHERE  FINANCIALINFO_ID = p_FINANCIALINFO_ID and TYPE = p_type;
         
      END IF;
  
   ELSEIF p_insdel = 1
   THEN
      SELECT   COUNT(*)
        INTO   v_count2
        FROM   taxids
       WHERE   FINANCIALINFO_ID = p_FINANCIALINFO_ID;
       
        SET v_financialinfo_id=p_FINANCIALINFO_ID;

      IF (v_count2>0)
      THEN
         DELETE FROM   taxids
               WHERE   FINANCIALINFO_ID = p_FINANCIALINFO_ID and TYPE = p_type;
         
      END IF;
   END IF;

      SELECT   *
        FROM   taxids
       WHERE   financialinfo_id = v_financialinfo_id;
END ;;
