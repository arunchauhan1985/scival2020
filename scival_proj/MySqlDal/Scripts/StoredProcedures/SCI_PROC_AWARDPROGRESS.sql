CREATE DEFINER=`root`@`localhost` PROCEDURE `SCI_PROC_AWARDPROGRESS`(
p_workflowid       INTEGER
)
BEGIN
   declare V_ID         bigint;
   declare V_MODULEID   bigint;
   SELECT   ID, MODULEID INTO V_ID, V_MODULEID FROM SCI_WORKFLOW WHERE WORKFLOWID = P_WORKFLOWID;
   IF V_MODULEID = 4
   THEN
         SELECT * from 
         (SELECT 'Award' TAB,'awardbase.aspx' page,( case when COUNT(*) = 0 then 0 else 1 end) flag FROM   AWARD WHERE   AWARD_ID = v_ID
		UNION ALL
		SELECT 'AwardAmount' TAB, 'Amount.aspx?pid=4' page,( case when COUNT(*)=0 then 0 else 1 end) flag FROM   AMOUNT WHERE   AWARD_ID = v_ID
        UNION ALL
		SELECT 'Classification Group' TAB,'CLASSIFICATIONinfo.aspx' page, ( case when a=0 then 0 else 1 end) flag from (select COUNT(*) a FROM   CLASSIFICATIONGROUP WHERE   AWARD_ID = v_ID) C
		UNION ALL
		SELECT  'Awardees' TAB,'AWARDEES.aspx' page,( case when COUNT(*)=0 then 0 else 1 end) flag FROM  AWARDEES WHERE   AWARD_ID = v_ID
		UNION ALL
        SELECT 'Award Manager' TAB,'Contact.aspx?pid=4' page,( case when COUNT(*)=0 then 0 else 1 end) flag FROM AWARDMANAGERS WHERE   AWARD_ID = v_ID
		UNION ALL
        SELECT 'Related Programs' TAB,'Relatedprograms.aspx' page,( case when COUNT(*)=0 then 0 else 1 end) flag FROM RELATEDPROGRAMS WHERE   AWARD_ID = v_ID
		UNION ALL
        SELECT 'Relatedpportunities' tab,'RelatedOpportunities.aspx' page,(CASE WHEN COUNT( * ) = 0 THEN 0 ELSE 1 END) flag FROM sci_related_opportunity WHERE Award_ID = v_id
	    UNION ALL
        SELECT 'Related Fundingbodies' TAB,'RelatedOrg.aspx' page,( case when COUNT(*)=0 then 0 else 1 end) flag FROM RELATEDORGS WHERE AWARD_ID = v_ID
        UNION ALL
        SELECT 'AwardLocation' TAB,'AwardLocation.aspx' page, ( case when COUNT(*)=0 then 0 else 1 end) flag FROM award_location WHERE AWARD_ID = v_ID
        UNION ALL 
        SELECT 'RelatedItems' TAB, 'RelatedItems.aspx' page,( case when COUNT(*)=0 then 0 else 1 end) flag FROM RelatedItems WHERE AWARD_ID = v_ID
		UNION ALL
        SELECT 'Publication' TAB,'Publication.aspx' page,( case when COUNT(*)=0 then 0 else 1 end) flag FROM PublicationData WHERE AWARD_ID = v_ID
        UNION ALL
        SELECT 'Publication identifier' TAB,'ScholarlyOutput.aspx' page,( case when COUNT(*)=0 then 0 else 1 end) flag FROM   RESEARCHOUTCOME WHERE   AWARD_ID = v_ID
        UNION ALL  
        SELECT 'Funds' TAB,'Funds.aspx' page,( case when COUNT(*)=0 then 0 else 1 end) flag FROM FundedProjectDetail WHERE   AWARD_ID = v_ID
        ) as AwardProg;
   END IF;
END