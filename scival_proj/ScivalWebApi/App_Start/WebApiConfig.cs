using System.Web.Http;

namespace ScivalWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            CommonOperationRegister(config);
            AwardOperationRegister(config);
            FundingBodyOperationRegister(config);
            OpportunityOperationRegister(config);
            TaskOperationRegister(config);
            WebWatcherOperationRegister(config);
            XmlOperationRegister(config);
        }

        private static void CommonOperationRegister(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "GetDatabaseVersion",
                routeTemplate: "api/Common/GetDatabaseVersion",
                defaults: new { controller = "Common", action = "GetDatabaseVersion" }
            );

            config.Routes.MapHttpRoute(
                name: "ValidateLogin",
                routeTemplate: "api/Common/ValidateLogin/{username}/{password}",
                defaults: new { controller = "Common", action = "ValidateLogin" }
            );

            config.Routes.MapHttpRoute(
                name: "GetExpireAlertsCount",
                routeTemplate: "api/Common/GetExpireAlertsCount",
                defaults: new { controller = "Common", action = "GetExpireAlertsCount" }
            );

            config.Routes.MapHttpRoute(
                name: "UpdateExpireAlert",
                routeTemplate: "api/Common/UpdateExpireAlert",
                defaults: new { controller = "Common", action = "UpdateExpireAlert" }
            );
        }

        private static void AwardOperationRegister(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "GetAwardListsByTask",
                routeTemplate: "api/Award/GetAwardListsByTask/{fundingBodyId}/{taskId}/{updateFlag}/{userId}",
                defaults: new { controller = "Award", action = "GetAwardListsByTask" }
            );

            config.Routes.MapHttpRoute(
                name: "GetAwardWorkflowId",
                routeTemplate: "api/Award/GetAwardWorkflowId/{fundingBodyId}/{userId}",
                defaults: new { controller = "Award", action = "GetAwardWorkflowId" }
            );

            config.Routes.MapHttpRoute(
                name: "GetDashBoardDetails",
                routeTemplate: "api/Award/GetDashBoardDetails/{userId}/{userName}/{workflowId}",
                defaults: new { controller = "Award", action = "GetDashBoardDetails" }
            );

            config.Routes.MapHttpRoute(
                name: "GetDashBoardDetailsUrl",
                routeTemplate: "api/Award/GetDashBoardDetailsUrl",
                defaults: new { controller = "Award", action = "GetDashBoardDetailsUrl" }
            );

            config.Routes.MapHttpRoute(
                name: "GetDashBoardDetailsDashboardTask",
                routeTemplate: "api/Award/GetDashBoardDetailsDashboardTask",
                defaults: new { controller = "Award", action = "GetDashBoardDetailsDashboardTask" }
            );

            config.Routes.MapHttpRoute(
                name: "GetDashBoardDetailsDashboardRemark",
                routeTemplate: "api/Award/GetDashBoardDetailsDashboardRemark",
                defaults: new { controller = "Award", action = "GetDashBoardDetailsDashboardRemark" }
            );
        }

        private static void FundingBodyOperationRegister(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
               name: "GetUserFundingLists",
               routeTemplate: "api/FundingBody/GetUserFundingLists/{userId}/{moduleId}",
               defaults: new { controller = "FundingBody", action = "GetUserFundingLists" }
           );

            config.Routes.MapHttpRoute(
               name: "GetUserFundingListsByTask",
               routeTemplate: "api/FundingBody/GetUserFundingListsByTask/{userId}/{moduleId}/{taskId}",
               defaults: new { controller = "FundingBody", action = "GetUserFundingListsByTask" }
           );

            config.Routes.MapHttpRoute(
               name: "GetDashBoardDetails",
               routeTemplate: "api/FundingBody/GetDashBoardDetails/{userId}/{userName}/{id}/{moduleId}/{taskId}/{cycle}",
               defaults: new { controller = "FundingBody", action = "GetDashBoardDetails" }
           );

            config.Routes.MapHttpRoute(
               name: "GetDashBoardDetailsTaskList",
               routeTemplate: "api/FundingBody/GetDashBoardDetailsTaskList",
               defaults: new { controller = "FundingBody", action = "GetDashBoardDetailsTaskList" }
           );

            config.Routes.MapHttpRoute(
               name: "GetDashBoardDetailsRemarkList",
               routeTemplate: "api/FundingBody/GetDashBoardDetailsRemarkList",
               defaults: new { controller = "FundingBody", action = "GetDashBoardDetailsRemarkList" }
           );

            config.Routes.MapHttpRoute(
               name: "GetLanguageMasters",
               routeTemplate: "api/FundingBody/GetLanguageMasters/{languageLength}",
               defaults: new { controller = "FundingBody", action = "GetLanguageMasters" }
           );
        }

        private static void OpportunityOperationRegister(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
               name: "GetOpportunityListsByTask",
               routeTemplate: "api/Opportunity/GetOpportunityListsByTask/{fundingBodyId}/{taskId}/{updateFlag}/{userId}",
               defaults: new { controller = "Opportunity", action = "GetOpportunityListsByTask" }
           );

            config.Routes.MapHttpRoute(
               name: "GetOpportunityWorkflowId",
               routeTemplate: "api/Opportunity/GetOpportunityWorkflowId/{fundingBodyId}/{userId}",
               defaults: new { controller = "Opportunity", action = "GetOpportunityWorkflowId" }
           );

            config.Routes.MapHttpRoute(
               name: "GetDashBoardDetails",
               routeTemplate: "api/Opportunity/GetDashBoardDetails/{userId}/{userName}/{workflowId}",
               defaults: new { controller = "Opportunity", action = "GetDashBoardDetails" }
           );

            config.Routes.MapHttpRoute(
               name: "GetDashBoardDetailsUrl",
               routeTemplate: "api/Opportunity/GetDashBoardDetailsUrl",
               defaults: new { controller = "Opportunity", action = "GetDashBoardDetailsUrl" }
           );

            config.Routes.MapHttpRoute(
               name: "GetDashBoardDetailsTask",
               routeTemplate: "api/Opportunity/GetDashBoardDetailsTask",
               defaults: new { controller = "Opportunity", action = "GetDashBoardDetailsTask" }
           );

            config.Routes.MapHttpRoute(
               name: "GetDashBoardDetailsRemarks",
               routeTemplate: "api/Opportunity/GetDashBoardDetailsRemarks",
               defaults: new { controller = "Opportunity", action = "GetDashBoardDetailsRemarks" }
           );

            config.Routes.MapHttpRoute(
               name: "GetDashBoardDetailsWorkflowId",
               routeTemplate: "api/Opportunity/GetDashBoardDetailsWorkflowId",
               defaults: new { controller = "Opportunity", action = "GetDashBoardDetailsWorkflowId" }
           );

            config.Routes.MapHttpRoute(
               name: "GetExpiryDashBoardDetail",
               routeTemplate: "api/Opportunity/GetExpiryDashBoardDetail/{opportunityId}/{userId}/{userName}",
               defaults: new { controller = "Opportunity", action = "GetExpiryDashBoardDetail" }
           );
        }

        private static void TaskOperationRegister(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
               name: "GetModuleWiseUserTasksCountByModuleName",
               routeTemplate: "api/Task/GetModuleWiseUserTasksCountByModuleName/{userId}/{userName}/{moduleName}/{reloadTasks}",
               defaults: new { controller = "Task", action = "GetModuleWiseUserTasksCountByModuleName" }
           );
            config.Routes.MapHttpRoute(
               name: "GetModuleWiseUserTasksCountByModuleId",
               routeTemplate: "api/Task/GetModuleWiseUserTasksCountByModuleId/{userId}/{userName}/{moduleId}/{reloadTasks}",
               defaults: new { controller = "Task", action = "GetModuleWiseUserTasksCountByModuleId" }
           );
            config.Routes.MapHttpRoute(
               name: "GetModuleWiseUserTasksCountByModuleIdAndTaskId",
               routeTemplate: "api/Task/GetModuleWiseUserTasksCountByModuleIdAndTaskId/{userId}/{userName}/{moduleId}/{taskId}/{reloadTasks}",
               defaults: new { controller = "Task", action = "GetModuleWiseUserTasksCountByModuleIdAndTaskId" }
           );
            config.Routes.MapHttpRoute(
               name: "GetModuleWiseUserTasksCountByTaskName",
               routeTemplate: "api/Task/GetModuleWiseUserTasksCountByTaskName/{userId}/{userName}/{taskName}/{reloadTasks}",
               defaults: new { controller = "Task", action = "GetModuleWiseUserTasksCountByTaskName" }
           );
            config.Routes.MapHttpRoute(
               name: "GetModuleWiseUserTasksCount",
               routeTemplate: "api/Task/GetModuleWiseUserTasksCount/{userId}/{userName}/{reloadTasks}",
               defaults: new { controller = "Task", action = "GetModuleWiseUserTasksCount" }
           );
            config.Routes.MapHttpRoute(
               name: "GetUserTasks",
               routeTemplate: "api/Task/GetUserTasks/{userId}/{userName}/{reloadTasks}",
               defaults: new { controller = "Task", action = "GetUserTasks" }
           );
            config.Routes.MapHttpRoute(
               name: "GetTaskForUser",
               routeTemplate: "api/Task/GetTaskForUser/{userId}",
               defaults: new { controller = "Task", action = "GetTaskForUser" }
           );
            config.Routes.MapHttpRoute(
               name: "GetUserModuleId",
               routeTemplate: "api/Task/GetUserModuleId/{userId}",
               defaults: new { controller = "Task", action = "GetUserModuleId" }
           );
            config.Routes.MapHttpRoute(
               name: "GetDummyTaskList",
               routeTemplate: "api/Task/GetDummyTaskList/{userId}/{moduleId}/{taskId}/{cycle}/{allocation}",
               defaults: new { controller = "Task", action = "GetDummyTaskList" }
           );
            config.Routes.MapHttpRoute(
               name: "GetUserExpireDetails",
               routeTemplate: "api/Task/GetUserExpireDetails/{userId}",
               defaults: new { controller = "Task", action = "GetUserExpireDetails" }
           );
            config.Routes.MapHttpRoute(
               name: "GetTabSelectionValue",
               routeTemplate: "api/Task/GetTabSelectionValue/{userId}",
               defaults: new { controller = "Task", action = "GetTabSelectionValue" }
           );
            config.Routes.MapHttpRoute(
               name: "GetNewTaskList",
               routeTemplate: "api/Task/GetNewTaskList/{userId}/{moduleId}/{taskId}/{cycle}/{allocation}",
               defaults: new { controller = "Task", action = "GetNewTaskList" }
           );
            config.Routes.MapHttpRoute(
               name: "BackToTaskBoard",
               routeTemplate: "api/Task/BackToTaskBoard/{userId}/{Id}/{moduleId}/{taskId}/{cycle}",
               defaults: new { controller = "Task", action = "BackToTaskBoard" }
           );
        }

        private static void WebWatcherOperationRegister(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
               name: "GetFundingBodyCountByOrgDbId",
               routeTemplate: "api/WebWatcher/GetFundingBodyCountByOrgDbId/{OrgDbId}",
               defaults: new { controller = "WebWatcher", action = "GetFundingBodyCountByOrgDbId" }
           );

            config.Routes.MapHttpRoute(
               name: "InsertFundingUrls",
               routeTemplate: "api/WebWatcher/InsertFundingUrls/{OrgDbId}/{UserId}",
               defaults: new { controller = "WebWatcher", action = "InsertFundingUrls" }
           );

            config.Routes.MapHttpRoute(
               name: "GetFundingbodyMasters",
               routeTemplate: "api/WebWatcher/GetFundingbodyMasters",
               defaults: new { controller = "WebWatcher", action = "GetFundingbodyMasters" }
           );

            config.Routes.MapHttpRoute(
               name: "GetFundingForLevel2",
               routeTemplate: "api/WebWatcher/GetFundingForLevel2",
               defaults: new { controller = "WebWatcher", action = "GetFundingForLevel2" }
           );

            config.Routes.MapHttpRoute(
               name: "GetExportUrl",
               routeTemplate: "api/WebWatcher/GetExportUrl/{fundingId}/{batchId}",
               defaults: new { controller = "WebWatcher", action = "GetExportUrl" }
           );

            config.Routes.MapHttpRoute(
               name: "GetUrlDetail",
               routeTemplate: "api/WebWatcher/GetUrlDetail/{fundingId}/{id}/{moduleId}/{batch}",
               defaults: new { controller = "WebWatcher", action = "GetUrlDetail" }
           );

            config.Routes.MapHttpRoute(
               name: "GetUrlDetailAndCount",
               routeTemplate: "api/WebWatcher/GetUrlDetailAndCount",
               defaults: new { controller = "WebWatcher", action = "GetUrlDetailAndCount" }
           );

            config.Routes.MapHttpRoute(
               name: "UnGrouping",
               routeTemplate: "api/WebWatcher/UnGrouping/{id}/{fundingId}/{urlIds}/{userId}/{moduleId}/{batchId}",
               defaults: new { controller = "WebWatcher", action = "UnGrouping" }
           );

            config.Routes.MapHttpRoute(
               name: "GetUrlList",
               routeTemplate: "api/WebWatcher/GetUrlList/{fundingId}/{batch}",
               defaults: new { controller = "WebWatcher", action = "GetUrlList" }
           );

            config.Routes.MapHttpRoute(
               name: "DeleteAndRetainAll",
               routeTemplate: "api/WebWatcher/DeleteAndRetainAll/{fundingId}/{moduleId}/{userId}/{mode}",
               defaults: new { controller = "WebWatcher", action = "DeleteAndRetainAll" }
           );

            config.Routes.MapHttpRoute(
               name: "DeleteAndRetainUrl",
               routeTemplate: "api/WebWatcher/DeleteAndRetainUrl/{fundingId}/{moduleId}/{mode}/{urlId}/{userId}",
               defaults: new { controller = "WebWatcher", action = "DeleteAndRetainUrl" }
           );

            config.Routes.MapHttpRoute(
               name: "GetFundingBodyList",
               routeTemplate: "api/WebWatcher/GetFundingBodyList",
               defaults: new { controller = "WebWatcher", action = "GetFundingBodyList" }
           );

            config.Routes.MapHttpRoute(
               name: "GetUrlForGroup",
               routeTemplate: "api/WebWatcher/GetUrlForGroup{fundingId}/{moduleId}/{batchId}",
               defaults: new { controller = "WebWatcher", action = "GetUrlForGroup" }
           );

            config.Routes.MapHttpRoute(
               name: "GetUrlGroupDetail",
               routeTemplate: "api/WebWatcher/GetUrlGroupDetail",
               defaults: new { controller = "WebWatcher", action = "GetUrlGroupDetail" }
           );

            config.Routes.MapHttpRoute(
               name: "Grouping",
               routeTemplate: "api/WebWatcher/Grouping/{fundingId}/{moduleId}/{urlId}/{userId}/{batchId}",
               defaults: new { controller = "WebWatcher", action = "Grouping" }
           );

            config.Routes.MapHttpRoute(
               name: "DeleteUrl",
               routeTemplate: "api/WebWatcher/DeleteUrl/{moduleId}/{urlId}/{userId}",
               defaults: new { controller = "WebWatcher", action = "DeleteUrl" }
           );

            config.Routes.MapHttpRoute(
               name: "UrlUngrouping",
               routeTemplate: "api/WebWatcher/UrlUngrouping/{groupId}/{orgDbId}/{batchId}/{moduleId}/{urlNumber}",
               defaults: new { controller = "WebWatcher", action = "UrlUngrouping" }
           );
        }

        private static void XmlOperationRegister(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
               name: "GetXmlGenrationLimit",
               routeTemplate: "api/Xml/GetXmlGenrationLimit/{limits}/{select}",
               defaults: new { controller = "Xml", action = "GetXmlGenrationLimit" }
           );
        }
    }
}
