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
    
    public partial class fundingbody_master
    {
        public long FUNDINGBODY_ID { get; set; }
        public string FUNDINGBODYNAME { get; set; }
        public string URL { get; set; }
        public System.DateTime CREATEDDATE { get; set; }
        public Nullable<long> CREATEDBY { get; set; }
        public Nullable<System.DateTime> CYCLECOMPLETIONDATE { get; set; }
        public Nullable<long> CYCLECOMPLETEDBY { get; set; }
        public Nullable<System.DateTime> LASTUPDATEDDATE { get; set; }
        public Nullable<long> LASTUPDATEDBY { get; set; }
        public Nullable<long> CYCLE { get; set; }
        public Nullable<long> STATUSCODE { get; set; }
        public Nullable<long> RUSH { get; set; }
        public Nullable<long> RUSHBY { get; set; }
        public string ALLOCATIONMODE { get; set; }
        public string ISOPORTUNITY { get; set; }
        public string COUNTRY { get; set; }
        public Nullable<System.DateTime> DUEDATE { get; set; }
        public string ISVIEWED { get; set; }
        public Nullable<long> BATCHNO { get; set; }
        public Nullable<System.DateTime> BATCHRECIEVEDATE { get; set; }
        public string SUBTYPE { get; set; }
        public Nullable<long> HIDDEN_FLAG { get; set; }
    }
}
