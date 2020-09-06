//using System;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace ScivalApiCall
//{
//    public static class XmlApiCall
//    {
//        public static async Task<string> GetXmlGenrationLimit(Int64 limits, Int64 select)
//        {
//            string requestUri = string.Format("api/Xml/GetXmlGenrationLimit/{0}/{1}", limits, select);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<string>();
//            else
//                return null;
//        }
//    }
//}
