//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Threading.Tasks;
//using MySqlDal;

//namespace ScivalApiCall
//{
//    public static class AwardApiCall
//    {
//        public static string ErrorMessage
//        {
//            get
//            {
//                return HttpClientInstance.ErrorMessage;
//            }
//        }

//        public static async Task<List<AwardList>> GetAwardListsByTask(Int64 fundingBodyId, Int64 taskId, Int32 updateFlag, Int64 userId)
//        {
//            string requestUri = string.Format("api/Award/GetAwardListsByTask/{0}/{1}/{2}/{3}", fundingBodyId, taskId, updateFlag, userId);
//            var response = HttpClientInstance.GetCall(requestUri);
            
//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<AwardList>>();
//            else
//                return null;
//        }

//        public static async Task<Int64> GetAwardWorkflowId(Int64 fundingBodyId, Int64 userId)
//        {
//            string requestUri = string.Format("api/Award/GetAwardWorkflowId/{0}/{1}", fundingBodyId, userId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<Int64>();
//            else
//                return 0;
//        }

//        public static async Task<List<DashboardUserFunding>> GetDashBoardDetails(Int64 userId, string userName, Int64 workflowId)
//        {
//            string requestUri = string.Format("api/Award/GetDashBoardDetails/{0}/{1}/{2}", userId, userName, workflowId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<DashboardUserFunding>>();
//            else
//                return null;
//        }

//        public static async Task<string> GetDashBoardDetailsUrl()
//        {
//            var response = HttpClientInstance.GetCall("api/Award/GetDashBoardDetailsUrl");

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<string>();
//            else
//                return string.Empty;
//        }

//        public static async Task<List<DashboardTask>> GetDashBoardDetailsDashboardTask()
//        {
//            var response = HttpClientInstance.GetCall("api/Award/GetDashBoardDetailsDashboardTask");

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<DashboardTask>>();
//            else
//                return null;
//        }

//        public static async Task<List<DashboardRemark>> GetDashBoardDetailsDashboardRemark()
//        {
//            var response = HttpClientInstance.GetCall("api/Award/GetDashBoardDetailsDashboardRemark");

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<DashboardRemark>>();
//            else
//                return null;
//        }

//        public static async Task<List<startwork>> GetStartWork(long workId, long userId)
//        {
//            string requestUri = string.Format("api/Award/GetStartWork/{0}/{1}", workId, userId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<startwork>>();
//            else
//                return null;
//        }

//        public static async Task<List<sci_language_master>> GetLanguageMasterDetails(int LangLength)
//        {
//            string requestUri = string.Format("api/Award/GetLanguageMasterDetails/{0}", LangLength);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<sci_language_master>>();
//            else
//                return null;
//        }

//        public static async Task<List<ProgressTable>> GetProgress(long workId)
//        {
//            string requestUri = string.Format("api/Award/GetProgress/{0}", workId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<ProgressTable>>();
//            else
//                return null;
//        }

//        public static async Task<List<PageUrl>> AddAndDeletePageURL(long workId, string clickPage, string url, long uSERID, int pagemode)
//        {
//            string requestUri = string.Format("api/Award/AddAndDeletePageURL/{0}/{1}/{2}/{3}/{4}", workId, clickPage, url, uSERID, pagemode);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<PageUrl>>();
//            else
//                return null;
//        }
//        public static async Task<List<PageUrl>> GetURL(long workId, string clickPage)
//        {
//            string requestUri = string.Format("api/Award/GetURL/{0}/{1}", workId, clickPage);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<PageUrl>>();
//            else
//                return null;
//        }
//    }
//}
