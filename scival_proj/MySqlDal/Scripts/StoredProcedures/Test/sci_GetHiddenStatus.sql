CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_GetHiddenStatus`(
	p_WorkId INTEGER
)
BEGIN
	select sci_checkhiddenstatus_fb(p_WorkId) hidden_cnt from dual;
END ;;
