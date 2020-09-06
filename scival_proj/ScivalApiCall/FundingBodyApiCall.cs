//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Threading.Tasks;
//using MySqlDal;

//namespace ScivalApiCall
//{
//    public static class FundingBodyApiCall
//    {
//        public static async Task<List<UserFunding>> GetUserFundingLists(Int64 userId, Int64 moduleId)
//        {
//            string requestUri = string.Format("api/FundingBody/GetUserFundingLists/{0}/{1}", userId, moduleId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<UserFunding>>();
//            else
//                return null;
//        }

//        public static async Task<List<UserFunding>> GetUserFundingListsByTask(Int64 userId, Int64 moduleId, Int64 taskId)
//        {
//            string requestUri = string.Format("api/FundingBody/GetUserFundingListsByTask/{0}/{1}/{2}", userId, moduleId, taskId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<UserFunding>>();
//            else
//                return null;
//        }

//        public static async Task<List<DashboardUserFunding>> GetDashBoardDetails(Int64 userId, string userName, Int64 id, Int64 moduleId, Int64 taskId, Int64 cycle)
//        {
//            string requestUri = string.Format("api/FundingBody/GetDashBoardDetails/{0}/{1}/{2}/{3}/{4}/{5}", userId, userName, id, moduleId, taskId, cycle);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<DashboardUserFunding>>();
//            else
//                return null;
//        }
        
//        public static async Task<List<DashboardTask>> GetDashBoardDetailsTaskList()
//        {
//            var response = HttpClientInstance.GetCall("api/FundingBody/GetDashBoardDetailsTaskList");

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<DashboardTask>>();
//            else
//                return null;
//        }

//        public static async Task<List<DashboardRemark>> GetDashBoardDetailsRemarkList()
//        {
//            var response = HttpClientInstance.GetCall("api/FundingBody/GetDashBoardDetailsRemarkList");

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<DashboardRemark>>();
//            else
//                return null;
//        }

//        public static async Task<List<sci_language_master>> GetLanguageMasters(Int64 fundingBodyId)
//        {
//            string requestUri = string.Format("api/FundingBody/GetLanguageMasters/{0}", fundingBodyId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<sci_language_master>>();
//            else
//                return null;
//        }
//    }
//}
