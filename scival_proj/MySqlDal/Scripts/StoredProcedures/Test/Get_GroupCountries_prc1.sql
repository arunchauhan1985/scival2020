CREATE DEFINER=`root`@`localhost` PROCEDURE `Get_GroupCountries_prc1`(
	in p_countrygroupid integer  
)
begin    
	select COUNTRYGROUP_ID,COUNTRYGROUP_NAME,COUNTRYGROUP_STATUS from SCI_COUNTRYGROUP;
  
	select COUNTRY_ID,COUNTRY_NAME,COUNTRY_CODE,COUNTRYGROUP_ID,
	(select COUNTRYID from sci_countrycodes where upper(LCODE)= country_code)  old_countryid
	from SCI_COUNTRIES
	where  COUNTRYGROUP_ID= p_countrygroupid;   
end ;;
