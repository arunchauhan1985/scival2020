CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_establishmentinsert`(
   p_workflowid                     integer,
   p_ESTABLISHMENTDATE              VARCHAR(4000),
   p_ESTABLISHMENTCITY              VARCHAR(4000),
   p_ESTABLISHMENTSTATE             VARCHAR(4000),
   p_ESTABLISHMENTCOUNTRYCODE       VARCHAR(4000),
   p_ESTABLISHMENTDESCRIPTION       VARCHAR(4000),
   p_lang                            VARCHAR(4000)
)
BEGIN
   DECLARE v_fundingbodyid   integer;
   DECLARE v_count           integer;
    
   SELECT   ID
     INTO   v_fundingbodyid
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;

   SELECT   COUNT(*)
     INTO   v_count
     FROM   establishmentinfo
    WHERE   FUNDINGBODY_ID = v_fundingbodyid;

   IF v_count = 0
   THEN
      INSERT INTO establishmentinfo (ESTABLISHMENTDATE,
                                     ESTABLISHMENTCITY,
                                     ESTABLISHMENTSTATE,
                                     ESTABLISHMENTCOUNTRYCODE,
                                     ESTABLISHMENTDESCRIPTION,
                                     lang,
                                     FUNDINGBODY_ID)
        VALUES   (p_ESTABLISHMENTDATE,
                  p_ESTABLISHMENTCITY,
                  p_ESTABLISHMENTSTATE,
                  p_ESTABLISHMENTCOUNTRYCODE,
                  p_ESTABLISHMENTDESCRIPTION,
                  p_lang,
                  v_fundingbodyid);
   ELSE
      UPDATE   establishmentinfo
         SET   ESTABLISHMENTDATE = p_ESTABLISHMENTDATE,
               ESTABLISHMENTCITY = p_ESTABLISHMENTCITY,
               ESTABLISHMENTSTATE = p_ESTABLISHMENTSTATE,
               ESTABLISHMENTCOUNTRYCODE = p_ESTABLISHMENTCOUNTRYCODE,
               ESTABLISHMENTDESCRIPTION = p_ESTABLISHMENTDESCRIPTION,
               lang=p_lang
              where  FUNDINGBODY_ID = v_fundingbodyid;
   END IF;

      SELECT   ESTABLISHMENTDATE,ESTABLISHMENTCITY,ESTABLISHMENTSTATE,ESTABLISHMENTCOUNTRYCODE
      ,ESTABLISHMENTDESCRIPTION,FUNDINGBODY_ID,(CASE
                   WHEN lang IS NULL THEN 'en'
                   ELSE lang
                END) Lang
        FROM   establishmentinfo
       WHERE   FUNDINGBODY_ID = v_fundingbodyid;
   
END ;;
