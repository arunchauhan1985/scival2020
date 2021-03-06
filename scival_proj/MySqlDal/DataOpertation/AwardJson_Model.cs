﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace DAL.JsonModel
{
    class AwardJson_Model : Attribute
    {
        public Int64 grantAwardId { get; set; }
        public string fundingBodyAwardId { get; set; }
        public List<Title> title { get; set; }
        //public DateTime noticeDate { get; set; }
        public string noticeDate { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string grantType { get; set; }
        public string funderSchemeType { get; set; }
        public HomePage homePage { get; set; }
        public List<Synopsis> synopsis { get; set; }
        public List<Keyword> keyword { get; set; }
        public List<Funds> funds { get; set; }
        public FundingDetail fundingDetail { get; set; }
        public List<AwardeeDetail> awardeeDetail { get; set; }
        public List<Classification> classification { get; set; }
        public List<RelatedOpportunity> relatedOpportunity { get; set; }
        public RelatedFunder relatedFunder { get; set; }
        public HasProvenance hasProvenance { get; set; }
        public licenseInformation_Aw[] licenseInformation_Aw { get; set; }
    }


    public class Title
    {
        public string value { get; set; }
        public string language { get; set; }
    }

    public class licenseInformation_Aw
    {
        public Abstract abstract_aw { get; set; }
        public string source { get; set; }
    }
   
    public class HomePage
    {
        public string link { get; set; }
        public DateTime publishedDate { get; set; }
        public DateTime modifiedDate { get; set; }
    }

    public class Abstract
    {
        public string language { get; set; }
        public string value { get; set; }
    }

    public class Synopsis
    {
        public Abstract @abstract { get; set; }
        public string source { get; set; }
    }

    public class Keyword
    {
        public string language { get; set; }
        public string value { get; set; }
    }

    public class Budget
    {
        public string currency { get; set; }
        public string amount { get; set; }
    }

    public class HasPart
    {
        public string fundingBodyProjectId { get; set; }
        public List<Budget> budget { get; set; }
    }


    public class HasPostalAddress
    {
        public string addressCountry { get; set; }
        public string addressRegion { get; set; }
        public string addressLocality { get; set; }
        public string addressPostalCode { get; set; }
        public string streetAddress { get; set; }
    }



    public class Funds
    {
        public string fundingBodyProjectId { get; set; }
        public List<HasPart> hasPart { get; set; }
        public List<Title> title { get; set; }
        public string acronym { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public HasPostalAddress hasPostalAddress { get; set; }
        public string link { get; set; }
        public List<Budget> budget { get; set; }
        public string status { get; set;}
    }

    public class FundedAmount
    {
        public string currency { get; set; }
        public Int64 amount { get; set; }
    }

    public class Installment
    {
        public int financialYear { get; set; }
        public int index { get; set; }
        public List<FundedAmount> fundedAmount { get; set; }
    }

    public class FundingTotal
    {
        public string currency { get; set; }
        public Int64 amount { get; set; }
    }

    public class FundingDetail
    {
        public List<Installment> installment { get; set; }
        public List<FundingTotal> fundingTotal { get; set; }
    }

    public class Name
    {
        public string language { get; set; }
        public string value { get; set; }
    }

    public class Name_Awardee
    {
        public string language { get; set; }
        public string value { get; set; }
    }



    public class Identifier
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Identifier_awardee
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class DepartmentName
    {
        public string language { get; set; }
        public string value { get; set; }
    }


    public class AffiliationOf
    {
        public string role { get; set; }
        public List<Name> name { get; set; }
        public string givenName { get; set; }
        public string familyName { get; set; }
        public string initials { get; set; }  
        public string emailAddress { get; set; }
        public string awardeePersonId { get; set; }

        public List<Identifier> identifier { get; set; }
        public string fundingBodyPersonId { get; set; }
    }

    public class AwardeeDetail
    {
        public string role_awardee { get; set; }
        public List<Name_Awardee> name_awardee { get; set; }
        public string awardeeAffiliationId { get; set; } 
        public List<FundingTotal> fundingTotal { get; set; }
        public string fundingBodyOrganizationId { get; set; }
        public string vatNumber { get; set; }
        public HasPostalAddress hasPostalAddress { get; set; }
        public List<Identifier_awardee> identifier_awardee { get; set; }
        public List<DepartmentName> departmentName { get; set; }
        public string activityType { get; set; }
        public List<AffiliationOf> affiliationOf { get; set; }
    }



    public class HasSubject
    {
        public string preferredLabel { get; set; }
        public string orgSpecificClassification { get; set; }
        public Identifier identifier { get; set; }
    }

    public class Classification
    {
        public string type { get; set; }
        public HasSubject hasSubject { get; set; }
    }



    public class RelatedOpportunity
    {
        public Int64 grantOpportunityId { get; set; }
        public string fundingBodyOpportunityId { get; set; }
        public List<Title> title { get; set; }
        public string description { get; set; }
    }

    public class LeadFunder
    {
        public Int64 fundingBodyId { get; set; }
    }

    public class HasFunder
    {
        public Int64 fundingBodyId { get; set; }
    }

    public class RelatedFunder
    {
        public LeadFunder leadFunder { get; set; }
        public List<HasFunder> hasFunder { get; set; }
    }

    public class HasProvenance
    {
        public string wasAttributedTo { get; set; }
        public string derivedFrom { get; set; }
        public DateTime createdOn { get; set; }
        public string contactPoint { get; set; }
        public string status { get; set; }
        public string version { get; set; }
        public bool hidden { get; set; }
        public bool defunct { get; set; }
        public DateTime lastUpdateOn { get; set; }
    }

}
