using System;
using System.Web.Http;
using MySqlDal;

namespace ScivalWebApi.Controllers
{
    public class XmlController : ApiController
    {
        [HttpGet]
        [Route("api/Xml/GetXmlGenrationLimit/{limits}/{select}")]
        public string GetXmlGenrationLimit(Int64 limits, Int64 select)
        {
            return XmlDataOperations.GetXmlGenrationLimit(limits, select);
        }
    }
}
