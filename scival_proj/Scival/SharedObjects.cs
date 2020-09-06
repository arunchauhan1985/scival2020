using System;
using System.Collections.Generic;
using System.Data;
using MySqlDal;

namespace Scival
{
    public static class SharedObjects
    {
        
        public static DataSet TaskBoard { get; set; }
        public static string TaskFlow { get; set; }
        public static sci_usermaster User { get; set; }
        public static int ExpireAlertCount { get; set; }
        public static List<ModuleWiseUserTask> UserTasks { get; set; }
        public static Int64 ModuleId { get; set; }
        public static Int64 TaskId { get; set; }
        public static Int64 ID { get; set; }
        public static string AwardTitle2018 { get; internal set; }
        public static string FundingBodyName { get; set; }
        public static Int64 Cycle { get; set; }
        public static Int64 WorkId { get; set; }
        public static String Message { get; set; }
        public static Int64 TRAN_TYPE_ID { get; set; }
        public static Int64 Allocation { get; set; }
        public static DateTime DueDate { get; set; }
        public static string OPFBID { get; set; }
        public static Int64 PageIds { get; set; }
        public static string PreviousUrl { get; set; }
        public static string CurrentUrl { get; set; }
        public static string Domain { get; set; }
        public static Int64 TransactionId { get; set; }
        public static string FundingClickPage { get; set; }
        public static string ClickPage { get; set; }
        public static bool IsOpportunityBaseFilled { get; set; }
        public static string DefaultLoad { get; set; }

        public static DataSet StartWork { get; set; }
        public static string NotListedCountry { get; set; }

        public static string FundingBodyContextName { get; set; }
        public static string upOppID { get; set; }
        public static Int64 RecCurrentStatus { get; set; }
        public static string OppStatusdisp { get; set; }
        public static string OppDis { get; set; }


        public static List<startwork> startworks { get; set; }
        public static bool IsAwardBaseFilled { get; set; }
        public static string TotalAmountChangedValue { get; set; }
        public static string Relatedorgs_ORGDBIDUdateID { get; set; }
        public static string RelatedorgsUdateID { get; set; }
        public static string AbstarctTitleLang { get; set; }
        public static string AbstarctTitle2018 { get; internal set; }
        public static string AwardTitleLang2018 { get; internal set; }
        public static string MultipleInitial { get; internal set; }
        public static bool IsFundingBaseFilled { get; internal set; }
    }
}
