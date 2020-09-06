//using System;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;
//using Newtonsoft.Json.Linq;

//namespace ScivalApiCall
//{
//    public static class HttpClientInstance
//    {
//        public static string ErrorMessage { get; set; }
//        private static HttpClient httpClient = null;
//        private static string apiLink = string.Empty;

//        public static bool InitializeHttpClient(string apiUrl)
//        {
//            apiLink = apiUrl;

//            if (ApiInstance != null)
//                return true;
//            else
//                return false;
//        }

//        private static HttpClient ApiInstance
//        {
//            get
//            {
//                if (httpClient == null)
//                {
//                    if (string.IsNullOrEmpty(apiLink))
//                        throw new Exception("Api Link not set");

//                    httpClient = new HttpClient
//                    {
//                        BaseAddress = new Uri(apiLink)
//                    };

//                    httpClient.DefaultRequestHeaders.Accept.Clear();
//                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//                }

//                return httpClient;
//            }
//        }

//        internal static async Task<HttpResponseMessage> GetCall(string requestUri)
//        {
//            ErrorMessage = String.Empty;

//            HttpResponseMessage response = await ApiInstance.GetAsync(requestUri).ConfigureAwait(false);

//            if (response.IsSuccessStatusCode)
//                return response;
//            else
//            {
//                string message = response.Content.ReadAsStringAsync().Result;

//                var parsedJson = JObject.Parse(message);
//                ErrorMessage = Convert.ToString(parsedJson["Message"]);

//                return null;
//            }
//        }
//    }
//}
