CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_relatedorgsinsert`(
   p_workflowid       integer,
   p_insdel           integer,              
   p_HIERARCHY        varchar(4000),
   p_orgdbid          varchar(4000),
   p_RELTYPE          varchar(4000)
)
BEGIN
	DECLARE v_count           integer;
	DECLARE v_fundingbodyid   integer;
	DECLARE v_value           integer;
	DECLARE v_counter1        integer  DEFAULT  0;
	DECLARE v_counter         integer  DEFAULT  0;
	DECLARE v_orgcunt         integer;
	DECLARE v_ORG_TEXT        varchar (2000);
	DECLARE NOT_FOUND INT DEFAULT 0;

	DECLARE CONTINUE HANDLER FOR NOT FOUND SET NOT_FOUND = 1; 
   
	SELECT ID INTO v_fundingbodyid FROM sci_workflow
    WHERE WORKFLOWID = p_workflowid;

	IF p_insdel = 0
	THEN
		SELECT   COUNT(*) INTO   v_count FROM   relatedorgs
		WHERE   FUNDINGBODY_ID = v_fundingbodyid;

		IF v_count > 0
		THEN
			SELECT   RELATEDORGS_ID INTO   v_value FROM   relatedorgs
			WHERE   FUNDINGBODY_ID = v_fundingbodyid;
		ELSE
			SELECT   RELATEDORGS_SEQ.NEXTVAL INTO v_value FROM DUAL;

			INSERT INTO relatedorgs (HIERARCHY, RELATEDORGS_ID, FUNDINGBODY_ID)
			VALUES   (p_HIERARCHY, v_value, v_fundingbodyid);
		END IF;

		/*CLARE i CURSOR FOR SELECT list FROM table (sci_getcsvtoLIST (p_orgdbid));
		OPEN i;
		FETCH i INTO
		WHILE NOT_FOUND=0
		DO
		begin declare exit handler for not found begin
            SET v_ORG_TEXT=null;
		END;
        
		SELECT   FUNDINGBODYNAME
           INTO   v_ORG_TEXT
           FROM   fundingbody_master
		WHERE   FUNDINGBODY_ID = (SELECT FUNDINGBODY_ID
							  FROM   fundingbody
                              WHERE   ORGDBID = i.list);
	    END;                           
          
	if  v_ORG_TEXT is null then
          
           SELECT  vendor_fundingbody_name
               into v_ORG_TEXT
              FROM   sci_related_orgs_vendor
                    where VENDOR_ID= i.list;  
                    end if;   
                            
			INSERT INTO org (ORGDBID,
                          RELTYPE,
                          ORG_TEXT,
                          RELATEDORGS_ID)
           VALUES   (i.list,
                     p_RELTYPE,
                     v_ORG_TEXT,
                     v_value);
      FETCH  INTO;
      END WHILE;
      CLOSE ;
   ELSEIF p_insdel = 1
   THEN
      DECLARE i CURSOR FOR SELECT   list FROM table (sci_getcsvtoLIST (p_orgdbid));
      OPEN i;
      FETCH i INTO;
      WHILE NOT_FOUND=0
      DO
         DELETE FROM   org
               WHERE   ORGDBID = i.list and RELATEDORGS_ID in(select RELATEDORGS_ID from relatedorgs where  FUNDINGBODY_ID= v_fundingbodyid );

         SELECT   COUNT(*)
           INTO   v_orgcunt
           FROM   org
          WHERE   RELATEDORGS_ID =
                     (SELECT   RELATEDORGS_ID
                        FROM   relatedorgs
                       WHERE   FUNDINGBODY_ID = v_fundingbodyid);


         IF v_orgcunt = 0
         THEN
            DELETE FROM   relatedorgs
                  WHERE   FUNDINGBODY_ID = v_fundingbodyid;
         END IF;
      FETCH  INTO;
      END WHILE;
      CLOSE ;*/
   ELSEIF p_insdel = 2
   THEN
      UPDATE   relatedorgs
         SET   HIERARCHY = p_HIERARCHY
       WHERE   FUNDINGBODY_ID = v_fundingbodyid;
      
   END IF;
      
         SELECT   fb.ORGDBID, FUNDINGBODYNAME
           FROM   fundingbody fb, fundingbody_master fm
          WHERE       fb.FUNDINGBODY_ID = fm.FUNDINGBODY_ID
                  AND IFNULL (STATUSCODE, 1) <> 3
	
                  AND NOT EXISTS
                        (SELECT   *
                           FROM   org
                          WHERE   RELATEDORGS_ID IN
                                        (SELECT   RELATEDORGS_ID
                                           FROM   relatedorgs
                                          WHERE   FUNDINGBODY_ID =
                                                     v_fundingbodyid)
                                  AND ORGDBID = fb.FUNDINGBODY_ID);

   SET NOT_FOUND = 0;
   
      SELECT   FUNDINGBODYNAME,
               rd.HIERARCHY,
               rd.RELATEDORGS_ID,
               rd.FUNDINGBODY_ID,
               o.ORGDBID,
               o.RELTYPE,
   
               (CASE WHEN fm.STATUSCODE = 3 THEN 1 ELSE 0 END) flag
        FROM   relatedorgs rd,
               org o,
               fundingbody fb,
               fundingbody_master fm
       WHERE       rd.RELATEDORGS_ID = o.RELATEDORGS_ID
               AND O.ORGDBID = fb.ORGDBID
               AND FB.FUNDINGBODY_ID = fm.FUNDINGBODY_ID
               AND rd.FUNDINGBODY_ID = v_fundingbodyid
               UNION
SELECT   VENDOR_FUNDINGBODY_NAME FUNDINGBODYNAME,
         rd.HIERARCHY,
         rd.RELATEDORGS_ID,
         rd.FUNDINGBODY_ID,
         o.ORGDBID,
         o.RELTYPE,
   
         0 flag
  FROM   sci_related_orgs_vendor rov, relatedorgs rd, org o
 WHERE   rd.RELATEDORGS_ID = o.RELATEDORGS_ID AND rov.vendor_id = o.ORGDBID
          AND rd.FUNDINGBODY_ID = v_fundingbodyid ;

END ;;
