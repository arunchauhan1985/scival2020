//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MySqlDal
{
    using System;
    using System.Collections.Generic;
    
    public partial class sci_related_opportunity
    {
        public Nullable<long> REL_OPP_ID { get; set; }
        public Nullable<long> OPPORTUNITY_ID { get; set; }
        public Nullable<long> RELATED_OPP_ID { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string OPPORTUNITYNAME { get; set; }
        public string RELAION_NAME { get; set; }
        public long ID { get; set; }
        public Nullable<long> AWARD_ID { get; set; }
        public string DESCRIPTION { get; set; }
    }
}