using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlDal.DataOpertation
{
    public class PreferredName
    {
        public string language { get; set; }
        public string value { get; set; }
    }

    public class AlternateName
    {
        public string language { get; set; }
        public string value { get; set; }
    }

    public class FBAbstract
    {
        public string language { get; set; }
        public string value { get; set; }
    }

    public class Description
    {
        public FBAbstract Abstract { get; set; }
        public string source { get; set; }
    }

    public class AwardSuccessRate
    {
        public int percentage { get; set; }
    }

    public class Establishment
    {
        public int establishmentYear { get; set; }
    }

    public class FBHasPostalAddress
    {
        public string addressCountry { get; set; }
        public string addressLocality { get; set; }
    }

    public class ContactInformation
    {
        public string link { get; set; }
        public FBHasPostalAddress hasPostalAddress { get; set; }
    }

    public class FundingBodyDataset
    {
        public string collectionCode { get; set; }
        public bool extended { get; set; }
        public int tier { get; set; }
    }

    public class OpportunityDataset
    {
        public string collectionCode { get; set; }
        public bool capture { get; set; }
    }

    public class AwardDataset
    {
        public string collectionCode { get; set; }
        public bool capture { get; set; }
    }

    public class PublicationDataset
    {
        public string collectionCode { get; set; }
        public bool capture { get; set; }
    }

    public class Registry
    {
        public FundingBodyDataset fundingBodyDataset { get; set; }
        public OpportunityDataset opportunityDataset { get; set; }
        public AwardDataset awardDataset { get; set; }
        public PublicationDataset publicationDataset { get; set; }
    }

    public class FBHasProvenance
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

    public class DefaultFBModel
    {
        public string status { get; set; }
        public long fundingBodyId { get; set; }
        public string homePage { get; set; }
        public List<PreferredName> preferredName { get; set; }
        public string financeType { get; set; }
        public string activityType { get; set; }
        public string profitabilityType { get; set; }
        public List<AlternateName> alternateName { get; set; }
        public string country { get; set; }
        public List<Description> description { get; set; }
        public AwardSuccessRate awardSuccessRate { get; set; }
        public Establishment establishment { get; set; }
        public ContactInformation contactInformation { get; set; }
        public Registry registry { get; set; }
        public FBHasProvenance hasProvenance { get; set; }
    }
}
