CREATE PROCEDURE `sci_pageurls`(
	p_workflowid       integer,
  p_pagename         varchar(4000),
  p_url              varchar(4000),
  p_userid           integer,
  p_mode             integer -- 0 for insert ,1 for delete
)
BEGIN
   DECLARE v_moduleid   integer;
   DECLARE v_id         integer;
   DECLARE v_count      integer;
   
   SELECT   moduleid, id
     INTO   v_moduleid, v_id
     FROM   sci_workflow
    WHERE   workflowid = p_workflowid;

   IF p_mode = 0
   THEN
      INSERT INTO sci_urls (moduleid,
                            id,
                            PAGENAME,
                            URL,
                            USERID)
        VALUES   (v_moduleid,
                  v_id,
                  p_PAGENAME,
                  p_URL,
                  p_USERID);
   ELSEIF p_mode = 1
   THEN
      UPDATE   sci_urls
         SET   ISACTIVE = 1, DELETEDBY = p_userid
       WHERE       MODULEID = v_MODULEID
               AND ID = v_ID
               AND PAGENAME = p_PAGENAME
               AND URL = p_URL;
   END IF;

      SELECT   URL
        FROM   sci_urls
       WHERE   moduleid = v_moduleid AND id = v_id AND PAGENAME =p_pagename and ISACTIVE = '0';

   COMMIT;
  
      UPDATE   sci_urls
         SET   ISACTIVE = '0', USERID = p_userid,DELETEDBY=null
       WHERE       MODULEID = v_MODULEID
               AND ID = v_ID
               AND PAGENAME = p_PAGENAME
               AND URL = p_URL              
               AND ISACTIVE = '1';

      set v_count = ROW_COUNT;
       

      IF v_count > 0
      THEN
           SELECT   URL
              FROM   sci_urls
             WHERE   moduleid = v_moduleid AND id = v_id AND PAGENAME =p_pagename and ISACTIVE = '0';
      ELSE
      
            SELECT   URL
              FROM   sci_urls
             WHERE   moduleid = v_moduleid AND id = v_id AND PAGENAME =p_pagename and ISACTIVE = '0';
      END IF;
   END;
