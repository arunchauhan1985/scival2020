CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_fundingbase_41`(
   INOUT p_FUNDINGBODYID            integer,
   p_ORGDBID                         integer,
   p_TYPE                            varchar(4000),
   p_TRUSTING                        varchar(4000),
   p_COUNTRY                         varchar(4000),
   p_STATE                           varchar(4000),
   p_COLLECTIONCODE                  varchar(4000),
   p_HIDDEN                          varchar(4000),
   p_CANONICALNAME                   LONGTEXT,
   p_PREFERREDORGNAME                LONGTEXT,
   p_CONTEXTNAME                     LONGTEXT,
   p_ABBREVNAME                      LONGTEXT,
   p_ELIGIBILITYDESCRIPTION          LONGTEXT,
   p_IDsubtypeid                     LONGTEXT,
   p_SUBTYPE_TEXT                    LONGTEXT,
   p_workflowid                      integer,
   p_userid                          integer,
   p_projectname                     varchar(4000), 
   p_recordsource                    varchar(4000),  
   p_awardsuccesrate                 double,    
   p_comment_desc                    longtext,      
   p_defunct                        varchar(4000),  
   p_crossrefid                      varchar(4000),
   P_extendedRecord                varchar(4000),
   p_capop                         varchar(4000),
   p_oppsup                        varchar(4000),
   p_CapAwards                     varchar(4000),
   p_AwardsSup                      varchar(4000),
   p_tierinfo                      varchar(4000),
   p_profit                       varchar(4000),
   p_oppFrequency                 varchar(4000),
   p_awFrequency                  varchar(4000)
)
BEGIN
   
DECLARE v_REVISIONHISTORYID integer;
DECLARE x_MODULEID integer;DECLARE x_CYCLE integer;
DECLARE v_region varchar(2000);
DECLARE v_subregion varchar(2000);

   SELECT   ID,MODULEID,CYCLE
     INTO   p_FUNDINGBODYID,x_MODULEID,x_CYCLE
     FROM   sci_workflow
    WHERE   workflowid = p_workflowid;
  Select region into v_region from TEMPTABLE_SUBREGION where Upper(Alpha_3)= Upper(p_COUNTRY);
  Select subregion into v_subregion from TEMPTABLE_SUBREGION where Upper(Alpha_3)= Upper(p_COUNTRY);

   IF p_ORGDBID = 0
   THEN
      INSERT INTO fundingbody (FUNDINGBODY_ID,
                               ORGDBID,
                               TYPE,
                               TRUSTING,
                               COUNTRY,
                               STATE,
                               COLLECTIONCODE,
                               HIDDEN,
                               ELIGIBILITYDESCRIPTION,
                               RECORDSOURCE,
                               AWARDSUCCESRATE,
                               comment_desc, 
                              defunct,
                              crossrefid,
                              extendedRecord,
                               captureawards,
                               captureopportunities,
                               tierinfo,
                               awardssupplier,
                               opportunitiessupplier,
                              profit,
                              opportunitiesfrequency,
                              awardsfrequency)
        VALUES   (p_FUNDINGBODYID,
                  p_FUNDINGBODYID,
                  p_TYPE,
                  p_TRUSTING,
                  p_COUNTRY,
                  p_STATE,
                  p_COLLECTIONCODE,
                  p_HIDDEN,
                  p_ELIGIBILITYDESCRIPTION,
                  P_RECORDSOURCE,
                  P_AWARDSUCCESRATE,
                  p_comment_desc, 
                  p_defunct,
                  p_crossrefid,
                  P_extendedRecord,
                  p_CapAwards,
                  p_capop,
                  p_tierinfo,
                  p_AwardsSup,
                  p_oppsup,
                  p_profit,
                  p_oppFrequency,
                  p_awFrequency);

      INSERT INTO subtype (ID, SUBTYPE_TEXT, FUNDINGBODY_ID)
        VALUES   (p_IDsubtypeid, p_SUBTYPE_TEXT, p_FUNDINGBODYID);
        
      SELECT REVISIONHISTORYID_SEQ.NEXTVAL INTO V_REVISIONHISTORYID  FROM DUAL;
        
      INSERT INTO REVISIONHISTORY(STATUS,REVISIONHISTORY_ID,FUNDINGBODY_ID) VALUES('new',V_REVISIONHISTORYID,P_FUNDINGBODYID);
        
      INSERT INTO CREATEDDATE(VERSION,CREATEDDATE_TEXT,REVISIONHISTORY_ID)VALUES (0,SYSDATE(),V_REVISIONHISTORYID);
        
      INSERT INTO sci_userlog (workflowid,
                              moduleid,
                              id,
                              cycleid,
                              pagename,
                              userid,
                              action)
     VALUES   (p_workflowid,
               x_moduleid,
               p_FUNDINGBODYID,
               X_cycle,
               p_projectname,
               p_userid,
               'INSERT');
              
                 ELSE
      UPDATE   fundingbody
         SET   TYPE = p_TYPE,
               TRUSTING = p_TRUSTING,
               COUNTRY = p_COUNTRY,
               STATE = p_STATE,
               COLLECTIONCODE = p_COLLECTIONCODE,
               HIDDEN = p_HIDDEN,
               ELIGIBILITYDESCRIPTION = p_ELIGIBILITYDESCRIPTION,
               RECORDSOURCE = P_RECORDSOURCE,
               AWARDSUCCESRATE = P_AWARDSUCCESRATE,
               comment_desc = p_comment_desc, 
               defunct= p_defunct,    
               crossrefid=p_crossrefid, 
               extendedRecord =P_extendedRecord,  
               captureawards =p_CapAwards, 
               captureopportunities= p_capop, 
               tierinfo=p_tierinfo, 
               awardssupplier=p_AwardsSup, 
                opportunitiessupplier= p_oppsup,    
                profit=p_profit, 
                opportunitiesfrequency=p_oppfrequency, 
                awardsfrequency=p_awfrequency 
       WHERE   FUNDINGBODY_ID = p_FUNDINGBODYID AND ORGDBID = p_ORGDBID;
       
          UPDATE   subtype
         SET   ID = p_IDsubtypeid, SUBTYPE_TEXT = p_SUBTYPE_TEXT
       WHERE   FUNDINGBODY_ID = p_FUNDINGBODYID;
       
      UPDATE   award
         SET   trusting = p_TRUSTING,recordsource = p_recordsource
       WHERE   fundingbody_id = p_FUNDINGBODYID;
    
      UPDATE   opportunity
         SET   trusting = p_TRUSTING, recordsource = p_recordsource
       WHERE   fundingbody_id = p_FUNDINGBODYID;  
       
       INSERT INTO sci_userlog (workflowid,
                            moduleid,
                            id,
                            cycleid,
                            pagename,
                            userid,
                            action)
     VALUES   (p_workflowid,
               x_moduleid,
               p_FUNDINGBODYID,
               X_cycle,
               p_projectname,
               p_userid,
               'UPDATE');
   END IF;

 UPDATE   FUNDINGBODY_MASTER
    SET   FUNDINGBODYNAME = P_PREFERREDORGNAME
  WHERE   FUNDINGBODY_ID =P_FUNDINGBODYID;
 
  UPDATE   ORG
     SET   ORG_TEXT = P_PREFERREDORGNAME
   WHERE   ORGDBID = P_ORGDBID;
 
 UPDATE   FBPROGRESS
    SET   FUNDINGBODYNAME = P_PREFERREDORGNAME
  WHERE   FUNDINGBODY_ID = P_FUNDINGBODYID;
  
 COMMIT;
  /*IF v_region is not null and v_subregion is not null
               THEN
               sci_iteminserttemp41(p_workflowid,7,0,0,'region',v_region,'','','en',p_mdata,p_o_status,p_o_error);
               sci_iteminserttemp41(p_workflowid,11,0,0,'subRegion',v_subregion,'','','en',p_mdata,p_o_status,p_o_error);
               END IF;*/
   
      SELECT   fd.FUNDINGBODY_ID,
               ORGDBID,
               TYPE,
               TRUSTING,
               COUNTRY,
               STATE,
               COLLECTIONCODE,
               HIDDEN,               
               ELIGIBILITYDESCRIPTION,
               ID subtypeid,
               SUBTYPE_TEXT,
               recordsource, 
               awardsuccesrate,
               comment_desc, 
               defunct,
               CROSSREFID, 
               extendedRecord,
               captureawards,
               captureopportunities,
                               tierinfo,
                               awardssupplier,
                               opportunitiessupplier,
                               profit,
                               opportunitiesfrequency,
                               awardsfrequency
        FROM   fundingbody fd RIGHT JOIN subtype su
        ON fd.FUNDINGBODY_ID = su.FUNDINGBODY_ID
       WHERE  fd.FUNDINGBODY_ID = p_FUNDINGBODYID;
   
END ;;
