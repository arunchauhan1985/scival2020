using System;
using System.Collections.Generic;
using System.Web.Http;
using MySqlDal;

namespace ScivalWebApi.Controllers
{
    public class WebWatcherController : ApiController
    {
        [HttpGet]
        [Route("api/WebWatcher/GetFundingBodyCountByOrgDbId/{OrgDbId}")]
        public Int32 GetFundingBodyCountByOrgDbId(Int64 OrgDbId)
        {
            return WebWatcherDataOperation.GetFundingBodyCountByOrgDbId(OrgDbId);
        }

        [HttpGet]
        [Route("api/WebWatcher/InsertFundingUrls/{OrgDbId}/{UserId}")]
        public void InsertFundingUrls(Int64 OrgDbId, Int64 UserId)
        {
            WebWatcherDataOperation.InsertFundingUrls(OrgDbId, UserId);
        }

        [HttpGet]
        [Route("api/WebWatcher/GetFundingbodyMasters")]
        public List<FundingBodyMaster> GetFundingbodyMasters()
        {
            return WebWatcherDataOperation.GetFundingbodyMasters();
        }

        [HttpGet]
        [Route("api/WebWatcher/GetFundingForLevel2")]
        public List<FundingBodyMaster> GetFundingForLevel2()
        {
            return WebWatcherDataOperation.GetFundingForLevel2();
        }

        [HttpGet]
        [Route("api/WebWatcher/GetExportUrl/{fundingId}/{batchId}")]
        public List<String> GetExportUrl(Int64 fundingId, Int64 batchId)
        {
            return WebWatcherDataOperation.GetExportUrl(fundingId, batchId);
        }

        [HttpGet]
        [Route("api/WebWatcher/GetUrlDetail/{fundingId}/{id}/{moduleId}/{batch}")]
        public List<UrlGroupDetail> GetUrlDetail(Int64 fundingId, Int64 id, Int64 moduleId, Int64 batch)
        {
            return WebWatcherDataOperation.GetUrlDetail(fundingId, id, moduleId, batch);
        }

        [HttpGet]
        [Route("api/WebWatcher/GetUrlDetailAndCount")]
        public List<UrlDetailAndCount> GetUrlDetailAndCount()
        {
            return WebWatcherDataOperation.GetUrlDetailAndCount();
        }

        [HttpGet]
        [Route("api/WebWatcher/UnGrouping/{id}/{fundingId}/{urlIds}/{userId}/{moduleId}/{batchId}")]
        public List<UrlDetailAndCount> UnGrouping(Int64 id, Int64 fundingId, List<String> urlIds, Int64 userId, Int64 moduleId, Int64 batchId)
        {
            return WebWatcherDataOperation.UnGrouping(id, fundingId, urlIds, userId, moduleId, batchId);
        }

        [HttpGet]
        [Route("api/WebWatcher/GetUrlList/{fundingId}/{batch}")]
        public List<WebWatcherUrl> GetUrlList(Int64 fundingId, Int64 batch)
        {
            return WebWatcherDataOperation.GetUrlList(fundingId, batch);
        }

        [HttpGet]
        [Route("api/WebWatcher/DeleteAndRetainAll/{fundingId}/{moduleId}/{userId}/{mode}")]
        public void DeleteAndRetainAll(Int64 fundingId, Int64 moduleId, Int64 userId, Int64 mode)
        {
            WebWatcherDataOperation.DeleteAndRetainAll(fundingId, moduleId, userId, mode);
        }

        [HttpGet]
        [Route("api/WebWatcher/DeleteAndRetainUrl/{fundingId}/{moduleId}/{mode}/{urlId}/{userId}")]
        public List<WebWatcherUrl> DeleteAndRetainUrl(Int64 fundingId, Int64 moduleId, Int64 mode, Int32 urlId, Int64 userId)
        {
            return WebWatcherDataOperation.DeleteAndRetainUrl(fundingId, moduleId, mode, urlId, userId);
        }

        [HttpGet]
        [Route("api/WebWatcher/GetFundingBodyList")]
        public List<FundingBodyMaster> GetFundingBodyList()
        {
            return WebWatcherDataOperation.GetFundingBodyList();
        }

        [HttpGet]
        [Route("api/WebWatcher/GetUrlForGroup{fundingId}/{moduleId}/{batchId}")]
        public List<UrlDetailAndCount> GetUrlForGroup(Int64 fundingId, Int64 moduleId, Int64 batchId)
        {
            return WebWatcherDataOperation.GetUrlForGroup(fundingId, moduleId, batchId);
        }

        [HttpGet]
        [Route("api/WebWatcher/GetUrlGroupDetail")]
        public List<UrlGroupDetail> GetUrlGroupDetail()
        {
            return WebWatcherDataOperation.GetUrlGroupDetail();
        }

        [HttpGet]
        [Route("api/WebWatcher/Grouping/{fundingId}/{moduleId}/{urlId}/{userId}/{batchId}")]
        public List<UrlDetailAndCount> Grouping(Int64 fundingId, Int64 moduleId, string urlId, Int64 userId, Int64 batchId)
        {
            return WebWatcherDataOperation.Grouping(fundingId, moduleId, urlId, userId, batchId);
        }

        [HttpGet]
        [Route("api/WebWatcher/DeleteUrl/{moduleId}/{urlId}/{userId}")]
        public List<UrlDetailAndCount> DeleteUrl(Int64 moduleId, Int64 urlId, Int64 userId)
        {
            return WebWatcherDataOperation.DeleteUrl(moduleId, urlId, userId);
        }

        [HttpGet]
        [Route("api/WebWatcher/UrlUngrouping/{groupId}/{orgDbId}/{batchId}/{moduleId}/{urlNumber}")]
        public List<UrlDetailAndCount> UrlUngrouping(Int64 groupId, Int64 orgDbId, Int64 batchId, Int64 moduleId, Int64 urlNumber)
        {
            return WebWatcherDataOperation.UrlUngrouping(groupId, orgDbId, batchId, moduleId, urlNumber);
        }
    }
}
