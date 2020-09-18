using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;
using MySqlDal;
using MySqlDal.DataOpertation;
using MySqlDalAL;
//using System.Object;
//using System.Attribute;
//using Newtonsoft.Json.JsonPropertyAttribute;
namespace MySqlDal.DataOpertation
{
    #region JSON Model is created By Pankaj in JAN 2020..
    public class OPP_jsonModel
    {
        //public Int64 OpportunityId { get; set; }
        //public contactInformations contactInformations { get; set; }
        public contactInformation_OPP[] contactInformation_OPP { get; set; }
        public hasProvenance_OPP hasProvenance_OPP { get; set; }
        //public hasProvenances hasProvenances { get; set; }
        public opportunityLocation[] opportunityLocation { get; set; }
        public keyword[] keyword { get; set; }
        public title[] title { get; set; }

        //public expirationDateDetail expirationDateDetail { get; set; }
        
        public synopsis[] synopsis { get; set; }
        public description[] description { get; set; }
        public subjectMatter[] subjectMatter { get; set; }
        public duration duration { get; set; }
        public homePage homePage { get; set; }
        public associatedAmount associatedAmount { get; set; }
        public opportunityDate opportunityDate { get; set; }
        public eligibilityClassification eligibilityClassification { get; set; }


        public relatedFunder relatedFunder { get; set; }
        public hasFunder[] hasFunder { get; set; }
        public classification[] classification { get; set; }
        public relatedOpportunity relatedOpportunity { get; set; }
        public licenseInformation[] licenseInformation { get; set; }
        public instruction[] instruction { get; set; }
        public contactPerson[] contactPerson { get; set; }

        public string funderSchemeType { get; set; }
        public string fundingBodyOpportunityId { get; set; }
        public Int64 grantOpportunityId { get; set; }
        public string grantType { get; set; }
        public string repeatingOpportunity { get; set; }
        public Int64 numberOfAwards { get; set; }
        public string status { get; set; }

    }
    public class abstract_OPP
    {
        public string language { get; set; }
        public string value { get; set; }
    }
    //public class title_opp
    //{
    //    public string language { get; set; }
    //    public string value { get; set; }
    //}

    public class hasProvenance_OPP
    {
        public string contactPoint { get; set; }
        public string createdOn { get; set; }
        public string defunct { get; set; }
        public string derivedFrom { get; set; }
        public string hidden { get; set; }
        public string lastUpdateOn { get; set; }
        public string status { get; set; }
        public string version { get; set; }
        public string wasAttributedTo { get; set; }
    }
    //public class hasProvenances
    //{
    //    public string contactPoint { get; set; }
    //    public string createdOn { get; set; }
    //    public string defunct { get; set; }
    //    public string derivedFrom { get; set; }
    //    public string hidden { get; set; }
    //    public string lastUpdateOn { get; set; }
    //    public string status { get; set; }
    //    public string version { get; set; }
    //    public string wasAttributedTo { get; set; }
    //}

    public class contactInformation_OPP
    {
        public string link { get; set; }
        //public hasPostalAddress establishmentCountryCode { get; set; }  
        public hasPostalAddress hasPostalAddress { get; set; }
        public contactPerson[] contactPerson { get; set; }


    }

    //public class contactInformations
    //{
    //    public string link { get; set; }
    //    //public hasPostalAddress establishmentCountryCode { get; set; }  
    //    public hasPostalAddress hasPostalAddress { get; set; }
    //    public contactPerson[] contactPerson { get; set; }


    //}
    //public class hasPostalAddress
    //{
    //    public string addressCountry { get; set; }
    //    public string addressRegion { get; set; }
    //    public string addressLocality { get; set; }
    //    public string addressPostalCode { get; set; }
    //    public string postOfficeBoxNumber { get; set; }
    //    public string streetAddress { get; set; }
    //}

    public class contactPerson
    {
        public string honorific { get; set; }
        public string initials { get; set; }
        public string givenName { get; set; }
        public string middleName { get; set; }
        public string familyName { get; set; }
        public string emailAddress { get; set; }
    }
    public class opportunityLocation
    {
        public string country { get; set; }
        public string city { get; set; }
        public string state { get; set; }
    }

    public class keyword
    {
        public string language { get; set; }
        public string value { get; set; }
    }
    public class synopsis
    {
        public abstract_OPP abstract_OPP { get; set; }

        public string source { get; set; }
    }

    public class duration
    {
        public description_OPP[] description_OPP { get; set; }
        public string durationExpression { get; set; }
    }

    public class description_OPP
    {
        public abstract_OPP abstract_OPP { get; set; }

        public string source { get; set; }
    }

    public class subjectMatter
    {
        public abstract_OPP abstract_OPP { get; set; }

        public string source { get; set; }
    }

    public class homePage
    {
        public string link { get; set; }
        public string modifiedDate { get; set; }
        public string publishedDate { get; set; }
    }

    public class relatedFunder
    {
        public leadFunder leadFunder { get; set; }
        public hasFunder[] hasFunder { get; set; }

    }
    public class leadFunder
    {
        public Int64 fundingBodyId { get; set; }
    }
    public class hasFunder
    {
        public Int64 fundingBodyId { get; set; }
    }

    public class classification
    {
        public string type { get; set; }

        public hasSubject hasSubject { get; set; }
    }

    public class hasSubject
    {
        public string preferredLabel { get; set; }

        public identifiers identifier_oppclass { get; set; }
    }


    public class identifiers
    {
        public string type { get; set; }
        public string value { get; set; }
    }



    public class relatedOpportunity
    {
        public relatedTo relatedTo { get; set; }
        public replacedBy replacedBy { get; set; }
        public replaces_f replaces_f { get; set; }
    }

    public class relatedTo
    {
        public Int64 grantOpportunityId { get; set; }
        public string fundingBodyOpportunityId { get; set; }

        public title[] title { get; set; }
        public string description { get; set; }

    }

    public class replacedBy
    {
        public string grantOpportunityId { get; set; }
        public string fundingBodyOpportunityId { get; set; }

        public title[] title { get; set; }
        public string description { get; set; }
    }
    public class replaces_f
    {
        public string grantOpportunityId { get; set; }
        public string fundingBodyOpportunityId { get; set; }

        public title[] title { get; set; }
        public string description { get; set; }
    }

    public class title
    {
        public string language { get; set; }
        public string value { get; set; }
    }
    public class licenseInformation
    {
        public abstract_OPP abstract_OPP { get; set; }
        public string source { get; set; }
    }

    public class instruction
    {
        public abstract_OPP abstract_OPP { get; set; }
        public string source { get; set; }
    }



    public class associatedAmount
    {
        public description_OPP[] description_OPP { get; set; }
        public ceiling[] ceiling { get; set; }
        public estimatedTotal[] estimatedTotal { get; set; }
        public floor[] floor { get; set; }
    }

    public class ceiling
    {
        public string currency { get; set; }
        public Int64 amount { get; set; }
    }

    public class estimatedTotal
    {
        public string currency { get; set; }
        public Int64 amount { get; set; }
    }
    public class floor
    {
        public string currency { get; set; }
        public Int64 amount { get; set; }
    }

    public class eligibilityClassification
    {
        public description_OPP[] description_OPP { get; set; }

        public limitedSubmission limitedSubmission { get; set; }

        public individualEligibility individualEligibility { get; set; }

        public organisationEligibility organisationEligibility { get; set; }

        public citizenship citizenship { get; set; }

        public regionSpecific regionSpecific { get; set; }

        public restrictionScope restrictionScope { get; set; }
    }

    public class limitedSubmission
    {
        public string limitation { get; set; }
        public Int64 numberOfApplications { get; set; }
        public description_OPP[] description_OPP { get; set; }

    }

    public class individualEligibility
    {
       
        public string limitation { get; set; }
        //public List<applicantType> applicantType { get; set; }
        public List<string> applicantType { get; set; }
        //public  applicantType[] { get; set; }
        public List<string> degreeRequirement { get; set; }
        //public  degreeRequirement[] { get; set; }
    }
    //public class applicantType
    //{
    //    public string type { get; set; } 
    //}
    //public class degreeRequirement
    //{
    //    public string type { get; set; }
    //}

    public class organisationEligibility
    {
        public List<string> applicantType { get; set; }
        public string limitation { get; set; }
    }

  

    public class citizenship
    {
        public string limitation { get; set; }
        public List<string> country { get; set; }
    }

    public class regionSpecific
    {
        public string limitation { get; set; }
        public location[] location { get; set; }
    }

    public class location
    {
        public string country { get; set; }
        public string city { get; set; }
        public string state { get; set; }
    }

    public class restrictionScope
    {
        public string limitation { get; set; }
        //public restriction[] restriction { get; set; }
        public List<string> restriction { get; set; }

    }

    //public class restriction
    //{
    //  public string restriction { get; set; }
    //}

    public class opportunityDate
    {
        public cycle[] cycle { get; set; }
        public expirationDateDetail expirationDateDetail { get; set; }
    }

    public class description_exp
    {
        public abstract_OPP abstract_OPP { get; set; }

        public string source { get; set; }


    }

    public class expirationDateDetail
    {
        public string date { get; set; }
        public description_exp description_exp { get; set; }

    }

    public class cycle
    {
        public decision[] decision { get; set; }
        public endDateDetail[] endDateDetail { get; set; }
        public Int64 index { get; set; }
        public string label { get; set; }
        public letterOfIntent[] letterOfIntent { get; set; }
        public preproposal[] preproposal { get; set; }
        public proposal[] proposal { get; set; }
        public startDateDetail[] startDateDetail { get; set; }
    }

    public class startDateDetail
    {
        public string date { get; set; }
        public description description { get; set; }
        public string limitation { get; set; }
        public string required { get; set; }
    }

    public class endDateDetail
    {
        public string date { get; set; }
        public description description { get; set; }
        public string limitation { get; set; }
        public string required { get; set; }
    }

    public class letterOfIntent
    {
        public string date { get; set; }
        public description description { get; set; }
        public string limitation { get; set; }
        public string required { get; set; }
    }
    public class preproposal
    {
        public string date { get; set; }
        public description description { get; set; }
        public string limitation { get; set; }
        public string required { get; set; }
    }

    public class proposal
    {
        public string limitation { get; set; }
        public string date { get; set; }
        public string required { get; set; }
        public description description { get; set; }
    }

    public class decision
    {
        public string limitation { get; set; }
        public string date { get; set; }
        public string required { get; set; }
        public description description { get; set; }
    }

    #endregion
}