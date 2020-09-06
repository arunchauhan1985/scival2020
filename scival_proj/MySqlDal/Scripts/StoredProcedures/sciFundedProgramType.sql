CREATE DEFINER=`root`@`localhost` PROCEDURE `sciFundedProgramType`(
   p_workflowid       integer,
   p_id               varchar(4000),
   p_typeid           varchar(4000),
   p_mode             integer,
   p_oldtypeid      varchar(4000)
)
sp_lbl:

BEGIN
   DECLARE v_fundingbodyid            integer;
   DECLARE v_FUNDEDPROGRAMSTYPESID    integer;
   DECLARE v_FUNDEDPROGRAMTYPESTEXT   varchar (2000);
   DECLARE v_count                    integer;
   DECLARE v_counfundid               integer;
   DECLARE v_counttype                integer;

   SELECT   ID
     INTO   v_fundingbodyid
     FROM   sci_workflow
    WHERE   WORKFLOWID = p_workflowid;

   IF p_mode = 0
   THEN
      SELECT   COUNT(*)
        INTO   v_count
        FROM   FUNDEDPROGRAMSTYPES
       WHERE   FUNDINGBODY_ID = v_fundingbodyid;

      IF v_count = 0
      THEN
         SELECT   FUNDEDPROGRAMSTYPES_SEQ.NEXTVAL
           INTO   v_FUNDEDPROGRAMSTYPESID
           FROM   DUAL;

         INSERT INTO FUNDEDPROGRAMSTYPES
           VALUES   (v_FUNDEDPROGRAMSTYPESID, v_fundingbodyid);

         SELECT   FUNDEDPROGRAMTYPESTEXT
           INTO   v_FUNDEDPROGRAMTYPESTEXT
           FROM   sci_fundedprogramstypelist
          WHERE   TRIM (TYPEID) = TRIM (p_typeid);

         SELECT   COUNT(*)
           INTO   v_counttype
           FROM   FUNDEDPROGRAMSTYPE
          WHERE   FUNDEDPROGRAMSTYPES_ID IN
                        (SELECT   FUNDEDPROGRAMSTYPES_ID
                           FROM   FUNDEDPROGRAMSTYPES
                          WHERE   FUNDINGBODY_ID = v_fundingbodyid)
                  AND ID = TRIM (p_typeid);

         IF v_counttype > 0
         THEN
            ROLLBACK;

               SELECT   FUNDINGBODY_ID,
                        fps.FUNDEDPROGRAMSTYPES_ID,
                        ID,
                        FUNDEDPROGRAMSTYPE_TEXT
                 FROM   FUNDEDPROGRAMSTYPES fps, FUNDEDPROGRAMSTYPE fp
                WHERE   fp.FUNDEDPROGRAMSTYPES_ID =
                           fps.FUNDEDPROGRAMSTYPES_ID
                        AND fps.FUNDINGBODY_ID = v_fundingbodyid;
            
         END IF;

         INSERT INTO FUNDEDPROGRAMSTYPE
           VALUES   (
                        p_typeid,
                        v_FUNDEDPROGRAMTYPESTEXT,
                        v_FUNDEDPROGRAMSTYPESID
                    );
      ELSE
         SELECT   FUNDEDPROGRAMSTYPES_ID
           INTO   v_FUNDEDPROGRAMSTYPESID
           FROM   FUNDEDPROGRAMSTYPES
          WHERE   FUNDINGBODY_ID = v_fundingbodyid;

         SELECT   FUNDEDPROGRAMTYPESTEXT
           INTO   v_FUNDEDPROGRAMTYPESTEXT
           FROM   sci_fundedprogramstypelist
          WHERE   TRIM (TYPEID) = TRIM (p_typeid);

         SELECT   COUNT(*)
           INTO   v_counttype
           FROM   FUNDEDPROGRAMSTYPE
          WHERE   FUNDEDPROGRAMSTYPES_ID IN
                        (SELECT   FUNDEDPROGRAMSTYPES_ID
                           FROM   FUNDEDPROGRAMSTYPES
                          WHERE   FUNDINGBODY_ID = v_fundingbodyid)
                  AND ID = TRIM (p_typeid);

         IF v_counttype > 0
         THEN
            ROLLBACK;
            
               SELECT   FUNDINGBODY_ID,
                        fps.FUNDEDPROGRAMSTYPES_ID,
                        ID,
                        FUNDEDPROGRAMSTYPE_TEXT
                 FROM   FUNDEDPROGRAMSTYPES fps, FUNDEDPROGRAMSTYPE fp
                WHERE   fp.FUNDEDPROGRAMSTYPES_ID =
                           fps.FUNDEDPROGRAMSTYPES_ID
                        AND fps.FUNDINGBODY_ID = v_fundingbodyid;            
            
         END IF;

         INSERT INTO FUNDEDPROGRAMSTYPE
           VALUES   (
                        p_typeid,
                        v_FUNDEDPROGRAMTYPESTEXT,
                        v_FUNDEDPROGRAMSTYPESID
                    );
      END IF;
   ELSEIF p_mode = 1
   THEN
      DELETE FROM   FUNDEDPROGRAMSTYPE
            WHERE   FUNDEDPROGRAMSTYPES_ID =p_id 
                    AND trim (ID) = TRIM (p_typeid);

      SELECT   COUNT(*)
        INTO   v_counfundid
        FROM   FUNDEDPROGRAMSTYPE
       WHERE   FUNDEDPROGRAMSTYPES_ID =p_id ;

      IF v_counfundid = 0
      THEN
         DELETE FROM   FUNDEDPROGRAMSTYPES
               WHERE   FUNDEDPROGRAMSTYPES_ID =p_id ;
      END IF;
   ELSEIF p_mode = 2
   THEN
      SELECT   COUNT(*)
        INTO   v_counfundid
        FROM   FUNDEDPROGRAMSTYPE
       WHERE   TRIM (ID) = TRIM (p_typeid)
               AND FUNDEDPROGRAMSTYPES_ID IN
                        (SELECT   FUNDEDPROGRAMSTYPES_ID
                           FROM   FUNDEDPROGRAMSTYPES
                          WHERE   FUNDINGBODY_ID = v_fundingbodyid);

      IF v_counfundid > 0
      THEN         
         
            SELECT   FUNDINGBODY_ID,
                     fps.FUNDEDPROGRAMSTYPES_ID,
                     ID,
                     FUNDEDPROGRAMSTYPE_TEXT
              FROM   FUNDEDPROGRAMSTYPES fps, FUNDEDPROGRAMSTYPE fp
             WHERE   fp.FUNDEDPROGRAMSTYPES_ID = fps.FUNDEDPROGRAMSTYPES_ID
                     AND fps.FUNDINGBODY_ID = v_fundingbodyid;
                                          
      END IF;

      SELECT   FUNDEDPROGRAMTYPESTEXT
        INTO   v_FUNDEDPROGRAMTYPESTEXT
        FROM   sci_fundedprogramstypelist
       WHERE   TRIM (TYPEID) = TRIM (p_typeid);

      UPDATE   FUNDEDPROGRAMSTYPE
         SET   ID = p_typeid,
               FUNDEDPROGRAMSTYPE_TEXT = v_FUNDEDPROGRAMTYPESTEXT
       WHERE   FUNDEDPROGRAMSTYPES_ID IN
                     (SELECT   FUNDEDPROGRAMSTYPES_ID
                        FROM   FUNDEDPROGRAMSTYPES
                       WHERE   FUNDINGBODY_ID = v_fundingbodyid)
               AND id = p_oldtypeid;
   ELSE
      
      LEAVE sp_lbl;
   END IF;
   
      SELECT   FUNDINGBODY_ID,
               fps.FUNDEDPROGRAMSTYPES_ID,
               ID,
               FUNDEDPROGRAMSTYPE_TEXT
        FROM   FUNDEDPROGRAMSTYPES fps, FUNDEDPROGRAMSTYPE fp
       WHERE   fp.FUNDEDPROGRAMSTYPES_ID = fps.FUNDEDPROGRAMSTYPES_ID
               AND fps.FUNDINGBODY_ID = v_fundingbodyid;   

   COMMIT;
END ;;
