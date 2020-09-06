using System;
using System.Collections.Generic;
using System.Web.Http;
using MySqlDal;

namespace ScivalWebApi.Controllers
{
    public class FundingBodyController : ApiController
    {
        [HttpGet]
        [Route("api/FundingBody/GetUserFundingLists/{userId}/{moduleId}")]
        public List<UserFunding> GetUserFundingLists(Int64 userId, Int64 moduleId)
        {
            return FundingBodyDataOperations.GetUserFundingLists(userId, moduleId);
        }

        [HttpGet]
        [Route("api/FundingBody/GetUserFundingListsByTask/{userId}/{moduleId}/{taskId}")]
        public List<UserFunding> GetUserFundingListsByTask(Int64 userId, Int64 moduleId, Int64 taskId)
        {
            return FundingBodyDataOperations.GetUserFundingListsByTask(userId, moduleId, taskId);
        }

        [HttpGet]
        [Route("api/FundingBody/GetDashBoardDetails/{userId}/{userName}/{id}/{moduleId}/{taskId}/{cycle}")]
        public List<DashboardUserFunding> GetDashBoardDetails(Int64 userId, string userName, Int64 id, Int64 moduleId, Int64 taskId, Int64 cycle)
        {
            return FundingBodyDataOperations.GetDashBoardDetails(userId, userName, id, moduleId, taskId, cycle);
        }

        [HttpGet]
        [Route("api/FundingBody/GetDashBoardDetailsTaskList")]
        public List<DashboardTask> GetDashBoardDetailsTaskList()
        {
            return FundingBodyDataOperations.GetDashBoardDetailsTaskList();
        }

        [HttpGet]
        [Route("api/FundingBody/GetDashBoardDetailsRemarkList")]
        public List<DashboardRemark> GetDashBoardDetailsRemarkList()
        {
            return FundingBodyDataOperations.GetDashBoardDetailsRemarkList();
        }

        [HttpGet]
        [Route("api/FundingBody/GetLanguageMasters/{languageLength}")]
        public List<sci_language_master> GetLanguageMasters(Int64 languageLength)
        {
            return DashboardDataOperations.GetLanguageMasters(languageLength);
        }
    }
}
