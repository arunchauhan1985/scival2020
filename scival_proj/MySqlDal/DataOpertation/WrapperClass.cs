using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DAL
{
    class WrapperClass
    {
    }
   
    class Award_Wrap : Attribute
    {
        public JObject Award { get; set; }
    }
    class FundingBody_Wrap : Attribute
    {
        public JObject FundingBody { get; set; }
    }
    class Opportunity_Wrap : Attribute
    {
        public JObject Opportunity { get; set; }
    }
    class Publication_Wrap : Attribute
    {
        public JObject Publication { get; set; }
    }
}
