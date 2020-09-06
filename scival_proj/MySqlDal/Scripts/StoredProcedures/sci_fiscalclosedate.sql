CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_fiscalclosedate`(
   p_workflowid             integer,
   p_insdel                 integer,   -- 0 * insert ,1 * delete ,2 * update
   p_userid                 integer,
   p_FINANCIALINFO_ID       integer,
   p_fiscalclosedate_column  datetime
)
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

         INSERT INTO fiscalclosedate (
                                         fiscalclosedate_column,
                                         financialinfo_id
                    )
           VALUES   (p_fiscalclosedate_column, v_financialinfo_id);
         
      ELSE
         SELECT   FINANCIALINFO_ID
           INTO   v_FINANCIALINFO_ID
           FROM   financialinfo
          WHERE   fundingbody_id = v_fundingbody_id;
          
           select count(*) into v_count  from fiscalclosedate where financialinfo_id=v_financialinfo_id 
           and FISCALCLOSEDATE_COLUMN=p_FISCALCLOSEDATE_COLUMN;
          if(v_count=0)
           then

         INSERT INTO fiscalclosedate (
                                         fiscalclosedate_column,
                                         financialinfo_id
                    )
           VALUES   (p_fiscalclosedate_column, v_financialinfo_id);				
         
         end if;
      END IF;
   --
   ELSEIF p_insdel = 2
   THEN
      SELECT   COUNT(*)
        INTO   v_count1
        FROM   fiscalclosedate
       WHERE   financialinfo_id = p_financialinfo_id;
        SET v_financialinfo_id=p_financialinfo_id;
      IF (v_count1 > 0)
      THEN
         UPDATE   fiscalclosedate
            SET   fiscalclosedate_column = p_fiscalclosedate_column
          WHERE   financialinfo_id = p_financialinfo_id ;
         
      END IF;
  
   ELSEIF p_insdel = 1
   THEN
      SELECT   COUNT(*)
        INTO   v_count2
        FROM   fiscalclosedate
       WHERE   financialinfo_id = p_financialinfo_id;
       
        SET v_financialinfo_id=p_financialinfo_id;

      IF (v_count2 > 0)
      THEN
         DELETE FROM   fiscalclosedate
               WHERE   financialinfo_id = p_financialinfo_id and FISCALCLOSEDATE_COLUMN=p_fiscalclosedate_column;
		
      END IF;
   END IF;

       SELECT  FISCALCLOSEDATE_COLUMN,FINANCIALINFO_ID
        FROM fiscalclosedate
       WHERE   financialinfo_id = v_financialinfo_id;
END ;;
