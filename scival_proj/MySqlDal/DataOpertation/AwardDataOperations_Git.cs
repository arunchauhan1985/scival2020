using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;

namespace MySqlDal
{
    public static class AwardDataOperations
    {
        static Replace r = new Replace();
        static string getDashBoardDetailsUrl = string.Empty;
        static List<DashboardTask> getDashBoardDetailsDashboardTasks = null;
        static List<DashboardRemark> getDashBoardDetailsRemarks = null;

        private static ScivalEntities ScivalEntities { get { return ScivalEntitiesInstance.GetInstance(); } }

        public static List<AwardList> GetAwardListsByTask(Int64 fundingBodyId, Int64 taskId, Int32 updateFlag, Int64 userId)
        {
            var awardList = ScivalEntities.Database.SqlQuery<AwardList>("sci_aw_awexist @p_fundingbodyid, @p_taskid, @p_updateflag, @p_userid",
                new SqlParameter("p_fundingbodyid", fundingBodyId), new SqlParameter("p_taskid", taskId), new SqlParameter("p_updateflag", updateFlag), new SqlParameter("p_userid", userId))
                .ToList();

            return awardList;
        }

        public static Int64 GetAwardWorkflowId(Int64 fundingBodyId, Int64 userId)
        {
            var count = (from am in ScivalEntities.award_master
                         join sw in ScivalEntities.sci_workflow on am.AWARDID equals sw.ID
                         where am.FUNDINGBODYID == fundingBodyId && am.AWARDNAME == null && sw.TASKID == 1 && sw.STATUSID == null
                         select new { am.AWARDID })
                         .Count();

            Int64 awardId, workflowId = 0;

            if (count > 0)
            {
                var award = (from om in ScivalEntities.award_master
                             join sw in ScivalEntities.sci_workflow on om.AWARDID equals sw.ID
                             where om.FUNDINGBODYID == fundingBodyId && om.AWARDNAME == null && sw.TASKID == 1 && sw.STATUSID == null
                             select new { om.AWARDID, sw.WORKFLOWID })
                                   .FirstOrDefault();

                awardId = award.AWARDID;
                workflowId = award.WORKFLOWID;

                List<sci_workflow> workflows = ScivalEntities.sci_workflow.Where(wf => wf.ID == workflowId && wf.STATUSID == null).ToList();

                foreach (sci_workflow workflow in workflows)
                {
                    workflow.STARTBY = userId;
                    workflow.STARTDATE = DateTime.Today;
                    workflow.STATUSID = 7;
                }

                ScivalEntities.SaveChanges();
            }
            else
            {
                var maxAwardId = ScivalEntities.award_master.Max(op => op.AWARDID);
                maxAwardId = maxAwardId + 1;

                award_master award = new award_master { AWARDID = maxAwardId, FUNDINGBODYID = fundingBodyId, CREATEDBY = userId, CYCLE = 0, STATUSCODE = 1 };

                ScivalEntities.award_master.Add(award);
                ScivalEntities.SaveChanges();

                var templateId = ScivalEntities.sci_defaulttemplate.Where(t => t.ACTIVE == 1 && t.MODULEID == 4).Select(t => t.TEMPLATEID).FirstOrDefault();
                var workflowTemplates = ScivalEntities.sci_workflowtemplate.Where(w => w.TEMPLATEID == templateId).ToList();

                var workId = ScivalEntities.sci_workflow.Max(wf => wf.WORKFLOWID);

                foreach (sci_workflowtemplate template in workflowTemplates)
                {
                    sci_workflow workflow;
                    workId = workId + 1;

                    if (template.TASKID == 1)
                    {
                        workflow = new sci_workflow
                        {
                            WORKFLOWID = workId,
                            MODULEID = 4,
                            ID = maxAwardId,
                            CYCLE = 0,
                            TEMPLATEID = templateId,
                            TASKID = template.TASKID,
                            SEQUENCE = template.SEQUENCE,
                            STARTDATE = DateTime.Today,
                            STARTBY = userId,
                            STATUSID = 7
                        };
                    }
                    else
                    {
                        workflow = new sci_workflow
                        {
                            WORKFLOWID = workId,
                            MODULEID = 4,
                            ID = maxAwardId,
                            CYCLE = 0,
                            TEMPLATEID = templateId,
                            TASKID = template.TASKID,
                            SEQUENCE = template.SEQUENCE
                        };
                    }

                    ScivalEntities.sci_workflow.Add(workflow);
                }

                ScivalEntities.SaveChanges();
            }

            return workflowId;
        }
        public static List<DashboardUserFunding> GetDashBoardDetails(Int64 userId, string userName, Int64 workflowId)
        {
            getDashBoardDetailsUrl = string.Empty;
            getDashBoardDetailsDashboardTasks = null;
            getDashBoardDetailsRemarks = null;

            List<DashboardUserFunding> dashboardUserFunding = null;

            var statusIds = new List<Int64> { 8, 4 };
            var count = ScivalEntities.sci_workflow.Where(wf => wf.WORKFLOWID == workflowId && statusIds.Contains(wf.STATUSID.Value)).Count();

            if (count > 0)
                throw new ScivalDataException("Critical Error.");

            var workflow = ScivalEntities.sci_workflow.Where(wf => wf.WORKFLOWID == workflowId).FirstOrDefault();

            count = ScivalEntities.sci_workflow.Where(wf => wf.ID == workflow.ID && wf.MODULEID == workflow.MODULEID && wf.TASKID == workflow.TASKID
                                                && wf.CYCLE == workflow.CYCLE && wf.STATUSID.Value == 7 && wf.STARTBY == userId).Count();

            List<sci_workflow> workflows = null;

            if (count == 0)
                workflows = ScivalEntities.sci_workflow.Where(wf => wf.ID == workflow.ID && wf.MODULEID == workflow.MODULEID && wf.TASKID == workflow.TASKID
                && wf.CYCLE == workflow.CYCLE && wf.STATUSID.Value != 7).ToList();
            else
                workflows = ScivalEntities.sci_workflow.Where(wf => wf.ID == workflow.ID && wf.MODULEID == workflow.MODULEID && wf.TASKID == workflow.TASKID
                && wf.CYCLE == workflow.CYCLE).ToList();

            foreach (sci_workflow sci_Workflow in workflows)
            {
                sci_Workflow.STARTBY = userId;
                sci_Workflow.STARTDATE = DateTime.Today;
                sci_Workflow.STATUSID = 7;
            }

            var rowCount = ScivalEntities.SaveChanges();

            if (count == 0 && rowCount == 0)
                throw new ScivalDataException("Oops! Task Already Started.");

            var fundingBodyId = ScivalEntities.award_master.Where(o => o.AWARDID == workflow.ID).Select(o => o.FUNDINGBODYID).FirstOrDefault();

            getDashBoardDetailsUrl = ScivalEntities.fundingbody_master.Where(f => f.FUNDINGBODY_ID == fundingBodyId).Select(f => f.URL).FirstOrDefault();

            if (workflow.MODULEID == 4)
            {
                dashboardUserFunding = (from ab in ScivalEntities.awards where ab.AWARD_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Award", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.amounts where ab.AWARD_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Award Amount", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.classificationgroups where ab.AWARD_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Classification Group", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.awardees where ab.AWARD_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Awardees", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.awardmanagers where ab.AWARD_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Award Managers", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.relatedprograms where ab.AWARD_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Related Programs", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.relatedorgs where ab.AWARD_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Related Funding Bodies", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.sci_scholarly_output where ab.AWARD_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Research Outcome", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.award_location where Convert.ToInt64(ab.AWARD_ID) == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Award Location", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.relateditems where ab.AWARD_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Related Items", Count = a.Count() })
                    .ToList();

                getDashBoardDetailsDashboardTasks = (from sw in ScivalEntities.sci_workflowtemplate
                                                     join st in ScivalEntities.sci_tasks on sw.TASKID equals st.TASKID
                                                     where sw.TEMPLATEID == 2
                                                     orderby sw.SEQUENCE
                                                     select new DashboardTask { TaskName = st.TASKNAME, TaskId = st.TASKID, Sequence = sw.SEQUENCE.Value })
                             .ToList();

                getDashBoardDetailsRemarks = (from str in ScivalEntities.sci_timesheetremarks
                                              join sw in ScivalEntities.sci_workflow on str.WORKFLOWID equals sw.WORKFLOWID
                                              where sw.MODULEID == workflow.MODULEID && sw.ID == workflow.ID && sw.CYCLE == workflow.CYCLE
                                              orderby str.CREATEDDATE
                                              select new DashboardRemark
                                              {
                                                  Remark = str.REMARKS,
                                                  Task = (sw.TASKID == 1) ? "Collection" : "Quality Check",
                                                  UserName = userName,
                                                  CreatedDate = str.CREATEDDATE.Value,
                                                  CreatedDate1 = str.CREATEDDATE.Value.ToString("DD-Mon-YYYY HH:MI AM")
                                              }
                    )
                    .ToList();

            }

            return dashboardUserFunding;
        }


        public static string GetDashBoardDetailsUrl()
        {
            return getDashBoardDetailsUrl;
        }

        public static List<DashboardTask> GetDashBoardDetailsDashboardTask()
        {
            return getDashBoardDetailsDashboardTasks;
        }

        public static List<DashboardRemark> GetDashBoardDetailsDashboardRemark()
        {
            return getDashBoardDetailsRemarks;
        }

        public static List<startwork> GetStartWork(Int64 workflowId, Int64 userId)
        {
            var startworkdtls = ScivalEntities.Database.SqlQuery<startwork>("sci_inserttimesheet41 @p_workflowid,@p_userid",
                new SqlParameter("p_workflowid", workflowId), new SqlParameter("p_userid", userId)).ToList();

            return startworkdtls;
        }

        public static List<ProgressTable> GetProgress(Int64 workId)
        {
            var ProgressDtls = ScivalEntities.Database.SqlQuery<ProgressTable>("SCI_PROC_AWARDPROGRESS @p_workflowid",
                new SqlParameter("p_workflowid", workId)).ToList();

            return ProgressDtls;
        }

        public static List<sci_language_master> GetLanguageMasterDetails(int LangLength)
        {
            return ScivalEntities.sci_language_master.Where(lM => lM.CODE_LENGTH == LangLength).OrderBy(lm => lm.LANGUAGE_NAME).ToList();
        }

        public static List<PageUrl> AddAndDeletePageURL(Int64 workId, string clickPage, string url, Int64 userId, int pagemode)
        {
            var ProgressDtls = ScivalEntities.Database.SqlQuery<PageUrl>("sci_pageurls @p_workflowid,@p_pagename,@p_url,@p_userid,@p_mode",
                new SqlParameter("p_workflowid", workId), new SqlParameter("p_pagename", clickPage), new SqlParameter("p_url", url), new SqlParameter("p_userid", userId), new SqlParameter("p_mode", pagemode)).ToList();

            return ProgressDtls;
        }


        public static List<PageUrl> GetURL(long workId, string clickPage)
        {
            var ProgressDtls = ScivalEntities.Database.SqlQuery<PageUrl>("sci_urllinks @p_workflowid,@p_pagename",
                new SqlParameter("p_workflowid", workId), new SqlParameter("p_pagename", clickPage)).ToList();

            return ProgressDtls;
        }


        public static DataSet GetScholarlyOutput(long wfId)
        {
            var ScholarOutput = ScivalEntities.Database.SqlQuery<DataSet>("sci_aw_research_output @p_workflowid",
               new SqlParameter("p_workflowid", wfId)).FirstOrDefault();
            ScholarOutput.Tables[0].TableName = "DisplayData";
            ScholarOutput.Tables[1].TableName = "RelType";
            ScholarOutput.Tables[2].TableName = "Type";
            ScholarOutput.Tables[3].TableName = "Item";
            return ScholarOutput;
        }

        public static DataSet SaveAbdUpdateScholarlyOutputType_new(Int64 WFId, Int64 mode, Int64 Id, string p_Relation_type, string p_item_type, string p_Doi,
            string p_Pubmed, string p_Pmc, string p_Medline, string p_Scopus, string p_Item)
        {
            var saveScholarOutputType = ScivalEntities.Database.SqlQuery<DataSet>("sci_aw_research_out_insupdel @p_workflowid, @p_insdel,@p_id,@p_Relation_type,@p_item_type,@p_Doi," +
                "@p_Pubmed,@p_Pmc,@p_Medline,@p_Scopus,@p_Item",
                  new SqlParameter("p_workflowid", WFId),
                  new SqlParameter("p_insdel", mode),
                  new SqlParameter("p_id", Id),
                  new SqlParameter("p_Relation_type", p_Relation_type),
                  new SqlParameter("p_item_type", p_item_type),
                  new SqlParameter("p_Doi", p_Doi),
                  new SqlParameter("p_Pubmed", p_Pubmed),
                  new SqlParameter("p_Pmc", p_Pmc),
                  new SqlParameter("p_Medline", p_Medline),
                  new SqlParameter("p_Scopus", p_Scopus),
                  new SqlParameter("p_Item", p_Item)
                  ).FirstOrDefault();

            saveScholarOutputType.Tables[0].TableName = "ResearchOutcome";
            return saveScholarOutputType;
        }
        public static DataSet TimeSheetStopContinueForQC(long wFId, long userId, long transId, long PageIds, string remarkText)
        {
            var saveScholarOutputType = ScivalEntities.Database.SqlQuery<DataSet>("sci_timesheetstopcontinue_QA @p_workflowid, @p_userid,@p_TRANSITIONALID,@p_REMARKS",
                  new SqlParameter("p_workflowid", wFId),
                  new SqlParameter("p_userid", userId),
                  new SqlParameter("p_TRANSITIONALID", transId),
                  new SqlParameter("p_REMARKS", remarkText)
                  ).FirstOrDefault();

            saveScholarOutputType.Tables[0].TableName = "Result";
            return saveScholarOutputType;
        }


        public static DataSet TimeSheetStopContinue(long wFId, long userId, long transId, long mode, string remarkText)
        {
            return ScivalEntities.Database.SqlQuery<DataSet>("sci_timesheetstoP_CONTINUE @p_workflowid, @p_userid,@p_TRANSITIONALID,@p_REMARKS",
                new SqlParameter("p_workflowid", wFId),
                new SqlParameter("p_userid", userId),
                new SqlParameter("p_TRANSITIONALID", transId),
                new SqlParameter("p_REMARKS", remarkText)
                ).FirstOrDefault();
        }

        public static DataSet TimeSheetStop(long wFId, long userId, long transId, long mode, string remarkText)
        {

            return ScivalEntities.Database.SqlQuery<DataSet>("sci_timesheetstop @p_workflowid, @p_userid,@p_TRANSITIONALID,,@p_mode,@p_REMARKS",
           new SqlParameter("p_workflowid", wFId),
           new SqlParameter("p_userid", userId),
           new SqlParameter("p_TRANSITIONALID", transId),
           new SqlParameter("p_mode", mode),
           new SqlParameter("p_REMARKS", remarkText)
           ).FirstOrDefault();
        }


        public static DataSet GetRelatedPrograms(long wfId)
        {
            var RelatedProg = ScivalEntities.Database.SqlQuery<DataSet>("sci_op_relatedprogeam @p_workflowid",
                     new SqlParameter("p_workflowid", wfId)
                     ).FirstOrDefault();

            RelatedProg.Tables[1].TableName = "Hierarchy";
            RelatedProg.Tables[2].TableName = "ListData";
            RelatedProg.Tables[3].TableName = "DisplayData";
            return RelatedProg;
        }


        public static DataSet CheckAwardDuplicate(Int64 WFId, Int64 userid, Int64 transId, Int64 mode, Int64 queryMode, string taskName)
        {
            return ScivalEntities.Database.SqlQuery<DataSet>("sci_check_award_duplicate @p_workflowid, @p_userid,@p_TRANSITIONALID,@p_mode,@p_QueryMode,@p_TaskName",
                new SqlParameter("p_workflowid", WFId),
                new SqlParameter("p_userid", userid),
                new SqlParameter("p_TRANSITIONALID", transId),
                new SqlParameter("p_mode", mode),
                 new SqlParameter("p_QueryMode", queryMode),
                new SqlParameter("p_TaskName", taskName)
                ).FirstOrDefault();
        }


        public static DataSet SaveAbdUpdateRelatedLProgram(Int64 WFId, Int64 mode, Int64 NewId, string NewHieRar, string NewProtext, string NewRelType, Int64 oldId, string oldRelType, string OldProText)
        {

            return ScivalEntities.Database.SqlQuery<DataSet>("sci_op_relatedprogeamins @p_workflowid, @p_insdel,@p_id,@p_HIERARCHY,@p_RELATEDPROGRAMTEXT,@p_RELTYPE,@x_id,@x_RELTYPE,@x_RELATEDPROGRAMTEXT",
           new SqlParameter("p_workflowid", WFId),
           new SqlParameter("p_insdel", mode),
           new SqlParameter("p_id", NewId),
           new SqlParameter("p_HIERARCHY", NewHieRar),
            new SqlParameter("p_RELATEDPROGRAMTEXT", NewProtext),
           new SqlParameter("p_RELTYPE", NewRelType),
            new SqlParameter("x_id", oldId),
           new SqlParameter("x_RELTYPE", oldRelType),
           new SqlParameter("x_RELATEDPROGRAMTEXT", OldProText)
           ).FirstOrDefault();
        }

        public static DataSet GetRelatedOpportunities(long workflowid)
        {
            var RelatedOpportunity = ScivalEntities.Database.SqlQuery<DataSet>("sci_op_relatedopplist_prc @p_workflowid",
                    new SqlParameter("p_workflowid", workflowid)).FirstOrDefault();

            RelatedOpportunity.Tables[1].TableName = "RelatedOpp";
            RelatedOpportunity.Tables[2].TableName = "Opportunity";
            return RelatedOpportunity;

        }


        public static DataSet SaveAndDeleteRelatedOpps(Int64 WFID, Int64 mode, string OppDbId, Int64? reltype, string Description = "")
        {
            var SaveDelRelatedOpp = ScivalEntities.Database.SqlQuery<DataSet>("sci_rel_opportunity_dml_prc5 @p_workflowid, @p_insdel,@p_orgdbid,@p_RELTYPE,@p_Desc",
            new SqlParameter("p_workflowid", WFID),
            new SqlParameter("p_insdel", mode),
            new SqlParameter("p_orgdbid", OppDbId),
            new SqlParameter("p_RELTYPE", mode),
             new SqlParameter("p_Desc", reltype),
            new SqlParameter("p_TaskName", Description)
            ).FirstOrDefault();
            SaveDelRelatedOpp.Tables[0].TableName = "RelatedOpp";
            SaveDelRelatedOpp.Tables[1].TableName = "Opportunity";
            return SaveDelRelatedOpp;
        }

        public static DataSet GetItemsList(Int64 WFId, Int64 pagemode)
        {
            var ItemLst = ScivalEntities.Database.SqlQuery<DataSet>("sci_itemlist @p_workflowid,@p_mode",
                      new SqlParameter("p_workflowid", WFId),
                      new SqlParameter("p_mode", pagemode)
                      ).FirstOrDefault();

            ItemLst.Tables[1].TableName = "ItemListDisplay";
            return ItemLst;
        }

        public static DataSet SaveAndDeleteItemsLIst(Int64 WFId, Int64 pagemode, Int64 WorkMode, string Reltype, string Description,
            string URl, string UrlText, string lang, Int64 ItemId)
        {
            var SaveDelItemLst = ScivalEntities.Database.SqlQuery<DataSet>("sci_iteminserttemp @p_workflowid,@p_mode, @p_insdel,@p_itemid,@p_RELTYPE,@p_DESCRIPTION,@p_urltext,@p_lang",
              new SqlParameter("p_workflowid", WFId),
              new SqlParameter("p_mode", pagemode),
              new SqlParameter("p_insdel", WorkMode),
              new SqlParameter("p_itemid", ItemId),
              new SqlParameter("p_RELTYPE", Reltype),
               new SqlParameter("p_DESCRIPTION", Description),
              new SqlParameter("p_url", URl),
              new SqlParameter("p_urltext", UrlText),
              new SqlParameter("p_lang", lang)
              ).FirstOrDefault();
            SaveDelItemLst.Tables[0].TableName = "ItemListDisplay";
            return SaveDelItemLst;
        }
        public static DataSet GetAmount(long workflowid, long pagemode)
        {
            var amount = ScivalEntities.Database.SqlQuery<DataSet>("sci_op_estfundinglist @p_workflowid,@p_mode",
                      new SqlParameter("p_workflowid", workflowid),
                      new SqlParameter("p_mode", pagemode)
                      ).FirstOrDefault();

            amount.Tables[1].TableName = "DisplayData";
            return amount;
        }
        public static DataSet GetRelatedOrgs(long workflowid)
        {
            var RelatedOrg = ScivalEntities.Database.SqlQuery<DataSet>("sci_op_relatedorglist_y @p_workflowid",
                      new SqlParameter("p_workflowid", workflowid)
                      ).FirstOrDefault();

            RelatedOrg.Tables[1].TableName = "Hirarchy";
            RelatedOrg.Tables[2].TableName = "RelatedOrgs";
            RelatedOrg.Tables[3].TableName = "FundingBody";
            return RelatedOrg;
        }
        public static DataSet GetAutoLeadRelorgs(Int64 ID, Int64 moduleid, Int64 flag)
        {
            var AutoleadRelorg = ScivalEntities.Database.SqlQuery<DataSet>("sci_auto_lead_relorgs_prc @p_id,@p_moduleid,@p_flag",
                  new SqlParameter("p_id", ID),
                  new SqlParameter("p_moduleid", moduleid),
                  new SqlParameter("p_flag", flag)
                  ).FirstOrDefault();

            AutoleadRelorg.Tables[1].TableName = "AutoLeadDetail";
            return AutoleadRelorg;
        }
        public static DataSet SaveAndDeleteRelatedOrgs(Int64 WFID, string Amount, string currency, Int64 mode, string Hieracrhy, string ORgDbId, string reltype, string relatedorgsid)
        {
            var SaveDelRelatedOpp = ScivalEntities.Database.SqlQuery<DataSet>("sci_op_relorgins_y @p_workflowid,@p_amount,@p_currency,@p_insdel,@p_HIERARCHY,@p_orgdbid,@p_RELTYPE,p_relatedorgsid",
               new SqlParameter("p_workflowid", WFID),
               new SqlParameter("p_amount", Amount),
               new SqlParameter("p_currency", currency),
               new SqlParameter("p_insdel", mode),
                new SqlParameter("p_HIERARCHY", Hieracrhy),
                new SqlParameter("p_orgdbid", ORgDbId),
               new SqlParameter("p_RELTYPE", reltype),
               new SqlParameter("p_relatedorgsid", relatedorgsid)
               ).FirstOrDefault();
            SaveDelRelatedOpp.Tables[0].TableName = "FundingBody";
            return SaveDelRelatedOpp;
        }


        public static DataSet Save_Update_PublicationData(Int64 WFId, Int64 mode, string p_FundingBodyProjectId = "", string p_PublishedDate = null, string p_PublicationURL = "",
        Int64 p_PublicationOutputId = 0, string p_IngestionId = "", string p_JournalTitle = "", string p_Journalidentifier = "", string p_Authors = "", string p_Description = "")
        {
            var SaveUpPublicationData = ScivalEntities.Database.SqlQuery<DataSet>("sci_Publication_out_insupdel @p_workflowid, @p_insdel,@p_publication_ID,@p_FundingBodyProjectId," +
                "@p_PublishedDate,@p_PublicationURL,@p_PublicationOutputId,@p_IngestionId,@p_JournalTitle,@p_Journalidentifier,@p_Authors,@p_Description",
                 new SqlParameter("p_workflowid", WFId),
                 new SqlParameter("p_insdel", mode),
                 new SqlParameter("p_publication_ID", 0),
                 new SqlParameter("p_FundingBodyProjectId", p_FundingBodyProjectId),
                  new SqlParameter("p_PublishedDate", p_PublishedDate),
                 new SqlParameter("p_PublicationURL", p_PublicationURL),
                  new SqlParameter("p_PublicationOutputId", p_PublicationOutputId),
                 new SqlParameter("p_IngestionId", p_IngestionId),
                 new SqlParameter("p_JournalTitle", p_JournalTitle),
                  new SqlParameter("p_Journalidentifier", p_Journalidentifier),
                 new SqlParameter("p_Authors", p_Authors),
                 new SqlParameter("p_Description", p_Description)
                 ).FirstOrDefault();
            return SaveUpPublicationData;
        }

        public static DataSet SavePublication_Title(Int64 WFId, Int64 mode, string p_Lang = "", string p_Title = "")
        {
            var SavePublicationTitle = ScivalEntities.Database.SqlQuery<DataSet>("sci_PublicationTitle_insupdel @p_workflowid,@p_insdel,@p_publication_ID,@p_Lang,@p_Title",
             new SqlParameter("p_workflowid", WFId),
             new SqlParameter("p_insdel", mode),
             new SqlParameter("p_publication_ID", 0),
             new SqlParameter("p_Lang", p_Lang),
              new SqlParameter("p_Title", p_Title)
             ).FirstOrDefault();
            SavePublicationTitle.Tables[0].TableName = "PublicationTitle";
            return SavePublicationTitle;
        }


        public static DataSet getWorkFlowDetails(long wId)
        {
            return ScivalEntities.Database.SqlQuery<DataSet>("select sw.ID, sw.MODULEID, am.fundingbodyid from sci_workflow sw, award_master am  where am.awardid=sw.id and workflowid=@WorkFlowId",
               new SqlParameter("WorkFlowId", wId)).FirstOrDefault();
        }


        public static DataSet LoadLanguageData(string OPPID, int mode_id, int tran_type_id)
        {
            return ScivalEntities.Database.SqlQuery<DataSet>("SELECT tran_id, scival_id, column_desc, column_id, column_name, moduleid, modulename, language_id, language_name, language_code," +
                " tran_type_id,tran_name, language_group_id, language_code FROM table (sci_language_detail_fnc (scival_id_in => @OppId, moduleid_in => @Mode_id)",
                new SqlParameter("OppId", OPPID),
                new SqlParameter("Mode_id", mode_id)).FirstOrDefault();
        }


        public static DataSet SaveAwardLang(DataTable AwardLang)
        {
            var SaveAwardLang = new DataSet();
            if (AwardLang.Rows.Count > 0)
            {
                for (Int32 x = 0; x < AwardLang.Rows.Count; x++)
                {
                    try
                    {
                        SaveAwardLang = ScivalEntities.Database.SqlQuery<DataSet>("sci_language_detail_dml_prc @tran_id_in,@scival_id_in,@column_desc_in," +
                            "@column_id_in,@mode_id_in,@LANGUAGE_ID",
             new SqlParameter("tran_id_in", AwardLang.Rows[x]["tran_id"]),
             new SqlParameter("scival_id_in", AwardLang.Rows[x]["AW_ID"]),
             new SqlParameter("column_desc_in", r.WieredChar_ReplacementHexValue(AwardLang.Rows[x]["COLUMN_DESC"].ToString())),
             new SqlParameter("column_id_in", AwardLang.Rows[x]["COLUMN_ID"]),
              new SqlParameter("mode_id_in", AwardLang.Rows[x]["MODULEID"]),
              new SqlParameter("LANGUAGE_ID", AwardLang.Rows[x]["LANGUAGE_ID"]),
              new SqlParameter("tran_type_id_in", AwardLang.Rows[x]["TRAN_TYPE_ID"]),
              new SqlParameter("flag_in", AwardLang.Rows[x]["FLAG_IN"])
             ).FirstOrDefault();

                    }
                    catch (Exception ex)
                    { }
                }
            }
            SaveAwardLang.Tables[0].TableName = "AwardLangTable";
            return SaveAwardLang;
        }


        public static DataSet Getfunding_details(long wFID)
        {
            var fundingDtls = ScivalEntities.Database.SqlQuery<DataSet>("get_funding_details @p_workflowid",
                      new SqlParameter("p_workflowid", wFID)
                      ).FirstOrDefault();

            fundingDtls.Tables[1].TableName = "DisplayData";
            return fundingDtls;
        }

        public static DataSet SaveFunds(string url_txt_fundingBodyProjectId_f, string url_txt_Amount_f, string url_ddlCuurency_f, string url_txt_fundingBodyProjectId_h, string url_txt_Amount_h, string url_ddlCuurency_h, string url_txt_acronym, DateTime url_txtSrtDate, DateTime url_txtEndDateDate,
       string url_txtStatus, string url_txt_link, string url_txtpostofficeboxn, string url_TextStreet, string url_txt_locality, string url_txtregion,
       string url_txtPostalCode, string url_country, string url_LANGUAGE_NAME, Int64 COLUMN_ID, Int64 TRAN_TYPE_ID, string url_COLUMN_DESC, Int64 pagemode, Int64 WFID, Int64 mode, Int64 p_SEQUENCE_ID = 0)
        {
            var savefund = ScivalEntities.Database.SqlQuery<DataSet>("save_funding_prc3 @P_fundingBodyProjectId_f,@P_txt_Amount_f,@P_ddlCurrency_f,@P_fundingBodyProjectId_h," +
                "@P_Amount_h,@P_ddlCurrency_h,@P_acronym,@P_SrtDate,@P_EndDateDate,@P_Status,@P_link,@P_postofficeboxn,@P_Street,@P_locality,@P_region,@P_PostalCode,@P_COUNTRY," +
                "@P_COLUMN_DESC,@P_LANGUAGE_NAME,@P_COLUMN_ID,@P_TRAN_TYPE_ID,@P_WORKFLOWID,@P_pagemode,@P_mode,@P_SEQUENCE_ID",
             new SqlParameter("P_fundingBodyProjectId_f", url_txt_fundingBodyProjectId_f),
             new SqlParameter("P_txt_Amount_f", url_txt_Amount_f),
             new SqlParameter("P_ddlCurrency_f", url_ddlCuurency_f),
             new SqlParameter("P_fundingBodyProjectId_h", url_txt_fundingBodyProjectId_h),
              new SqlParameter("P_Amount_h", url_txt_Amount_h),
              new SqlParameter("P_ddlCurrency_h", url_ddlCuurency_h),
              new SqlParameter("P_acronym", url_txt_acronym),
              new SqlParameter("P_SrtDate", url_txtSrtDate),
              new SqlParameter("P_EndDateDate", url_txtEndDateDate),
              new SqlParameter("P_Status", url_txtStatus),
              new SqlParameter("P_link", url_txt_link),
              new SqlParameter("P_postofficeboxn", url_txtpostofficeboxn),
              new SqlParameter("P_Street", url_TextStreet),
              new SqlParameter("P_locality", url_txt_locality),
              new SqlParameter("P_region", url_txtregion),
              new SqlParameter("P_PostalCode", url_txtPostalCode),
              new SqlParameter("P_COUNTRY", url_country),
              new SqlParameter("P_COLUMN_DESC", url_COLUMN_DESC),
              new SqlParameter("P_LANGUAGE_NAME", url_LANGUAGE_NAME),
              new SqlParameter("P_COLUMN_ID", COLUMN_ID),
              new SqlParameter("P_TRAN_TYPE_ID", TRAN_TYPE_ID),
              new SqlParameter("P_WORKFLOWID", WFID),
              new SqlParameter("P_pagemode", pagemode),
              new SqlParameter("P_mode", mode),
              new SqlParameter("P_SEQUENCE_ID", p_SEQUENCE_ID)).FirstOrDefault();
            savefund.Tables[1].TableName = "DisplayData";
            return savefund;
        }

        public static DataSet GetFBId(long workflowid)
        {
           return ScivalEntities.Database.SqlQuery<DataSet>("select sci_get_fb_id (@workflowID) FBID from dual", new SqlParameter("@workflowID", workflowid))
                    .FirstOrDefault();
         
        }

        public static DataSet UpdateClassification(Int64 WFId, Int64 mode, string type, Int64 oldfrequency, string oldcode, Int64 frequency, string code, string ClaasText, string ClassFcID)
        {
            var UpClassification = ScivalEntities.Database.SqlQuery<DataSet>("sci_classificationupdel @p_workflowid,@p_insdel,@p_type,@p_FREQUENCY,@p_CODE,@p_CLASSIFICATION_TEXT," +
                "@p_CLASSIFICATIONS_ID,@O_FREQUENCY,@O_CODE",
                      new SqlParameter("p_workflowid", WFId),
                      new SqlParameter("p_insdel", mode),
                      new SqlParameter("p_type", type),
                      new SqlParameter("p_FREQUENCY", frequency),
                      new SqlParameter("p_CODE", code),
                      new SqlParameter("p_CLASSIFICATION_TEXT", ClaasText),
                      new SqlParameter("p_CLASSIFICATIONS_ID", ClassFcID),
                      new SqlParameter("O_FREQUENCY", oldfrequency),
                      new SqlParameter("O_CODE", oldcode)
                      ).FirstOrDefault();
            UpClassification.Tables[1].TableName = "DisplayData";
            return UpClassification;
        }

        public static DataSet Getaffiliation(long AWARDEEID)
        {
            var affiliation = ScivalEntities.Database.SqlQuery<DataSet>("SCI_AW_INSTITUTIONLIST @P_AWARDEEID",
                      new SqlParameter("P_AWARDEEID", AWARDEEID)
                      ).FirstOrDefault();

            affiliation.Tables[1].TableName = "AffiliationGrid";
            return affiliation;
        }


        public static DataSet SaveAndDeleteAwardee(Int64 WFId, Int64 mode, string type, Int64 scopusId, string indexName, string givenName, string initials, string surname, Int64 awardeeId, string currency, Int64 amount, string P_ORCID, string P_externalRes_Id)
        {
            var awardee = ScivalEntities.Database.SqlQuery<DataSet>("sci_Aw_awardeeinsdel_41 @p_workflowid,@P_INSDEL,@P_TYPE,@P_SCOPUSAUTHORID,@P_externalRes_Id,@P_ORCID," +
                "@P_INDEXEDNAME,@P_GIVENNAME,@P_INITIALS,@P_SURNAME,@p_AWARDEE_ID,@p_currency,@p_amount",
                     new SqlParameter("p_workflowid", WFId),
                     new SqlParameter("P_INSDEL", mode),
                     new SqlParameter("P_TYPE", type),
                     new SqlParameter("P_SCOPUSAUTHORID", scopusId),
                     new SqlParameter("P_externalRes_Id", r.WieredChar_ReplacementHexValue(P_externalRes_Id)),
                     new SqlParameter("P_ORCID", P_ORCID),
                     new SqlParameter("P_INDEXEDNAME", r.WieredChar_ReplacementHexValue(indexName)),
                     new SqlParameter("P_GIVENNAME", r.WieredChar_ReplacementHexValue(givenName)),
                     new SqlParameter("P_INITIALS", r.WieredChar_ReplacementHexValue(initials)),
                     new SqlParameter("P_SURNAME", r.WieredChar_ReplacementHexValue(surname)),
                     new SqlParameter("p_AWARDEE_ID", awardeeId),
                     new SqlParameter("p_currency", currency),
                     new SqlParameter("p_amount", amount)
                     ).FirstOrDefault();
               return awardee;
        }

        public static DataSet SaveAndDeleteAwardee50(Int64 WFId, Int64 mode, Int64 awardeeId, string activityType, string awardeeAffiliationId, string departmentName, string fundingBodyOrganizationId, string link, string name, string role, string vatNumber, string DUNS, string ROR, string WIKIDATA, string currency, Int64 amount)
        {
            var awardee50 = ScivalEntities.Database.SqlQuery<DataSet>("SCI_AW_AWARDEEINSDEL_50 @p_workflowid,@P_INSDEL,@P_TYPE,@P_SCOPUSAUTHORID,@P_externalRes_Id,@P_ORCID," +
                "@P_INDEXEDNAME,@P_GIVENNAME,@P_INITIALS,@P_SURNAME,@p_AWARDEE_ID,@p_currency,@p_amount,@P_link,@P_DUNS,@P_ROR,@P_WIKIDATA",
                     new SqlParameter("p_workflowid", WFId),
                     new SqlParameter("P_INSDEL", mode),
                     new SqlParameter("P_TYPE", role),
                     new SqlParameter("P_SCOPUSAUTHORID", 0),
                     new SqlParameter("P_externalRes_Id", r.WieredChar_ReplacementHexValue(vatNumber)),
                     new SqlParameter("P_ORCID", r.WieredChar_ReplacementHexValue(fundingBodyOrganizationId)),
                     new SqlParameter("P_INDEXEDNAME", r.WieredChar_ReplacementHexValue(departmentName)),
                     new SqlParameter("P_GIVENNAME", r.WieredChar_ReplacementHexValue(name)),
                     new SqlParameter("P_INITIALS", r.WieredChar_ReplacementHexValue(awardeeAffiliationId)),
                     new SqlParameter("P_SURNAME", r.WieredChar_ReplacementHexValue(activityType)),
                     new SqlParameter("p_AWARDEE_ID", awardeeId),
                     new SqlParameter("p_currency", currency),
                     new SqlParameter("p_amount", amount),
                     new SqlParameter("P_link", r.WieredChar_ReplacementHexValue(link)),
                     new SqlParameter("P_DUNS", r.WieredChar_ReplacementHexValue(DUNS)),
                     new SqlParameter("P_ROR", r.WieredChar_ReplacementHexValue(ROR)),
                     new SqlParameter("P_WIKIDATA", r.WieredChar_ReplacementHexValue(WIKIDATA))
                     ).FirstOrDefault();
            return awardee50;
        }
        public static DataSet SaveAndDeleteAffiliation50(Int64 AWARDEEID, Int64 mode, Int64 affliationId, string awardeePersonId, string emailAddress, string familyName,
            string fundingBodyPersonId, string givenName, string ORCID_Aff, string initials_Aff, string role_Aff, string name_Aff)
        {
            var SaveAffiliation50 = ScivalEntities.Database.SqlQuery<DataSet>("SCI_AW_INSTITUTIONINSDEL50 @P_AWARDEEID,@P_INSDEL,@P_SCOPUSINSTITUTIONID,@P_ORG,@P_DEPT,@P_STARTDATE," +
                "@P_ENDDATE,@P_EMAIL,@P_WEBPAGE,@P_EXTAFFI_Id,@P_awardeePersonId,@P_familyName,@P_fundingBodyPersonId,@P_givenName,@P_ORCID,@P_initials,@P_role,@P_name,@P_AFFILIATION_ID",
                     new SqlParameter("P_AWARDEEID", AWARDEEID),
                     new SqlParameter("P_INSDEL", mode),
                     new SqlParameter("P_SCOPUSINSTITUTIONID", null),
                     new SqlParameter("P_ORG", r.WieredChar_ReplacementHexValue("")),
                     new SqlParameter("P_DEPT", r.WieredChar_ReplacementHexValue("")),
                     new SqlParameter("P_STARTDATE", null),
                     new SqlParameter("P_ENDDATE", null),
                     new SqlParameter("P_EMAIL", r.WieredChar_ReplacementHexValue(emailAddress)),
                     new SqlParameter("P_WEBPAGE", r.WieredChar_ReplacementHexValue("")),
                     new SqlParameter("P_EXTAFFI_Id", r.WieredChar_ReplacementHexValue("")),
                     new SqlParameter("P_awardeePersonId", r.WieredChar_ReplacementHexValue(awardeePersonId)),
                     new SqlParameter("P_familyName", r.WieredChar_ReplacementHexValue(familyName)),
                     new SqlParameter("P_fundingBodyPersonId", r.WieredChar_ReplacementHexValue(fundingBodyPersonId)),
                     new SqlParameter("P_givenName", r.WieredChar_ReplacementHexValue(givenName)),
                     new SqlParameter("P_ORCID", r.WieredChar_ReplacementHexValue(ORCID_Aff)),
                     new SqlParameter("P_initials", r.WieredChar_ReplacementHexValue(initials_Aff)),
                     new SqlParameter("P_role", r.WieredChar_ReplacementHexValue(role_Aff)),
                     new SqlParameter("P_name", r.WieredChar_ReplacementHexValue(name_Aff)),
                     new SqlParameter("P_AFFILIATION_ID",affliationId)
                     ).FirstOrDefault();

            SaveAffiliation50.Tables[1].TableName = "DisplayAffiliation";

            DataTable DT = new DataTable();
            SaveAffiliation50.Tables.Add(DT);
            if (SaveAffiliation50.Tables["DisplayAffiliation"].Rows.Count > 0)
            {
                for (int intCount = 0; intCount < SaveAffiliation50.Tables["DisplayAffiliation"].Rows.Count; intCount++)
                {
                    string UpdateFunding_ORG = r.Return_WieredChar_Original(Convert.ToString(SaveAffiliation50.Tables["DisplayAffiliation"].Rows[intCount]["ORG"].ToString()));
                    if (UpdateFunding_ORG != "")
                    {
                        SaveAffiliation50.Tables["DisplayAffiliation"].Rows[intCount]["ORG"] = UpdateFunding_ORG;
                    }
                    string UpdateFunding_DEPT = r.Return_WieredChar_Original(Convert.ToString(SaveAffiliation50.Tables["DisplayAffiliation"].Rows[intCount]["DEPT"].ToString()));
                    if (UpdateFunding_DEPT != "")
                    {
                        SaveAffiliation50.Tables["DisplayAffiliation"].Rows[intCount]["DEPT"] = UpdateFunding_DEPT;

                    }
                    string UpdateFunding_EMAIL = r.Return_WieredChar_Original(Convert.ToString(SaveAffiliation50.Tables["DisplayAffiliation"].Rows[intCount]["EMAIL"].ToString()));
                    if (UpdateFunding_EMAIL != "")
                    {
                        SaveAffiliation50.Tables["DisplayAffiliation"].Rows[intCount]["EMAIL"] = UpdateFunding_EMAIL;
                    }
                    string UpdateFunding_WEBPAGE = r.Return_WieredChar_Original(Convert.ToString(SaveAffiliation50.Tables["DisplayAffiliation"].Rows[intCount]["WEBPAGE"].ToString()));
                    if (UpdateFunding_WEBPAGE != "")
                    {
                        SaveAffiliation50.Tables["DisplayAffiliation"].Rows[intCount]["WEBPAGE"] = UpdateFunding_WEBPAGE;
                    }

                    string UpdateEXTERNALAFFILIATIONIDENTIFIER = r.Return_WieredChar_Original(Convert.ToString(SaveAffiliation50.Tables["DisplayAffiliation"].Rows[intCount]["EXTERNALAFFILIATIONIDENTIFIER"].ToString()));
                    if (UpdateEXTERNALAFFILIATIONIDENTIFIER != "")
                    {
                        SaveAffiliation50.Tables["DisplayAffiliation"].Rows[intCount]["EXTERNALAFFILIATIONIDENTIFIER"] = UpdateEXTERNALAFFILIATIONIDENTIFIER;
                    }
                }
                SaveAffiliation50.Tables["DisplayAffiliation"].AcceptChanges();
            }
            return SaveAffiliation50;
        }


        public static DataSet SaveAndDeleteAffiliation(Int64 AWARDEEID, Int64 mode, Int64 institueId, string org, string dpart, string srtDate, string endDate, string email, string webpage, string extAffi_Id, Int64 affliationId)
        {
            var SaveAffiliation = ScivalEntities.Database.SqlQuery<DataSet>("SCI_AW_INSTITUTIONINSDEL41 @P_AWARDEEID,@P_INSDEL,@P_SCOPUSINSTITUTIONID,@P_ORG,@P_DEPT,@P_STARTDATE," +
                "@P_ENDDATE,@P_EMAIL,@P_WEBPAGE,@P_EXTAFFI_Id,@P_awardeePersonId,@P_familyName,@P_fundingBodyPersonId,@P_givenName,@P_ORCID,@P_initials,@P_role,@P_name,@P_AFFILIATION_ID",
                        new SqlParameter("P_AWARDEEID", AWARDEEID),
                        new SqlParameter("P_INSDEL", mode),
                        new SqlParameter("P_SCOPUSINSTITUTIONID", institueId),
                        new SqlParameter("P_ORG", r.WieredChar_ReplacementHexValue(org)),
                        new SqlParameter("P_DEPT", r.WieredChar_ReplacementHexValue(dpart)),
                        new SqlParameter("P_STARTDATE", srtDate),
                        new SqlParameter("P_ENDDATE", endDate),
                        new SqlParameter("P_EMAIL", r.WieredChar_ReplacementHexValue(email)),
                        new SqlParameter("P_WEBPAGE", r.WieredChar_ReplacementHexValue(webpage)),
                        new SqlParameter("P_EXTAFFI_Id", r.WieredChar_ReplacementHexValue(extAffi_Id)),
                        new SqlParameter("P_AFFILIATION_ID", affliationId)
                        ).FirstOrDefault();
            SaveAffiliation.Tables[1].TableName = "DisplayAffiliation";
            DataTable DT = new DataTable();
            SaveAffiliation.Tables.Add(DT);
            if (SaveAffiliation.Tables["DisplayAffiliation"].Rows.Count > 0)
            {
                for (int intCount = 0; intCount < SaveAffiliation.Tables["DisplayAffiliation"].Rows.Count; intCount++)
                {
                    string UpdateFunding_ORG = r.Return_WieredChar_Original(Convert.ToString(SaveAffiliation.Tables["DisplayAffiliation"].Rows[intCount]["ORG"].ToString()));
                    if (UpdateFunding_ORG != "")
                    {
                        SaveAffiliation.Tables["DisplayAffiliation"].Rows[intCount]["ORG"] = UpdateFunding_ORG;
                    }
                    string UpdateFunding_DEPT = r.Return_WieredChar_Original(Convert.ToString(SaveAffiliation.Tables["DisplayAffiliation"].Rows[intCount]["DEPT"].ToString()));
                    if (UpdateFunding_DEPT != "")
                    {
                        SaveAffiliation.Tables["DisplayAffiliation"].Rows[intCount]["DEPT"] = UpdateFunding_DEPT;

                    }
                    string UpdateFunding_EMAIL = r.Return_WieredChar_Original(Convert.ToString(SaveAffiliation.Tables["DisplayAffiliation"].Rows[intCount]["EMAIL"].ToString()));
                    if (UpdateFunding_EMAIL != "")
                    {
                        SaveAffiliation.Tables["DisplayAffiliation"].Rows[intCount]["EMAIL"] = UpdateFunding_EMAIL;
                    }
                    string UpdateFunding_WEBPAGE = r.Return_WieredChar_Original(Convert.ToString(SaveAffiliation.Tables["DisplayAffiliation"].Rows[intCount]["WEBPAGE"].ToString()));
                    if (UpdateFunding_WEBPAGE != "")
                    {
                        SaveAffiliation.Tables["DisplayAffiliation"].Rows[intCount]["WEBPAGE"] = UpdateFunding_WEBPAGE;
                    }

                    string UpdateEXTERNALAFFILIATIONIDENTIFIER = r.Return_WieredChar_Original(Convert.ToString(SaveAffiliation.Tables["DisplayAffiliation"].Rows[intCount]["EXTERNALAFFILIATIONIDENTIFIER"].ToString()));
                    if (UpdateEXTERNALAFFILIATIONIDENTIFIER != "")
                    {
                        SaveAffiliation.Tables["DisplayAffiliation"].Rows[intCount]["EXTERNALAFFILIATIONIDENTIFIER"] = UpdateEXTERNALAFFILIATIONIDENTIFIER;
                    }
                }
                SaveAffiliation.Tables["DisplayAffiliation"].AcceptChanges();
            }
            return SaveAffiliation;

        }
        public static DataSet SaveArrayKeywordswithlang(Int64 p_workflowid, Int64 p_insdel, string p_LANG, string p_DESCRIPTION, Int64? p_keywordid = null)
        {
            var keywithlang = ScivalEntities.Database.SqlQuery<DataSet>("sci_keywordinsert_avi @p_workflowid,@p_insdel,@p_keywordid,@p_LANG,@p_DESCRIPTION,@p_oldDESCRIPTION",
                    new SqlParameter("p_workflowid", p_workflowid),
                    new SqlParameter("p_insdel", p_insdel),
                    new SqlParameter("p_keywordid", p_keywordid),
                    new SqlParameter("p_LANG", p_LANG),
                    new SqlParameter("p_DESCRIPTION", p_DESCRIPTION),
                    new SqlParameter("p_oldDESCRIPTION", null)
                    ).FirstOrDefault();
            keywithlang.Tables[1].TableName = "Keywords";
            return keywithlang;
        {           
                 return ScivalEntities.Database.SqlQuery<DataSet>("sci_op_relatedprogeamins @p_workflowid, @p_insdel,@p_id,@p_HIERARCHY,@p_RELATEDPROGRAMTEXT,@p_RELTYPE,@x_id,@x_RELTYPE,@x_RELATEDPROGRAMTEXT",
                new SqlParameter("p_workflowid", WFId),
                new SqlParameter("p_insdel", mode),
                new SqlParameter("p_id", NewId),
                new SqlParameter("p_HIERARCHY", NewHieRar),
                 new SqlParameter("p_RELATEDPROGRAMTEXT", NewProtext),
                new SqlParameter("p_RELTYPE", NewRelType),
                 new SqlParameter("x_id", oldId),
                new SqlParameter("x_RELTYPE", oldRelType),
                new SqlParameter("x_RELATEDPROGRAMTEXT", OldProText)
                ).FirstOrDefault();
        }
        public static DataSet SaveAmount(Int32 TotalAmount, string InstallmentStart, string InstallmentEnd, Int64 WFID, string currency, string amount, Int64 pagemode, Int64 mode, Int64 p_SEQUENCE_ID = 0)
        {
            var saveamt = ScivalEntities.Database.SqlQuery<DataSet>("SCI_OP_estfundingins_y @p_workflowid,@p_currency,@p_amount,@p_amount_description,@p_total_amount,@p_startdate," +
                "@p_enddate,@p_url,@p_url_text,@p_mode,@PAGE_MODE,@p_SEQUENCE_ID",
                       new SqlParameter("p_workflowid", WFID),
                       new SqlParameter("p_currency", currency.Trim().ToUpper()),
                       new SqlParameter("p_amount", amount),
                       new SqlParameter("p_amount_description", null),
                       new SqlParameter("p_total_amount", TotalAmount),
                       new SqlParameter("p_startdate", InstallmentStart),
                       new SqlParameter("p_enddate", InstallmentEnd),
                       new SqlParameter("p_url", ""),
                       new SqlParameter("p_url_text", ""),
                       new SqlParameter("p_mode", mode),
                       new SqlParameter("PAGE_MODE", pagemode),
                       new SqlParameter("p_SEQUENCE_ID", p_SEQUENCE_ID)
                       ).FirstOrDefault();
            return saveamt;
        }

        public static DataSet SaveAndDeleteContactsLIst(Int64 WFId, Int64 pagemode, Int64 WorkMode, Int64 Contact_Id, string TYPE, string TITLE, string TELEPHONE, string FAX, 
            string EMAIL, string url, string WEBSITE_TEXT, string COUNTRY, string ROOM, string STREET, string STATE, string CITY, string POSTALCODE, string PREFIX, 
            string GIVENNAME, string MIDDLENAME, string SURNAME, string SUFFIX)
        {
            var savecontactlst = ScivalEntities.Database.SqlQuery<DataSet>("sci_managerinsdel_1 @p_workflowid,@p_insdel,@p_contactid,@p_type,@p_title,@p_telephone," +
                "@p_fax,@p_email,@p_url,@p_website_text,@p_MANAGERID,@p_country,@p_room,@p_street,@p_state,@p_city,@p_postalcode,@p_prefix,@p_givenname,@p_middlename,@p_surname,@p_suffix",
                       new SqlParameter("p_workflowid", WFId),
                       new SqlParameter("p_insdel", WorkMode),
                       new SqlParameter("p_contactid", Contact_Id),
                       new SqlParameter("p_type", r.WieredChar_ReplacementHexValue(TYPE)),
                       new SqlParameter("p_title", r.WieredChar_ReplacementHexValue(TITLE)),
                       new SqlParameter("p_telephone", TELEPHONE),
                       new SqlParameter("p_fax", FAX),
                       new SqlParameter("p_email", r.WieredChar_ReplacementHexValue(EMAIL)),
                       new SqlParameter("p_url", r.WieredChar_ReplacementHexValue(url)),
                       new SqlParameter("p_website_text", r.WieredChar_ReplacementHexValue(WEBSITE_TEXT)),
                       new SqlParameter("p_MANAGERID", Contact_Id),
                       new SqlParameter("p_country", COUNTRY),
                       new SqlParameter("p_room", r.WieredChar_ReplacementHexValue(ROOM)),
                       new SqlParameter("p_street", r.WieredChar_ReplacementHexValue(STREET)),
                       new SqlParameter("p_state", r.WieredChar_ReplacementHexValue(STATE)),
                       new SqlParameter("p_city", r.WieredChar_ReplacementHexValue(CITY)),
                       new SqlParameter("p_postalcode", POSTALCODE),
                       new SqlParameter("p_prefix", r.WieredChar_ReplacementHexValue(PREFIX)),
                       new SqlParameter("p_givenname", r.WieredChar_ReplacementHexValue(GIVENNAME)),
                       new SqlParameter("p_middlename", r.WieredChar_ReplacementHexValue(MIDDLENAME)),
                       new SqlParameter("p_surname", r.WieredChar_ReplacementHexValue(SURNAME)),
                       new SqlParameter("p_suffix", r.WieredChar_ReplacementHexValue(SUFFIX))
                       ).FirstOrDefault();
            return savecontactlst;
        }

        public static DataSet SaveAndUpdateAddress(Int64 AFFILIATIONID, Int64 mode, string country, string Room, string Street, string City, string State, 
            string postalcode, string CountryOldValue, string RoomOldValue, string StreetOldValue, string CityOldValue, string StateOldValue, string PostalCodeOldValue)
        {
            var saveAddress = ScivalEntities.Database.SqlQuery<DataSet>("SCI_AW_ADDRESSINSDEL @P_AFFILIATION_ID_IN,@P_COUNTRY_IN,@P_ROOM_IN,@P_STREET_IN,@P_CITY_IN,@P_STATE_IN," +
                "@P_POSTALCODE_IN,@P_OLD_COUNTRY_IN,@P_OLD_ROOM_IN,@P_OLD_STREET_IN,@P_OLD_CITY_IN,@P_OLD_STATE_IN,@P_OLD_POSTALCODE_IN  ",
                       new SqlParameter("P_AFFILIATION_ID_IN", AFFILIATIONID),
                       new SqlParameter("P_COUNTRY_IN", country),
                       new SqlParameter("P_ROOM_IN", Room),
                       new SqlParameter("P_STREET_IN", Street),
                       new SqlParameter("P_CITY_IN", City),
                       new SqlParameter("P_STATE_IN", State),
                       new SqlParameter("P_POSTALCODE_IN", postalcode),
                       new SqlParameter("P_OLD_COUNTRY_IN", CountryOldValue),
                       new SqlParameter("P_OLD_ROOM_IN", RoomOldValue),
                       new SqlParameter("P_OLD_STREET_IN", StreetOldValue),
                       new SqlParameter("P_OLD_CITY_IN", CityOldValue),
                       new SqlParameter("P_OLD_STATE_IN", StateOldValue),
                       new SqlParameter("P_OLD_POSTALCODE_IN", PostalCodeOldValue)
                       ).FirstOrDefault();
            saveAddress.Tables[1].TableName = "DisplayAddress";
            return saveAddress;
        }
        public static DataSet SaveUpDelLocation(Int64 WFId, Int64 insdel, string countrytest, string COUNTRY, string ROOM, string STREET, string CITY, string STATE, string POSTALCODE, Int64 location_id)
        {
            var saveLocation   = ScivalEntities.Database.SqlQuery<DataSet>("award_location_insupdel @p_workflowid,@p_insdel,@p_countrytest,@p_country,@p_room,@p_STREET,@p_CITY," +
                "@p_STATE,@p_POSTALCODE,@p_location_id  ",
                       new SqlParameter("p_workflowid", WFId),
                       new SqlParameter("p_insdel", insdel),
                       new SqlParameter("p_countrytest", r.WieredChar_ReplacementHexValue(countrytest)),
                       new SqlParameter("p_country", COUNTRY),
                       new SqlParameter("p_room", r.WieredChar_ReplacementHexValue(ROOM)),
                       new SqlParameter("p_STREET", r.WieredChar_ReplacementHexValue(STREET)),
                       new SqlParameter("p_CITY", r.WieredChar_ReplacementHexValue(CITY)),
                       new SqlParameter("p_STATE", r.WieredChar_ReplacementHexValue(STATE)),
                       new SqlParameter("p_POSTALCODE", POSTALCODE),
                       new SqlParameter("p_location_id", location_id)
                       ).FirstOrDefault();
            saveLocation.Tables[1].TableName = "LocationList";
            return saveLocation;
        }

        public static DataSet SaveClassiFication(Int64 WFId, string type, Int64 frequency, string[] code, string[] ClaasText)
        {
            var saveClassification = ScivalEntities.Database.SqlQuery<DataSet>("sci_classificationinsert @p_workflowid,@p_type,@p_FREQUENCY,@p_CODE,@p_CLASSIFICATION_TEXT",
                          new SqlParameter("p_workflowid", WFId),
                          new SqlParameter("p_type", type),
                          new SqlParameter("p_FREQUENCY", frequency),
                          new SqlParameter("p_CODE", code),
                          new SqlParameter("p_CLASSIFICATION_TEXT", ClaasText)
                          ).FirstOrDefault();
            saveClassification.Tables[1].TableName = "DisplayData";
            return saveClassification;
        }
        public static DataSet SaveAwrad(DataTable Opportunity)
        {
            var saveAward = new DataSet();
            if (Opportunity.Rows.Count > 0)
            {
                for (Int32 x = 0; x < Opportunity.Rows.Count; x++)
                {
                    try
                    {
                         saveAward = ScivalEntities.Database.SqlQuery<DataSet>("sci_aw_awradins50 @p_workflowid,@p_FUNDINGBODYAWARDID,@p_TYPE,@p_TRUSTING,@P_COLLECTIONCODE," +
                            "@P_HIDDEN,@P_NAME,@p_STARTDATE,@p_LASTAMENDEDDATE,@p_ENDDATE,@p_ABSTRACT,@p_url,@p_mode,@p_recordsource,@p_AwardNoticeDate,@p_PublishedDate",
                         new SqlParameter("p_workflowid", Convert.ToInt64(Opportunity.Rows[x]["WFID"])),
                         new SqlParameter("p_FUNDINGBODYAWARDID", Opportunity.Rows[x]["FAID"].ToString()),
                         new SqlParameter("p_TYPE", Opportunity.Rows[x]["TYPEID"].ToString()),
                         new SqlParameter("p_TRUSTING", Opportunity.Rows[x]["TRUSTING"].ToString()),
                         new SqlParameter("P_COLLECTIONCODE", Opportunity.Rows[x]["COLLECTIONCODE"].ToString()),
                         new SqlParameter("P_HIDDEN", Opportunity.Rows[x]["HIDDEN"].ToString()),
                         new SqlParameter("P_NAME", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["NAME"].ToString())),
                         new SqlParameter("p_STARTDATE", Convert.ToString(Opportunity.Rows[x]["STARTDATE"])),
                         new SqlParameter("p_LASTAMENDEDDATE", Convert.ToString(Opportunity.Rows[x]["AMENDEDDATE"])),
                         new SqlParameter("p_ENDDATE", Convert.ToDateTime(Opportunity.Rows[x]["ENDDATE"])),
                         new SqlParameter("p_ABSTRACT", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["ABSTRACT"].ToString())),
                         new SqlParameter("p_url", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["URL"].ToString())),
                         new SqlParameter("p_mode", Convert.ToInt64(Opportunity.Rows[x]["mode"])),
                         new SqlParameter("p_recordsource", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["RECORDSOURCE"].ToString())),
                         new SqlParameter("p_AwardNoticeDate", Convert.ToDateTime(Opportunity.Rows[x]["AwardNoticeDate"])),
                          new SqlParameter("p_PublishedDate", Convert.ToDateTime(Opportunity.Rows[x]["PublishedDate"]))
                          ).FirstOrDefault();
                        saveAward.Tables[1].TableName = "FundingBodyTable";                        
                    }
                    catch(Exception e)
                    { }
                }
            }
            return saveAward;
        }
        public static DataSet SaveAndUpdateTelephone(Int64 AFFILIATIONID, Int64 mode, string telephone, string XOldValue)
        { 
            var saveTelephone = ScivalEntities.Database.SqlQuery<DataSet>("SCI_AW_TELINSDEL @P_AFFILIATIONID,@P_INSDEL,@P_TELEPHONE,@X_TELEPHONE",
                          new SqlParameter("P_AFFILIATIONID", AFFILIATIONID),
                          new SqlParameter("P_INSDEL", mode),
                          new SqlParameter("P_TELEPHONE", telephone),
                          new SqlParameter("X_TELEPHONE", XOldValue)
                          ).FirstOrDefault();
            saveTelephone.Tables[1].TableName = "DisplayTelephone";
            return saveTelephone;
        }

        public static DataSet SaveAndUpdateFax(Int64 AFFILIATIONID, Int64 mode, string fax, string XOldValue)
        {
            var savefax = ScivalEntities.Database.SqlQuery<DataSet>("SCI_AW_FAXINSDEL @P_AFFILIATIONID,@P_INSDEL,@P_FAX,@X_FAX",
                            new SqlParameter("P_AFFILIATIONID", AFFILIATIONID),
                            new SqlParameter("P_INSDEL", mode),
                            new SqlParameter("P_FAX", fax),
                            new SqlParameter("X_FAX", XOldValue)
                            ).FirstOrDefault();
            savefax.Tables[1].TableName = "DisplayFax";
            return savefax;
        }
        public static DataSet DeleteClassification(Int64 WFId, Int64 mode, string type, Int64 frequency, string code, string ClaasText, string ClassFcID)
        {
            var UpClassification = ScivalEntities.Database.SqlQuery<DataSet>("sci_classificationupdel @p_workflowid,@p_insdel,@p_type,@p_FREQUENCY,@p_CODE,@p_CLASSIFICATION_TEXT," +
               "@p_CLASSIFICATIONS_ID",
                     new SqlParameter("p_workflowid", WFId),
                     new SqlParameter("p_insdel", mode),
                     new SqlParameter("p_type", type),
                     new SqlParameter("p_FREQUENCY", frequency),
                     new SqlParameter("p_CODE", code),
                     new SqlParameter("p_CLASSIFICATION_TEXT", ClaasText),
                     new SqlParameter("p_CLASSIFICATIONS_ID", ClassFcID)
                     ).FirstOrDefault();
            UpClassification.Tables[1].TableName = "DisplayData";
            return UpClassification;

        }
        public static DataSet GetFax(Int64 AFFILIATIONID)
        {
            var getfax = ScivalEntities.Database.SqlQuery<DataSet>("SCI_AW_faxlist @P_AFFILIATIONID",
                          new SqlParameter("P_AFFILIATIONID", AFFILIATIONID)
                          ).FirstOrDefault();
            getfax.Tables[1].TableName = "FaxData";
            return getfax;
        }

        public static DataSet GetTelephone(Int64 AFFILIATIONID)
        {
            var getfax = ScivalEntities.Database.SqlQuery<DataSet>("SCI_AW_TELlist @P_AFFILIATIONID",
                          new SqlParameter("P_AFFILIATIONID", AFFILIATIONID)
                          ).FirstOrDefault();
            getfax.Tables[1].TableName = "TelephoneData";
            return getfax;
        }
        public static DataSet GetAwardee(Int64 WFId)
        {
            var getAwardee = ScivalEntities.Database.SqlQuery<DataSet>("sci_Aw_awardeelist50 @p_workflowid",
                           new SqlParameter("p_workflowid", WFId)
                           ).FirstOrDefault();
            getAwardee.Tables[1].TableName = "DisplayData";
            return getAwardee;
        }

        public static DataSet Getopportunity_location(Int64 WFId)
        {
            var getOppLoc = ScivalEntities.Database.SqlQuery<DataSet>("award_location_list @p_workflowid",
                           new SqlParameter("p_workflowid", WFId)
                           ).FirstOrDefault();
            getOppLoc.Tables[1].TableName = "LocationList";
            return getOppLoc;
        }

        public static DataSet GetSubASJCTypeList()
        {
            var getSubASjcT = ScivalEntities.Database.SqlQuery<DataSet>("asjc_description_prc").FirstOrDefault();
            getSubASjcT.Tables[1].TableName = "SubASJCType";
            return getSubASjcT;
        }

        public static DataSet GetClassiFicatilList(Int64 wfId)
        {
            var getClsList = ScivalEntities.Database.SqlQuery<DataSet>("sci_classificationlist @p_workflowid",
                           new SqlParameter("p_workflowid", wfId)
                           ).FirstOrDefault();
            getClsList.Tables[1].TableName = "DisplayData";
            getClsList.Tables[2].TableName = "ASJCDesc";
            return getClsList;
        }

        public static DataSet GetContactsList(Int64 WFId, Int64 pagemode)
        {
            var getContactList = ScivalEntities.Database.SqlQuery<DataSet>("sci_aw_managerlist @p_workflowid,@p_awmanger",
                          new SqlParameter("p_workflowid", WFId),
                          new SqlParameter("p_awmanger", pagemode)
                          ).FirstOrDefault();
            getContactList.Tables[1].TableName = "ContactsList";
            return getContactList;
        }

        public static DataSet GetAwardBase(int wfid)
        {
            var getAwardBase = ScivalEntities.Database.SqlQuery<DataSet>("SCI_PROC_AWARDBASE @p_workflowid",
                          new SqlParameter("p_workflowid", wfid)
                          ).FirstOrDefault();
            getAwardBase.Tables[1].TableName = "Progress";
            return getAwardBase;
        }
        public static DataSet GetAddress(Int64 AFFILIATIONID)
        {
            var getAdd = ScivalEntities.Database.SqlQuery<DataSet>("SCI_AW_addresslist @P_AFFILIATIONID",
                          new SqlParameter("P_AFFILIATIONID", AFFILIATIONID)
                          ).FirstOrDefault();
            getAdd.Tables[1].TableName = "AddressData";
            return getAdd;
        }
        public static DataSet updatetitlebytxnid(Int64 p_trans_id, string p_title)
        {
            var uptitbytxnid = ScivalEntities.Database.SqlQuery<DataSet>("update_award_title @p_trans_id,@p_title",
                          new SqlParameter("p_trans_id", p_trans_id),
                          new SqlParameter("p_title", p_trans_id)
                          ).FirstOrDefault();
            uptitbytxnid.Tables[1].TableName = "title";
            return uptitbytxnid;
        }

        public static DataSet updateKeywords(Int64 p_workflowid, Int64 p_insdel, string p_LANG, string p_DESCRIPTION, string p_keywordid, string p_oldDESCRIPTION)
        {
            var upkeyword = ScivalEntities.Database.SqlQuery<DataSet>("sci_keywordinsert @p_workflowid,@p_insdel,@p_keywordid,@p_LANG,@p_DESCRIPTION,@p_oldDESCRIPTION",
                   new SqlParameter("p_workflowid", p_workflowid),
                   new SqlParameter("p_insdel", p_insdel),
                   new SqlParameter("p_keywordid", p_keywordid),
                   new SqlParameter("p_LANG", p_LANG),
                   new SqlParameter("p_DESCRIPTION", p_DESCRIPTION),
                   new SqlParameter("p_oldDESCRIPTION", null)
                   ).FirstOrDefault();
            upkeyword.Tables[1].TableName = "Keywords";
            return upkeyword;
        }
        public static string InsertDemoData(string TextDetail)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("value");
            string a = TextDetail;
            string HexValue = "", intermed = "";
            var insrtDemoData = ScivalEntities.Database.SqlQuery<DataSet>("Chk_ValidText @p_Text,@p_title",
                         new SqlParameter("p_Text", a),
                         new SqlParameter("p_TextN", TextDetail)
                         ).FirstOrDefault();
            insrtDemoData.Tables[1].TableName = "TEXT";

            char[] names1 = a.ToCharArray(); char[] names2 = insrtDemoData.Tables["TEXT"].Rows[0]["Text"].ToString().ToCharArray();
            IEnumerable<char> differenceQuery = names1.Except(names2);
            foreach (char s in differenceQuery)
            {
                HexValue = String.Format("{0:x4}", (uint)System.Convert.ToUInt32(s));
                HexValue = string.Concat("&#x", HexValue, ";");
                intermed = TextDetail.Replace(Convert.ToString(s), Convert.ToString(HexValue));
                TextDetail = intermed;
            }
            string originalv = ReturnInt(TextDetail);

            DataRow dr = dt.NewRow();
            dr[0] = TextDetail;

            dt.Rows.Add(dr);

            return TextDetail;
        }
        public static string ReturnInt(string str)
        {
            StringBuilder HexaValue = new StringBuilder();
            StringBuilder FinalString = new StringBuilder();
            int Count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '&' && str[i + 1] == '#')
                {
                    Count++;
                }
                if (Count > 0)
                {
                    HexaValue.Append(str[i]);
                }
                else
                {
                    FinalString.Append(str[i]);
                }

                if (str[i] == ';')
                {
                    Count = 0;
                    string SpecialCharacter = Convert.ToString(HexaValue).Replace("&#x", "").Replace(";", "");
                    char dd = System.Convert.ToChar(System.Convert.ToUInt32(SpecialCharacter, 16));
                    SpecialCharacter = Convert.ToString(dd);
                    if (SpecialCharacter == "" && Convert.ToString(HexaValue).Trim() != "")
                    {
                        SpecialCharacter = Convert.ToString(HexaValue).Trim();
                    }
                    FinalString.Append(SpecialCharacter);
                    HexaValue.Length = 0;
                }
            }
            return Convert.ToString(FinalString);
        }


        public static DataSet SaveArrayKeywordswithlang(Int64 p_workflowid, Int64 p_insdel, string p_LANG, string p_DESCRIPTION, Int64? p_keywordid = null)
        {
            var RelatedProg = ScivalEntities.Database.SqlQuery<DataSet>("sci_keywordinsert_avi @p_workflowid,@p_insdel,@p_keywordid," +
                "@p_LANG,@p_DESCRIPTION,@p_oldDESCRIPTION",
                     new SqlParameter("p_workflowid", p_workflowid),
                     new SqlParameter("p_insdel", p_insdel),
                     new SqlParameter("p_keywordid", p_keywordid),
                     new SqlParameter("p_LANG", p_LANG),
                     new SqlParameter("p_DESCRIPTION", p_DESCRIPTION),
                     new SqlParameter("p_oldDESCRIPTION", null)).FirstOrDefault();            
            return RelatedProg;
        }
    }
}
