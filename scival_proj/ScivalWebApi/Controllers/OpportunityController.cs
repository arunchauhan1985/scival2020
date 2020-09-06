using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MySqlDal;

namespace ScivalWebApi.Controllers
{
    public class OpportunityController : ApiController
    {
        [HttpGet]
        [Route("api/Opportunity/GetOpportunityListsByTask/{fundingBodyId}/{taskId}/{updateFlag}/{userId}")]
        public List<OpportunityList> GetOpportunityListsByTask(Int64 fundingBodyId, Int64 taskId, Int32 updateFlag, Int64 userId)
        {
            return OpportunityDataOperations.GetOpportunityListsByTask(fundingBodyId, taskId, updateFlag, userId);
        }

        [HttpGet]
        [Route("api/Opportunity/GetOpportunityWorkflowId/{fundingBodyId}/{userId}")]
        public Int64 GetOpportunityWorkflowId(Int64 fundingBodyId, Int64 userId) 
        {
            return OpportunityDataOperations.GetOpportunityWorkflowId(fundingBodyId, userId);
        }

        [HttpGet]
        [Route("api/Opportunity/GetDashBoardDetails/{userId}/{userName}/{workflowId}")]
        public List<DashboardUserFunding> GetDashBoardDetails(Int64 userId, string userName, Int64 workflowId)
        {
            return OpportunityDataOperations.GetDashBoardDetails(userId, userName, workflowId);
        }

        [HttpGet]
        [Route("api/Opportunity/GetDashBoardDetailsUrl")]
        public string GetDashBoardDetailsUrl()
        {
            return OpportunityDataOperations.GetDashBoardDetailsUrl();
        }

        [HttpGet]
        [Route("api/Opportunity/GetDashBoardDetailsTask")]
        public List<DashboardTask> GetDashBoardDetailsTask() 
        {
            return OpportunityDataOperations.GetDashBoardDetailsTask();
        }

        [HttpGet]
        [Route("api/Opportunity/GetDashBoardDetailsRemarks")]
        public List<DashboardRemark> GetDashBoardDetailsRemarks() 
        {
            return OpportunityDataOperations.GetDashBoardDetailsRemarks();
        }

        [HttpGet]
        [Route("api/Opportunity/GetDashBoardDetailsWorkflowId")]
        public Int64 GetDashBoardDetailsWorkflowId() 
        {
            return OpportunityDataOperations.GetDashBoardDetailsWorkflowId();
        }

        [HttpGet]
        [Route("api/Opportunity/GetExpiryDashBoardDetail/{opportunityId}/{userId}/{userName}")]
        public List<DashboardUserFunding> GetExpiryDashBoardDetail(Int64 opportunityId, Int64 userId, string userName) 
        {
            return OpportunityDataOperations.GetExpiryDashBoardDetail(opportunityId, userId, userName);
        }
    }
}
