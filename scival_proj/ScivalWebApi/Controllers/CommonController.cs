using System.Net.Http;
using System.Web.Http;
using MySqlDal;

namespace ScivalWebApi.Controllers
{
    public class CommonController : ApiController
    {
        [Route("api/Common/GetDatabaseVersion")]
        [HttpGet]
        public string GetDatabaseVersion()
        {
            return CommonDataOperation.GetDatabaseVersion();
        }

        [HttpGet]
        [Route("api/Common/ValidateLogin/{username}/{password}")]
        public HttpResponseMessage ValidateLogin(string username, string password)
        {
            var isValidUser = CommonDataOperation.ValidateLogin(username, password, out string validationMessage, out sci_usermaster user);

            if (isValidUser)
                return Request.CreateResponse<sci_usermaster>(user);
            else
                return Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, new HttpError(validationMessage));
        }

        [Route("api/Common/GetExpireAlertsCount")]
        [HttpGet]
        public int GetExpireAlertsCount()
        {
            return CommonDataOperation.GetExpireAlertsCount();
        }

        [Route("api/Common/UpdateExpireAlert")]
        [HttpGet]
        public void UpdateExpireAlert()
        {
            CommonDataOperation.UpdateExpireAlert();
        }
    }
}
