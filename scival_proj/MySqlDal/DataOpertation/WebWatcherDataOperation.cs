using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace MySqlDal
{
    public static class WebWatcherDataOperation
    {
        private static ScivalEntities ScivalEntities { get { return ScivalEntitiesInstance.GetInstance(); } }

        static List<UrlDetailAndCount> leftUrlList = null;
        static List<UrlGroupDetail> urlGroupDetail = null;

        public static Int32 GetFundingBodyCountByOrgDbId(Int64 OrgDbId)
        {
            return ScivalEntities.fundingbodies.Where(fb => fb.ORGDBID == OrgDbId).Count();
        }

        public static void InsertFundingUrls(Int64 OrgDbId, Int64 UserId)
        {
            ScivalEntities.Database.SqlQuery<UserFunding>("CALL sci_fundingbodyurlinsert(@param1,@param2)",
                new MySqlParameter("param1", OrgDbId), new MySqlParameter("param2", UserId))
                .ToList();
        }

        public static List<FundingBodyMaster> GetFundingbodyMasters()
        {
            return (from fb in ScivalEntities.fundingbody_master
                    orderby fb.FUNDINGBODYNAME
                    select new FundingBodyMaster
                    {
                        FundingBodyId = fb.FUNDINGBODY_ID,
                        FundingBodyName = fb.FUNDINGBODYNAME,
                        Batch = 1
                    }).ToList();
        }

        public static List<FundingBodyMaster> GetFundingForLevel2()
        {
            return (from fb in ScivalEntities.fundingbody_master
                    join gu in ScivalEntities.sci_groupurl on fb.FUNDINGBODY_ID equals gu.ID
                    select new { fb, gu } into t1
                    group t1 by new { t1.fb.FUNDINGBODY_ID, t1.fb.FUNDINGBODYNAME } into g
                    orderby g.Key.FUNDINGBODYNAME
                    select new FundingBodyMaster
                    {
                        FundingBodyId = g.Key.FUNDINGBODY_ID,
                        FundingBodyName = g.Key.FUNDINGBODYNAME,
                        Batch = g.Max(sgu => sgu.gu.BATCH.Value)
                    }).ToList();
        }

        public static List<String> GetExportUrl(Int64 fundingId, Int64 batchId)
        {
            var urlList1 = ScivalEntities.sci_urlretain.Where(ur => ur.ORGDBID == fundingId).Select(ur => ur.URL).ToList();

            var urlList2 = ScivalEntities.sci_urls.Where(ur => ur.MODULEID == 2 && ur.ID == fundingId && ur.ISACTIVE == "0").Select(ur => ur.URL).ToList();

            var urlList3 = (from ur in ScivalEntities.sci_urls
                            join op in ScivalEntities.opportunity_master on ur.ID equals op.OPPORTUNITYID
                            where ur.MODULEID == 3 && ur.ISACTIVE == "0" && op.FUNDINGBODYID == fundingId
                            select ur.URL)
                            .ToList();

            var urlList4 = (from ur in ScivalEntities.sci_urls
                            join aw in ScivalEntities.awards on ur.ID equals aw.AWARD_ID
                            where ur.MODULEID == 4 && ur.ISACTIVE == "0" && aw.FUNDINGBODY_ID == fundingId
                            select ur.URL)
                            .ToList();

            return urlList1.Union(urlList2).Union(urlList3).Union(urlList4).ToList();

        }

        public static List<UrlGroupDetail> GetUrlDetail(Int64 fundingId, Int64 id, Int64 moduleId, Int64 batch)
        {
            leftUrlList = null;

            var rightUrlList = (from sg in ScivalEntities.sci_groupurl
                                join sgu in ScivalEntities.sci_group_url_detail on sg.GROUP_ID equals sgu.GROUP_ID
                                join sur in ScivalEntities.sci_urlretain on sg.MODULEID equals sur.MODULEID
                                where sg.MODULEID == moduleId && sg.BATCH == sur.BATCH && sur.URLID == sgu.URL_NUMBER.Value.ToString()
                                && sg.BATCH == batch && sg.ID == id
                                select new UrlGroupDetail
                                {
                                    GroupId = sgu.GROUP_ID,
                                    Id = sg.ID,
                                    UrlNumber = sgu.URL_NUMBER,
                                    Url = sur.URL,
                                    ModuleId = sg.MODULEID,
                                    Batch = sg.BATCH
                                })
                                .ToList();

            leftUrlList = (from ur in ScivalEntities.sci_urlretain
                           where ur.ORGDBID == fundingId && ur.MODULEID == moduleId && ur.BATCH == batch
                           orderby ur.URL
                           select new UrlDetailAndCount
                           {
                               UrlId = ur.URLID,
                               Url = ur.URL,
                               Count = ur.GROUPCOUNTER
                           })
                               .ToList();

            return rightUrlList;
        }

        public static List<UrlDetailAndCount> GetUrlDetailAndCount()
        {
            return leftUrlList;
        }

        public static List<UrlDetailAndCount> UnGrouping(Int64 id, Int64 fundingId, List<String> urlIds, Int64 userId, Int64 moduleId, Int64 batchId)
        {
            var count = ScivalEntities.sci_workflow.Where(w => w.ID == id && w.TASKID == 2 && w.COMPLETEDDATE != null).Count();

            var group = ScivalEntities.sci_groupurl.Where(g => g.ID == id).Select(g => new { g.MODULEID, g.BATCH }).FirstOrDefault();

            if (count == 0)
            {
                if (urlIds.Count() > 0)
                {
                    var urlIdArray = ScivalEntities.sci_groupurl.Where(g => g.ID == id).Select(g => g.URLID).FirstOrDefault();
                    var urlIdString = Encoding.ASCII.GetString(urlIdArray);
                    var urlArray = urlIdString.Split(',');

                    var urlRetain = ScivalEntities.sci_urlretain.Where(u => urlArray.Contains(u.URLID) && u.MODULEID == moduleId && u.BATCH == batchId).ToList();

                    foreach (sci_urlretain url in urlRetain)
                        url.GROUPCOUNTER = url.GROUPCOUNTER - 1;

                    urlRetain = ScivalEntities.sci_urlretain.Where(u => urlIds.Contains(u.URLID) && u.MODULEID == moduleId && u.BATCH == batchId
                    && u.ORGDBID == fundingId).ToList();

                    foreach (sci_urlretain url in urlRetain)
                        url.GROUPCOUNTER = url.GROUPCOUNTER + 1;

                    var groupUrl = ScivalEntities.sci_groupurl.Where(g => g.ID == id).ToList();

                    string urlId = "";

                    foreach (string url in urlIds)
                        urlId = urlId + url + ",";

                    urlId = urlId.Substring(urlId.Length - 1, 1);

                    foreach (sci_groupurl groupurl in groupUrl)
                        groupurl.URLID = Encoding.ASCII.GetBytes(urlId);

                    ScivalEntities.SaveChanges();
                }
            }
            else
            {
                throw new ScivalDataException("Can not update due to collection completed.");
            }

            var leftUrlList = (from ur in ScivalEntities.sci_urlretain
                               where ur.ORGDBID == fundingId && ur.MODULEID == moduleId && ur.BATCH == batchId
                               orderby ur.URL
                               select new UrlDetailAndCount
                               {
                                   UrlId = ur.URLID,
                                   Url = ur.URL,
                                   Count = ur.GROUPCOUNTER
                               })
                               .ToList();

            return leftUrlList;
        }

        public static List<WebWatcherUrl> GetUrlList(Int64 fundingId, Int64 batch)
        {
            var urlList = ScivalEntities.sci_urlmaster.Where(u => u.ORGDBID == fundingId && u.STATUS == 0 && u.BATCH == batch)
                .OrderBy(u => u.URL)
                .Select(u => new WebWatcherUrl { Url = u.URL, UrlId = u.URLID })
                .ToList();

            return urlList;
        }

        public static void DeleteAndRetainAll(Int64 fundingId, Int64 moduleId, Int64 userId, Int64 mode)
        {
            var urlMasters = ScivalEntities.sci_urlmaster.Where(u => u.ORGDBID == fundingId && u.STATUS == 0).ToList();

            if (mode == 0)
            {
                foreach (sci_urlmaster urlmaster in urlMasters)
                {
                    urlmaster.STATUS = -1;
                    urlmaster.MODULEID = moduleId;
                    urlmaster.RETAIN_DELETEBY = userId;
                    urlmaster.RETAIN_DELETEDATE = DateTime.Today;
                }

                ScivalEntities.SaveChanges();
            }
            else if (mode == 1)
            {
                sci_urlmaster sci_Urlmaster = urlMasters.FirstOrDefault();

                sci_urlretain urlretain = new sci_urlretain
                {
                    ORGDBID = fundingId,
                    URLID = sci_Urlmaster.URLID.ToString(),
                    URL = sci_Urlmaster.URL,
                    MODULEID = moduleId,
                    BATCH = sci_Urlmaster.BATCH
                };

                ScivalEntities.sci_urlretain.Add(urlretain);

                foreach (sci_urlmaster urlmaster in urlMasters)
                {
                    urlmaster.STATUS = 1;
                    urlmaster.MODULEID = moduleId;
                    urlmaster.RETAIN_DELETEBY = userId;
                    urlmaster.RETAIN_DELETEDATE = DateTime.Today;
                }

                ScivalEntities.SaveChanges();
            }
        }

        public static List<WebWatcherUrl> DeleteAndRetainUrl(Int64 fundingId, Int64 moduleId, Int64 mode, Int32 urlId, Int64 userId)
        {
            var urlMasters = ScivalEntities.sci_urlmaster.Where(u => u.ORGDBID == fundingId && u.URLID == urlId).ToList();

            if (mode == 0)
            {
                foreach (sci_urlmaster urlmaster in urlMasters)
                {
                    urlmaster.STATUS = -1;
                    urlmaster.MODULEID = moduleId;
                    urlmaster.RETAIN_DELETEBY = userId;
                    urlmaster.RETAIN_DELETEDATE = DateTime.Today;
                }
            }
            else if (mode == 1)
            {
                foreach (sci_urlmaster urlmaster in urlMasters)
                {
                    urlmaster.STATUS = 1;
                    urlmaster.MODULEID = moduleId;
                    urlmaster.RETAIN_DELETEBY = userId;
                    urlmaster.RETAIN_DELETEDATE = DateTime.Today;
                }

                sci_urlmaster sci_Urlmaster = urlMasters.FirstOrDefault();

                if (moduleId == 0)
                {
                    sci_urlretain urlretain0 = new sci_urlretain
                    {
                        ORGDBID = fundingId,
                        URLID = sci_Urlmaster.URLID.ToString(),
                        URL = sci_Urlmaster.URL,
                        MODULEID = 3,
                        BATCH = sci_Urlmaster.BATCH
                    };

                    sci_urlretain urlretain1 = new sci_urlretain
                    {
                        ORGDBID = fundingId,
                        URLID = sci_Urlmaster.URLID.ToString(),
                        URL = sci_Urlmaster.URL,
                        MODULEID = 4,
                        BATCH = sci_Urlmaster.BATCH
                    };

                    ScivalEntities.sci_urlretain.Add(urlretain0);
                    ScivalEntities.sci_urlretain.Add(urlretain1);
                }
                else
                {
                    sci_urlretain urlretain0 = new sci_urlretain
                    {
                        ORGDBID = fundingId,
                        URLID = sci_Urlmaster.URLID.ToString(),
                        URL = sci_Urlmaster.URL,
                        MODULEID = moduleId,
                        BATCH = sci_Urlmaster.BATCH
                    };
                }
            }

            ScivalEntities.SaveChanges();

            var urlList = ScivalEntities.sci_urlmaster.Where(u => u.ORGDBID == fundingId && u.STATUS == 0)
                .OrderBy(u => u.URL)
                .Select(u => new WebWatcherUrl { Url = u.URL, UrlId = u.URLID })
                .Take(50)
                .ToList();

            return urlList;
        }

        public static List<FundingBodyMaster> GetFundingBodyList()
        {
            return (from fb in ScivalEntities.fundingbody_master
                    join um in ScivalEntities.sci_urlmaster on fb.FUNDINGBODY_ID equals um.ORGDBID
                    select new { fb, um } into t1
                    group t1 by new { t1.fb.FUNDINGBODY_ID, t1.fb.FUNDINGBODYNAME } into g
                    orderby g.Key.FUNDINGBODYNAME
                    select new FundingBodyMaster
                    {
                        FundingBodyId = g.Key.FUNDINGBODY_ID,
                        FundingBodyName = g.Key.FUNDINGBODYNAME,
                        Batch = g.Max(sgu => sgu.um.BATCH.Value)
                    }).ToList();
        }

        public static List<UrlDetailAndCount> GetUrlForGroup(Int64 fundingId, Int64 moduleId, Int64 batchId)
        {
            urlGroupDetail = null;

            var urlDetails = ScivalEntities.sci_urlretain.Where(u => u.ORGDBID == fundingId && u.MODULEID == moduleId && u.BATCH == batchId)
                .OrderBy(u => u.URL)
                .Select(u => new UrlDetailAndCount { Url = u.URL, UrlId = u.URLID, Count = u.GROUPCOUNTER })
                .ToList();

            urlGroupDetail = (from sg in ScivalEntities.sci_groupurl
                              join sgu in ScivalEntities.sci_group_url_detail on sg.GROUP_ID equals sgu.GROUP_ID
                              join sur in ScivalEntities.sci_urlretain on sg.MODULEID equals sur.MODULEID
                              where sg.MODULEID == moduleId && sg.BATCH == sur.BATCH && sur.URLID == sgu.URL_NUMBER.Value.ToString()
                              && sg.BATCH == batchId && sg.ID == fundingId
                              select new UrlGroupDetail
                              {
                                  GroupId = sgu.GROUP_ID,
                                  Id = sg.ID,
                                  UrlNumber = sgu.URL_NUMBER,
                                  Url = sur.URL,
                                  ModuleId = sg.MODULEID,
                                  Batch = sg.BATCH
                              })
                            .ToList();

            return urlDetails;
        }

        public static List<UrlGroupDetail> GetUrlGroupDetail()
        {
            return urlGroupDetail;
        }

        public static List<UrlDetailAndCount> Grouping(Int64 fundingId, Int64 moduleId, string urlId, Int64 userId, Int64 batchId)
        {
            urlGroupDetail = null;

            var urlArray = urlId.Split(',');
            var urlRetain = ScivalEntities.sci_urlretain.Where(u => urlArray.Contains(u.URLID) && u.MODULEID == moduleId && u.ORGDBID == fundingId).ToList();

            var groupCount = ScivalEntities.sci_groupurl.Where(g => g.ID == fundingId && g.BATCH == batchId && g.MODULEID == moduleId).Count();

            long? groupId;

            if (groupCount > 0)
                groupId = ScivalEntities.sci_groupurl.Where(g => g.ID == fundingId && g.BATCH == batchId && g.MODULEID == moduleId).Select(g => g.GROUP_ID).FirstOrDefault();
            else
                groupId = ScivalEntities.sci_groupurl.Max(g => g.GROUP_ID);

            sci_groupurl groupurl = new sci_groupurl
            {
                MODULEID = moduleId,
                ID = fundingId,
                CREATEDATE = DateTime.Now,
                CREATEDBY = userId,
                BATCH = batchId,
                GROUP_ID = groupId
            };

            ScivalEntities.sci_groupurl.Add(groupurl);

            for (int i = 0; i < urlArray.Length; i++)
            {
                sci_group_url_detail detail = new sci_group_url_detail
                {
                    GROUP_ID = groupId.Value,
                    URL_NUMBER = Convert.ToInt64(urlArray[i])
                };

                ScivalEntities.sci_group_url_detail.Add(detail);
            }

            var urlDetails = GetUrlForGroup(fundingId, moduleId, batchId);

            return urlDetails;
        }

        public static List<UrlDetailAndCount> DeleteUrl(Int64 moduleId, Int64 urlId, Int64 userId)
        {
            var urlCount = ScivalEntities.sci_urlretain.Where(u => u.URLID == urlId.ToString() && u.MODULEID == moduleId && u.GROUPCOUNTER == 0).Count();

            var urlRetain = ScivalEntities.sci_urlretain.Where(u => u.URLID == urlId.ToString() && u.MODULEID == moduleId).FirstOrDefault();

            if (urlCount > 0)
            {
                var urlMasters = ScivalEntities.sci_urlmaster.Where(u => u.ORGDBID == urlRetain.ORGDBID && u.URLID == urlId && u.BATCH == urlRetain.BATCH
                && u.MODULEID == moduleId).ToList();

                foreach (sci_urlmaster master in urlMasters)
                {
                    master.STATUS = -1;
                    master.LASTUPDATEDBY = userId;
                    master.LASTUPDATEDDATE = DateTime.Now;
                }

                var urlRetainList = ScivalEntities.sci_urlretain.Where(u => u.ORGDBID == urlRetain.ORGDBID && u.URLID == urlId.ToString()
                            && u.BATCH == urlRetain.BATCH && u.MODULEID == moduleId).ToList();

                ScivalEntities.sci_urlretain.RemoveRange(urlRetainList);

                ScivalEntities.SaveChanges();
            }

            var urlDetails = ScivalEntities.sci_urlretain.Where(u => u.ORGDBID == urlRetain.ORGDBID && u.MODULEID == moduleId && u.BATCH == urlRetain.BATCH)
               .OrderBy(u => u.URL)
               .Select(u => new UrlDetailAndCount { Url = u.URL, UrlId = u.URLID, Count = u.GROUPCOUNTER })
               .ToList();

            return urlDetails;
        }

        public static List<UrlDetailAndCount> UrlUngrouping(Int64 groupId, Int64 orgDbId, Int64 batchId, Int64 moduleId, Int64 urlNumber)
        {
            urlGroupDetail = null;

            var groupUrlList = ScivalEntities.sci_group_url_detail.Where(d => d.GROUP_ID == groupId && d.URL_NUMBER == urlNumber).ToList();

            ScivalEntities.sci_group_url_detail.RemoveRange(groupUrlList);

            var urlRetainList = ScivalEntities.sci_urlretain.Where(u => u.URLID == urlNumber.ToString()).ToList();

            foreach (sci_urlretain urlretain in urlRetainList)
                urlretain.GROUPCOUNTER = urlretain.GROUPCOUNTER - 1;

            ScivalEntities.SaveChanges();

            var urlDetails = ScivalEntities.sci_urlretain.Where(u => u.ORGDBID == orgDbId && u.MODULEID == moduleId && u.BATCH == batchId)
               .OrderBy(u => u.URL)
               .Select(u => new UrlDetailAndCount { Url = u.URL, UrlId = u.URLID, Count = u.GROUPCOUNTER })
               .ToList();

            urlGroupDetail = (from sg in ScivalEntities.sci_groupurl
                              join sgu in ScivalEntities.sci_group_url_detail on sg.GROUP_ID equals sgu.GROUP_ID
                              join sur in ScivalEntities.sci_urlretain on sg.MODULEID equals sur.MODULEID
                              where sg.MODULEID == moduleId && sg.BATCH == sur.BATCH && sur.URLID == sgu.URL_NUMBER.Value.ToString()
                              && sg.BATCH == batchId && sg.ID == orgDbId
                              select new UrlGroupDetail
                              {
                                  GroupId = sgu.GROUP_ID,
                                  Id = sg.ID,
                                  UrlNumber = sgu.URL_NUMBER,
                                  Url = sur.URL,
                                  ModuleId = sg.MODULEID,
                                  Batch = sg.BATCH
                              })
                            .ToList();

            return urlDetails;
        }
    }
}
