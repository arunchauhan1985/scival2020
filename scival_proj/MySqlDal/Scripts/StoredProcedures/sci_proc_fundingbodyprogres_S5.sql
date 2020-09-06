CREATE DEFINER=`root`@`localhost` PROCEDURE `sci_proc_fundingbodyprogres_S5`(
   p_id             INTEGER  
)
BEGIN
      
		SELECT   *
		FROM   (SELECT   'Fundingbody' tab, 'FundingBase.aspx' page,
                         ( case when COUNT(*)=0 then 0 else 1 end) flag
					FROM   fundingbody
					WHERE   fundingbody_id = p_id
                
		UNION ALL
                
		SELECT  'Discription' tab, 'Item.aspx?pid=1' page,
                         ( case when COUNT(*)=0 then 0 else 1 end) flag
					FROM   about                  
					WHERE   fundingbody_id = p_id
                 
		UNION ALL
                
		SELECT   'Related Orgs' tab, 'RelatedOrg.aspx' page,
                         ( case when COUNT(*)=0 then 0 else 1 end) flag
					FROM   relatedorgs
					WHERE   fundingbody_id = p_id
		
		UNION ALL
        
        SELECT   'Establishment Info' tab, 'establish.aspx' page,
                         ( case when COUNT(*)=0 then 0 else 1 end) flag
					FROM   establishmentinfo
					WHERE   fundingbody_id = p_id
                
		UNION ALL
		
		SELECT   'Contacts' tab, 'Contact.aspx?pid=1' page,
                        ( case when COUNT(*)=0 then 0 else 1 end) flag
					FROM   contacts
					WHERE   fundingbody_id = p_id
                
		UNION ALL
                
		SELECT   'Funding Policy' tab, 'Item.aspx?pid=2' page,
                        ( case when COUNT(*)=0 then 0 else 1 end) flag
					FROM   Fundingpolicy ,item 
					WHERE   item.FUNDINGPOLICY_ID=Fundingpolicy.FUNDINGPOLICY_ID and fundingbody_id = p_id
                 
		UNION ALL
                
		SELECT   'Opportunities Source' tab, 'Opportunitiessource.aspx' page,
                        ( case when COUNT(*)=0 then 0 else 1 end) flag
					FROM   OPPORTUNITIESSOURCE
					WHERE   fundingbody_id = p_id
                 
		UNION ALL
		 
        SELECT   'Awards Source' tab, 'Awardssource.aspx' page,
                        ( case when COUNT(*)=0 then 0 else 1 end) flag
					FROM   AWARDSSOURCE
					WHERE   fundingbody_id = p_id
                 
		UNION ALL
                 
		SELECT   'Fundingbodies Source' tab, 'fundingBodyDataSource.aspx' page,
                        ( case when COUNT(*)=0 then 0 else 1 end) flag
					FROM   fundingBodyDataset
					WHERE   fundingbody_id = p_id
                 
		UNION ALL
                 
		SELECT   'Publications Source' tab, 'publicationDataSource.aspx' page,
                        ( case when COUNT(*)=0 then 0 else 1 end) flag
					FROM   publicationDataset
					WHERE   fundingbody_id = p_id
					
		UNION ALL
        
		SELECT   'AwardSuccessRate Description' tab, 'AwardSuccessRateDescription.aspx' page,
                        ( case when COUNT(*)=0 then 0 else 1 end) flag
					FROM   awardSuccessRatedesc
					WHERE   fundingbody_id = p_id
                  
		UNION ALL                                                     
        
		SELECT   'Identifier' tab, 'identifier.aspx' page,
                        ( case when COUNT(*)=0 then 0 else 1 end) flag
					FROM   identifier
					WHERE   fundingbody_id = p_id) as a;
                                
END ;;
