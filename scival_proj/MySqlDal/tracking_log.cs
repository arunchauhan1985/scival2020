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
    
    public partial class tracking_log
    {
        public long TLOGID { get; set; }
        public Nullable<long> MODULEID { get; set; }
        public Nullable<long> TASKID { get; set; }
        public Nullable<long> SEQUENCE { get; set; }
        public Nullable<System.DateTime> STARTDATE { get; set; }
        public Nullable<long> STARTBY { get; set; }
        public Nullable<System.DateTime> COMPLETEDDATE { get; set; }
        public Nullable<long> COMPLETEDBY { get; set; }
        public Nullable<long> STATUSID { get; set; }
    }
}