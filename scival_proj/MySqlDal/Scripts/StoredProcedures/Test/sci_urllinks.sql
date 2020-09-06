CREATE PROCEDURE `sci_urllinks`(
	p_workflowid       integer,
	p_pagename         varchar(4000)
)
BEGIN
   DECLARE v_moduleid   integer;
   DECLARE v_id         integer;
  
   SELECT   moduleid, id
     INTO   v_moduleid, v_id
     FROM   sci_workflow
    WHERE   workflowid = p_workflowid;

      SELECT   URL
        FROM   sci_urls
       WHERE       moduleid = v_moduleid
               AND id = v_id
               AND PAGENAME = p_pagename
               AND ISACTIVE = '0';
END;
