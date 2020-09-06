CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_aw_managerlist`(
p_workflowid       integer
)
BEGIN
   DECLARE v_id         integer;
   DECLARE v_moduleid   integer;

   SELECT   ID, moduleid
     INTO   v_id, v_moduleid
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;

   IF v_moduleid = 4
   THEN

      SELECT   b.TYPE,
               b.TITLE,
               b.TELEPHONE,
               b.FAX,
               b.EMAIL,
               b.AWARDMANAGER_ID contact_id,
               c.country countrycode,
                     cc.name  countryname,
               c.ROOM,
               c.STREET,
               c.CITY,
                ifnull(c.state,c.state) statecode,
                     ifnull(sc.name,c.state) statename,
               c.POSTALCODE,
               d.PREFIX,
               d.GIVENNAME,
               d.MIDDLENAME,
               d.SURNAME,
               d.SUFFIX,
               e.URL,
               e.WEBSITE_TEXT
        FROM   Awardmanagers a,
               Awardmanager b,
               address c,
               AWARDCONTACTNAME d,
               AWARDWEBSITE e,
               SCI_COUNTRYCODES cc,
                  SCI_STATECODES sc
       WHERE       a.AWARDMANAGERS_ID = b.AWARDMANAGERS_ID
        and sc.code=c.STATE
          and cc.LCODE=c.COUNTRY
               AND b.AWARDMANAGER_ID = c.AWARDMANAGER_ID
               AND b.AWARDMANAGER_ID = d.AWARDMANAGER_ID
               AND b.AWARDMANAGER_ID = e.AWARDMANAGER_ID
               AND AWARD_ID = v_id;
   END IF;

      
END