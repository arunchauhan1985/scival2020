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
    
    public partial class item
    {
        public string RELTYPE { get; set; }
        public byte[] DESCRIPTION { get; set; }
        public decimal ITEM_ID { get; set; }
        public Nullable<decimal> GEOSCOPE_ID { get; set; }
        public Nullable<decimal> APPINFO_ID { get; set; }
        public Nullable<decimal> ABOUT_ID { get; set; }
        public Nullable<decimal> RELATEDITEMS_ID { get; set; }
        public Nullable<decimal> SYNOPSIS_ID { get; set; }
        public Nullable<decimal> SUBJECTMATTER_ID { get; set; }
        public string LANG { get; set; }
        public Nullable<decimal> FUNDINGPOLICY_ID { get; set; }
        public Nullable<decimal> ELIGIBILITYDESCRIPTION_ID { get; set; }
        public Nullable<decimal> ESTIMATEDAMOUNTDESCRIPTION_ID { get; set; }
        public Nullable<decimal> LIMITEDSUBMISSIONDESC_ID { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
    }
}
