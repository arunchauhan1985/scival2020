//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Threading.Tasks;
//using MySqlDal;

//namespace ScivalApiCall
//{
//    public static class OpportunityApiCall
//    {
//        public static async Task<List<OpportunityList>> GetOpportunityListsByTask(Int64 fundingBodyId, Int64 taskId, Int32 updateFlag, Int64 userId)
//        {
//            string requestUri = string.Format("api/Opportunity/GetOpportunityListsByTask/{0}/{1}/{2}/{3}", fundingBodyId, taskId, updateFlag, userId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<OpportunityList>>();
//            else
//                return null;
//        }

//        public static async Task<Int64> GetOpportunityWorkflowId(Int64 fundingBodyId, Int64 userId)
//        {
//            string requestUri = string.Format("api/Opportunity/GetOpportunityWorkflowId/{0}/{1}", fundingBodyId, userId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<Int64>();
//            else
//                return 0;
//        }

//        public static async Task<List<DashboardUserFunding>> GetDashBoardDetails(Int64 userId, string userName, Int64 workflowId)
//        {
//            string requestUri = string.Format("api/Opportunity/GetDashBoardDetails/{0}/{1}/{2}", userId, userName, workflowId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<DashboardUserFunding>>();
//            else
//                return null;
//        }

//        public static async Task<string> GetDashBoardDetailsUrl()
//        {
//            var response = HttpClientInstance.GetCall("api/Opportunity/GetDashBoardDetailsUrl");

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<string>();
//            else
//                return null;
//        }

//        public static async Task<List<DashboardTask>> GetDashBoardDetailsTask()
//        {
//            var response = HttpClientInstance.GetCall("api/Opportunity/GetDashBoardDetailsTask");

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<DashboardTask>>();
//            else
//                return null;
//        }

//        public static async Task<List<DashboardRemark>> GetDashBoardDetailsRemarks()
//        {
//            var response = HttpClientInstance.GetCall("api/Opportunity/GetDashBoardDetailsRemarks");

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<DashboardRemark>>();
//            else
//                return null;
//        }

//        public static async Task<Int64> GetDashBoardDetailsWorkflowId()
//        {
//            var response = HttpClientInstance.GetCall("api/Opportunity/GetDashBoardDetailsWorkflowId");

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<Int64>();
//            else
//                return 0;
//        }

//        public static async Task<List<DashboardUserFunding>> GetExpiryDashBoardDetail(Int64 opportunityId, Int64 userId, string userName)
//        {
//            string requestUri = string.Format("api/Opportunity/GetExpiryDashBoardDetail/{0}/{1}/{2}", opportunityId, userId, userName);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<DashboardUserFunding>>();
//            else
//                return null;
//        }
//    }
//}
