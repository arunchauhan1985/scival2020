CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_related_vendor_dml_prc`(
   p_rel_orgs_vendorid             INTEGER /* default null */,
   p_vendor_id                     INTEGER /* default null */,
   p_vendor_fundingbody_name       VARCHAR(4000) /* default null */,
   p_mode                          VARCHAR(4000)
)
BEGIN
   DECLARE v_count   INTEGER;
   DECLARE l_namecount   integer;
   DECLARE l_rel_orgs_vendorid  integer;
      
   IF p_mode = 'I'
   THEN
		SELECT   COUNT(*) INTO v_count FROM sci_related_orgs_vendor WHERE vendor_id=p_vendor_id;
      
        select ifnull(max(REL_ORGS_VENDORID),0)+1 into l_rel_orgs_vendorid  from sci_related_orgs_vendor;
        
        INSERT INTO sci_related_orgs_vendor (   rel_orgs_vendorid,
                                                 vendor_id,
                                                 vendor_fundingbody_name
                    )
        VALUES   (l_rel_orgs_vendorid,
           p_vendor_id, 
           p_vendor_fundingbody_name);
         
		SELECT 
			rel_orgs_vendorid, VENDOR_ID, VENDOR_FUNDINGBODY_NAME
		FROM
		sci_related_orgs_vendor;
               
   ELSEIF p_mode = 'U'
   THEN
		SELECT   COUNT(*)
        INTO   v_count
        FROM   sci_related_orgs_vendor
		WHERE   vendor_id = p_vendor_id
		and rel_orgs_vendorid <> p_rel_orgs_vendorid;
       
		SELECT 
		COUNT(*)
		INTO l_namecount FROM
		sci_related_orgs_vendor
		WHERE
		UPPER(vendor_fundingbody_name) = UPPER(p_vendor_fundingbody_name)
        AND rel_orgs_vendorid <> p_rel_orgs_vendorid;
       			
		IF v_count =0 and l_namecount=0
		THEN
			UPDATE   sci_related_orgs_vendor
            SET   vendor_fundingbody_name = p_vendor_fundingbody_name,
             vendor_id = p_vendor_id
              WHERE  rel_orgs_vendorid=p_rel_orgs_vendorid ;
            
			SELECT 
			rel_orgs_vendorid, vendor_id, vendor_fundingbody_name
			FROM
			sci_related_orgs_vendor;         
                         
			SELECT 
			rel_orgs_vendorid, vendor_id, vendor_fundingbody_name
			FROM
			sci_related_orgs_vendor;

		END IF;      
	ELSEIF p_mode = 'V'
	THEN
          
		SELECT   rel_orgs_vendorid,vendor_id, vendor_fundingbody_name
              FROM   sci_related_orgs_vendor
              where VENDOR_ID= (CASE
                               WHEN p_vendor_id  is null  THEN VENDOR_ID
                               ELSE p_vendor_id
                            END);
                            
	END IF;
END ;;
