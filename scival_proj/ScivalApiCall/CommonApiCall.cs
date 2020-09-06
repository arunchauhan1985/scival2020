//using System.Net.Http;
//using System.Threading.Tasks;
//using MySqlDal;
//using Newtonsoft.Json;

//namespace ScivalApiCall
//{
//    public static class CommonApiCall
//    {
//        public static string ErrorMessage
//        {
//            get
//            {
//                return HttpClientInstance.ErrorMessage;
//            }
//        }

//        public static async Task<string> GetDatabaseVersion()
//        {
//            var response = HttpClientInstance.GetCall("api/Common/GetDatabaseVersion");

//            if (response.Result != null)
//            {
//                string version = await response.Result.Content.ReadAsStringAsync();
//                return JsonConvert.DeserializeObject<string>(version);
//            }
//            else
//                return null;
//        }

//        public static async Task<sci_usermaster> ValidateLogin(string username, string password)
//        {
//            string requestUri = string.Format("api/Common/ValidateLogin/{0}/{1}", username, password);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<sci_usermaster>();
//            else
//                return null;
//        }

//        public static async Task<int> GetExpireAlertsCount()
//        {
//            var response = HttpClientInstance.GetCall("api/Common/GetExpireAlertsCount");

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<int>();
//            else
//                return 0;
//        }

//        public static async void UpdateExpireAlert()
//        {
//            await HttpClientInstance.GetCall("api/Common/UpdateExpireAlert");
//        }
//    }
//}
