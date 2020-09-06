CREATE PROCEDURE `SCI_TrackUnstoppedFbAwardOpp`(
	P_WORKFLOWID       INTEGER,
	P_USERID           INTEGER,
	P_PAGENAME         VARCHAR(4000)
)
sp_lbl:

BEGIN
   DECLARE V_MODULEID        INTEGER;
   DECLARE V_SEQ             INTEGER;

   DECLARE V_TASKID          INTEGER;
   DECLARE V_SEQUENCE        INTEGER;
   DECLARE V_STARTDATE       DATETIME;
   DECLARE V_STARTBY         INTEGER;
   DECLARE V_COMPLETEDDATE   DATETIME;
   DECLARE V_COMPLETEDBY     INTEGER;
   DECLARE V_STATUSID        INTEGER;
   DECLARE V_ID              INTEGER;
   DECLARE V_COUNT           INTEGER;
   DECLARE V_STATUS          VARCHAR (50);
 
   IF P_WORKFLOWID IS NULL OR P_USERID IS NULL OR P_PAGENAME IS NULL
   THEN
      LEAVE sp_lbl;
   ELSE
      SELECT   COUNT( * )
        INTO   V_COUNT
        FROM   SCI_WORKFLOW
       WHERE   WORKFLOWID = P_WORKFLOWID;

      IF V_COUNT < 1
      THEN
         LEAVE sp_lbl;
      END IF;
   END IF;

   SELECT   MODULEID,
            ID,
            TASKID,
            SEQUENCE,
            STARTDATE,
            STARTBY,
            COMPLETEDDATE,
            COMPLETEDBY,
            STATUSID
     INTO   V_MODULEID,
            V_ID,
            V_TASKID,
            V_SEQUENCE,
            V_STARTDATE,
            V_STARTBY,
            V_COMPLETEDDATE,
            V_COMPLETEDBY,
            V_STATUSID
     FROM   SCI_WORKFLOW
    WHERE   WORKFLOWID = P_WORKFLOWID;


   SET V_SEQ = TRACKINGLOG_SEQ.NEXTVAL;


   INSERT INTO TRACKING_LOG (TLOGID,
                             MODULEID,
                             TASKID,
                             SEQUENCE,
                             STARTDATE,
                             STARTBY,
                             COMPLETEDDATE,
                             COMPLETEDBY,
                             STATUSID)
     VALUES   (V_SEQ,
               V_MODULEID,
               V_TASKID,
               V_SEQUENCE,
               V_STARTDATE,
               V_STARTBY,
               V_COMPLETEDDATE,
               V_COMPLETEDBY,
               V_STATUSID);


   IF V_MODULEID = 2
   THEN
      SELECT   (SELECT   STATUSCODE
                  FROM   FUNDINGBODY_MASTER
                 WHERE   FUNDINGBODY_ID = V_ID)
        INTO   V_STATUS
        FROM   DUAL;

      IF V_STATUS IS NULL
      THEN
         LEAVE sp_lbl;
      END IF;

      IF UPPER (V_STATUS) = '2'
      THEN
         INSERT INTO TRACKING_FB (TID,
                                  WORKFLOWID,
                                  USERID,
                                  PAGENAME,
                                  FUNDINGBODYID)
           VALUES   (V_SEQ,
                     P_WORKFLOWID,
                     P_USERID,
                     LTRIM (RTRIM (P_PAGENAME)),
                     V_ID);
      END IF;
   ELSEIF V_MODULEID = 3
   THEN
      SELECT   (SELECT   STATUSCODE
                  FROM   OPPORTUNITY_MASTER
                 WHERE   OPPORTUNITYID = V_ID)
        INTO   V_STATUS
        FROM   DUAL;

      IF V_STATUS IS NULL
      THEN
         LEAVE sp_lbl;
      END IF;

      IF UPPER (V_STATUS) = '2'
      THEN
         INSERT INTO TRACKING_OPP (TID,
                                   WORKFLOWID,
                                   USERID,
                                   PAGENAME,
                                   OPPORTUNITYID)
           VALUES   (V_SEQ,
                     P_WORKFLOWID,
                     P_USERID,
                     LTRIM (RTRIM (P_PAGENAME)),
                     V_ID);
      END IF;
   ELSEIF V_MODULEID = 4
   THEN
      -- SELECT (SELECT STATUS FROM REVISIONHISTORY WHERE AWARD_ID=V_ID) INTO V_STATUS FROM DUAL;
      SELECT   (SELECT   STATUSCODE
                  FROM   AWARD_MASTER
                 WHERE   AWARDID = V_ID)
        INTO   V_STATUS
        FROM   DUAL;

      IF V_STATUS IS NULL
      THEN
         LEAVE sp_lbl;
      END IF;

      IF UPPER (V_STATUS) = '2'
      THEN
         INSERT INTO TRACKING_AWARD (TID,
                                     WORKFLOWID,
                                     USERID,
                                     PAGENAME,
                                     AWARDID)
           VALUES   (V_SEQ,
                     P_WORKFLOWID,
                     P_USERID,
                     LTRIM (RTRIM (P_PAGENAME)),
                     V_ID);
      END IF;
   END IF;
END;
