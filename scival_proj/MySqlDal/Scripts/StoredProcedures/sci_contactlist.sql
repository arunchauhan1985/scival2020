CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_contactlist`(p_workflowid       integer,
		 p_mode             integer
	 )
BEGIN
   DECLARE v_fundingbodyid   integer;
   DECLARE v_moduleid integer;
  
   SELECT   ID,moduleid
     INTO   v_fundingbodyid,v_moduleid
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;

if v_moduleid=2
then

   IF p_mode = 1
   THEN
      
         SELECT   c.TYPE,
                  c.TITLE,
                  c.TELEPHONE,
                  c.email,
                  c.FAX,
                  c.CONTACT_ID,
                  w.URL,
                  w.WEBSITE_TEXT,
                  Case  when w.LANG is null then 'en' 
                  else w.Lang End LANG,
                  a.countryTEST countrycode,
				  cc.name  countryname,
                  a.ROOM,
                  a.STREET,
                  a.CITY,
                  ifnull(a.state,a.state) statecode,
                     ifnull(sc.name,a.state) statename,
                  a.POSTALCODE,
                  cn.PREFIX,
                  cn.GIVENNAME,
                  cn.MIDDLENAME,
                  cn.SURNAME,
                  cn.SUFFIX
           FROM contact c LEFT JOIN  website w
			ON c.CONTACT_ID = w.CONTACT_ID
			LEFT JOIN address a
			ON c.CONTACT_ID = a.CONTACT_ID
			LEFT JOIN contactname cn
			ON c.CONTACT_ID = cn.CONTACT_ID
			RIGHT JOIN SCI_STATECODES sc 
			ON sc.code = a.STATE
			RIGHT JOIN SCI_COUNTRYCODES cc
			ON cc.LCODE=a.COUNTRYTEST,
			contacts cs
			WHERE c.CONTACTS_ID = cs.CONTACTS_ID
			AND cs.FUNDINGBODY_ID = v_fundingbodyid;
				  
   ELSEIF p_mode = 2
   THEN
      
         SELECT   c.TYPE,
                  c.TITLE,
                  c.TELEPHONE,
                  c.email,
                  c.FAX,
                  c.CONTACT_ID,
                  w.URL,
                  w.WEBSITE_TEXT,
                  Case  when w.LANG is null then 'en' 
                  else w.Lang End LANG,
                  a.countryTEST countrycode,
                     cc.name  countryname,
                  a.ROOM,
                  a.STREET,
                  a.CITY,
                  ifnull(a.state,a.state) statecode,
                     ifnull(sc.name,a.state) statename,
                  a.POSTALCODE,
                  cn.PREFIX,
                  cn.GIVENNAME,
                  cn.MIDDLENAME,
                  cn.SURNAME,
                  cn.SUFFIX
           FROM   contact c 
           LEFT JOIN website w 
           ON c.CONTACT_ID=w.CONTACT_ID
           LEFT JOIN address a
           ON c.CONTACT_ID = a.CONTACT_ID
           LEFT JOIN contactname cn
           ON c.CONTACT_ID = cn.CONTACT_ID
           LEFT JOIN officers cs
           ON c.OFFICERS_ID = cs.OFFICERS_ID
           RIGHT JOIN SCI_COUNTRYCODES cc
           ON cc.LCODE=a.COUNTRYTEST
           RIGHT JOIN SCI_STATECODES sc
           ON sc.code=a.STATE
          WHERE cs.FUNDINGBODY_ID = v_fundingbodyid;
          
   ELSEIF P_mode = 3
   THEN
      
         SELECT   c.TYPE,
                  c.TITLE,
                  c.TELEPHONE,
                  c.email,
                  c.FAX,
                  c.CONTACT_ID,
                  w.URL,
                  w.WEBSITE_TEXT,
                  Case  when w.LANG is null then 'en' 
                  else w.Lang End LANG,
                  a.countryTEST countrycode,
                     cc.name  countryname,
                  a.ROOM,
                  a.STREET,
                  a.CITY,
                   ifnull(a.state,a.state) statecode,
                     ifnull(sc.name,a.state) statename,
                  a.POSTALCODE,
                  cn.PREFIX,
                  cn.GIVENNAME,
                  cn.MIDDLENAME,
                  cn.SURNAME,
                  cn.SUFFIX
           FROM   contact c 
           LEFT JOIN website w 
           ON c.CONTACT_ID=w.CONTACT_ID
		   LEFT JOIN address a
           ON c.CONTACT_ID = a.CONTACT_ID
           LEFT JOIN contactname cn
           ON c.CONTACT_ID = cn.CONTACT_ID
           RIGHT JOIN SCI_STATECODES sc
           ON sc.code=a.STATE
           RIGHT JOIN SCI_COUNTRYCODES cc
           ON cc.LCODE=a.COUNTRYTEST,
		   contactinfo cs                  
		   WHERE c.CONTACTINFO_ID = cs.CONTACTINFO_ID
	       AND cs.FUNDINGBODY_ID = v_fundingbodyid;
           
   END IF;
   
elseif v_moduleid=3
then  

  IF P_mode = 3
   THEN
      
         SELECT   c.TYPE,
                  c.TITLE,
                  c.TELEPHONE,
                  c.email,
                  c.FAX,
                  c.CONTACT_ID,
                  w.URL,
                  w.WEBSITE_TEXT,
                  Case  when w.LANG is null then 'en' 
                  else w.Lang End LANG,
                 a.countryTEST countrycode,
                     cc.name  countryname,
                  a.ROOM,
                  a.STREET,
                  a.CITY,
                  ifnull(a.state,a.state) statecode,
                     ifnull(sc.name,a.state) statename,
                  a.POSTALCODE,
                  cn.PREFIX,
                  cn.GIVENNAME,
                  cn.MIDDLENAME,
                  cn.SURNAME,
                  cn.SUFFIX
           FROM   contact c 
           LEFT JOIN website w 
           ON c.CONTACT_ID=w.CONTACT_ID
		   LEFT JOIN address a
           ON c.CONTACT_ID = a.CONTACT_ID
           LEFT JOIN contactname cn
           ON c.CONTACT_ID = cn.CONTACT_ID       
           RIGHT JOIN SCI_COUNTRYCODES cc
           ON cc.LCODE=a.COUNTRYTEST
           RIGHT JOIN SCI_STATECODES sc
           ON sc.code=a.STATE,
           contactinfo cs				
           WHERE c.CONTACTINFO_ID = cs.CONTACTINFO_ID
		   AND cs.OPPORTUNITY_ID = v_fundingbodyid;
           
   END IF;

end if;
  
END ;;
