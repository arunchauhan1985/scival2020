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
    
    public partial class change
    {
        public string TYPE { get; set; }
        public Nullable<System.DateTime> POSTDATE { get; set; }
        public long VERSION { get; set; }
        public string CHANGE_TEXT { get; set; }
        public Nullable<long> CHANGEHISTORY_ID { get; set; }
        public Nullable<long> POSTDATE_IS_SAVE { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public long CHANGE_ID { get; set; }
    }
}
