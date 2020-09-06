using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;
using System.ComponentModel;

namespace DAL 
{
   public class FB_JSON_Model
    {

    public string status { get; set; }
    public Int64 fundingBodyId { get; set; }
    //public Int64  orgDbId{ get; set; }
    public string homePage { get; set; }
    //public preferredOrgName preferredOrgName { get; set; }
    public List<preferredName> preferredName { get; set; }
    //public List<abbrevName> abbrevName { get; set; }
    //public acronym[] acronym { get; set; }
    public List<acronym> acronym { get; set; }
    public List<abbrevName> abbrevName { get; set; }
    //public contextName[] contextName { get; set; }

    //public List<contextName> contextName { get; set; }

    //public string type { get; set; }

    public string financeType { get; set; }
    public string activityType { get; set; }
    public string profitabilityType { get; set; }
    public List<alternateName> alternateName { get; set; }
    //public alternateName alternateName { get; set; }
    public string country { get; set; }
    //public string awardSuccessRate { get; set; }
    //public string description { get; set; }
   
    public fundingPolicy[] fundingPolicy { get; set; }

    //public string subType { get; set; }
    //public string profitability { get; set; }
    //public string countryCode { get; set; }
    public string state { get; set; }
    public List<identifier> identifier { get; set; }
   // public identifier identifier { get; set; }

    //public externalIdentifiers[] externalIdentifiers { get; set; }

    public description[] description { get; set; }
    //public description description { get; set; }

    public awardSuccessRate awardSuccessRate { get; set; }
    public establishment establishment { get; set; }
    //public description[] description { get; set; }
    public contactInformation contactInformation { get; set; }

    //public hasPostalAddress hasPostalAddress { get; set; }
    //public relatedOrgs[] relatedOrgs { get; set; }

    //public operationalDetails operationalDetails { get; set; }

    public registry registry { get; set; }

    public hasProvenance hasProvenance { get; set; }

    //public string abstracts{get;set;}

    //public source[] source { get; set; }

    //public string relation { get; set; } 
    public relation relation { get; set; } 
}

   public class relation
   {
       ////[JsonProperty("part")]
       //public string reltype { get; set; }
       //public reltype reltype { get; set; }
       //public string reltypeValue { get; set; }
       public List<string> applicantType { get; set; }

      // public partOf[] partOf { get; set; }
      
       //public parentOf[] parentOf { get; set; }
      // public hasPart[] hasPart { get; set; }
      
       //public CHANGE[] CHANGE { get; set; }
       //public affiliatedWith[] affiliatedWith { get; set; }
       //public continuationOf[] continuationOf { get; set; }
       //public renamedAs[] renamedAs { get; set; }
       //public mergedWith[] mergedWith { get; set; }
       //public mergerOf[] mergerOf { get; set; }
       //public incrorporatedInto[] incrorporatedInto { get; set; }
       //public incorporates[] incorporates { get; set; }
       //public splitInto[] splitInto { get; set; }
       //public splitFrom[] splitFrom { get; set; }
       //public isReplacedBy[] isReplacedBy { get; set; }
       //public replaces[] replaces { get; set; }

       public List<Int64> partOf { get; set; }
       public List<Int64> hasPart { get; set; }
       public List<Int64> CHANGE { get; set; }
       public List<Int64> affiliatedWith { get; set; }
       public List<Int64> continuationOf { get; set; }
       public List<Int64> renamedAs { get; set; }
       public List<Int64> mergedWith { get; set; }
       public List<Int64> mergerOf { get; set; }
       public List<Int64> incrorporatedInto { get; set; }
       public List<Int64> incorporates { get; set; }
       public List<Int64> splitInto { get; set; }
       public List<Int64> splitFrom { get; set; }
       public List<Int64> isReplacedBy { get; set; }
       public List<Int64> replaces { get; set; }
   }

   //public class reltype
   //{
   //    //public string reltype { get; set; }
   //    public string reltypeValue { get; set; }
   //}
   ////public class reltypeValue
   ////{
   ////    public string reltypeValue { get; set; }
   ////}

   public class partOf
   {
       public string reltypeValue { get; set; }
   }
   public class parentOf
   {
       public string reltypeValue { get; set; }
   }

   public class hasPart
   {
       public string reltypeValue { get; set; }
   }
   public class CHANGE
   {
       public string reltypeValue { get; set; }
   }

   public class affiliatedWith
   {
       public string reltypeValue { get; set; }
   }
   public class continuationOf
   {
       public string reltypeValue { get; set; }
   }
   public class renamedAs
   {
       public string reltypeValue { get; set; }
   }

   public class mergedWith
   {
       public string reltypeValue { get; set; }
   }
   public class mergerOf
   {
       public string reltypeValue { get; set; }
   }

   public class incrorporatedInto
   {
       public string reltypeValue { get; set; }
   }
   public class incorporates
   {
       public string reltypeValue { get; set; }
   }

   public class splitInto
   {
       public string reltypeValue { get; set; }
   }
   public class splitFrom
   {
       public string reltypeValue { get; set; }
   }

   public class isReplacedBy
   {
       public string reltypeValue { get; set; }
   }
   public class replaces
   {
       public string reltypeValue { get; set; }
   }

    
public class abstracts
{
    public string language { get; set; }
    public string value { get; set; }
}

public class preferredName
{
    public string language { get; set; }
    public string value { get; set; }
}
public class abbrevName
{
    public string language { get; set; }
    public string value { get; set; }
}
public class acronym
{
    public string language { get; set; }
    public string value { get; set; }
}
public class contextName
{
    public string language { get; set; }
    public string value { get; set; }
}

public class alternateName
{
    public string language { get; set; }
    public string value { get; set; }
}
public class externalIdentifiers
{
    public string type { get; set; }
    public string[] value { get; set; }
}

public class identifier
{
    public string type { get; set; }
    //public string[] value { get; set; }
    public string value { get; set; }
}
public class description
{
    public abstracts abstracts { get; set; }

    //public string language { get; set; }
    //public string value { get; set; }
    public string source { get; set; }

    
}


public class awardSuccessRate
{
    public Int32 percentage { get; set; }
    //public int percentage { get; set; }
    public description[] description { get; set; }    
    //public description description { get; set; }    
}
public class establishment
{
    public Int32 establishmentYear { get; set; }
    public string country { get; set; }
    public description[] description { get; set; }
    //public description description { get; set; }
}
public class fundingPolicy
{
  
   public abstracts abstracts { get; set; }

   public string source { get; set; }
}

public class hasProvenance
{
    public string contactPoint { get; set; }
    public string createdOn { get; set; }
    public bool defunct { get; set; }
    public string derivedFrom { get; set; }
    public bool hidden { get; set; }
    public string lastUpdateOn { get; set; }
    public string status { get; set; }
    public string version { get; set; }
    public string wasAttributedTo { get; set; }
}

public class contactInformation
{
    public string link { get; set; }
    //public hasPostalAddress establishmentCountryCode { get; set; }  
    public hasPostalAddress hasPostalAddress { get; set; }  
}
public class hasPostalAddress
{
    public string addressCountry { get; set; }
    public string addressRegion { get; set; }
    public string addressLocality { get; set; }
    public string addressPostalCode { get; set; }
    public string postOfficeBoxNumber { get; set; }
    public string streetAddress { get; set; }
}
//public class relatedOrgs
//{
//    public int orgDbId { get; set; }
//    public string relType { get; set; }
//}
public class operationalDetails
{
    public string collectionCode { get; set; }
    public bool hidden { get; set; }
    public string status { get; set; }
    public string version { get; set; }
    public string createddate { get; set; }
}
public class registry
{
    public fundingBodyDataset fundingBodyDataset { get; set; }
    public opportunityDataset opportunityDataset { get; set; }
    public awardDataset awardDataset { get; set; }
    public publicationDataset publicationDataset { get; set; }

}
//public class registry
//{
//    public fundingBodyDataset fundingBodyDataset { get; set; }
//    public fundingBodyDataset opportunityDataset { get; set; }
//    public fundingBodyDataset awardDataset { get; set; }
//    public fundingBodyDataset publicationDataset { get; set; }
   
//}
public class fundingBodyDataset
{
   
    public string collectionCode { get; set; }
    public bool extended { get; set; }
    public Int64 tier { get; set; }
    public source[] source { get; set; }
}
public class awardDataset
{
    public string collectionCode { get; set; }
    public bool capture { get; set; }
    public source[] source { get; set; }
}

public class opportunityDataset
{
    public string collectionCode { get; set; }
    public bool capture { get; set; }
    public source[] source { get; set; }
}

public class publicationDataset
{
    public string collectionCode { get; set; }
    public bool capture { get; set; }
    public source[] source { get; set; }
}


public class source
{
    public string name { get; set; }
    public string url { get; set; }
    public string status { get; set; }
    public string frequency { get; set; }
    public string captureStart { get; set; }
    public string captureEnd { get; set; }
    public string comment { get; set; }
}
//public class dataset
//{
//    public bool capture { get; set; }
//    public string collectionCode { get; set; }
//    public source[] source { get; set; }
//}
  

    
}
