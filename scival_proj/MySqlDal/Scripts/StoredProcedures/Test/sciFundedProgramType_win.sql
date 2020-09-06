CREATE DEFINER=`root`@`localhost` PROCEDURE `sciFundedProgramType_win`(
   p_workflowid       integer,
   p_id               varchar(4000),
   p_typeid           varchar(4000),
   p_mode             integer,
   p_oldtypeid        varchar(4000)  
)
sp_lbl:

BEGIN
	DECLARE v_fundingbodyid            integer;
	DECLARE v_FUNDEDPROGRAMSTYPESID    integer;
	DECLARE v_FUNDEDPROGRAMTYPESTEXT   varchar (2000);
	DECLARE v_count                    integer;
	DECLARE v_counfundid               integer;
	DECLARE v_counttype                integer;
	DECLARE v_tab                      varchar(4000);
	DECLARE NOT_FOUND INT DEFAULT 0;


	-- SELECT list BULK COLLECT INTO   v_tab FROM   table (scigetcsvtoLIST (p_typeid));

	SELECT ID INTO   v_fundingbodyid FROM   sci_workflow	WHERE   WORKFLOWID = p_workflowid;

	IF p_mode = 0
	THEN
      SELECT   COUNT(*) INTO   v_count FROM   FUNDEDPROGRAMSTYPES WHERE   FUNDINGBODY_ID = v_fundingbodyid;

	IF v_count = 0
    THEN
         SELECT   FUNDEDPROGRAMSTYPES_SEQ.NEXTVAL INTO   v_FUNDEDPROGRAMSTYPESID FROM   DUAL;

         INSERT INTO FUNDEDPROGRAMSTYPES VALUES   (v_FUNDEDPROGRAMSTYPESID, v_fundingbodyid);
         
         /*DECLARE i CURSOR FOR v_tab.FIRST . v_tab.LAST
         OPEN i;
         FETCH i INTO;
         WHILE NOT_FOUND=0
         DO
            SELECT   FUNDEDPROGRAMTYPESTEXT INTO   v_FUNDEDPROGRAMTYPESTEXT FROM   sci_fundedprogramstypelist
			WHERE   TRIM (TYPEID) = TRIM (v_tab (i));

            SELECT COUNT(*) INTO   v_counttype FROM   FUNDEDPROGRAMSTYPE
			WHERE FUNDEDPROGRAMSTYPES_ID IN
			(SELECT FUNDEDPROGRAMSTYPES_ID FROM   FUNDEDPROGRAMSTYPES
			WHERE FUNDINGBODY_ID = v_fundingbodyid) AND ID = TRIM (v_tab (i));

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
               
               RAISE e_error;
            END IF;

            INSERT INTO FUNDEDPROGRAMSTYPE
              VALUES   (
                           v_tab(i),
                           v_FUNDEDPROGRAMTYPESTEXT,
                           v_FUNDEDPROGRAMSTYPESID
                       );
         FETCH  INTO;
         END WHILE;
         CLOSE ;*/
      

      ELSE
        SELECT   FUNDEDPROGRAMSTYPES_ID INTO   v_FUNDEDPROGRAMSTYPESID FROM   FUNDEDPROGRAMSTYPES
		WHERE   FUNDINGBODY_ID = v_fundingbodyid;

         /*DECLARE i CURSOR FOR v_tab.FIRST . v_tab.LAST
         OPEN i;
         FETCH i INTO;
         WHILE NOT_FOUND=0
         DO
            SELECT   FUNDEDPROGRAMTYPESTEXT INTO   v_FUNDEDPROGRAMTYPESTEXT FROM   sci_fundedprogramstypelist
			WHERE   TRIM (TYPEID) = TRIM (v_tab (i));

            SELECT   COUNT(*) INTO   v_counttype FROM   FUNDEDPROGRAMSTYPE WHERE   FUNDEDPROGRAMSTYPES_ID IN
			(SELECT   FUNDEDPROGRAMSTYPES_ID FROM   FUNDEDPROGRAMSTYPES WHERE   FUNDINGBODY_ID = v_fundingbodyid)
			AND ID = TRIM (v_tab (i));

            IF v_counttype > 0
            THEN
               ROLLBACK;

               SET NOT_FOUND = 0;
               
                  SELECT   FUNDINGBODY_ID,
                           fps.FUNDEDPROGRAMSTYPES_ID,
                           ID,
                           FUNDEDPROGRAMSTYPE_TEXT
                    FROM   FUNDEDPROGRAMSTYPES fps, FUNDEDPROGRAMSTYPE fp
                   WHERE   fp.FUNDEDPROGRAMSTYPES_ID =
						   fps.FUNDEDPROGRAMSTYPES_ID AND fps.FUNDINGBODY_ID = v_fundingbodyid;
               
               RAISE e_error;
            END IF;

            INSERT INTO FUNDEDPROGRAMSTYPE
              VALUES   (
                           v_tab(i),
                           v_FUNDEDPROGRAMTYPESTEXT,
                           v_FUNDEDPROGRAMSTYPESID
                       );
         FETCH  INTO;
         END WHILE;
         CLOSE ;*/
      END IF;
   ELSEIF p_mode = 1
   THEN
      /*DECLARE i CURSOR FOR v_tab.FIRST . v_tab.LAST
      OPEN i;
      FETCH i INTO;
      WHILE NOT_FOUND=0
      DO
         DELETE FROM   FUNDEDPROGRAMSTYPE
               WHERE   FUNDEDPROGRAMSTYPES_ID = p_id
                       AND TRIM (ID) = TRIM (v_tab (i));
      FETCH  INTO;
      END WHILE;
      CLOSE ; */

      SELECT COUNT(*) INTO v_counfundid FROM FUNDEDPROGRAMSTYPE WHERE FUNDEDPROGRAMSTYPES_ID = p_id;


      IF v_counfundid = 0
      THEN
         DELETE FROM FUNDEDPROGRAMSTYPES WHERE   FUNDEDPROGRAMSTYPES_ID = p_id;
      END IF;
   /*ELSEIF p_mode = 2
   THEN
      DECLARE i CURSOR FOR v_tab.FIRST . v_tab.LAST
      OPEN i;
      FETCH i INTO;
      WHILE NOT_FOUND=0
      DO
        SELECT COUNT(*) INTO   v_counfundid FROM   FUNDEDPROGRAMSTYPE
		WHERE TRIM(ID) = TRIM(v_tab(i)) AND FUNDEDPROGRAMSTYPES_ID 
        IN (SELECT FUNDEDPROGRAMSTYPES_ID FROM FUNDEDPROGRAMSTYPES WHERE FUNDINGBODY_ID = v_fundingbodyid);

         IF v_counfundid > 0
         THEN
            ROLLBACK;
            
            SET NOT_FOUND = 0;
            
				SELECT   FUNDINGBODY_ID, fps.FUNDEDPROGRAMSTYPES_ID, ID, FUNDEDPROGRAMSTYPE_TEXT
				FROM   FUNDEDPROGRAMSTYPES fps, FUNDEDPROGRAMSTYPE fp
                WHERE   fp.FUNDEDPROGRAMSTYPES_ID = fps.FUNDEDPROGRAMSTYPES_ID AND fps.FUNDINGBODY_ID = v_fundingbodyid;

            RAISE e_error;
         END IF;

        SELECT   FUNDEDPROGRAMTYPESTEXT INTO   v_FUNDEDPROGRAMTYPESTEXT FROM   sci_fundedprogramstypelist
		WHERE   TRIM (TYPEID) = TRIM (v_tab (i));

         UPDATE   FUNDEDPROGRAMSTYPE
            SET   ID = v_tab(i),
                  FUNDEDPROGRAMSTYPE_TEXT = v_FUNDEDPROGRAMTYPESTEXT
          WHERE   FUNDEDPROGRAMSTYPES_ID IN
                        (SELECT   FUNDEDPROGRAMSTYPES_ID
                           FROM   FUNDEDPROGRAMSTYPES
                          WHERE   FUNDINGBODY_ID = v_fundingbodyid)
                  AND id = p_oldtypeid;
      FETCH  INTO;
      END WHILE;
      CLOSE ;   
      
   END IF;*/


   SET NOT_FOUND = 0;
   
      SELECT   FUNDINGBODY_ID,
               fps.FUNDEDPROGRAMSTYPES_ID,
               ID,
               FUNDEDPROGRAMSTYPE_TEXT
        FROM   FUNDEDPROGRAMSTYPES fps, FUNDEDPROGRAMSTYPE fp
       WHERE   fp.FUNDEDPROGRAMSTYPES_ID = fps.FUNDEDPROGRAMSTYPES_ID
               AND fps.FUNDINGBODY_ID = v_fundingbodyid;
   
   COMMIT;
END IF;

END ;;
