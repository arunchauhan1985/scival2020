using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

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
            var awardList = ScivalEntities.Database.SqlQuery<AwardList>("CALL sci_aw_awexist(@p_fundingbodyid,@p_taskid,@p_updateflag,@p_userid)",
                new MySqlParameter("p_fundingbodyid", fundingBodyId), new MySqlParameter("p_taskid", taskId), new MySqlParameter("p_updateflag", updateFlag), new MySqlParameter("p_userid", userId))
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
            var startworkdtls = ScivalEntities.Database.SqlQuery<startwork>("CALL sci_inserttimesheet41(@p_workflowid,@p_userid)",
                new MySqlParameter("p_workflowid", workflowId), new MySqlParameter("p_userid", userId)).ToList();

            return startworkdtls;
        }

        public static List<ProgressTable> GetProgress(Int64 workId)
        {
            var ProgressDtls = ScivalEntities.Database.SqlQuery<ProgressTable>("CALL SCI_PROC_AWARDPROGRESS(@p_workflowid)",
                new MySqlParameter("p_workflowid", workId)).ToList();

            return ProgressDtls;
        }

        public static List<sci_language_master> GetLanguageMasterDetails(int LangLength)
        {
            return ScivalEntities.sci_language_master.Where(lM => lM.CODE_LENGTH == LangLength).OrderBy(lm => lm.LANGUAGE_NAME).ToList();
        }

        public static List<PageUrl> AddAndDeletePageURL(Int64 workId, string clickPage, string url, Int64 userId, int pagemode)
        {
            var ProgressDtls = ScivalEntities.Database.SqlQuery<PageUrl>("CALL sci_pageurls(@p_workflowid,@p_pagename,@p_url,@p_userid,@p_mode)",
                new MySqlParameter("p_workflowid", workId), new MySqlParameter("p_pagename", clickPage), new MySqlParameter("p_url", url), new MySqlParameter("p_userid", userId), new MySqlParameter("p_mode", pagemode)).ToList();

            return ProgressDtls;
        }


        public static List<PageUrl> GetURL(long workId, string clickPage)
        {
            var ProgressDtls = ScivalEntities.Database.SqlQuery<PageUrl>("CALL sci_urllinks(@p_workflowid,@p_pagename)",
                new MySqlParameter("p_workflowid", workId), new MySqlParameter("p_pagename", clickPage)).ToList();

            return ProgressDtls;
        }


        public static DataSet GetScholarlyOutput(long wfId)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", wfId)
            };
            var ScholarOutput = CommonDataOperation.ExecuteStoredProcedure("sci_aw_research_output", parameters);
            ScholarOutput.Tables[0].TableName = "DisplayData";
            ScholarOutput.Tables[1].TableName = "RelType";
            ScholarOutput.Tables[2].TableName = "Type";
            ScholarOutput.Tables[3].TableName = "Item";
            return ScholarOutput;
        }

        public static DataSet SaveAbdUpdateScholarlyOutputType_new(Int64 WFId, Int64 mode, Int64 Id, string p_Relation_type, string p_item_type, string p_Doi,
            string p_Pubmed, string p_Pmc, string p_Medline, string p_Scopus, string p_Item)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFId),
                  new MySqlParameter("p_insdel", mode),
                  new MySqlParameter("p_id", Id),
                  new MySqlParameter("p_Relation_type", p_Relation_type),
                  new MySqlParameter("p_item_type", p_item_type),
                  new MySqlParameter("p_Doi", p_Doi),
                  new MySqlParameter("p_Pubmed", p_Pubmed),
                  new MySqlParameter("p_Pmc", p_Pmc),
                  new MySqlParameter("p_Medline", p_Medline),
                  new MySqlParameter("p_Scopus", p_Scopus),
                  new MySqlParameter("p_Item", p_Item)
            };
            var saveScholarOutputType = CommonDataOperation.ExecuteStoredProcedure("sci_aw_research_out_insupdel", parameters);
            saveScholarOutputType.Tables[0].TableName = "ResearchOutcome";
            return saveScholarOutputType;
        }
        public static DataSet TimeSheetStopContinueForQC(long wFId, long userId, long transId, long PageIds, string remarkText)
        {
            var parameters = new List<MySqlParameter>
            {
                      new MySqlParameter("p_workflowid", wFId),
                      new MySqlParameter("p_userid", userId),
                      new MySqlParameter("p_TRANSITIONALID", transId),
                      new MySqlParameter("p_REMARKS", remarkText)
            };
            var TimeSheetStopContiForQA = CommonDataOperation.ExecuteStoredProcedure("sci_timesheetstopcontinue_QA", parameters);
            TimeSheetStopContiForQA.Tables[0].TableName = "Result";
            return TimeSheetStopContiForQA;
        }


        public static DataSet TimeSheetStopContinue(long wFId, long userId, long transId, long mode, string remarkText)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", wFId),
                new MySqlParameter("p_userid", userId),
                new MySqlParameter("p_TRANSITIONALID", transId),
                new MySqlParameter("p_REMARKS", remarkText)
            };
            return CommonDataOperation.ExecuteStoredProcedure("sci_timesheetstoP_CONTINUE", parameters);
        }

        public static DataSet TimeSheetStop(long wFId, long userId, long transId, long mode, string remarkText)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", wFId),
                new MySqlParameter("p_userid", userId),
                new MySqlParameter("p_TRANSITIONALID", transId),
                new MySqlParameter("p_mode", mode),
                new MySqlParameter("p_REMARKS", remarkText)
            };
            return CommonDataOperation.ExecuteStoredProcedure("sci_timesheetstop", parameters);
        }


        public static DataSet GetRelatedPrograms(long wfId)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", wfId)
            };
            var RelatedProg = CommonDataOperation.ExecuteStoredProcedure("sci_op_relatedprogeam", parameters);
            RelatedProg.Tables[1].TableName = "Hierarchy";
            RelatedProg.Tables[2].TableName = "ListData";
            RelatedProg.Tables[3].TableName = "DisplayData";
            return RelatedProg;
        }


        public static DataSet CheckAwardDuplicate(Int64 WFId, Int64 userid, Int64 transId, Int64 mode, Int64 queryMode, string taskName)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFId),
                new MySqlParameter("p_userid", userid),
                new MySqlParameter("p_TRANSITIONALID", transId),
                new MySqlParameter("p_mode", mode),
                 new MySqlParameter("p_QueryMode", queryMode),
                new MySqlParameter("p_TaskName", taskName)
            };
            return CommonDataOperation.ExecuteStoredProcedure("sci_check_award_duplicate", parameters);
        }


        public static DataSet SaveAbdUpdateRelatedLProgram(Int64 WFId, Int64 mode, Int64 NewId, string NewHieRar, string NewProtext, string NewRelType, Int64 oldId, string oldRelType, string OldProText)
        {
            var parameters = new List<MySqlParameter>
            {
               new MySqlParameter("p_workflowid", WFId),
               new MySqlParameter("p_insdel", mode),
               new MySqlParameter("p_id", NewId),
               new MySqlParameter("p_HIERARCHY", NewHieRar),
               new MySqlParameter("p_RELATEDPROGRAMTEXT", NewProtext),
               new MySqlParameter("p_RELTYPE", NewRelType),
               new MySqlParameter("x_id", oldId),
               new MySqlParameter("x_RELTYPE", oldRelType),
               new MySqlParameter("x_RELATEDPROGRAMTEXT", OldProText)
            };
            return CommonDataOperation.ExecuteStoredProcedure("sci_op_relatedprogeamins", parameters);
        }

        public static DataSet GetRelatedOpportunities(long workflowid)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", workflowid)
            };
            var RelatedOpportunity = CommonDataOperation.ExecuteStoredProcedure("sci_op_relatedopplist_prc", parameters);
            RelatedOpportunity.Tables[1].TableName = "RelatedOpp";
            RelatedOpportunity.Tables[2].TableName = "Opportunity";
            return RelatedOpportunity;
        }


        public static DataSet SaveAndDeleteRelatedOpps(Int64 WFID, Int64 mode, string OppDbId, Int64? reltype, string Description = "")
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFID),
            new MySqlParameter("p_insdel", mode),
            new MySqlParameter("p_orgdbid", OppDbId),
            new MySqlParameter("p_RELTYPE", mode),
             new MySqlParameter("p_Desc", reltype),
            new MySqlParameter("p_TaskName", Description)
            };
            var SaveDelRelatedOpp = CommonDataOperation.ExecuteStoredProcedure("sci_rel_opportunity_dml_prc5", parameters);
            SaveDelRelatedOpp.Tables[0].TableName = "RelatedOpp";
            SaveDelRelatedOpp.Tables[1].TableName = "Opportunity";
            return SaveDelRelatedOpp;
        }

        public static DataSet GetItemsList(Int64 WFId, Int64 pagemode)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFId),
                new MySqlParameter("p_mode", pagemode)
            };
            var ItemLst = CommonDataOperation.ExecuteStoredProcedure("sci_itemlist", parameters);
            ItemLst.Tables[1].TableName = "ItemListDisplay";
            return ItemLst;
        }

        public static DataSet SaveAndDeleteItemsLIst(Int64 WFId, Int64 pagemode, Int64 WorkMode, string Reltype, string Description,
            string URl, string UrlText, string lang, Int64 ItemId)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFId),
                new MySqlParameter("p_mode", pagemode),
                new MySqlParameter("p_insdel", WorkMode),
                new MySqlParameter("p_itemid", ItemId),
                new MySqlParameter("p_RELTYPE", Reltype),
                new MySqlParameter("p_DESCRIPTION", Description),
                new MySqlParameter("p_url", URl),
                new MySqlParameter("p_urltext", UrlText),
                new MySqlParameter("p_lang", lang)
            };
            var SaveDelItemLst = CommonDataOperation.ExecuteStoredProcedure("sci_iteminserttemp", parameters);
            SaveDelItemLst.Tables[0].TableName = "ItemListDisplay";
            return SaveDelItemLst;
        }
        public static DataSet GetAmount(long workflowid, long pagemode)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", workflowid),
                new MySqlParameter("p_mode", pagemode)
            };
            var amount = CommonDataOperation.ExecuteStoredProcedure("sci_op_estfundinglist", parameters);
            amount.Tables[1].TableName = "DisplayData";
            return amount;
        }
        public static DataSet GetRelatedOrgs(long workflowid)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", workflowid)
            };
            var RelatedOrg = CommonDataOperation.ExecuteStoredProcedure("sci_op_relatedorglist_y", parameters);
            RelatedOrg.Tables[1].TableName = "Hirarchy";
            RelatedOrg.Tables[2].TableName = "RelatedOrgs";
            RelatedOrg.Tables[3].TableName = "FundingBody";
            return RelatedOrg;
        }
        public static DataSet GetAutoLeadRelorgs(Int64 ID, Int64 moduleid, Int64 flag)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_id", ID),
                new MySqlParameter("p_moduleid", moduleid),
                new MySqlParameter("p_flag", flag)
            };
            var AutoleadRelorg = CommonDataOperation.ExecuteStoredProcedure("sci_auto_lead_relorgs_prc", parameters);
            AutoleadRelorg.Tables[1].TableName = "AutoLeadDetail";
            return AutoleadRelorg;
        }
        public static DataSet SaveAndDeleteRelatedOrgs(Int64 WFID, string Amount, string currency, Int64 mode, string Hieracrhy, string ORgDbId, string reltype, string relatedorgsid)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFID),
               new MySqlParameter("p_amount", Amount),
               new MySqlParameter("p_currency", currency),
               new MySqlParameter("p_insdel", mode),
                new MySqlParameter("p_HIERARCHY", Hieracrhy),
                new MySqlParameter("p_orgdbid", ORgDbId),
               new MySqlParameter("p_RELTYPE", reltype),
               new MySqlParameter("p_relatedorgsid", relatedorgsid)
            };
            var SaveDelRelatedOpp = CommonDataOperation.ExecuteStoredProcedure("sci_op_relorgins_y", parameters);
            SaveDelRelatedOpp.Tables[0].TableName = "FundingBody";
            return SaveDelRelatedOpp;
        }


        public static DataSet Save_Update_PublicationData(Int64 WFId, Int64 mode, string p_FundingBodyProjectId = "", string p_PublishedDate = null, string p_PublicationURL = "",
        Int64 p_PublicationOutputId = 0, string p_IngestionId = "", string p_JournalTitle = "", string p_Journalidentifier = "", string p_Authors = "", string p_Description = "")
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFId),
                 new MySqlParameter("p_insdel", mode),
                 new MySqlParameter("p_publication_ID", 0),
                 new MySqlParameter("p_FundingBodyProjectId", p_FundingBodyProjectId),
                  new MySqlParameter("p_PublishedDate", p_PublishedDate),
                 new MySqlParameter("p_PublicationURL", p_PublicationURL),
                  new MySqlParameter("p_PublicationOutputId", p_PublicationOutputId),
                 new MySqlParameter("p_IngestionId", p_IngestionId),
                 new MySqlParameter("p_JournalTitle", p_JournalTitle),
                  new MySqlParameter("p_Journalidentifier", p_Journalidentifier),
                 new MySqlParameter("p_Authors", p_Authors),
                 new MySqlParameter("p_Description", p_Description)
            };
            var SaveUpPublicationData = CommonDataOperation.ExecuteStoredProcedure("sci_Publication_out_insupdel", parameters);
            return SaveUpPublicationData;
        }

        public static DataSet SavePublication_Title(Int64 WFId, Int64 mode, string p_Lang = "", string p_Title = "")
        {
            var parameters = new List<MySqlParameter>
            {
             new MySqlParameter("p_workflowid", WFId),
             new MySqlParameter("p_insdel", mode),
             new MySqlParameter("p_publication_ID", 0),
             new MySqlParameter("p_Lang", p_Lang),
             new MySqlParameter("p_Title", p_Title)
            };
            var SavePublicationTitle = CommonDataOperation.ExecuteStoredProcedure("sci_PublicationTitle_insupdel", parameters);
            SavePublicationTitle.Tables[0].TableName = "PublicationTitle";
            return SavePublicationTitle;
        }
        public static DataSet getWorkFlowDetails(long wId)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("WorkFlowId", wId)
            };
            return CommonDataOperation.ExecuteStoredProcedure("select sw.ID, sw.MODULEID, am.fundingbodyid from sci_workflow sw, award_master am  where am.awardid=sw.id and workflowid=@WorkFlowId", parameters);
        }


        public static DataSet LoadLanguageData(string OPPID, int mode_id, int tran_type_id)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("OppId", OPPID),
                new MySqlParameter("Mode_id", mode_id)
            };
            return CommonDataOperation.ExecuteStoredProcedure("SELECT tran_id, scival_id, column_desc, column_id, column_name, moduleid, modulename, language_id, language_name, language_code," +
                " tran_type_id,tran_name, language_group_id, language_code FROM table (sci_language_detail_fnc (scival_id_in =>(@OppId, moduleid_in =>(@Mode_id)", parameters);

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
                       var parameters = new List<MySqlParameter>
                          {
                             new MySqlParameter("tran_id_in", AwardLang.Rows[x]["tran_id"]),
                             new MySqlParameter("scival_id_in", AwardLang.Rows[x]["AW_ID"]),
                             new MySqlParameter("column_desc_in", r.WieredChar_ReplacementHexValue(AwardLang.Rows[x]["COLUMN_DESC"].ToString())),
                             new MySqlParameter("column_id_in", AwardLang.Rows[x]["COLUMN_ID"]),
                             new MySqlParameter("mode_id_in", AwardLang.Rows[x]["MODULEID"]),
                             new MySqlParameter("LANGUAGE_ID", AwardLang.Rows[x]["LANGUAGE_ID"]),
                             new MySqlParameter("tran_type_id_in", AwardLang.Rows[x]["TRAN_TYPE_ID"]),
                             new MySqlParameter("flag_in", AwardLang.Rows[x]["FLAG_IN"])
                           };
                        SaveAwardLang = CommonDataOperation.ExecuteStoredProcedure("sci_language_detail_dml_prc", parameters);
                    }
                    catch
                    { }
                }
            }
            SaveAwardLang.Tables[0].TableName = "AwardLangTable";
            return SaveAwardLang;
        }


        public static DataSet Getfunding_details(long wFID)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", wFID)
            };
            var fundingDtls = CommonDataOperation.ExecuteStoredProcedure("get_funding_details", parameters);
            fundingDtls.Tables[1].TableName = "DisplayData";
            return fundingDtls;
        }

        public static DataSet SaveFunds(string url_txt_fundingBodyProjectId_f, string url_txt_Amount_f, string url_ddlCuurency_f, string url_txt_fundingBodyProjectId_h, string url_txt_Amount_h, string url_ddlCuurency_h, string url_txt_acronym, DateTime url_txtSrtDate, DateTime url_txtEndDateDate,
       string url_txtStatus, string url_txt_link, string url_txtpostofficeboxn, string url_TextStreet, string url_txt_locality, string url_txtregion,
       string url_txtPostalCode, string url_country, string url_LANGUAGE_NAME, Int64 COLUMN_ID, Int64 TRAN_TYPE_ID, string url_COLUMN_DESC, Int64 pagemode, Int64 WFID, Int64 mode, Int64 p_SEQUENCE_ID = 0)
        {
            var parameters = new List<MySqlParameter>
            {
              new MySqlParameter("P_fundingBodyProjectId_f", url_txt_fundingBodyProjectId_f),
              new MySqlParameter("P_txt_Amount_f", url_txt_Amount_f),
              new MySqlParameter("P_ddlCurrency_f", url_ddlCuurency_f),
              new MySqlParameter("P_fundingBodyProjectId_h", url_txt_fundingBodyProjectId_h),
              new MySqlParameter("P_Amount_h", url_txt_Amount_h),
              new MySqlParameter("P_ddlCurrency_h", url_ddlCuurency_h),
              new MySqlParameter("P_acronym", url_txt_acronym),
              new MySqlParameter("P_SrtDate", url_txtSrtDate),
              new MySqlParameter("P_EndDateDate", url_txtEndDateDate),
              new MySqlParameter("P_Status", url_txtStatus),
              new MySqlParameter("P_link", url_txt_link),
              new MySqlParameter("P_postofficeboxn", url_txtpostofficeboxn),
              new MySqlParameter("P_Street", url_TextStreet),
              new MySqlParameter("P_locality", url_txt_locality),
              new MySqlParameter("P_region", url_txtregion),
              new MySqlParameter("P_PostalCode", url_txtPostalCode),
              new MySqlParameter("P_COUNTRY", url_country),
              new MySqlParameter("P_COLUMN_DESC", url_COLUMN_DESC),
              new MySqlParameter("P_LANGUAGE_NAME", url_LANGUAGE_NAME),
              new MySqlParameter("P_COLUMN_ID", COLUMN_ID),
              new MySqlParameter("P_TRAN_TYPE_ID", TRAN_TYPE_ID),
              new MySqlParameter("P_WORKFLOWID", WFID),
              new MySqlParameter("P_pagemode", pagemode),
              new MySqlParameter("P_mode", mode),
              new MySqlParameter("P_SEQUENCE_ID", p_SEQUENCE_ID)
            };
            var savefund = CommonDataOperation.ExecuteStoredProcedure("save_funding_prc3", parameters);
            savefund.Tables[1].TableName = "DisplayData";
            return savefund;
        }

        public static DataSet GetFBId(long workflowid)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@workflowID", workflowid)
            };
            return CommonDataOperation.ExecuteStoredProcedure("select sci_get_fb_id (@workflowID) FBID from dual", parameters);

        }

        public static DataSet UpdateClassification(Int64 WFId, Int64 mode, string type, Int64 oldfrequency, string oldcode, Int64 frequency, string code, string ClaasText, string ClassFcID)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFId),
                      new MySqlParameter("p_insdel", mode),
                      new MySqlParameter("p_type", type),
                      new MySqlParameter("p_FREQUENCY", frequency),
                      new MySqlParameter("p_CODE", code),
                      new MySqlParameter("p_CLASSIFICATION_TEXT", ClaasText),
                      new MySqlParameter("p_CLASSIFICATIONS_ID", ClassFcID),
                      new MySqlParameter("O_FREQUENCY", oldfrequency),
                      new MySqlParameter("O_CODE", oldcode)
            };
            var UpClassification = CommonDataOperation.ExecuteStoredProcedure("sci_classificationupdel", parameters);
            UpClassification.Tables[1].TableName = "DisplayData";
            return UpClassification;
        }

        public static DataSet Getaffiliation(long AWARDEEID)
        {
             var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("P_AWARDEEID", AWARDEEID)
            };
            var affiliation = CommonDataOperation.ExecuteStoredProcedure("SCI_AW_INSTITUTIONLIST", parameters);
            affiliation.Tables[1].TableName = "AffiliationGrid";
            return affiliation;
        }


        public static DataSet SaveAndDeleteAwardee(Int64 WFId, Int64 mode, string type, Int64 scopusId, string indexName, string givenName, string initials, string surname, Int64 awardeeId, string currency, Int64 amount, string P_ORCID, string P_externalRes_Id)
        {            
            var parameters = new List<MySqlParameter>
            {
                 new MySqlParameter("p_workflowid", WFId),
                     new MySqlParameter("P_INSDEL", mode),
                     new MySqlParameter("P_TYPE", type),
                     new MySqlParameter("P_SCOPUSAUTHORID", scopusId),
                     new MySqlParameter("P_externalRes_Id", r.WieredChar_ReplacementHexValue(P_externalRes_Id)),
                     new MySqlParameter("P_ORCID", P_ORCID),
                     new MySqlParameter("P_INDEXEDNAME", r.WieredChar_ReplacementHexValue(indexName)),
                     new MySqlParameter("P_GIVENNAME", r.WieredChar_ReplacementHexValue(givenName)),
                     new MySqlParameter("P_INITIALS", r.WieredChar_ReplacementHexValue(initials)),
                     new MySqlParameter("P_SURNAME", r.WieredChar_ReplacementHexValue(surname)),
                     new MySqlParameter("p_AWARDEE_ID", awardeeId),
                     new MySqlParameter("p_currency", currency),
                     new MySqlParameter("p_amount", amount)
            };
            return CommonDataOperation.ExecuteStoredProcedure("sci_Aw_awardeeinsdel_41", parameters);


        }

        public static DataSet SaveAndDeleteAwardee50(Int64 WFId, Int64 mode, Int64 awardeeId, string activityType, string awardeeAffiliationId, string departmentName, string fundingBodyOrganizationId, string link, string name, string role, string vatNumber, string DUNS, string ROR, string WIKIDATA, string currency, Int64 amount)
        {
            var parameters = new List<MySqlParameter>
            {
                      new MySqlParameter("p_workflowid", WFId),
                     new MySqlParameter("P_INSDEL", mode),
                     new MySqlParameter("P_TYPE", role),
                     new MySqlParameter("P_SCOPUSAUTHORID", 0),
                     new MySqlParameter("P_externalRes_Id", r.WieredChar_ReplacementHexValue(vatNumber)),
                     new MySqlParameter("P_ORCID", r.WieredChar_ReplacementHexValue(fundingBodyOrganizationId)),
                     new MySqlParameter("P_INDEXEDNAME", r.WieredChar_ReplacementHexValue(departmentName)),
                     new MySqlParameter("P_GIVENNAME", r.WieredChar_ReplacementHexValue(name)),
                     new MySqlParameter("P_INITIALS", r.WieredChar_ReplacementHexValue(awardeeAffiliationId)),
                     new MySqlParameter("P_SURNAME", r.WieredChar_ReplacementHexValue(activityType)),
                     new MySqlParameter("p_AWARDEE_ID", awardeeId),
                     new MySqlParameter("p_currency", currency),
                     new MySqlParameter("p_amount", amount),
                     new MySqlParameter("P_link", r.WieredChar_ReplacementHexValue(link)),
                     new MySqlParameter("P_DUNS", r.WieredChar_ReplacementHexValue(DUNS)),
                     new MySqlParameter("P_ROR", r.WieredChar_ReplacementHexValue(ROR)),
                     new MySqlParameter("P_WIKIDATA", r.WieredChar_ReplacementHexValue(WIKIDATA))
            };
            return CommonDataOperation.ExecuteStoredProcedure("SCI_AW_AWARDEEINSDEL_50", parameters);

        }
        public static DataSet SaveAndDeleteAffiliation50(Int64 AWARDEEID, Int64 mode, Int64 affliationId, string awardeePersonId, string emailAddress, string familyName,
            string fundingBodyPersonId, string givenName, string ORCID_Aff, string initials_Aff, string role_Aff, string name_Aff)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("P_AWARDEEID", AWARDEEID),
                     new MySqlParameter("P_INSDEL", mode),
                     new MySqlParameter("P_SCOPUSINSTITUTIONID", null),
                     new MySqlParameter("P_ORG", r.WieredChar_ReplacementHexValue("")),
                     new MySqlParameter("P_DEPT", r.WieredChar_ReplacementHexValue("")),
                     new MySqlParameter("P_STARTDATE", null),
                     new MySqlParameter("P_ENDDATE", null),
                     new MySqlParameter("P_EMAIL", r.WieredChar_ReplacementHexValue(emailAddress)),
                     new MySqlParameter("P_WEBPAGE", r.WieredChar_ReplacementHexValue("")),
                     new MySqlParameter("P_EXTAFFI_Id", r.WieredChar_ReplacementHexValue("")),
                     new MySqlParameter("P_awardeePersonId", r.WieredChar_ReplacementHexValue(awardeePersonId)),
                     new MySqlParameter("P_familyName", r.WieredChar_ReplacementHexValue(familyName)),
                     new MySqlParameter("P_fundingBodyPersonId", r.WieredChar_ReplacementHexValue(fundingBodyPersonId)),
                     new MySqlParameter("P_givenName", r.WieredChar_ReplacementHexValue(givenName)),
                     new MySqlParameter("P_ORCID", r.WieredChar_ReplacementHexValue(ORCID_Aff)),
                     new MySqlParameter("P_initials", r.WieredChar_ReplacementHexValue(initials_Aff)),
                     new MySqlParameter("P_role", r.WieredChar_ReplacementHexValue(role_Aff)),
                     new MySqlParameter("P_name", r.WieredChar_ReplacementHexValue(name_Aff)),
                     new MySqlParameter("P_AFFILIATION_ID",affliationId)
            };
            var SaveAffiliation50 = CommonDataOperation.ExecuteStoredProcedure("SCI_AW_INSTITUTIONINSDEL50", parameters);
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
                    var parameters = new List<MySqlParameter>
                    {
                        new MySqlParameter("P_AWARDEEID", AWARDEEID),
                        new MySqlParameter("P_INSDEL", mode),
                        new MySqlParameter("P_SCOPUSINSTITUTIONID", institueId),
                        new MySqlParameter("P_ORG", r.WieredChar_ReplacementHexValue(org)),
                        new MySqlParameter("P_DEPT", r.WieredChar_ReplacementHexValue(dpart)),
                        new MySqlParameter("P_STARTDATE", srtDate),
                        new MySqlParameter("P_ENDDATE", endDate),
                        new MySqlParameter("P_EMAIL", r.WieredChar_ReplacementHexValue(email)),
                        new MySqlParameter("P_WEBPAGE", r.WieredChar_ReplacementHexValue(webpage)),
                        new MySqlParameter("P_EXTAFFI_Id", r.WieredChar_ReplacementHexValue(extAffi_Id)),
                        new MySqlParameter("P_AFFILIATION_ID", affliationId)
                     };
                      var SaveAffiliation = CommonDataOperation.ExecuteStoredProcedure("SCI_AW_INSTITUTIONINSDEL41", parameters);
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
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", p_workflowid),
                    new MySqlParameter("p_insdel", p_insdel),
                    new MySqlParameter("p_keywordid", p_keywordid),
                    new MySqlParameter("p_LANG", p_LANG),
                    new MySqlParameter("p_DESCRIPTION", p_DESCRIPTION),
                    new MySqlParameter("p_oldDESCRIPTION", null)
            };
            var keywithlang = CommonDataOperation.ExecuteStoredProcedure("sci_keywordinsert_avi", parameters);
            keywithlang.Tables[1].TableName = "Keywords";
            return keywithlang;
        }
        
        public static DataSet SaveAmount(Int32 TotalAmount, string InstallmentStart, string InstallmentEnd, Int64 WFID, string currency, string amount, Int64 pagemode, Int64 mode, Int64 p_SEQUENCE_ID = 0)
        {
            //var saveamt = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_OP_estfundingins_y(@p_workflowid,@p_currency,@p_amount,@p_amount_description,@p_total_amount,@p_startdate," +
            //    "@p_enddate,@p_url,@p_url_text,@p_mode,@PAGE_MODE,@p_SEQUENCE_ID)",
            //           new MySqlParameter("p_workflowid", WFID),
            //           new MySqlParameter("p_currency", currency.Trim().ToUpper()),
            //           new MySqlParameter("p_amount", amount),
            //           new MySqlParameter("p_amount_description", null),
            //           new MySqlParameter("p_total_amount", TotalAmount),
            //           new MySqlParameter("p_startdate", InstallmentStart),
            //           new MySqlParameter("p_enddate", InstallmentEnd),
            //           new MySqlParameter("p_url", ""),
            //           new MySqlParameter("p_url_text", ""),
            //           new MySqlParameter("p_mode", mode),
            //           new MySqlParameter("PAGE_MODE", pagemode),
            //           new MySqlParameter("p_SEQUENCE_ID", p_SEQUENCE_ID)
            //           ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFID),
                       new MySqlParameter("p_currency", currency.Trim().ToUpper()),
                       new MySqlParameter("p_amount", amount),
                       new MySqlParameter("p_amount_description", null),
                       new MySqlParameter("p_total_amount", TotalAmount),
                       new MySqlParameter("p_startdate", InstallmentStart),
                       new MySqlParameter("p_enddate", InstallmentEnd),
                       new MySqlParameter("p_url", ""),
                       new MySqlParameter("p_url_text", ""),
                       new MySqlParameter("p_mode", mode),
                       new MySqlParameter("PAGE_MODE", pagemode),
                       new MySqlParameter("p_SEQUENCE_ID", p_SEQUENCE_ID)
            };
            var saveamt = CommonDataOperation.ExecuteStoredProcedure("SCI_OP_estfundingins_y", parameters);
            return saveamt;
        }

        public static DataSet SaveAndDeleteContactsLIst(Int64 WFId, Int64 pagemode, Int64 WorkMode, Int64 Contact_Id, string TYPE, string TITLE, string TELEPHONE, string FAX, 
            string EMAIL, string url, string WEBSITE_TEXT, string COUNTRY, string ROOM, string STREET, string STATE, string CITY, string POSTALCODE, string PREFIX, 
            string GIVENNAME, string MIDDLENAME, string SURNAME, string SUFFIX)
        {
            //var savecontactlst = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_managerinsdel_1(@p_workflowid,@p_insdel,@p_contactid,@p_type,@p_title,@p_telephone," +
            //    "@p_fax,@p_email,@p_url,@p_website_text,@p_MANAGERID,@p_country,@p_room,@p_street,@p_state,@p_city,@p_postalcode,@p_prefix,@p_givenname,@p_middlename,@p_surname,@p_suffix)",
            //           new MySqlParameter("p_workflowid", WFId),
            //           new MySqlParameter("p_insdel", WorkMode),
            //           new MySqlParameter("p_contactid", Contact_Id),
            //           new MySqlParameter("p_type", r.WieredChar_ReplacementHexValue(TYPE)),
            //           new MySqlParameter("p_title", r.WieredChar_ReplacementHexValue(TITLE)),
            //           new MySqlParameter("p_telephone", TELEPHONE),
            //           new MySqlParameter("p_fax", FAX),
            //           new MySqlParameter("p_email", r.WieredChar_ReplacementHexValue(EMAIL)),
            //           new MySqlParameter("p_url", r.WieredChar_ReplacementHexValue(url)),
            //           new MySqlParameter("p_website_text", r.WieredChar_ReplacementHexValue(WEBSITE_TEXT)),
            //           new MySqlParameter("p_MANAGERID", Contact_Id),
            //           new MySqlParameter("p_country", COUNTRY),
            //           new MySqlParameter("p_room", r.WieredChar_ReplacementHexValue(ROOM)),
            //           new MySqlParameter("p_street", r.WieredChar_ReplacementHexValue(STREET)),
            //           new MySqlParameter("p_state", r.WieredChar_ReplacementHexValue(STATE)),
            //           new MySqlParameter("p_city", r.WieredChar_ReplacementHexValue(CITY)),
            //           new MySqlParameter("p_postalcode", POSTALCODE),
            //           new MySqlParameter("p_prefix", r.WieredChar_ReplacementHexValue(PREFIX)),
            //           new MySqlParameter("p_givenname", r.WieredChar_ReplacementHexValue(GIVENNAME)),
            //           new MySqlParameter("p_middlename", r.WieredChar_ReplacementHexValue(MIDDLENAME)),
            //           new MySqlParameter("p_surname", r.WieredChar_ReplacementHexValue(SURNAME)),
            //           new MySqlParameter("p_suffix", r.WieredChar_ReplacementHexValue(SUFFIX))
            //           ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFId),
                       new MySqlParameter("p_insdel", WorkMode),
                       new MySqlParameter("p_contactid", Contact_Id),
                       new MySqlParameter("p_type", r.WieredChar_ReplacementHexValue(TYPE)),
                       new MySqlParameter("p_title", r.WieredChar_ReplacementHexValue(TITLE)),
                       new MySqlParameter("p_telephone", TELEPHONE),
                       new MySqlParameter("p_fax", FAX),
                       new MySqlParameter("p_email", r.WieredChar_ReplacementHexValue(EMAIL)),
                       new MySqlParameter("p_url", r.WieredChar_ReplacementHexValue(url)),
                       new MySqlParameter("p_website_text", r.WieredChar_ReplacementHexValue(WEBSITE_TEXT)),
                       new MySqlParameter("p_MANAGERID", Contact_Id),
                       new MySqlParameter("p_country", COUNTRY),
                       new MySqlParameter("p_room", r.WieredChar_ReplacementHexValue(ROOM)),
                       new MySqlParameter("p_street", r.WieredChar_ReplacementHexValue(STREET)),
                       new MySqlParameter("p_state", r.WieredChar_ReplacementHexValue(STATE)),
                       new MySqlParameter("p_city", r.WieredChar_ReplacementHexValue(CITY)),
                       new MySqlParameter("p_postalcode", POSTALCODE),
                       new MySqlParameter("p_prefix", r.WieredChar_ReplacementHexValue(PREFIX)),
                       new MySqlParameter("p_givenname", r.WieredChar_ReplacementHexValue(GIVENNAME)),
                       new MySqlParameter("p_middlename", r.WieredChar_ReplacementHexValue(MIDDLENAME)),
                       new MySqlParameter("p_surname", r.WieredChar_ReplacementHexValue(SURNAME)),
                       new MySqlParameter("p_suffix", r.WieredChar_ReplacementHexValue(SUFFIX))
            };
            var savecontactlst = CommonDataOperation.ExecuteStoredProcedure("sci_managerinsdel_1", parameters);
            return savecontactlst;
        }

        public static DataSet SaveAndUpdateAddress(Int64 AFFILIATIONID, Int64 mode, string country, string Room, string Street, string City, string State, 
            string postalcode, string CountryOldValue, string RoomOldValue, string StreetOldValue, string CityOldValue, string StateOldValue, string PostalCodeOldValue)
        {
            //var saveAddress = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_AW_ADDRESSINSDEL(@P_AFFILIATION_ID_IN,@P_COUNTRY_IN,@P_ROOM_IN,@P_STREET_IN,@P_CITY_IN,@P_STATE_IN," +
            //    "@P_POSTALCODE_IN,@P_OLD_COUNTRY_IN,@P_OLD_ROOM_IN,@P_OLD_STREET_IN,@P_OLD_CITY_IN,@P_OLD_STATE_IN,@P_OLD_POSTALCODE_IN)",
            //           new MySqlParameter("P_AFFILIATION_ID_IN", AFFILIATIONID),
            //           new MySqlParameter("P_COUNTRY_IN", country),
            //           new MySqlParameter("P_ROOM_IN", Room),
            //           new MySqlParameter("P_STREET_IN", Street),
            //           new MySqlParameter("P_CITY_IN", City),
            //           new MySqlParameter("P_STATE_IN", State),
            //           new MySqlParameter("P_POSTALCODE_IN", postalcode),
            //           new MySqlParameter("P_OLD_COUNTRY_IN", CountryOldValue),
            //           new MySqlParameter("P_OLD_ROOM_IN", RoomOldValue),
            //           new MySqlParameter("P_OLD_STREET_IN", StreetOldValue),
            //           new MySqlParameter("P_OLD_CITY_IN", CityOldValue),
            //           new MySqlParameter("P_OLD_STATE_IN", StateOldValue),
            //           new MySqlParameter("P_OLD_POSTALCODE_IN", PostalCodeOldValue)
            //           ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                       new MySqlParameter("P_AFFILIATION_ID_IN", AFFILIATIONID),
                       new MySqlParameter("P_COUNTRY_IN", country),
                       new MySqlParameter("P_ROOM_IN", Room),
                       new MySqlParameter("P_STREET_IN", Street),
                       new MySqlParameter("P_CITY_IN", City),
                       new MySqlParameter("P_STATE_IN", State),
                       new MySqlParameter("P_POSTALCODE_IN", postalcode),
                       new MySqlParameter("P_OLD_COUNTRY_IN", CountryOldValue),
                       new MySqlParameter("P_OLD_ROOM_IN", RoomOldValue),
                       new MySqlParameter("P_OLD_STREET_IN", StreetOldValue),
                       new MySqlParameter("P_OLD_CITY_IN", CityOldValue),
                       new MySqlParameter("P_OLD_STATE_IN", StateOldValue),
                       new MySqlParameter("P_OLD_POSTALCODE_IN", PostalCodeOldValue)
            };
            var saveAddress = CommonDataOperation.ExecuteStoredProcedure("SCI_AW_ADDRESSINSDEL", parameters);
            saveAddress.Tables[1].TableName = "DisplayAddress";
            return saveAddress;
        }
        public static DataSet SaveUpDelLocation(Int64 WFId, Int64 insdel, string countrytest, string COUNTRY, string ROOM, string STREET, string CITY, string STATE, string POSTALCODE, Int64 location_id)
        {
            //var saveLocation   = ScivalEntities.Database.SqlQuery<DataSet>("CALL award_location_insupdel(@p_workflowid,@p_insdel,@p_countrytest,@p_country,@p_room,@p_STREET,@p_CITY," +
            //    "@p_STATE,@p_POSTALCODE,@p_location_id)",
            //           new MySqlParameter("p_workflowid", WFId),
            //           new MySqlParameter("p_insdel", insdel),
            //           new MySqlParameter("p_countrytest", r.WieredChar_ReplacementHexValue(countrytest)),
            //           new MySqlParameter("p_country", COUNTRY),
            //           new MySqlParameter("p_room", r.WieredChar_ReplacementHexValue(ROOM)),
            //           new MySqlParameter("p_STREET", r.WieredChar_ReplacementHexValue(STREET)),
            //           new MySqlParameter("p_CITY", r.WieredChar_ReplacementHexValue(CITY)),
            //           new MySqlParameter("p_STATE", r.WieredChar_ReplacementHexValue(STATE)),
            //           new MySqlParameter("p_POSTALCODE", POSTALCODE),
            //           new MySqlParameter("p_location_id", location_id)
            //           ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                     new MySqlParameter("p_workflowid", WFId),
                       new MySqlParameter("p_insdel", insdel),
                       new MySqlParameter("p_countrytest", r.WieredChar_ReplacementHexValue(countrytest)),
                       new MySqlParameter("p_country", COUNTRY),
                       new MySqlParameter("p_room", r.WieredChar_ReplacementHexValue(ROOM)),
                       new MySqlParameter("p_STREET", r.WieredChar_ReplacementHexValue(STREET)),
                       new MySqlParameter("p_CITY", r.WieredChar_ReplacementHexValue(CITY)),
                       new MySqlParameter("p_STATE", r.WieredChar_ReplacementHexValue(STATE)),
                       new MySqlParameter("p_POSTALCODE", POSTALCODE),
                       new MySqlParameter("p_location_id", location_id)
            };
            var saveLocation = CommonDataOperation.ExecuteStoredProcedure("award_location_insupdel", parameters);
            saveLocation.Tables[1].TableName = "LocationList";
            return saveLocation;
        }

        public static DataSet SaveClassiFication(Int64 WFId, string type, Int64 frequency, string[] code, string[] ClaasText)
        {
            //var saveClassification = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_classificationinsert(@p_workflowid,@p_type,@p_FREQUENCY,@p_CODE,@p_CLASSIFICATION_TEXT)",
            //              new MySqlParameter("p_workflowid", WFId),
            //              new MySqlParameter("p_type", type),
            //              new MySqlParameter("p_FREQUENCY", frequency),
            //              new MySqlParameter("p_CODE", code),
            //              new MySqlParameter("p_CLASSIFICATION_TEXT", ClaasText)
            //              ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                          new MySqlParameter("p_workflowid", WFId),
                          new MySqlParameter("p_type", type),
                          new MySqlParameter("p_FREQUENCY", frequency),
                          new MySqlParameter("p_CODE", code),
                          new MySqlParameter("p_CLASSIFICATION_TEXT", ClaasText)
            };
            var saveClassification = CommonDataOperation.ExecuteStoredProcedure("sci_classificationinsert", parameters);
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
                        //saveAward = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_aw_awradins50(@p_workflowid,@p_FUNDINGBODYAWARDID,@p_TYPE,@p_TRUSTING,@P_COLLECTIONCODE," +
                        //   "@P_HIDDEN,@P_NAME,@p_STARTDATE,@p_LASTAMENDEDDATE,@p_ENDDATE,@p_ABSTRACT,@p_url,@p_mode,@p_recordsource,@p_AwardNoticeDate,@p_PublishedDate)",
                        //new MySqlParameter("p_workflowid", Convert.ToInt64(Opportunity.Rows[x]["WFID"])),
                        //new MySqlParameter("p_FUNDINGBODYAWARDID", Opportunity.Rows[x]["FAID"].ToString()),
                        //new MySqlParameter("p_TYPE", Opportunity.Rows[x]["TYPEID"].ToString()),
                        //new MySqlParameter("p_TRUSTING", Opportunity.Rows[x]["TRUSTING"].ToString()),
                        //new MySqlParameter("P_COLLECTIONCODE", Opportunity.Rows[x]["COLLECTIONCODE"].ToString()),
                        //new MySqlParameter("P_HIDDEN", Opportunity.Rows[x]["HIDDEN"].ToString()),
                        //new MySqlParameter("P_NAME", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["NAME"].ToString())),
                        //new MySqlParameter("p_STARTDATE", Convert.ToString(Opportunity.Rows[x]["STARTDATE"])),
                        //new MySqlParameter("p_LASTAMENDEDDATE", Convert.ToString(Opportunity.Rows[x]["AMENDEDDATE"])),
                        //new MySqlParameter("p_ENDDATE", Convert.ToDateTime(Opportunity.Rows[x]["ENDDATE"])),
                        //new MySqlParameter("p_ABSTRACT", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["ABSTRACT"].ToString())),
                        //new MySqlParameter("p_url", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["URL"].ToString())),
                        //new MySqlParameter("p_mode", Convert.ToInt64(Opportunity.Rows[x]["mode"])),
                        //new MySqlParameter("p_recordsource", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["RECORDSOURCE"].ToString())),
                        //new MySqlParameter("p_AwardNoticeDate", Convert.ToDateTime(Opportunity.Rows[x]["AwardNoticeDate"])),
                        // new MySqlParameter("p_PublishedDate", Convert.ToDateTime(Opportunity.Rows[x]["PublishedDate"]))
                        // ).FirstOrDefault();
                        var parameters = new List<MySqlParameter>
            {
                          new MySqlParameter("p_workflowid", Convert.ToInt64(Opportunity.Rows[x]["WFID"])),
                         new MySqlParameter("p_FUNDINGBODYAWARDID", Opportunity.Rows[x]["FAID"].ToString()),
                         new MySqlParameter("p_TYPE", Opportunity.Rows[x]["TYPEID"].ToString()),
                         new MySqlParameter("p_TRUSTING", Opportunity.Rows[x]["TRUSTING"].ToString()),
                         new MySqlParameter("P_COLLECTIONCODE", Opportunity.Rows[x]["COLLECTIONCODE"].ToString()),
                         new MySqlParameter("P_HIDDEN", Opportunity.Rows[x]["HIDDEN"].ToString()),
                         new MySqlParameter("P_NAME", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["NAME"].ToString())),
                         new MySqlParameter("p_STARTDATE", Convert.ToString(Opportunity.Rows[x]["STARTDATE"])),
                         new MySqlParameter("p_LASTAMENDEDDATE", Convert.ToString(Opportunity.Rows[x]["AMENDEDDATE"])),
                         new MySqlParameter("p_ENDDATE", Convert.ToDateTime(Opportunity.Rows[x]["ENDDATE"])),
                         new MySqlParameter("p_ABSTRACT", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["ABSTRACT"].ToString())),
                         new MySqlParameter("p_url", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["URL"].ToString())),
                         new MySqlParameter("p_mode", Convert.ToInt64(Opportunity.Rows[x]["mode"])),
                         new MySqlParameter("p_recordsource", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["RECORDSOURCE"].ToString())),
                         new MySqlParameter("p_AwardNoticeDate", Convert.ToDateTime(Opportunity.Rows[x]["AwardNoticeDate"])),
                          new MySqlParameter("p_PublishedDate", Convert.ToDateTime(Opportunity.Rows[x]["PublishedDate"]))

            };
                        var saveClassification = CommonDataOperation.ExecuteStoredProcedure("sci_aw_awradins50", parameters);
                        saveAward.Tables[1].TableName = "FundingBodyTable";
                    }
                    catch
                    { }
                }
            }
            return saveAward;
        }
        public static DataSet SaveAndUpdateTelephone(Int64 AFFILIATIONID, Int64 mode, string telephone, string XOldValue)
        {
            //var saveTelephone = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_AW_TELINSDEL(@P_AFFILIATIONID,@P_INSDEL,@P_TELEPHONE,@X_TELEPHONE)",
            //              new MySqlParameter("P_AFFILIATIONID", AFFILIATIONID),
            //              new MySqlParameter("P_INSDEL", mode),
            //              new MySqlParameter("P_TELEPHONE", telephone),
            //              new MySqlParameter("X_TELEPHONE", XOldValue)
            //              ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                          new MySqlParameter("P_AFFILIATIONID", AFFILIATIONID),
                          new MySqlParameter("P_INSDEL", mode),
                          new MySqlParameter("P_TELEPHONE", telephone),
                          new MySqlParameter("X_TELEPHONE", XOldValue)

            };
            var saveTelephone = CommonDataOperation.ExecuteStoredProcedure("SCI_AW_TELINSDEL", parameters);
            saveTelephone.Tables[1].TableName = "DisplayTelephone";
            return saveTelephone;
        }

        public static DataSet SaveAndUpdateFax(Int64 AFFILIATIONID, Int64 mode, string fax, string XOldValue)
        {
            //var savefax = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_AW_FAXINSDEL(@P_AFFILIATIONID,@P_INSDEL,@P_FAX,@X_FAX)",
            //                new MySqlParameter("P_AFFILIATIONID", AFFILIATIONID),
            //                new MySqlParameter("P_INSDEL", mode),
            //                new MySqlParameter("P_FAX", fax),
            //                new MySqlParameter("X_FAX", XOldValue)
            //                ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                        new MySqlParameter("P_AFFILIATIONID", AFFILIATIONID),
                        new MySqlParameter("P_INSDEL", mode),
                        new MySqlParameter("P_FAX", fax),
                        new MySqlParameter("X_FAX", XOldValue)

            };
            var savefax = CommonDataOperation.ExecuteStoredProcedure("SCI_AW_FAXINSDEL", parameters);
            savefax.Tables[1].TableName = "DisplayFax";
            return savefax;
        }
        public static DataSet DeleteClassification(Int64 WFId, Int64 mode, string type, Int64 frequency, string code, string ClaasText, string ClassFcID)
        {
            //var UpClassification = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_classificationupdel(@p_workflowid,@p_insdel,@p_type,@p_FREQUENCY,@p_CODE,@p_CLASSIFICATION_TEXT," +
            //   "@p_CLASSIFICATIONS_ID)",
            //         new MySqlParameter("p_workflowid", WFId),
            //         new MySqlParameter("p_insdel", mode),
            //         new MySqlParameter("p_type", type),
            //         new MySqlParameter("p_FREQUENCY", frequency),
            //         new MySqlParameter("p_CODE", code),
            //         new MySqlParameter("p_CLASSIFICATION_TEXT", ClaasText),
            //         new MySqlParameter("p_CLASSIFICATIONS_ID", ClassFcID)
            //         ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                     new MySqlParameter("p_workflowid", WFId),
                     new MySqlParameter("p_insdel", mode),
                     new MySqlParameter("p_type", type),
                     new MySqlParameter("p_FREQUENCY", frequency),
                     new MySqlParameter("p_CODE", code),
                     new MySqlParameter("p_CLASSIFICATION_TEXT", ClaasText),
                     new MySqlParameter("p_CLASSIFICATIONS_ID", ClassFcID)
            };
            var UpClassification = CommonDataOperation.ExecuteStoredProcedure("sci_classificationupdel", parameters);
            UpClassification.Tables[1].TableName = "DisplayData";
            return UpClassification;

        }
        public static DataSet GetFax(Int64 AFFILIATIONID)
        {
            //var getfax = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_AW_faxlist(@P_AFFILIATIONID)",
            //              new MySqlParameter("P_AFFILIATIONID", AFFILIATIONID)
            //              ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("P_AFFILIATIONID", AFFILIATIONID)
            };
            var getfax = CommonDataOperation.ExecuteStoredProcedure("SCI_AW_faxlist", parameters);
            getfax.Tables[1].TableName = "FaxData";
            return getfax;
        }

        public static DataSet GetTelephone(Int64 AFFILIATIONID)
        {
            //var getfax = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_AW_TELlist(@P_AFFILIATIONID)",
            //              new MySqlParameter("P_AFFILIATIONID", AFFILIATIONID)
            //              ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("P_AFFILIATIONID", AFFILIATIONID)
            };
            var getfax = CommonDataOperation.ExecuteStoredProcedure("SCI_AW_TELlist", parameters);
            getfax.Tables[1].TableName = "TelephoneData";
            return getfax;
        }
        public static DataSet GetAwardee(Int64 WFId)
        {
            //var getAwardee = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_Aw_awardeelist50(@p_workflowid)",
            //               new MySqlParameter("p_workflowid", WFId)
            //               ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFId)
            };
            var getAwardee = CommonDataOperation.ExecuteStoredProcedure("sci_Aw_awardeelist50", parameters);
            getAwardee.Tables[1].TableName = "DisplayData";
            return getAwardee;
        }

        public static DataSet Getopportunity_location(Int64 WFId)
        {
            //var getOppLoc = ScivalEntities.Database.SqlQuery<DataSet>("CALL award_location_list(@p_workflowid)",
            //               new MySqlParameter("p_workflowid", WFId)
            //               ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFId)
            };
            var getOppLoc = CommonDataOperation.ExecuteStoredProcedure("award_location_list", parameters);
            getOppLoc.Tables[1].TableName = "LocationList";
            return getOppLoc;
        }

        public static DataSet GetSubASJCTypeList()
        {
            //var getSubASjcT = ScivalEntities.Database.SqlQuery<DataSet>("CALL asjc_description_prc").FirstOrDefault();
            var getSubASjcT = CommonDataOperation.ExecuteStoredProcedureWithoutParam("asjc_description_prc");
            getSubASjcT.Tables[1].TableName = "SubASJCType";
            return getSubASjcT;
        }

        public static DataSet GetClassiFicatilList(Int64 wfId)
        {
            //var getClsList = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_classificationlist(@p_workflowid)",
            //               new MySqlParameter("p_workflowid", wfId)
            //               ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", wfId)
            };
            var getClsList = CommonDataOperation.ExecuteStoredProcedure("sci_classificationlist", parameters);
            getClsList.Tables[1].TableName = "DisplayData";
            getClsList.Tables[2].TableName = "ASJCDesc";
            return getClsList;
        }

        public static DataSet GetContactsList(Int64 WFId, Int64 pagemode)
        {
            //var getContactList = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_aw_managerlist(@p_workflowid,@p_awmanger)",
            //              new MySqlParameter("p_workflowid", WFId),
            //              new MySqlParameter("p_awmanger", pagemode)
            //              ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                 new MySqlParameter("p_workflowid", WFId),
                 new MySqlParameter("p_awmanger", pagemode)
            };
            var getContactList = CommonDataOperation.ExecuteStoredProcedure("sci_aw_managerlist", parameters);
            getContactList.Tables[1].TableName = "ContactsList";
            return getContactList;
        }

        public static DataSet GetAwardBase(int wfid)
        {
            //var getAwardBase = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_PROC_AWARDBASE(@p_workflowid)",
            //              new MySqlParameter("p_workflowid", wfid)
            //              ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", wfid)
            };
            var getAwardBase = CommonDataOperation.ExecuteStoredProcedure("SCI_PROC_AWARDBASE", parameters);
            getAwardBase.Tables[1].TableName = "Progress";
            return getAwardBase;
        }
        public static DataSet GetAddress(Int64 AFFILIATIONID)
        {
            //var getAdd = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_AW_addresslist(@P_AFFILIATIONID)",
            //              new MySqlParameter("P_AFFILIATIONID", AFFILIATIONID)
            //              ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("P_AFFILIATIONID", AFFILIATIONID)
            };
            var getAdd = CommonDataOperation.ExecuteStoredProcedure("SCI_AW_addresslist", parameters);

            getAdd.Tables[1].TableName = "AddressData";
            return getAdd;
        }
        public static DataSet updatetitlebytxnid(Int64 p_trans_id, string p_title)
        {
            //var uptitbytxnid = ScivalEntities.Database.SqlQuery<DataSet>("CALL update_award_title(@p_trans_id,@p_title)",
            //              new MySqlParameter("p_trans_id", p_trans_id),
            //              new MySqlParameter("p_title", p_trans_id)
            //              ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                 new MySqlParameter("p_trans_id", p_trans_id),
                          new MySqlParameter("p_title", p_trans_id)
            };
            var uptitbytxnid = CommonDataOperation.ExecuteStoredProcedure("update_award_title", parameters);
            uptitbytxnid.Tables[1].TableName = "title";
            return uptitbytxnid;
        }

        public static DataSet updateKeywords(Int64 p_workflowid, Int64 p_insdel, string p_LANG, string p_DESCRIPTION, string p_keywordid, string p_oldDESCRIPTION)
        {
            //var upkeyword = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_keywordinsert(@p_workflowid,@p_insdel,@p_keywordid,@p_LANG,@p_DESCRIPTION,@p_oldDESCRIPTION)",
            //       new MySqlParameter("p_workflowid", p_workflowid),
            //       new MySqlParameter("p_insdel", p_insdel),
            //       new MySqlParameter("p_keywordid", p_keywordid),
            //       new MySqlParameter("p_LANG", p_LANG),
            //       new MySqlParameter("p_DESCRIPTION", p_DESCRIPTION),
            //       new MySqlParameter("p_oldDESCRIPTION", null)
            //       ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                 new MySqlParameter("p_workflowid", p_workflowid),
                   new MySqlParameter("p_insdel", p_insdel),
                   new MySqlParameter("p_keywordid", p_keywordid),
                   new MySqlParameter("p_LANG", p_LANG),
                   new MySqlParameter("p_DESCRIPTION", p_DESCRIPTION),
                   new MySqlParameter("p_oldDESCRIPTION", null)
            };
            var upkeyword = CommonDataOperation.ExecuteStoredProcedure("sci_keywordinsert", parameters);
            upkeyword.Tables[1].TableName = "Keywords";
            return upkeyword;
        }
        public static string InsertDemoData(string TextDetail)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("value");
            string a = TextDetail;
            string HexValue = "", intermed = "";
            //var insrtDemoData = ScivalEntities.Database.SqlQuery<DataSet>("CALL Chk_ValidText(@p_Text,@p_title)",
            //             new MySqlParameter("p_Text", a),
            //             new MySqlParameter("p_TextN", TextDetail)
            //             ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                   new MySqlParameter("p_Text", a),
                         new MySqlParameter("p_TextN", TextDetail)
            };
            var insrtDemoData = CommonDataOperation.ExecuteStoredProcedure("Chk_ValidText", parameters);

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
    }
}
