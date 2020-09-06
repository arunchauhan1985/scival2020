using System;
using System.Collections.Generic;
using System.Web.Http;
using MySqlDal;

namespace ScivalWebApi.Controllers
{
    public class AwardController : ApiController
    {
        [HttpGet]
        [Route("api/Award/GetAwardListsByTask/{fundingBodyId}/{taskId}/{updateFlag}/{userId}")]
        public List<AwardList> GetAwardListsByTask(Int64 fundingBodyId, Int64 taskId, Int32 updateFlag, Int64 userId)
        {
            return AwardDataOperations.GetAwardListsByTask(fundingBodyId, taskId, updateFlag, userId);
        }

        [HttpGet]
        [Route("api/Award/GetAwardWorkflowId/{fundingBodyId}/{userId}")]
        public Int64 GetAwardWorkflowId(Int64 fundingBodyId, Int64 userId)
        {
            return AwardDataOperations.GetAwardWorkflowId(fundingBodyId, userId);
        }

        [HttpGet]
        [Route("api/Award/GetDashBoardDetails/{userId}/{userName}/{workflowId}")]
        public List<DashboardUserFunding> GetDashBoardDetails(Int64 userId, string userName, Int64 workflowId)
        {
            return AwardDataOperations.GetDashBoardDetails(userId, userName, workflowId);
        }

        [HttpGet]
        [Route("api/Award/GetDashBoardDetailsUrl")]
        public string GetDashBoardDetailsUrl()
        {
            return AwardDataOperations.GetDashBoardDetailsUrl();
        }

        [HttpGet]
        [Route("api/Award/GetDashBoardDetailsDashboardTask")]
        public List<DashboardTask> GetDashBoardDetailsDashboardTask()
        {
            return AwardDataOperations.GetDashBoardDetailsDashboardTask();
        }

        [HttpGet]
        [Route("api/Award/GetDashBoardDetailsDashboardRemark")]
        public List<DashboardRemark> GetDashBoardDetailsDashboardRemark()
        {
            return AwardDataOperations.GetDashBoardDetailsDashboardRemark();
        }

        [HttpGet]
        [Route("api/Award/GetStartWork/{workId}/{userId}")]
        public List<startwork> GetStartWork(Int64 workId, Int64 userId)
        {
            return AwardDataOperations.GetStartWork(workId, userId);
        }

        [HttpGet]
        [Route("api/Award/GetLanguageMasterDetails/{LangLength}")]
        public List<sci_language_master> GetLanguageMasterDetails(int LangLength)
        {
            return AwardDataOperations.GetLanguageMasterDetails(LangLength);
        }

        [HttpGet]
        [Route("api/Award/GetProgress/{workId}")]
        public List<ProgressTable> GetProgress(int workId)
        {
            return AwardDataOperations.GetProgress(workId);
        }

        [HttpGet]
        [Route("api/Award/AddAndDeletePageURL/{workId}/{clickPage}/{url}/{userId}/{pagemode}")]
        public List<PageUrl> AddAndDeletePageURL(long workId, string clickPage, string url, long userId, int pagemode)
        {
            return AwardDataOperations.AddAndDeletePageURL(workId, clickPage, url, userId, pagemode);
        }

        [HttpGet]
        [Route("api/Award/GetURL/{workId}/{clickPage}")]
        public List<PageUrl> GetURL(Int64 workId, string clickPage)
        {
            return AwardDataOperations.GetURL(workId, clickPage);
        }
    }
}
