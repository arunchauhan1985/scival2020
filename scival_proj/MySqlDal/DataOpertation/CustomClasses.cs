using System;
using System.Collections.Generic;

namespace MySqlDal
{
    public class ModuleWiseUserTask
    {
        public Int64 ModuleId { get; set; }
        public String ModuleName { get; set; }
        public Int64 TaskId { get; set; }
        public String TaskName { get; set; }
        public Int64 UserId { get; set; }
        public String Name { get; set; }
    }

    public class UserTaskList
    {
        public string MODULENAME { get; set; }
        public long MODULEID { get; set; }
        public string FUNDINGBODYNAME { get; set; }
        public long ID { get; set; }
        public string TASKNAME { get; set; }
        public long TASKID { get; set; }
        public long? ASSIGNBY { get; set; }
        public DateTime? ASSIGNDATE { get; set; }
        public long CYCLE { get; set; }
        public long WORKFLOWID { get; set; }
        public long STATUSID { get; set; }
        public string OPPORTUNITYNAME { get; set; }
        public string OPURL { get; set; }
    }

    public class UserFunding
    {
        public Int64 FundingBodyId { get; set; }
        public String FundingBodyName { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class FundingBodyMaster
    {
        public Int64 FundingBodyId { get; set; }
        public String FundingBodyName { get; set; }
        public Int64 Batch { get; set; }
    }

    public class DummyTaskList
    {
        public string ModuleName { get; set; }
        public Int64 ModuleId { get; set; }
        public string FundingBodyName { get; set; }
        public Int64 Id { get; set; }
        public string TaskName { get; set; }
        public Int64 TaskId { get; set; }
        public string DueDate { get; set; }
    }

    public class ExpiryDetail
    {
        public string ModuleName { get; set; }
        public Int64 ModuleId { get; set; }
        public string FundingBodyName { get; set; }
        public string OpportunityName { get; set; }
        public Int64 Id { get; set; }
        public string TaskName { get; set; }
        public Int64 TaskId { get; set; }
        public Int64 Cycle { get; set; }
        public string DueDate { get; set; }
        public Int64 Cid { get; set; }
    }

    public class NewTaskList
    {
        public string ModuleName { get; set; }
        public Int64 ModuleId { get; set; }
        public string FundingBodyName { get; set; }
        public Int64 Id { get; set; }
        public string TaskName { get; set; }
        public Int64 TaskId { get; set; }
        public string DueDate { get; set; }
        public Int64 Cycle { get; set; }
    }

    public class OpportunityList
    {
        public Int64 WORKFLOWID { get; set; }
        public Int64 OPPORTUNITY_ID { get; set; }
        public Int64 FUNDINGBODYOPPORTUNITYID { get; set; }
        public string Name { get; set; }
        public Int64 Id { get; set; }
    }

    public class AwardList
    {
        public Int64 WORKFLOWID { get; set; }
        public Int64 AWARD_ID { get; set; }
        public Int64 FUNDINGBODYAWARDID { get; set; }
        public string Name { get; set; }
        public Int64 Id { get; set; }
    }

    public class DashboardUserFunding
    {
        public string Tab { get; set; }
        public Int64 Count { get; set; }
    }

    public class DashboardTask
    {
        public string TaskName { get; set; }
        public Int64 TaskId { get; set; }
        public Int64 WorkFlowId { get; set; }
        public string FundingBodyName { get; set; }
        public Int64 Sequence { get; set; }
    }

    public class DashboardRemark
    {
        public string Remark { get; set; }
        public string Task { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDate1 { get; set; }
    }

    public class UrlGroupDetail
    {
        public Int64 GroupId { get; set; }
        public Int64 Id { get; set; }
        public Int64? UrlNumber { get; set; }
        public string Url { get; set; }
        public Int64? ModuleId { get; set; }
        public Int64? Batch { get; set; }
    }

    public class UrlDetailAndCount
    {
        public string UrlId { get; set; }
        public string Url { get; set; }
        public Int64? Count { get; set; }
    }

    public class WebWatcherUrl
    {
        public string Url { get; set; }
        public Int64 UrlId { get; set; }
    }

    public class UpdateColumn
    {
        public List<string> updateChangeColour;

        public List<string> UpdateChangeColour
        {
            get { return updateChangeColour; }
            set { updateChangeColour.Add(value.ToString()); }
        }
    }

    public class ClassificationList
    {
        public Int64 CLASSIFICATIONGROUP_ID { get; set; }
        public Int64 FUNDINGBODY_ID { get; set; }
        public string TYPE { get; set; }
        public Int64 CLASSIFICATIONS_ID { get; set; }
        public Int64 FREQUENCY { get; set; }
        public string CODE { get; set; }
        public string CLASSIFICATION_TEXT { get; set; }
    }

    public class CustomAsjcdescription
    {
        public string CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string SUB_LEVEL_CODE { get; set; }
        public string SUB_LEVEL_DESCRIPTION { get; set; }        
    }

    public class startwork
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public Int64 TransitionalID { get; set; }

    }

    public class ProgressTable
    {
        public string Tab { get; set; }
        public string page { get; set; }
        public int flag { get; set; }
    }

    public class PageUrl
    {
        public string Url { get; set; }
    }
}