//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Threading.Tasks;
//using MySqlDal;

//namespace ScivalApiCall
//{
//    public static class WebWatcherApiCall
//    {
//        public static async Task<Int32> GetFundingBodyCountByOrgDbId(Int64 OrgDbId)
//        {
//            string requestUri = string.Format("api/WebWatcher/GetFundingBodyCountByOrgDbId/{0}", OrgDbId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<Int32>();
//            else
//                return 0;
//        }

//        public static async void InsertFundingUrls(Int64 OrgDbId, Int64 UserId)
//        {
//            string requestUri = string.Format("api/WebWatcher/InsertFundingUrls/{0}/{1}", OrgDbId, UserId);
//            await HttpClientInstance.GetCall(requestUri);
//        }

//        public static async Task<List<FundingBodyMaster>> GetFundingbodyMasters()
//        {
//            var response = HttpClientInstance.GetCall("api/WebWatcher/GetFundingbodyMasters");

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<FundingBodyMaster>>();
//            else
//                return null;
//        }

//        public static async Task<List<FundingBodyMaster>> GetFundingForLevel2()
//        {
//            var response = HttpClientInstance.GetCall("api/WebWatcher/GetFundingForLevel2");

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<FundingBodyMaster>>();
//            else
//                return null;
//        }

//        public static async Task<List<String>> GetExportUrl(Int64 fundingId, Int64 batchId)
//        {
//            string requestUri = string.Format("api/WebWatcher/GetExportUrl/{0}/{1}", fundingId, batchId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<String>>();
//            else
//                return null;
//        }

//        public static async Task<List<UrlGroupDetail>> GetUrlDetail(Int64 fundingId, Int64 id, Int64 moduleId, Int64 batch)
//        {
//            string requestUri = string.Format("api/WebWatcher/GetUrlDetail/{0}/{1}/{2}/{3}", fundingId, id, moduleId, batch);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<UrlGroupDetail>>();
//            else
//                return null;
//        }

//        public static async Task<List<UrlDetailAndCount>> GetUrlDetailAndCount()
//        {
//            var response = HttpClientInstance.GetCall("api/WebWatcher/GetUrlDetailAndCount");

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<UrlDetailAndCount>>();
//            else
//                return null;
//        }

//        public static async Task<List<UrlDetailAndCount>> UnGrouping(Int64 id, Int64 fundingId, List<String> urlIds, Int64 userId, Int64 moduleId, Int64 batchId)
//        {
//            string requestUri = string.Format("api/WebWatcher/UnGrouping/{0}/{1}/{2}/{3}/{4}/{5}", id, fundingId, urlIds, userId, moduleId, batchId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<UrlDetailAndCount>>();
//            else
//                return null;
//        }

//        public static async Task<List<WebWatcherUrl>> GetUrlList(Int64 fundingId, Int64 batch)
//        {
//            string requestUri = string.Format("api/WebWatcher/GetUrlList/{0}/{1}", fundingId, batch);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<WebWatcherUrl>>();
//            else
//                return null;
//        }

//        public static async void DeleteAndRetainAll(Int64 fundingId, Int64 moduleId, Int64 userId, Int64 mode)
//        {
//            string requestUri = string.Format("api/WebWatcher/DeleteAndRetainAll/{0}/{1}/{2}/{3}", fundingId, moduleId, userId, mode);
//            await HttpClientInstance.GetCall(requestUri);
//        }

//        public static async Task<List<WebWatcherUrl>> DeleteAndRetainUrl(Int64 fundingId, Int64 moduleId, Int64 mode, Int32 urlId, Int64 userId)
//        {
//            string requestUri = string.Format("api/WebWatcher/DeleteAndRetainUrl/{0}/{1}/{2}/{3}/{4}", fundingId, moduleId, mode, urlId, userId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<WebWatcherUrl>>();
//            else
//                return null;
//        }

//        public static async Task<List<FundingBodyMaster>> GetFundingBodyList()
//        {
//            var response = HttpClientInstance.GetCall("api/WebWatcher/GetFundingBodyList");

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<FundingBodyMaster>>();
//            else
//                return null;
//        }

//        public static async Task<List<UrlDetailAndCount>> GetUrlForGroup(Int64 fundingId, Int64 moduleId, Int64 batchId)
//        {
//            string requestUri = string.Format("api/WebWatcher/GetUrlForGroup{0}/{1}/{2}", fundingId, moduleId, batchId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<UrlDetailAndCount>>();
//            else
//                return null;
//        }

//        public static async Task<List<UrlGroupDetail>> GetUrlGroupDetail()
//        {
//            var response = HttpClientInstance.GetCall("api/WebWatcher/GetUrlGroupDetail");

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<UrlGroupDetail>>();
//            else
//                return null;
//        }

//        public static async Task<List<UrlDetailAndCount>> Grouping(Int64 fundingId, Int64 moduleId, string urlId, Int64 userId, Int64 batchId)
//        {
//            string requestUri = string.Format("api/WebWatcher/Grouping/{0}/{1}/{2}/{3}/{4}", fundingId, moduleId, urlId, userId, batchId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<UrlDetailAndCount>>();
//            else
//                return null;
//        }

//        public static async Task<List<UrlDetailAndCount>> DeleteUrl(Int64 moduleId, Int64 urlId, Int64 userId)
//        {
//            string requestUri = string.Format("api/WebWatcher/DeleteUrl/{0}/{1}/{2}", moduleId, urlId, userId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<UrlDetailAndCount>>();
//            else
//                return null;
//        }

//        public static async Task<List<UrlDetailAndCount>> UrlUngrouping(Int64 groupId, Int64 orgDbId, Int64 batchId, Int64 moduleId, Int64 urlNumber)
//        {
//            string requestUri = string.Format("api/WebWatcher/UrlUngrouping/{0}/{1}/{2}/{3}/{4}", groupId, orgDbId, batchId, moduleId, urlNumber);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<UrlDetailAndCount>>();
//            else
//                return null;
//        }
//    }
//}
