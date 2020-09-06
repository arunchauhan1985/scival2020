using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;

namespace MySqlDal
{
    public static class OpportunityDataOperations
    {
        static string url = string.Empty;
        static List<DashboardTask> dashboardTasks = null;
        static List<DashboardRemark> remarks = null;
        static Int64 workflowId = 0;
        static Replace r = new Replace();

        private static ScivalEntities ScivalEntities { get { return ScivalEntitiesInstance.GetInstance(); } }

        public static List<OpportunityList> GetOpportunityListsByTask(Int64 fundingBodyId, Int64 taskId, Int32 updateFlag, Int64 userId)
        {
            var opportunityList = ScivalEntities.Database.SqlQuery<OpportunityList>("CALL sci_op_opexist(@P_FUNDINGBODYID,@p_taskid,@p_updateflag,@p_userid)",
                new MySqlParameter("P_FUNDINGBODYID", fundingBodyId), new MySqlParameter("p_taskid", taskId), new MySqlParameter("p_updateflag", updateFlag),
                new MySqlParameter("v", userId))
                .ToList();

            return opportunityList;
        }

        public static Int64 GetOpportunityWorkflowId(Int64 fundingBodyId, Int64 userId)
        {
            var count = (from om in ScivalEntities.opportunity_master
                         join sw in ScivalEntities.sci_workflow on om.OPPORTUNITYID equals sw.ID
                         where om.FUNDINGBODYID == fundingBodyId && om.OPPORTUNITYNAME == null && sw.TASKID == 1 && sw.STATUSID == null
                         select new { om.OPPORTUNITYID })
                         .Count();

            Int64 opportunityId, workflowId = 0;

            if (count > 0)
            {
                var opportunity = (from om in ScivalEntities.opportunity_master
                                   join sw in ScivalEntities.sci_workflow on om.OPPORTUNITYID equals sw.ID
                                   where om.FUNDINGBODYID == fundingBodyId && om.OPPORTUNITYNAME == null && sw.TASKID == 1 && sw.STATUSID == null
                                   select new { om.OPPORTUNITYID, sw.WORKFLOWID })
                                   .FirstOrDefault();

                opportunityId = opportunity.OPPORTUNITYID;
                workflowId = opportunity.WORKFLOWID;

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
                var templateId = ScivalEntities.sci_defaulttemplate.Where(t => t.ACTIVE == 1 && t.MODULEID == 3).Select(t => t.TEMPLATEID).FirstOrDefault();

                var oppId = ScivalEntities.opportunity_master.Max(op => op.OPPORTUNITYID);
                oppId = oppId + 1;

                opportunity_master opportunity = new opportunity_master { OPPORTUNITYID = oppId, FUNDINGBODYID = fundingBodyId, CREATEDBY = userId, CYCLE = 0, STATUSCODE = 1 };

                ScivalEntities.opportunity_master.Add(opportunity);
                ScivalEntities.SaveChanges();

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
                            MODULEID = 3,
                            ID = oppId,
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
                            MODULEID = 3,
                            ID = oppId,
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
            List<DashboardUserFunding> dashboardUserFunding = null;
            dashboardTasks = null;
            remarks = null;
            url = string.Empty;

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

            var fundingBodyId = ScivalEntities.opportunity_master.Where(o => o.OPPORTUNITYID == workflow.ID).Select(o => o.FUNDINGBODYID).FirstOrDefault();

            url = ScivalEntities.fundingbody_master.Where(f => f.FUNDINGBODY_ID == fundingBodyId).Select(f => f.URL).FirstOrDefault();

            if (workflow.MODULEID == 3)
            {
                dashboardUserFunding = (from ab in ScivalEntities.oppotunities where ab.OPPORTUNITY_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Opportunity", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.oppotunities where ab.OPPORTUNITY_ID == workflow.ID && ab.DATE_FLAG == 1 group ab by 1 into a select new DashboardUserFunding { Tab = "OpportunityDates", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.eligibilityclassifications where ab.OPPORTUNITY_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Eligibility Classification", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.estimatedfundings where ab.OPPORTUNITY_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Estimated Funding", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.awardceilings where ab.OPPORTUNITY_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Award Ceiling", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.synopses where ab.OPPORTUNITY_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Synopsis", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.opp_subjectmatter where Convert.ToInt64(ab.OPPORTUNITY_ID) == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Subject Matter", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.classificationgroups where ab.OPPORTUNITY_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Classification Group", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.opportunity_location where ab.OPPORTUNITY_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Opportunity Location", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.sci_related_opportunity where ab.OPPORTUNITY_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Related Opportunities", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.relatedorgs where ab.OPPORTUNITY_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Related Funding Bodies", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.changehistories where ab.OPPORTUNITY_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Change History", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.eligibilitydescriptions where ab.OPPORTUNITY_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Eligibility Description", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.limitedsubmissiondescriptions where ab.OPPORTUNITY_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Limited Submission Description", Count = a.Count() })
                    .Concat(from ab in ScivalEntities.estimatedamountdescriptions where ab.OPPORTUNITY_ID == workflow.ID group ab by 1 into a select new DashboardUserFunding { Tab = "Estimated Amount Description", Count = a.Count() })
                    .ToList();

                dashboardTasks = (from sw in ScivalEntities.sci_workflowtemplate
                                  join st in ScivalEntities.sci_tasks on sw.TASKID equals st.TASKID
                                  where sw.TEMPLATEID == 2
                                  orderby sw.SEQUENCE
                                  select new DashboardTask { TaskName = st.TASKNAME, TaskId = st.TASKID, Sequence = sw.SEQUENCE.Value })
                             .ToList();

                remarks = (from str in ScivalEntities.sci_timesheetremarks
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
            return url;
        }

        public static List<DashboardTask> GetDashBoardDetailsTask()
        {
            return dashboardTasks;
        }

        public static List<DashboardRemark> GetDashBoardDetailsRemarks()
        {
            return remarks;
        }

        public static Int64 GetDashBoardDetailsWorkflowId()
        {
            return workflowId;
        }

        public static List<DashboardUserFunding> GetExpiryDashBoardDetail(Int64 opportunityId, Int64 userId, string userName)
        {
            dashboardTasks = null;
            remarks = null;
            url = string.Empty;
            workflowId = 0;

            var expiredOpportunity = ScivalEntities.sci_expired_opplist.Where(o => o.ID == opportunityId && o.FLAG != "1").ToList();

            foreach (sci_expired_opplist opplist in expiredOpportunity)
                opplist.FLAG = "1";

            ScivalEntities.SaveChanges();

            var cycle = ScivalEntities.opportunity_master.Where(om => om.OPPORTUNITYID == opportunityId).Select(om => om.CYCLE).FirstOrDefault();

            var count = ScivalEntities.sci_workflow.Where(wf => wf.ID == opportunityId && wf.CYCLE == cycle && wf.TASKID != 7).Count();

            if (count > 0)
                throw new ScivalDataException("Critical Error.");

            count = ScivalEntities.sci_workflow.Where(wf => wf.ID == opportunityId && wf.CYCLE == cycle && wf.TASKID == 7).Count();

            var maxWorkflowId = ScivalEntities.sci_workflow.Max(wf => wf.WORKFLOWID);

            workflowId = maxWorkflowId + 1;

            sci_workflow workflow = new sci_workflow
            {
                WORKFLOWID = workflowId,
                MODULEID = 3,
                ID = opportunityId,
                CYCLE = cycle,
                TEMPLATEID = 2,
                TASKID = 7,
                SEQUENCE = 1,
                CREATEDBY = 100,
                STATUSID = 7,
                STARTDATE = DateTime.Now,
                STARTBY = userId
            };

            if (count > 0)
            {
                var workflowList = ScivalEntities.sci_workflow.Where(wf => wf.ID == opportunityId && wf.TASKID == 7);

                var workflowIds = workflowList.Select(wf => wf.WORKFLOWID).ToList();

                var timeSheetRemarks = ScivalEntities.sci_timesheetremarks.Where(t => workflowIds.Contains(t.WORKFLOWID.Value)).ToList();
                var timeSheet = ScivalEntities.sci_timesheet.Where(t => workflowIds.Contains(t.WORKFLOWID.Value)).ToList();

                ScivalEntities.sci_timesheetremarks.RemoveRange(timeSheetRemarks);
                ScivalEntities.sci_timesheet.RemoveRange(timeSheet);
                ScivalEntities.sci_workflow.RemoveRange(workflowList.ToList());

                var oppMasters = ScivalEntities.opportunity_master.Where(om => om.OPPORTUNITYID == opportunityId).ToList();

                foreach (opportunity_master master in oppMasters)
                {
                    master.CYCLECOMPLETIONDATE = null;
                    master.CYCLECOMPLETEDBY = null;
                }
            }

            ScivalEntities.SaveChanges();

            var fundingBodyId = ScivalEntities.opportunity_master.Where(o => o.OPPORTUNITYID == opportunityId).Select(o => o.FUNDINGBODYID).FirstOrDefault();

            url = ScivalEntities.fundingbody_master.Where(f => f.FUNDINGBODY_ID == fundingBodyId).Select(f => f.URL).FirstOrDefault();

            List<DashboardUserFunding> dashboardUserFunding = (from ab in ScivalEntities.oppotunities where ab.OPPORTUNITY_ID == opportunityId group ab by 1 into a select new DashboardUserFunding { Tab = "Opportunity", Count = a.Count() })
                .Concat(from ab in ScivalEntities.oppotunities where ab.OPPORTUNITY_ID == opportunityId && ab.DATE_FLAG == 1 group ab by 1 into a select new DashboardUserFunding { Tab = "Opportunity Dates", Count = a.Count() })
                .Concat(from ab in ScivalEntities.eligibilityclassifications where ab.OPPORTUNITY_ID == opportunityId group ab by 1 into a select new DashboardUserFunding { Tab = "Eligibility Classification", Count = a.Count() })
                .Concat(from ab in ScivalEntities.estimatedfundings where ab.OPPORTUNITY_ID == opportunityId group ab by 1 into a select new DashboardUserFunding { Tab = "Estimated Funding", Count = a.Count() })
                .Concat(from ab in ScivalEntities.awardceilings where ab.OPPORTUNITY_ID == opportunityId group ab by 1 into a select new DashboardUserFunding { Tab = "Award Ceiling", Count = a.Count() })
                .Concat(from ab in ScivalEntities.synopses where ab.OPPORTUNITY_ID == opportunityId group ab by 1 into a select new DashboardUserFunding { Tab = "Synopsis", Count = a.Count() })
                .Concat(from ab in ScivalEntities.opp_subjectmatter where Convert.ToInt64(ab.OPPORTUNITY_ID) == opportunityId group ab by 1 into a select new DashboardUserFunding { Tab = "Subject Matter", Count = a.Count() })
                .Concat(from ab in ScivalEntities.classificationgroups where ab.OPPORTUNITY_ID == opportunityId group ab by 1 into a select new DashboardUserFunding { Tab = "Classification Group", Count = a.Count() })
                .Concat(from ab in ScivalEntities.opportunity_location where ab.OPPORTUNITY_ID == opportunityId group ab by 1 into a select new DashboardUserFunding { Tab = "Opportunity Location", Count = a.Count() })
                .Concat(from ab in ScivalEntities.sci_related_opportunity where ab.OPPORTUNITY_ID == opportunityId group ab by 1 into a select new DashboardUserFunding { Tab = "Related Opportunities", Count = a.Count() })
                .Concat(from ab in ScivalEntities.relatedorgs where ab.OPPORTUNITY_ID == opportunityId group ab by 1 into a select new DashboardUserFunding { Tab = "Related Funding Bodies", Count = a.Count() })
                .Concat(from ab in ScivalEntities.changehistories where ab.OPPORTUNITY_ID == opportunityId group ab by 1 into a select new DashboardUserFunding { Tab = "Change History", Count = a.Count() })
                .Concat(from ab in ScivalEntities.eligibilitydescriptions where ab.OPPORTUNITY_ID == opportunityId group ab by 1 into a select new DashboardUserFunding { Tab = "Eligibility Description", Count = a.Count() })
                .Concat(from ab in ScivalEntities.limitedsubmissiondescriptions where ab.OPPORTUNITY_ID == opportunityId group ab by 1 into a select new DashboardUserFunding { Tab = "Limited Submission Description", Count = a.Count() })
                .Concat(from ab in ScivalEntities.estimatedamountdescriptions where ab.OPPORTUNITY_ID == opportunityId group ab by 1 into a select new DashboardUserFunding { Tab = "Estimated Amount Description", Count = a.Count() })
                .ToList();

            dashboardTasks = (from sw in ScivalEntities.sci_workflowtemplate
                              join st in ScivalEntities.sci_tasks on sw.TASKID equals st.TASKID
                              where sw.TEMPLATEID == 2
                              orderby sw.SEQUENCE
                              select new DashboardTask { TaskName = st.TASKNAME, TaskId = st.TASKID, Sequence = sw.SEQUENCE.Value })
                         .ToList();

            remarks = (from str in ScivalEntities.sci_timesheetremarks
                       join sw in ScivalEntities.sci_workflow on str.WORKFLOWID equals sw.WORKFLOWID
                       where sw.MODULEID == 3 && sw.ID == opportunityId && sw.CYCLE == cycle
                       orderby str.CREATEDDATE
                       select new DashboardRemark
                       {
                           Remark = str.REMARKS,
                           Task = (sw.TASKID == 1) ? "Collection" : (sw.TASKID == 7) ? "Expiry" : "Quality Check",
                           UserName = userName,
                           CreatedDate = str.CREATEDDATE.Value,
                           CreatedDate1 = str.CREATEDDATE.Value.ToString("DD-Mon-YYYY HH:MI AM")
                       }
                )
                .ToList();

            return dashboardUserFunding;
        }

        public static List<change> GetChangeHistory(Int64 workflowId)
        {
            var changeList = ScivalEntities.Database.SqlQuery<change>("CALL sci_op_changehistorylist(@p_workflowid)", new MySqlParameter("p_workflowid", workflowId)).ToList();

            return changeList;
        }

        public static string GetOpportunityStatus(Int64 workflowId)
        {
            var status = (from om in ScivalEntities.opportunity_master
                          join wf in ScivalEntities.sci_workflow on om.OPPORTUNITYID equals wf.ID
                          where wf.WORKFLOWID == workflowId
                          select om.STATUSCODE).First();

            switch (status.Value)
            {
                case 1:
                    return "new";
                case 2:
                    return "update";
                case 3:
                    return "delete";
            }

            return string.Empty;
        }

        public static List<change> SaveAndUpdateChangeHistory(Int64? workflowId, Int64? opportunityId, string oldType, DateTime? oldPostTime, string oldChangeText, Int64 mode, Int64 changeHistoryId)
        {
            var changeList = ScivalEntities.Database.SqlQuery<change>("CALL sci_op_changehistoryins(@p_workflowid,@p_opportunity_id,@p_TYPE,@p_POSTDATE,@p_CHANGE_TEXT,@p_mode,@p_change_id)",
                new MySqlParameter("p_workflowid", workflowId), new MySqlParameter("p_opportunity_id", opportunityId), new MySqlParameter("p_TYPE", oldType), new MySqlParameter("p_POSTDATE", oldPostTime),
                new MySqlParameter("p_CHANGE_TEXT", oldChangeText), new MySqlParameter("p_mode", mode), new MySqlParameter("p_change_id", changeHistoryId)).ToList();

            return changeList;
        }

        public static void TrackUnstoppedFbAwardOpp(string p_workflowid, string UserID, string P_pageName)
        {
            //ScivalEntities.Database.SqlQuery<change>("CALL SCI_TrackUnstoppedFbAwardOpp(@P_WORKFLOWID,@P_USERID,@P_PAGENAME)",
            //    new MySqlParameter("P_WORKFLOWID", p_workflowid), new MySqlParameter("P_USERID", UserID), new MySqlParameter("P_PAGENAME", P_pageName)).ToList();
            var parameters = new List<MySqlParameter>
            {
              new MySqlParameter("P_WORKFLOWID", p_workflowid), 
                new MySqlParameter("P_USERID", UserID), 
                new MySqlParameter("P_PAGENAME", P_pageName)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("SCI_TrackUnstoppedFbAwardOpp", parameters);

        }

        public static List<ClassificationList> GetClassificationLists(Int64 workflowId)
        {
            return ScivalEntities.Database.SqlQuery<ClassificationList>("CALL sci_classificationlist(@p_workflowid)",
                new MySqlParameter("p_workflowid", workflowId)).ToList();
        }

        public static List<sci_classificationstypetype> GetClassificationsTypes()
        {
            return ScivalEntities.sci_classificationstypetype.ToList();
        }

        public static List<sci_asjcdescription> GetAsjcDescriptionsAsDetail()
        {
            return ScivalEntities.sci_asjcdescription.Where(a => a.DESCRIPTION == a.DETAIL).OrderBy(a => a.DESCRIPTION).ToList();
        }

        public static List<CustomAsjcdescription> GetCustomAsjcDescriptions()
        {
            return ScivalEntities.sci_asjcdescription.OrderBy(a => a.CODE)
                .Select(a => new CustomAsjcdescription { CODE = a.CODE, DESCRIPTION = a.DETAIL, SUB_LEVEL_CODE = a.CODE, SUB_LEVEL_DESCRIPTION = a.DESCRIPTION })
                .ToList();
        }

        public static List<ClassificationList> SaveClassification(Int64 workflowId, string classificationType, Int64 frequency, string[] code, string[] ClaasText)
        {
            var workflow = ScivalEntities.sci_workflow.Where(w => w.WORKFLOWID == workflowId).First();
            var fundingBodyId = workflow.ID;
            var moduleId = workflow.MODULEID;

            classificationgroup classificationGroup = null;
            classification classi;
            Int64 classificationGroupId;
            Int64 classificationId;
            List<ClassificationList> classificationLists = null;

            if (moduleId == 2)
                classificationGroup = ScivalEntities.classificationgroups.Where(c => c.FUNDINGBODY_ID == fundingBodyId).First();
            else if (moduleId == 3)
                classificationGroup = ScivalEntities.classificationgroups.Where(c => c.OPPORTUNITY_ID == fundingBodyId).First();
            else if (moduleId == 4)
                classificationGroup = ScivalEntities.classificationgroups.Where(c => c.AWARD_ID == fundingBodyId).First();

            if (classificationGroup == null)
            {
                classificationGroupId = ScivalEntities.classificationgroups.Max(c => c.CLASSIFICATIONGROUP_ID) + 1;

                classificationgroup group = new classificationgroup
                {
                    CLASSIFICATIONGROUP_ID = classificationGroupId,
                    FUNDINGBODY_ID = fundingBodyId
                };

                ScivalEntities.classificationgroups.Add(group);
            }
            else
                classificationGroupId = classificationGroup.CLASSIFICATIONGROUP_ID;

            classi = ScivalEntities.classifications.Where(c => c.TYPE == classificationType
                                    && c.CLASSIFICATIONGROUP_ID == classificationGroup.CLASSIFICATIONGROUP_ID).FirstOrDefault();

            if (classi == null)
            {
                classificationId = ScivalEntities.classifications.Max(c => c.CLASSIFICATIONS_ID) + 1;

                classification classification = new classification
                {
                    TYPE = classificationType,
                    CLASSIFICATIONS_ID = classificationId,
                    CLASSIFICATIONGROUP_ID = classificationGroupId
                };

                ScivalEntities.classifications.Add(classification);

                for (int i = 0; i < code.Length; i++)
                {
                    classification1 classs = new classification1
                    {
                        FREQUENCY = frequency,
                        CODE = code[i],
                        CLASSIFICATION_TEXT = ClaasText[i],
                        CLASSIFICATIONS_ID = classificationId
                    };

                    ScivalEntities.classifications1.Add(classs);
                }
            }
            else
            {
                for (int i = 0; i < code.Length; i++)
                {
                    classification1 classs = new classification1
                    {
                        FREQUENCY = frequency,
                        CODE = code[i],
                        CLASSIFICATION_TEXT = ClaasText[i],
                        CLASSIFICATIONS_ID = classi.CLASSIFICATIONS_ID
                    };

                    ScivalEntities.classifications1.Add(classs);
                }
            }

            ScivalEntities.SaveChanges();

            if (moduleId == 2)
            {
                classificationLists = (from cfs in ScivalEntities.classifications
                                       join cf in ScivalEntities.classifications1 on cfs.CLASSIFICATIONS_ID equals cf.CLASSIFICATIONS_ID
                                       join cgp in ScivalEntities.classificationgroups on cfs.CLASSIFICATIONGROUP_ID equals cgp.CLASSIFICATIONGROUP_ID
                                       where cgp.FUNDINGBODY_ID == fundingBodyId
                                       select new ClassificationList
                                       {
                                           CLASSIFICATIONGROUP_ID = cgp.CLASSIFICATIONGROUP_ID,
                                           FUNDINGBODY_ID = cgp.FUNDINGBODY_ID.Value,
                                           TYPE = cfs.TYPE,
                                           CLASSIFICATIONS_ID = cfs.CLASSIFICATIONS_ID,
                                           FREQUENCY = cf.FREQUENCY,
                                           CODE = cf.CODE,
                                           CLASSIFICATION_TEXT = cf.CLASSIFICATION_TEXT
                                       }
                    ).ToList();
            }
            else if (moduleId == 3)
            {
                classificationLists = (from cfs in ScivalEntities.classifications
                                       join cf in ScivalEntities.classifications1 on cfs.CLASSIFICATIONS_ID equals cf.CLASSIFICATIONS_ID
                                       join cgp in ScivalEntities.classificationgroups on cfs.CLASSIFICATIONGROUP_ID equals cgp.CLASSIFICATIONGROUP_ID
                                       where cgp.OPPORTUNITY_ID == fundingBodyId
                                       select new ClassificationList
                                       {
                                           CLASSIFICATIONGROUP_ID = cgp.CLASSIFICATIONGROUP_ID,
                                           FUNDINGBODY_ID = cgp.FUNDINGBODY_ID.Value,
                                           TYPE = cfs.TYPE,
                                           CLASSIFICATIONS_ID = cfs.CLASSIFICATIONS_ID,
                                           FREQUENCY = cf.FREQUENCY,
                                           CODE = cf.CODE,
                                           CLASSIFICATION_TEXT = cf.CLASSIFICATION_TEXT
                                       }
                    ).ToList();
            }
            else if (moduleId == 4)
            {
                classificationLists = (from cfs in ScivalEntities.classifications
                                       join cf in ScivalEntities.classifications1 on cfs.CLASSIFICATIONS_ID equals cf.CLASSIFICATIONS_ID
                                       join cgp in ScivalEntities.classificationgroups on cfs.CLASSIFICATIONGROUP_ID equals cgp.CLASSIFICATIONGROUP_ID
                                       where cgp.AWARD_ID == fundingBodyId
                                       select new ClassificationList
                                       {
                                           CLASSIFICATIONGROUP_ID = cgp.CLASSIFICATIONGROUP_ID,
                                           FUNDINGBODY_ID = cgp.FUNDINGBODY_ID.Value,
                                           TYPE = cfs.TYPE,
                                           CLASSIFICATIONS_ID = cfs.CLASSIFICATIONS_ID,
                                           FREQUENCY = cf.FREQUENCY,
                                           CODE = cf.CODE,
                                           CLASSIFICATION_TEXT = cf.CLASSIFICATION_TEXT
                                       }
                    ).ToList();
            }

            return classificationLists;
        }

        public static List<ClassificationList> DeleteAndUpdateClassification(Int64 WFId, Int64 mode, string type, Int64 frequency, string code, string ClaasText, string ClassFcI,
            Int64? oldfrequency = null, string oldcode = null)
        {
            return ScivalEntities.Database.SqlQuery<ClassificationList>("CALL SCI_CLASSIFICATIONUPDEL(@P_WORKFLOWID,@P_INSDEL,@P_TYPE,@P_FREQUENCY,@P_CODE,@P_CLASSIFICATION_TEXT, " +
                "@P_CLASSIFICATIONS_ID,@O_FREQUENCY,@O_CODE)",
                new MySqlParameter("P_WORKFLOWID", WFId), new MySqlParameter("P_INSDEL", mode), new MySqlParameter("P_TYPE", type), new MySqlParameter("P_FREQUENCY", frequency),
                new MySqlParameter("P_CODE", code), new MySqlParameter("P_CLASSIFICATION_TEXT", ClaasText), new MySqlParameter("P_CLASSIFICATIONS_ID", ClassFcI), new MySqlParameter("O_FREQUENCY", oldfrequency),
                new MySqlParameter("O_CODE", oldcode)).ToList();
        }

        public static DataTable GetContactsList(Int64 WFId, Int64 pagemode)
        {
            //return ScivalEntities.Database.SqlQuery<DataTable>("CALL SCI_CLASSIFICATIONUPDEL(@p_workflowid,@p_mode)",
            //    new MySqlParameter("p_workflowid", WFId), new MySqlParameter("p_mode", pagemode)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                      new MySqlParameter("p_workflowid", WFId), 
                      new MySqlParameter("p_mode", pagemode)

            };
            return CommonDataOperation.ExecuteStoredProcedure("SCI_CLASSIFICATIONUPDEL", parameters).Tables[0];
            
        }

        public static DataTable SaveAndDeleteContactsList(Int64 WFId, Int64 pagemode, Int64 WorkMode, Int64 Contact_Id, string TYPE, string TITLE, string TELEPHONE, string FAX,
            string EMAIL, string url, string WEBSITE_TEXT, string COUNTRY, string ROOM, string STREET, string STATE, string CITY, string POSTALCODE, string PREFIX, string GIVENNAME,
            string MIDDLENAME, string SURNAME, string SUFFIX, string Lang = "en")
        {
            //return ScivalEntities.Database.SqlQuery<DataTable>("CALL sci_contactinsert_s2(@p_workflowid,@p_mode,@p_insdel,@p_contactid,@p_type,@p_title,@p_telephone,@p_fax,@p_email, " +
            //    "@p_url,@p_website_text,@p_country,@p_room,@p_street,@p_state,@p_city,@p_postalcode,@p_prefix,@p_givenname,@p_middlename,@p_surname,@p_suffix,@p_LANG)",
            //    new MySqlParameter("p_workflowid", WFId), new MySqlParameter("p_mode", pagemode), new MySqlParameter("p_insdel", WorkMode), new MySqlParameter("p_contactid", Contact_Id), new MySqlParameter("p_type", TYPE),
            //    new MySqlParameter("p_title", TITLE), new MySqlParameter("p_telephone", TELEPHONE), new MySqlParameter("p_fax", FAX), new MySqlParameter("p_email", EMAIL), new MySqlParameter("p_url", url),
            //    new MySqlParameter("p_website_text", WEBSITE_TEXT), new MySqlParameter("p_country", COUNTRY), new MySqlParameter("p_room", ROOM), new MySqlParameter("p_street", STREET), new MySqlParameter("p_state", STATE),
            //    new MySqlParameter("p_city", CITY), new MySqlParameter("p_postalcode", POSTALCODE), new MySqlParameter("p_prefix", PREFIX), new MySqlParameter("p_givenname", GIVENNAME), new MySqlParameter("p_middlename", MIDDLENAME),
            //    new MySqlParameter("p_surname", SURNAME), new MySqlParameter("p_suffix", SUFFIX), new MySqlParameter("p_LANG", Lang)
            //    ).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFId),
                new MySqlParameter("p_mode", pagemode), 
                new MySqlParameter("p_insdel", WorkMode),
                new MySqlParameter("p_contactid", Contact_Id), 
                new MySqlParameter("p_type", TYPE),
                new MySqlParameter("p_title", TITLE), 
                new MySqlParameter("p_telephone", TELEPHONE), 
                new MySqlParameter("p_fax", FAX), 
                new MySqlParameter("p_email", EMAIL), 
                new MySqlParameter("p_url", url),
                new MySqlParameter("p_website_text", WEBSITE_TEXT), 
                new MySqlParameter("p_country", COUNTRY), 
                new MySqlParameter("p_room", ROOM), 
                new MySqlParameter("p_street", STREET), 
                new MySqlParameter("p_state", STATE),
                new MySqlParameter("p_city", CITY), 
                new MySqlParameter("p_postalcode", POSTALCODE), 
                new MySqlParameter("p_prefix", PREFIX), 
                new MySqlParameter("p_givenname", GIVENNAME), 
                new MySqlParameter("p_middlename", MIDDLENAME),
                new MySqlParameter("p_surname", SURNAME), 
                new MySqlParameter("p_suffix", SUFFIX), 
                new MySqlParameter("p_LANG", Lang)
            };
            return CommonDataOperation.ExecuteStoredProcedure("sci_contactinsert_s2", parameters).Tables[0];

        }

        public static DataSet GetESTFundings(Int64 WFID, Int64 pagemode)
        {
            //var estFundings = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_CLASSIFICATIONUPDEL(@P_WORKFLOWID,@page_mode)",
            //    new MySqlParameter("P_WORKFLOWID", WFID), new MySqlParameter("page_mode", pagemode)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                    new MySqlParameter("P_WORKFLOWID", WFID), 
                    new MySqlParameter("page_mode", pagemode)

            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("SCI_CLASSIFICATIONUPDEL", parameters);
            estFundings.Tables[0].TableName = "Currency";
            estFundings.Tables[1].TableName = "DisplayData";
            estFundings.Tables[2].TableName = "Currency2";

            return estFundings;
        }

        public static DataSet SaveESTFundings(Int64 WFID, string currency, string amount, string amountdec, string LinkURL, string LinkText, Int64 pagemode, Int64 mode)
        {
            //var estFundings = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_contactinsert_s2(@p_workflowid,@p_currency,@p_amount,@p_amount_description,@p_total_amount,@p_startdate, " +
            //    "@p_enddate,@p_url,@p_url_text,@p_mode,@PAGE_MODE,@p_SEQUENCE_ID)",
            //    new MySqlParameter("p_workflowid", WFID), new MySqlParameter("p_currency", currency), new MySqlParameter("p_amount", amount), new MySqlParameter("p_amount_description", amountdec),
            //    new MySqlParameter("p_total_amount", null), new MySqlParameter("p_startdate", null), new MySqlParameter("p_enddate", null), new MySqlParameter("p_url", LinkURL), new MySqlParameter("p_url_text", LinkText),
            //    new MySqlParameter("p_mode", mode), new MySqlParameter("PAGE_MODE", pagemode), new MySqlParameter("p_SEQUENCE_ID", null)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFID), 
                new MySqlParameter("p_currency", currency),
                new MySqlParameter("p_amount", amount), 
                new MySqlParameter("p_amount_description", amountdec),
                new MySqlParameter("p_total_amount", null), 
                new MySqlParameter("p_startdate", null),
                new MySqlParameter("p_enddate", null),
                new MySqlParameter("p_url", LinkURL), 
                new MySqlParameter("p_url_text", LinkText),
                new MySqlParameter("p_mode", mode), 
                new MySqlParameter("PAGE_MODE", pagemode), 
                new MySqlParameter("p_SEQUENCE_ID", null)

            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_contactinsert_s2", parameters);
            estFundings.Tables[0].TableName = "DisplayData";
            return estFundings;
        }

        public static DataSet GetItemsList(Int64 WFId, Int64 pagemode)
        {
            //var estFundings = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_itemlist5(@p_workflowid,@p_mode)",
            //    new MySqlParameter("p_workflowid", WFId), new MySqlParameter("p_mode", pagemode)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                     new MySqlParameter("p_workflowid", WFId), 
                     new MySqlParameter("p_mode", pagemode)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_itemlist5", parameters);
            estFundings.Tables[0].TableName = "ItemListDDLDisplay";
            estFundings.Tables[1].TableName = "ItemListDisplay";

            return estFundings;
        }

        public static string CheckOldEstFuDesc_OP(Int64 WFID, Int64 pagemode)
        {
            string str = string.Empty;

            var workflowIds = ScivalEntities.sci_workflow.Where(w => w.WORKFLOWID == WFID).Select(w => w.ID).ToList();

            if (pagemode == 10)
            {
                str = ScivalEntities.estimatedfundings.Where(e => workflowIds.Contains(e.OPPORTUNITY_ID)).Select(e => e.AMOUNT_DESCRIPTION).FirstOrDefault();
            }
            else if (pagemode == 8)
            {
                str = BitConverter.ToString(ScivalEntities.oppotunities.Where(e => workflowIds.Contains(e.OPPORTUNITY_ID)).Select(e => e.ELIGIBILITYDESCRIPTION).FirstOrDefault());
            }
            else if (pagemode == 9)
            {
                str = ScivalEntities.oppotunities.Where(e => workflowIds.Contains(e.OPPORTUNITY_ID)).Select(e => e.LIMITEDSUBMISSIONDESCRIPTION).FirstOrDefault();
            }

            return str;
        }

        public static DataSet SaveAndDeleteItemsList(Int64 WFId, Int64 pagemode, Int64 WorkMode, string Reltype, string Description, string URl, string UrlText, string lang, Int64 ItemId)
        {
            //var estFundings = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_iteminserttemp5(@p_workflowid,@p_mode,@p_insdel,@p_itemid,@p_RELTYPE,@p_DESCRIPTION,@p_url,@p_urltext,@p_lang)",
            //   new MySqlParameter("p_workflowid", WFId), new MySqlParameter("p_mode", pagemode), new MySqlParameter("p_insdel", WorkMode), new MySqlParameter("p_itemid", ItemId), new MySqlParameter("p_RELTYPE", Reltype),
            //   new MySqlParameter("p_DESCRIPTION", Description), new MySqlParameter("p_url", URl), new MySqlParameter("p_urltext", UrlText), new MySqlParameter("p_lang", lang)).FirstOrDefault();
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
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_iteminserttemp5", parameters);

            estFundings.Tables[0].TableName = "ItemListDisplay";

            return estFundings;
        }

        public static List<sci_countrygroup> GetCountryGroup()
        {
            return ScivalEntities.sci_countrygroup.ToList();
        }

        public static List<sci_countries> GetCountries(Int64 countryGroupId)
        {
            return ScivalEntities.sci_countries.Where(c => c.COUNTRYGROUP_ID == countryGroupId).ToList();
        }

        public static DataSet GetEligibiltyClassification(Int64 WFId)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_OP_ECLASSIFICATIONLIST(@P_WORKFLOWID)",
            //    new MySqlParameter("P_WORKFLOWID", WFId)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                    new MySqlParameter("P_WORKFLOWID", WFId)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("SCI_OP_ECLASSIFICATIONLIST", parameters);
            dsTaskList.Tables[0].TableName = "DisplayData";
            dsTaskList.Tables[1].TableName = "EClassText";
            dsTaskList.Tables[2].TableName = "Type";
            dsTaskList.Tables[3].TableName = "Country";
            return dsTaskList;
        }

        public static DataSet GetEligibiltyClassificationDetial(Int64 WFId)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_op_eligibilitylist(@p_workflowid)",
            //    new MySqlParameter("p_workflowid", WFId)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFId)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("sci_op_eligibilitylist", parameters);

            dsTaskList.Tables[0].TableName = "IndividualEligibility";
            dsTaskList.Tables[1].TableName = "organizationEligibility";
            dsTaskList.Tables[2].TableName = "restrictions";

            return dsTaskList;
        }

        public static DataSet SaveIndividualEligibilityType(Int64 p_workflowid, string p_not_specified, string p_degree, string p_graduate, string p_newfaculty,
            string p_undergraduate, string P_norestriction, string P_country, string P_Citizenship_text, Int64 mode, string p_lang = "en",
            Int64? p_ELIGIBILITYCLASSIFICATION_ID = null, Int64? p_INDIVIDUALELIGIBILITY_ID = null)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_INDELIGIBILITY_DML_PRC24Ap(@P_WORKFLOWID,@P_NOT_SPECIFIED,@P_DEGREE,@P_GRADUATE, " +
            //    "@P_NEWFACULTY,@P_UNDERGRADUATE,@P_NORESTRICTION,@P_COUNTRY,@P_CITIZENSHIP_TEXT,@P_ELIGIBILITYCLASSIFICATION_ID,@P_INDIVIDUALELIGIBILITY_ID,@p_lang,@P_MODE)",
            //    new MySqlParameter("P_WORKFLOWID", p_workflowid), new MySqlParameter("P_NOT_SPECIFIED", p_not_specified), new MySqlParameter("P_DEGREE", p_degree),
            //    new MySqlParameter("P_GRADUATE", p_graduate), new MySqlParameter("P_NEWFACULTY", p_newfaculty), new MySqlParameter("P_UNDERGRADUATE", p_undergraduate),
            //    new MySqlParameter("P_NORESTRICTION", P_norestriction), new MySqlParameter("P_COUNTRY", P_country), new MySqlParameter("P_CITIZENSHIP_TEXT", P_Citizenship_text),
            //    new MySqlParameter("P_ELIGIBILITYCLASSIFICATION_ID", p_ELIGIBILITYCLASSIFICATION_ID), new MySqlParameter("P_INDIVIDUALELIGIBILITY_ID", p_INDIVIDUALELIGIBILITY_ID),
            //    new MySqlParameter("p_lang", p_lang), new MySqlParameter("P_MODE", mode)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
               new MySqlParameter("P_WORKFLOWID", p_workflowid),                 
                new MySqlParameter("P_NOT_SPECIFIED", p_not_specified), 
                new MySqlParameter("P_DEGREE", p_degree),
                new MySqlParameter("P_GRADUATE", p_graduate),
                new MySqlParameter("P_NEWFACULTY", p_newfaculty), 
                new MySqlParameter("P_UNDERGRADUATE", p_undergraduate),
                new MySqlParameter("P_NORESTRICTION", P_norestriction), 
                new MySqlParameter("P_COUNTRY", P_country),
                new MySqlParameter("P_CITIZENSHIP_TEXT", P_Citizenship_text),
                new MySqlParameter("P_ELIGIBILITYCLASSIFICATION_ID", p_ELIGIBILITYCLASSIFICATION_ID), 
                new MySqlParameter("P_INDIVIDUALELIGIBILITY_ID", p_INDIVIDUALELIGIBILITY_ID),
                new MySqlParameter("p_lang", p_lang), 
                new MySqlParameter("P_MODE", mode)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("SCI_INDELIGIBILITY_DML_PRC24Ap", parameters);
            dsTaskList.Tables[0].TableName = "DisplayData";
            return dsTaskList;
        }

        public static DataSet SaveOrganizationEligibility(Int64 p_workflowid, string p_not_specified, string p_academic, string p_commercial, string p_government, string p_nonprofit,
            string P_sme, string P_norestriction, string P_city, string P_state, string P_country, string P_regionspecific_text, string P_Citizenship_text, Int64 P_MODE,
            string p_lang = "en", Int64? p_ELIGIBILITYCLASSIFICATION_ID = null, Int64? p_organizationeligibility_id = null)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_ORGNELIGIBILITYY_DML_25APR(@P_WORKFLOWID,@P_NOT_SPECIFIED,@P_ACADEMIC,@P_COMMERCIAL, " +
            //   "@P_GOVERNMENT,@P_NONPROFIT,@P_SME,@P_NORESTRICTION,@P_CITY,@P_STATE,@P_COUNTRY,@P_REGIONSPECIFIC_TEXT,@P_CITIZENSHIP_TEXT,@P_ELIGIBILITYCLASSIFICATION_ID, " +
            //   "@P_ORGANIZATIONELIGIBILITY_ID,@p_lang,@P_MODE)",
            //   new MySqlParameter("P_WORKFLOWID", p_workflowid), new MySqlParameter("P_NOT_SPECIFIED", p_not_specified), new MySqlParameter("P_ACADEMIC", p_academic),
            //   new MySqlParameter("P_COMMERCIAL", p_commercial), new MySqlParameter("P_GOVERNMENT", p_government), new MySqlParameter("P_NONPROFIT", p_nonprofit),
            //   new MySqlParameter("P_SME", P_sme), new MySqlParameter("P_NORESTRICTION", P_norestriction), new MySqlParameter("P_CITY", P_city), new MySqlParameter("P_STATE", P_state),
            //   new MySqlParameter("P_COUNTRY", P_country), new MySqlParameter("P_REGIONSPECIFIC_TEXT", P_regionspecific_text), new MySqlParameter("P_CITIZENSHIP_TEXT", P_Citizenship_text),
            //   new MySqlParameter("P_ELIGIBILITYCLASSIFICATION_ID", p_ELIGIBILITYCLASSIFICATION_ID), new MySqlParameter("P_ORGANIZATIONELIGIBILITY_ID", p_organizationeligibility_id),
            //   new MySqlParameter("p_lang", p_lang), new MySqlParameter("P_MODE", P_MODE)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
               new MySqlParameter("P_WORKFLOWID", p_workflowid), new MySqlParameter("P_NOT_SPECIFIED", p_not_specified), new MySqlParameter("P_ACADEMIC", p_academic),
               new MySqlParameter("P_COMMERCIAL", p_commercial), new MySqlParameter("P_GOVERNMENT", p_government), new MySqlParameter("P_NONPROFIT", p_nonprofit),
               new MySqlParameter("P_SME", P_sme), new MySqlParameter("P_NORESTRICTION", P_norestriction), new MySqlParameter("P_CITY", P_city), new MySqlParameter("P_STATE", P_state),
               new MySqlParameter("P_COUNTRY", P_country), new MySqlParameter("P_REGIONSPECIFIC_TEXT", P_regionspecific_text), new MySqlParameter("P_CITIZENSHIP_TEXT", P_Citizenship_text),
               new MySqlParameter("P_ELIGIBILITYCLASSIFICATION_ID", p_ELIGIBILITYCLASSIFICATION_ID), new MySqlParameter("P_ORGANIZATIONELIGIBILITY_ID", p_organizationeligibility_id),
               new MySqlParameter("p_lang", p_lang), new MySqlParameter("P_MODE", P_MODE)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("SCI_ORGNELIGIBILITYY_DML_25APR", parameters);

            dsTaskList.Tables[0].TableName = "DisplayData";

            return dsTaskList;
        }

        public static DataSet SaveRestrictions(Int64 p_workflowid, string p_not_specified, string p_disabilities, string p_invitationonly, string p_memberonly, string p_nominationonly,
            string p_minorties, string p_women, string p_numberofapplicantsallowed, string P_limitedsubmission_text, Int64 P_MODE, string p_lang = "en",
            Int64? p_ELIGIBILITYCLASSIFICATION_ID = null, Int64? P_RESTRICTIONS_ID = null)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_limitedsubmission_dml_prc(@P_WORKFLOWID,@P_NOT_SPECIFIED,@P_DISABILITIES,@P_INVITATIONONLY, " +
            //      "@P_MEMBERONLY,@P_NOMINATIONONLY,@P_MINORTIES,@P_WOMEN,@P_NUMBEROFAPPLICANTSALLOWED,@P_LIMITEDSUBMISSION_TEXT,@P_ELIGIBILITYCLASSIFICATION_ID,@P_RESTRICTIONS_ID,@p_lang,@P_MODE)",
            //      new MySqlParameter("P_WORKFLOWID", p_workflowid), new MySqlParameter("P_NOT_SPECIFIED", p_not_specified), new MySqlParameter("P_DISABILITIES", p_disabilities),
            //      new MySqlParameter("P_INVITATIONONLY", p_invitationonly), new MySqlParameter("P_MEMBERONLY", p_memberonly), new MySqlParameter("P_NOMINATIONONLY", p_nominationonly),
            //      new MySqlParameter("P_MINORTIES", p_minorties), new MySqlParameter("P_WOMEN", p_women), new MySqlParameter("P_NUMBEROFAPPLICANTSALLOWED", p_numberofapplicantsallowed),
            //      new MySqlParameter("P_LIMITEDSUBMISSION_TEXT", P_limitedsubmission_text), new MySqlParameter("P_ELIGIBILITYCLASSIFICATION_ID", p_ELIGIBILITYCLASSIFICATION_ID),
            //      new MySqlParameter("P_RESTRICTIONS_ID", P_RESTRICTIONS_ID), new MySqlParameter("p_lang", p_lang), new MySqlParameter("P_MODE", P_MODE)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("P_WORKFLOWID", p_workflowid), new MySqlParameter("P_NOT_SPECIFIED", p_not_specified), new MySqlParameter("P_DISABILITIES", p_disabilities),
                  new MySqlParameter("P_INVITATIONONLY", p_invitationonly), new MySqlParameter("P_MEMBERONLY", p_memberonly), new MySqlParameter("P_NOMINATIONONLY", p_nominationonly),
                  new MySqlParameter("P_MINORTIES", p_minorties), new MySqlParameter("P_WOMEN", p_women), new MySqlParameter("P_NUMBEROFAPPLICANTSALLOWED", p_numberofapplicantsallowed),
                  new MySqlParameter("P_LIMITEDSUBMISSION_TEXT", P_limitedsubmission_text), new MySqlParameter("P_ELIGIBILITYCLASSIFICATION_ID", p_ELIGIBILITYCLASSIFICATION_ID),
                  new MySqlParameter("P_RESTRICTIONS_ID", P_RESTRICTIONS_ID), new MySqlParameter("p_lang", p_lang), new MySqlParameter("P_MODE", P_MODE)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("sci_limitedsubmission_dml_prc", parameters);

            dsTaskList.Tables[0].TableName = "DisplayData";

            return dsTaskList;
        }

        public static DataSet SaveElClassification(Int64 WFID, string type, string ClassId, string ClassText, string Type_old,
                                            string clsaaid_old, string calsstext_old, Int64 mode, string country, string state, string city, Int64 specific_id)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_op_eclassificationins(@p_workflowid,@p_TYPE,@p_ID,@p_ELIGIBLECLASSIFICATION_TEXT, " +
            //      "@n_TYPE,@n_ID,@n_ELIGIBLECLASSIFICATION_TEXT,@P_COUNTRY_CODE,@P_STATE,@P_CITY,@p_region_specific_id,@p_mode)",
            //      new MySqlParameter("p_workflowid", WFID), new MySqlParameter("p_TYPE", type), new MySqlParameter("p_ID", ClassId),
            //      new MySqlParameter("p_ELIGIBLECLASSIFICATION_TEXT", ClassText), new MySqlParameter("n_TYPE", Type_old), new MySqlParameter("n_ID", clsaaid_old),
            //      new MySqlParameter("n_ELIGIBLECLASSIFICATION_TEXT", calsstext_old), new MySqlParameter("P_COUNTRY_CODE", country), new MySqlParameter("P_STATE", state),
            //      new MySqlParameter("P_CITY", city), new MySqlParameter("p_region_specific_id", specific_id), new MySqlParameter("p_mode", mode)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                 new MySqlParameter("p_workflowid", WFID), new MySqlParameter("p_TYPE", type), new MySqlParameter("p_ID", ClassId),
                  new MySqlParameter("p_ELIGIBLECLASSIFICATION_TEXT", ClassText), new MySqlParameter("n_TYPE", Type_old), new MySqlParameter("n_ID", clsaaid_old),
                  new MySqlParameter("n_ELIGIBLECLASSIFICATION_TEXT", calsstext_old), new MySqlParameter("P_COUNTRY_CODE", country), new MySqlParameter("P_STATE", state),
                  new MySqlParameter("P_CITY", city), new MySqlParameter("p_region_specific_id", specific_id), new MySqlParameter("p_mode", mode)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("sci_op_eclassificationins", parameters);

            dsTaskList.Tables[0].TableName = "DisplayData";

            return dsTaskList;
        }

        public static DataSet UpdateIndividualEligibilityType(Int64 p_workflowid, string p_not_specified, string p_degree, string p_graduate, string p_newfaculty, string p_undergraduate,
            string P_norestriction, string P_country, string P_Citizenship_text, Int64 mode, string p_ELIGIBILITYCLASSIFICATION_ID, string p_INDIVIDUALELIGIBILITY_ID, string p_lang)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_INDELIGIBILITY_DML_PRC23MA(@p_workflowid,@p_not_specified,@p_degree,@p_graduate, " +
            //    "@p_newfaculty,@p_undergraduate,@P_NORESTRICTION,@P_COUNTRY,@P_CITIZENSHIP_TEXT,@P_ELIGIBILITYCLASSIFICATION_ID,@P_INDIVIDUALELIGIBILITY_ID,@p_lang,@P_MODE)",
            //    new MySqlParameter("p_workflowid", p_workflowid), new MySqlParameter("p_not_specified", p_not_specified), new MySqlParameter("p_degree", p_degree),
            //    new MySqlParameter("p_graduate", p_graduate), new MySqlParameter("p_newfaculty", p_newfaculty), new MySqlParameter("p_undergraduate", p_undergraduate),
            //    new MySqlParameter("P_NORESTRICTION", P_norestriction), new MySqlParameter("P_COUNTRY", P_country), new MySqlParameter("P_CITIZENSHIP_TEXT", P_Citizenship_text),
            //    new MySqlParameter("P_ELIGIBILITYCLASSIFICATION_ID", p_ELIGIBILITYCLASSIFICATION_ID), new MySqlParameter("P_INDIVIDUALELIGIBILITY_ID", p_INDIVIDUALELIGIBILITY_ID),
            //    new MySqlParameter("p_lang", p_lang), new MySqlParameter("P_MODE", mode)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", p_workflowid), new MySqlParameter("p_not_specified", p_not_specified), new MySqlParameter("p_degree", p_degree),
                new MySqlParameter("p_graduate", p_graduate), new MySqlParameter("p_newfaculty", p_newfaculty), new MySqlParameter("p_undergraduate", p_undergraduate),
                new MySqlParameter("P_NORESTRICTION", P_norestriction), new MySqlParameter("P_COUNTRY", P_country), new MySqlParameter("P_CITIZENSHIP_TEXT", P_Citizenship_text),
                new MySqlParameter("P_ELIGIBILITYCLASSIFICATION_ID", p_ELIGIBILITYCLASSIFICATION_ID), new MySqlParameter("P_INDIVIDUALELIGIBILITY_ID", p_INDIVIDUALELIGIBILITY_ID),
                new MySqlParameter("p_lang", p_lang), new MySqlParameter("P_MODE", mode)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("SCI_INDELIGIBILITY_DML_PRC23MA", parameters);

            dsTaskList.Tables[0].TableName = "DisplayData";

            return dsTaskList;
        }

        public static DataSet UpdateOrganizationEligibility(Int64 p_workflowid, string p_not_specified, string p_academic, string p_commercial, string p_government, string p_nonprofit,
            string P_sme, string P_norestriction, string P_city, string P_state, string P_country, string P_regionspecific_text, string P_Citizenship_text, Int64 P_MODE,
            string p_ELIGIBILITYCLASSIFICATION_ID, string p_organizationeligibility_id, string p_lang)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_ORGNELIGIBILITYY_DML_23MAY(@P_WORKFLOWID,@P_NOT_SPECIFIED,@P_ACADEMIC,@P_COMMERCIAL, " +
            //   "@P_GOVERNMENT,@P_NONPROFIT,@P_SME,@P_NORESTRICTION,@P_CITY,@P_STATE,@P_COUNTRY,@P_REGIONSPECIFIC_TEXT,@P_CITIZENSHIP_TEXT,@P_ELIGIBILITYCLASSIFICATION_ID, " +
            //   "@P_ORGANIZATIONELIGIBILITY_ID,@p_lang,@P_MODE)",
            //   new MySqlParameter("P_WORKFLOWID", p_workflowid), new MySqlParameter("P_NOT_SPECIFIED", p_not_specified), new MySqlParameter("P_ACADEMIC", p_academic),
            //   new MySqlParameter("P_COMMERCIAL", p_commercial), new MySqlParameter("P_GOVERNMENT", p_government), new MySqlParameter("P_NONPROFIT", p_nonprofit),
            //   new MySqlParameter("P_SME", P_sme), new MySqlParameter("P_NORESTRICTION", P_norestriction), new MySqlParameter("P_CITY", P_city), new MySqlParameter("P_STATE", P_state),
            //   new MySqlParameter("P_COUNTRY", P_country), new MySqlParameter("P_REGIONSPECIFIC_TEXT", P_regionspecific_text), new MySqlParameter("P_CITIZENSHIP_TEXT", P_Citizenship_text),
            //   new MySqlParameter("P_ELIGIBILITYCLASSIFICATION_ID", p_ELIGIBILITYCLASSIFICATION_ID), new MySqlParameter("P_ORGANIZATIONELIGIBILITY_ID", p_organizationeligibility_id),
            //   new MySqlParameter("p_lang", p_lang), new MySqlParameter("P_MODE", P_MODE)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("P_WORKFLOWID", p_workflowid), new MySqlParameter("P_NOT_SPECIFIED", p_not_specified), new MySqlParameter("P_ACADEMIC", p_academic),
               new MySqlParameter("P_COMMERCIAL", p_commercial), new MySqlParameter("P_GOVERNMENT", p_government), new MySqlParameter("P_NONPROFIT", p_nonprofit),
               new MySqlParameter("P_SME", P_sme), new MySqlParameter("P_NORESTRICTION", P_norestriction), new MySqlParameter("P_CITY", P_city), new MySqlParameter("P_STATE", P_state),
               new MySqlParameter("P_COUNTRY", P_country), new MySqlParameter("P_REGIONSPECIFIC_TEXT", P_regionspecific_text), new MySqlParameter("P_CITIZENSHIP_TEXT", P_Citizenship_text),
               new MySqlParameter("P_ELIGIBILITYCLASSIFICATION_ID", p_ELIGIBILITYCLASSIFICATION_ID), new MySqlParameter("P_ORGANIZATIONELIGIBILITY_ID", p_organizationeligibility_id),
               new MySqlParameter("p_lang", p_lang), new MySqlParameter("P_MODE", P_MODE)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("SCI_ORGNELIGIBILITYY_DML_23MAY", parameters);

            dsTaskList.Tables[0].TableName = "DisplayData";

            return dsTaskList;
        }

        public static DataSet DeleteIndividualEligibilityType(string p_deletegroupid, Int64 p_workflowid, string p_not_specified, string p_degree, string p_graduate, string p_newfaculty, 
            string p_undergraduate, string P_norestriction, string P_country, string P_Citizenship_text, Int64 mode, string p_lang = "en", Int64? p_ELIGIBILITYCLASSIFICATION_ID = null, 
            Int64? p_INDIVIDUALELIGIBILITY_ID = null)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_INDELIGIBILITY_DML_PRC_s(@P_DELETEGROUPID,@p_workflowid,@p_not_specified,@p_degree,@p_graduate, " +
            //   "@p_newfaculty,@p_undergraduate,@P_NORESTRICTION,@P_COUNTRY,@P_CITIZENSHIP_TEXT,@P_ELIGIBILITYCLASSIFICATION_ID,@P_INDIVIDUALELIGIBILITY_ID,@p_lang,@P_MODE)",
            //   new MySqlParameter("P_DELETEGROUPID", p_deletegroupid), new MySqlParameter("p_workflowid", p_workflowid), new MySqlParameter("p_not_specified", p_not_specified), new MySqlParameter("p_degree", p_degree),
            //   new MySqlParameter("p_graduate", p_graduate), new MySqlParameter("p_newfaculty", p_newfaculty), new MySqlParameter("p_undergraduate", p_undergraduate),
            //   new MySqlParameter("P_NORESTRICTION", P_norestriction), new MySqlParameter("P_COUNTRY", P_country), new MySqlParameter("P_CITIZENSHIP_TEXT", P_Citizenship_text),
            //   new MySqlParameter("P_ELIGIBILITYCLASSIFICATION_ID", p_ELIGIBILITYCLASSIFICATION_ID), new MySqlParameter("P_INDIVIDUALELIGIBILITY_ID", p_INDIVIDUALELIGIBILITY_ID),
            //   new MySqlParameter("p_lang", p_lang), new MySqlParameter("P_MODE", mode)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
               new MySqlParameter("P_DELETEGROUPID", p_deletegroupid), new MySqlParameter("p_workflowid", p_workflowid), new MySqlParameter("p_not_specified", p_not_specified), new MySqlParameter("p_degree", p_degree),
               new MySqlParameter("p_graduate", p_graduate), new MySqlParameter("p_newfaculty", p_newfaculty), new MySqlParameter("p_undergraduate", p_undergraduate),
               new MySqlParameter("P_NORESTRICTION", P_norestriction), new MySqlParameter("P_COUNTRY", P_country), new MySqlParameter("P_CITIZENSHIP_TEXT", P_Citizenship_text),
               new MySqlParameter("P_ELIGIBILITYCLASSIFICATION_ID", p_ELIGIBILITYCLASSIFICATION_ID), new MySqlParameter("P_INDIVIDUALELIGIBILITY_ID", p_INDIVIDUALELIGIBILITY_ID),
               new MySqlParameter("p_lang", p_lang), new MySqlParameter("P_MODE", mode)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("SCI_INDELIGIBILITY_DML_PRC_s", parameters);

            dsTaskList.Tables[0].TableName = "DisplayData";

            return dsTaskList;
        }

        public static DataSet DeleteOrganizationEligibility(string p_DeleteGroupid, Int64 p_workflowid, string p_not_specified, string p_academic, string p_commercial, string p_government,
            string p_nonprofit, string P_sme, string P_norestriction, string P_city, string P_state, string P_country, string P_regionspecific_text, string P_Citizenship_text, Int64 P_MODE, 
            string p_lang = "en", Int64? p_ELIGIBILITYCLASSIFICATION_ID = null, Int64? p_organizationeligibility_id = null)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_ORGNELIGIBILITYY_DML_S(@P_DELETEGROUPID,@P_WORKFLOWID,@P_NOT_SPECIFIED,@P_ACADEMIC,@P_COMMERCIAL, " +
            //   "@P_GOVERNMENT,@P_NONPROFIT,@P_SME,@P_NORESTRICTION,@P_CITY,@P_STATE,@P_COUNTRY,@P_REGIONSPECIFIC_TEXT,@P_CITIZENSHIP_TEXT,@P_ELIGIBILITYCLASSIFICATION_ID, " +
            //   "@P_ORGANIZATIONELIGIBILITY_ID,@p_lang,@P_MODE)",
            //   new MySqlParameter("P_DELETEGROUPID", p_DeleteGroupid), new MySqlParameter("P_WORKFLOWID", p_workflowid), new MySqlParameter("P_NOT_SPECIFIED", p_not_specified), new MySqlParameter("P_ACADEMIC", p_academic),
            //   new MySqlParameter("P_COMMERCIAL", p_commercial), new MySqlParameter("P_GOVERNMENT", p_government), new MySqlParameter("P_NONPROFIT", p_nonprofit),
            //   new MySqlParameter("P_SME", P_sme), new MySqlParameter("P_NORESTRICTION", P_norestriction), new MySqlParameter("P_CITY", P_city), new MySqlParameter("P_STATE", P_state),
            //   new MySqlParameter("P_COUNTRY", P_country), new MySqlParameter("P_REGIONSPECIFIC_TEXT", P_regionspecific_text), new MySqlParameter("P_CITIZENSHIP_TEXT", P_Citizenship_text),
            //   new MySqlParameter("P_ELIGIBILITYCLASSIFICATION_ID", p_ELIGIBILITYCLASSIFICATION_ID), new MySqlParameter("P_ORGANIZATIONELIGIBILITY_ID", p_organizationeligibility_id),
            //   new MySqlParameter("p_lang", p_lang), new MySqlParameter("P_MODE", P_MODE)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("P_DELETEGROUPID", p_DeleteGroupid), new MySqlParameter("P_WORKFLOWID", p_workflowid), new MySqlParameter("P_NOT_SPECIFIED", p_not_specified), new MySqlParameter("P_ACADEMIC", p_academic),
               new MySqlParameter("P_COMMERCIAL", p_commercial), new MySqlParameter("P_GOVERNMENT", p_government), new MySqlParameter("P_NONPROFIT", p_nonprofit),
               new MySqlParameter("P_SME", P_sme), new MySqlParameter("P_NORESTRICTION", P_norestriction), new MySqlParameter("P_CITY", P_city), new MySqlParameter("P_STATE", P_state),
               new MySqlParameter("P_COUNTRY", P_country), new MySqlParameter("P_REGIONSPECIFIC_TEXT", P_regionspecific_text), new MySqlParameter("P_CITIZENSHIP_TEXT", P_Citizenship_text),
               new MySqlParameter("P_ELIGIBILITYCLASSIFICATION_ID", p_ELIGIBILITYCLASSIFICATION_ID), new MySqlParameter("P_ORGANIZATIONELIGIBILITY_ID", p_organizationeligibility_id),
               new MySqlParameter("p_lang", p_lang), new MySqlParameter("P_MODE", P_MODE)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("SCI_ORGNELIGIBILITYY_DML_S", parameters);

            dsTaskList.Tables[0].TableName = "DisplayData";

            return dsTaskList;
        }

        public static DataSet GetRelatedOpportunities(Int64 WFID)
        {
            //var dsRelOppList = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_op_relatedopplist_prc(@p_workflowid)",
            //    new MySqlParameter("p_workflowid", WFID)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
               new MySqlParameter("p_workflowid", WFID)
            };
            var dsRelOppList = CommonDataOperation.ExecuteStoredProcedure("sci_op_relatedopplist_prc", parameters);

            dsRelOppList.Tables[0].TableName = "RelType";
            dsRelOppList.Tables[1].TableName = "RelatedOpp";
            dsRelOppList.Tables[2].TableName = "Opportunity";

            return dsRelOppList;
        }

        public static DataSet SaveAndDeleteRelatedOpps(Int64 WFID, Int64 mode, string OppDbId, Int64? reltype, string Description = "")
        {
            //var dsRelOppList = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_rel_opportunity_dml_prc5(@p_workflowid,@p_insdel,@p_oppdbid,@p_RELTYPE,@p_Desc)",
            //    new MySqlParameter("p_workflowid", WFID), new MySqlParameter("p_insdel", mode), new MySqlParameter("p_oppdbid", OppDbId), 
            //    new MySqlParameter("p_RELTYPE", reltype), new MySqlParameter("p_Desc", Description)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
              new MySqlParameter("p_workflowid", WFID), 
                new MySqlParameter("p_insdel", mode), 
                new MySqlParameter("p_oppdbid", OppDbId),
                new MySqlParameter("p_RELTYPE", reltype), 
                new MySqlParameter("p_Desc", Description)
            };
            var dsRelOppList = CommonDataOperation.ExecuteStoredProcedure("sci_rel_opportunity_dml_prc5", parameters);

            dsRelOppList.Tables[0].TableName = "RelatedOpp";
            dsRelOppList.Tables[1].TableName = "Opportunity";

            return dsRelOppList;
        }

        public static DataSet Getopprotunity_Loi_status(Int64 wfid)
        {
            //var dsRelOppList = ScivalEntities.Database.SqlQuery<DataSet>("CALL opprotunity_loi_status(@P_WORKFLOWID)",
            //    new MySqlParameter("P_WORKFLOWID", wfid)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
              new MySqlParameter("P_WORKFLOWID", wfid)
            };
            var dsRelOppList = CommonDataOperation.ExecuteStoredProcedure("opprotunity_loi_status", parameters);

            dsRelOppList.Tables[0].TableName = "OpportunityCount";

            return dsRelOppList;
        }

        public static DataSet LoadLanguageData(string OPPID, int mode_id, int tran_type_id)
        {
            //var dsRelOppList = ScivalEntities.Database.SqlQuery<DataSet>("CALL LoadLanguageData").FirstOrDefault();
            var dsRelOppList = CommonDataOperation.ExecuteStoredProcedureWithoutParam("LoadLanguageData");

            dsRelOppList.Tables[0].TableName = "LanguageData";

            return dsRelOppList;
        }

        public static string CheckOppRecStatus(Int64 WFID)
        {
            var ids = ScivalEntities.sci_workflow.Where(w => w.WORKFLOWID == WFID).Select(w => w.ID).ToList();

            var maxRecId = ScivalEntities.recurrings.Where(r => ids.Contains(r.OPPORTUNITY_ID)).Max(r => r.REC_ID);

            return ScivalEntities.recurrings.Where(r => r.REC_ID == maxRecId).Select(r => r.RECURRING_STATUS).FirstOrDefault();
        }

        public static DataSet SaveOpportunity(DataTable Opportunity)
        {
            DataSet dsTaskList = null;

            for (Int32 x = 0; x < Opportunity.Rows.Count; x++)
            {
                //dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL opprotunity_loi_status(@P_WORKFLOWID,@P_FUNDINGBODYOPPORTUNITYID,@P_LIMITEDSUBMISSION, " +
                //"@P_TRUSTING,@P_COLLECTIONCODE,@P_HIDDEN,@P_NAME,@P_DUEDATEDESCRIPTION,@P_ELIGIBILITYDESCRIPTION,@P_ELIGIBILITYCATEGORY,@P_LINKTOFULLTEXT, " +
                //"@P_OPPORTUNITYSTATUS,@P_NUMBEROFAWARDS,@P_DURATION,@P_LIMITEDSUBMISSIONDESCRIPTION,@P_RAWTEXT,@p_id,@p_url,@p_mode,@p_recordsource, " +
                //"@p_loi_mandatory,@p_preProposalMandatory,@p_repeatingOpportunity,@p_langRSource)",
                //new MySqlParameter("P_WORKFLOWID", Convert.ToInt64(Opportunity.Rows[x]["WFID"])), new MySqlParameter("P_FUNDINGBODYOPPORTUNITYID", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["FOIF"].ToString())),
                //new MySqlParameter("P_LIMITEDSUBMISSION", Opportunity.Rows[x]["LIMITEDSUBMISSION"].ToString()), new MySqlParameter("P_TRUSTING", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["TRUSTING"].ToString())),
                //new MySqlParameter("P_COLLECTIONCODE", Opportunity.Rows[x]["COLLECTIONCODE"].ToString()), new MySqlParameter("P_HIDDEN", Opportunity.Rows[x]["HIDDEN"].ToString()),
                //new MySqlParameter("P_NAME", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["NAME"].ToString())), new MySqlParameter("P_DUEDATEDESCRIPTION", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["DUEDATEDESCRIPTION"].ToString())),
                //new MySqlParameter("P_ELIGIBILITYDESCRIPTION", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["ELIGIBILITYDESCRIPTION"].ToString())),
                //new MySqlParameter("P_ELIGIBILITYCATEGORY", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["ELIGIBILITYCATEGORY"].ToString())),
                //new MySqlParameter("P_LINKTOFULLTEXT", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["LINKTOFULLTEXT"].ToString())), new MySqlParameter("P_OPPORTUNITYSTATUS", Opportunity.Rows[x]["OPPORTUNITYSTATUS"].ToString()),
                //new MySqlParameter("P_NUMBEROFAWARDS", Convert.ToInt64(Opportunity.Rows[x]["NUMBEROFAWARDS"])), new MySqlParameter("P_DURATION", Opportunity.Rows[x]["DURATION"].ToString()),
                //new MySqlParameter("P_LIMITEDSUBMISSIONDESCRIPTION", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["LIMITEDSUBMISSIONDESCRIPTION"].ToString())),
                //new MySqlParameter("P_RAWTEXT", Opportunity.Rows[x]["RAWTEXT"].ToString()), new MySqlParameter("p_id", Opportunity.Rows[x]["TYPEID"].ToString()),
                //new MySqlParameter("p_url", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["URL"].ToString())), new MySqlParameter("p_mode", Convert.ToInt64(Opportunity.Rows[x]["mode"])),
                //new MySqlParameter("p_recordsource", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["RECORDSOURCE"].ToString())), new MySqlParameter("p_loi_mandatory", Opportunity.Rows[x]["LOIMANDATORY"].ToString()),
                //new MySqlParameter("p_preProposalMandatory", Opportunity.Rows[x]["preProposalMandatory"].ToString()), new MySqlParameter("p_repeatingOpportunity", Opportunity.Rows[x]["repeatingOpportunity"].ToString()), new MySqlParameter("p_langRSource", Opportunity.Rows[x]["lang_RSource"].ToString()))
                //.FirstOrDefault();
                var parameters = new List<MySqlParameter>
            {
                      new MySqlParameter("P_WORKFLOWID", Convert.ToInt64(Opportunity.Rows[x]["WFID"])), new MySqlParameter("P_FUNDINGBODYOPPORTUNITYID", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["FOIF"].ToString())),
                new MySqlParameter("P_LIMITEDSUBMISSION", Opportunity.Rows[x]["LIMITEDSUBMISSION"].ToString()), new MySqlParameter("P_TRUSTING", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["TRUSTING"].ToString())),
                new MySqlParameter("P_COLLECTIONCODE", Opportunity.Rows[x]["COLLECTIONCODE"].ToString()), new MySqlParameter("P_HIDDEN", Opportunity.Rows[x]["HIDDEN"].ToString()),
                new MySqlParameter("P_NAME", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["NAME"].ToString())), new MySqlParameter("P_DUEDATEDESCRIPTION", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["DUEDATEDESCRIPTION"].ToString())),
                new MySqlParameter("P_ELIGIBILITYDESCRIPTION", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["ELIGIBILITYDESCRIPTION"].ToString())),
                new MySqlParameter("P_ELIGIBILITYCATEGORY", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["ELIGIBILITYCATEGORY"].ToString())),
                new MySqlParameter("P_LINKTOFULLTEXT", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["LINKTOFULLTEXT"].ToString())), new MySqlParameter("P_OPPORTUNITYSTATUS", Opportunity.Rows[x]["OPPORTUNITYSTATUS"].ToString()),
                new MySqlParameter("P_NUMBEROFAWARDS", Convert.ToInt64(Opportunity.Rows[x]["NUMBEROFAWARDS"])), new MySqlParameter("P_DURATION", Opportunity.Rows[x]["DURATION"].ToString()),
                new MySqlParameter("P_LIMITEDSUBMISSIONDESCRIPTION", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["LIMITEDSUBMISSIONDESCRIPTION"].ToString())),
                new MySqlParameter("P_RAWTEXT", Opportunity.Rows[x]["RAWTEXT"].ToString()), new MySqlParameter("p_id", Opportunity.Rows[x]["TYPEID"].ToString()),
                new MySqlParameter("p_url", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["URL"].ToString())), new MySqlParameter("p_mode", Convert.ToInt64(Opportunity.Rows[x]["mode"])),
                new MySqlParameter("p_recordsource", r.WieredChar_ReplacementHexValue(Opportunity.Rows[x]["RECORDSOURCE"].ToString())), new MySqlParameter("p_loi_mandatory", Opportunity.Rows[x]["LOIMANDATORY"].ToString()),
                new MySqlParameter("p_preProposalMandatory", Opportunity.Rows[x]["preProposalMandatory"].ToString()), new MySqlParameter("p_repeatingOpportunity", Opportunity.Rows[x]["repeatingOpportunity"].ToString()), 
                    new MySqlParameter("p_langRSource", Opportunity.Rows[x]["lang_RSource"].ToString())
               

            };
                dsTaskList = CommonDataOperation.ExecuteStoredProcedure("opprotunity_loi_status", parameters);

                dsTaskList.Tables[0].TableName = "FundingBodyTable";
            }

            if (dsTaskList.Tables["FundingBodyTable"].Rows.Count > 0)
            {
                for (int intCount = 0; intCount < dsTaskList.Tables["FundingBodyTable"].Rows.Count; intCount++)
                {
                    string UpdateFunding_fundingbodyopportunityid = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["fundingbodyopportunityid"].ToString()));
                    if (UpdateFunding_fundingbodyopportunityid != "")
                    {
                        dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["fundingbodyopportunityid"] = UpdateFunding_fundingbodyopportunityid;
                    }
                    string UpdateFunding_trusting = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["trusting"].ToString()));
                    if (UpdateFunding_trusting != "")
                    {
                        dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["trusting"] = UpdateFunding_trusting;
                    }
                    string UpdateFunding_eligibilitycategory = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["eligibilitycategory"].ToString()));
                    if (UpdateFunding_eligibilitycategory != "")
                    {
                        dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["eligibilitycategory"] = UpdateFunding_eligibilitycategory;
                    }
                    string UpdateFunding_linktofulltext = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["linktofulltext"].ToString()));
                    if (UpdateFunding_linktofulltext != "")
                    {
                        dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["linktofulltext"] = UpdateFunding_linktofulltext;
                    }
                    string UpdateFunding_eligibilitydescription = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["eligibilitydescription"].ToString()));
                    if (UpdateFunding_eligibilitydescription != "")
                    {
                        dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["eligibilitydescription"] = UpdateFunding_eligibilitydescription;
                    }
                    string UpdateFunding_duedatedescription = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["duedatedescription"].ToString()));
                    if (UpdateFunding_duedatedescription != "")
                    {
                        dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["duedatedescription"] = UpdateFunding_duedatedescription;
                    }
                    string UpdateFunding_recordsource = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["recordsource"].ToString()));
                    if (UpdateFunding_recordsource != "")
                    {
                        dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["recordsource"] = UpdateFunding_recordsource;
                    }
                }
            }

            return dsTaskList;
        }

        public static DataSet getWorkFlowDetails(Int64 wfid)
        {
            //var dsWFList = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_GetWorkFlowDetails(@p_WorkflowId)",
            //    new MySqlParameter("p_WorkflowId", wfid)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_WorkflowId", wfid)
            };
            var dsWFList = CommonDataOperation.ExecuteStoredProcedure("sci_GetWorkFlowDetails", parameters);
            dsWFList.Tables[0].TableName = "WFlowTable";
            return dsWFList;
        }

        public static string CheckOppStatus(Int64 wfid)
        {
            //var dsWFList = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_CheckOppStatus(@p_WorkflowID)",
            //    new MySqlParameter("p_WorkflowID", wfid)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_WorkflowID", wfid)
            };
            var dsWFList = CommonDataOperation.ExecuteStoredProcedure("sci_CheckOppStatus", parameters);
            return dsWFList.Tables[0].Rows[0][0].ToString();
        }

        public static DataSet SaveOpportunityLang(DataTable OpportunityLang)
        {
            DataSet dsTaskList = null;

            for (Int32 x = 0; x < OpportunityLang.Rows.Count; x++)
            {
                //dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_language_detail_dml_prc(@tran_id_in,@scival_id_in, " +
                //"@column_desc_in,@column_id_in,@moduleid_in,@language_id_in,@tran_type_id_in,@flag_in)",
                //new MySqlParameter("tran_id_in", Convert.ToInt64(OpportunityLang.Rows[x]["tran_id"])),
                //new MySqlParameter("scival_id_in", Convert.ToInt64(OpportunityLang.Rows[x]["OPP_ID"].ToString())),
                //new MySqlParameter("column_desc_in", OpportunityLang.Rows[x]["COLUMN_DESC"].ToString()),
                //new MySqlParameter("column_id_in", Convert.ToInt64(OpportunityLang.Rows[x]["COLUMN_ID"])),
                //new MySqlParameter("moduleid_in", Convert.ToInt64(OpportunityLang.Rows[x]["MODE_ID"].ToString())),
                //new MySqlParameter("language_id_in", Convert.ToInt64(OpportunityLang.Rows[x]["LANGUAGE_ID"].ToString())),
                //new MySqlParameter("tran_type_id_in", Convert.ToInt64(OpportunityLang.Rows[x]["TRAN_TYPE_ID"].ToString()))).FirstOrDefault();
                var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("tran_id_in", Convert.ToInt64(OpportunityLang.Rows[x]["tran_id"])),
                new MySqlParameter("scival_id_in", Convert.ToInt64(OpportunityLang.Rows[x]["OPP_ID"].ToString())),
                new MySqlParameter("column_desc_in", OpportunityLang.Rows[x]["COLUMN_DESC"].ToString()),
                new MySqlParameter("column_id_in", Convert.ToInt64(OpportunityLang.Rows[x]["COLUMN_ID"])),
                new MySqlParameter("moduleid_in", Convert.ToInt64(OpportunityLang.Rows[x]["MODE_ID"].ToString())),
                new MySqlParameter("language_id_in", Convert.ToInt64(OpportunityLang.Rows[x]["LANGUAGE_ID"].ToString())),
                new MySqlParameter("tran_type_id_in", Convert.ToInt64(OpportunityLang.Rows[x]["TRAN_TYPE_ID"].ToString()))
            };
                var dsWFList = CommonDataOperation.ExecuteStoredProcedure("sci_language_detail_dml_prc", parameters);

                dsTaskList.Tables[0].TableName = "FundingBodyTable";
            }

            return dsTaskList;
        }

        public static DataSet updatetitle_lang_bytxnid(Int64 p_trans_id, string p_title, string p_languagecode)
        {
            //var dsWFList = ScivalEntities.Database.SqlQuery<DataSet>("CALL update_fbdetails_multilanguage(@p_trans_id,@p_languagecode,@p_title)",
            //    new MySqlParameter("p_trans_id", p_trans_id),
            //    new MySqlParameter("p_languagecode", p_languagecode),
            //    new MySqlParameter("p_title", p_title)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_trans_id", p_trans_id),
                new MySqlParameter("p_languagecode", p_languagecode),
                new MySqlParameter("p_title", p_title)
            };
            var dsWFList = CommonDataOperation.ExecuteStoredProcedure("update_fbdetails_multilanguage", parameters);

            return dsWFList;
        }

        public static DataSet GetOpportunityDates(Int64 p_WkId)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_GetOpportunityDates_5(@p_WorkflowId)",
            //    new MySqlParameter("p_WorkflowId", p_WkId)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_WorkflowId", p_WkId)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("sci_GetOpportunityDates_5", parameters);

            dsTaskList.Tables[0].TableName = "loi_data";
            dsTaskList.Tables[1].TableName = "due_data";
            dsTaskList.Tables[2].TableName = "expirationdate_data";
            dsTaskList.Tables[3].TableName = "firstpostdate_data";
            dsTaskList.Tables[4].TableName = "lastmodifedpostdate_data";
            dsTaskList.Tables[5].TableName = "opendate_data";
            dsTaskList.Tables[6].TableName = "OppStatus";
            dsTaskList.Tables[7].TableName = "PreProposalDate_data";
            dsTaskList.Tables[8].TableName = "DecisionDate_data";
            return dsTaskList;
        }

        public static DataSet Delete_DateList(Int64 WFId, Int64 dateType, Int64 sequenceId, Int64 mode)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_op_delete_date_prc(@P_WORKFLOWID,@p_date_type,@p_sequence_id,@p_mode)",
            //    new MySqlParameter("P_WORKFLOWID", WFId),
            //    new MySqlParameter("p_date_type", dateType),
            //    new MySqlParameter("p_sequence_id", sequenceId),
            //    new MySqlParameter("p_mode", mode)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("P_WORKFLOWID", WFId),
                new MySqlParameter("p_date_type", dateType),
                new MySqlParameter("p_sequence_id", sequenceId),
                new MySqlParameter("p_mode", mode)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("sci_op_delete_date_prc", parameters);

            return dsTaskList;
        }

        public static DataSet Delete_Op_Dates(Int64 WFId)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_OP_OPPORTUNITY_date_del(@P_WORKFLOWID)",
            //    new MySqlParameter("P_WORKFLOWID", WFId)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
               new MySqlParameter("P_WORKFLOWID", WFId)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("SCI_OP_OPPORTUNITY_date_del", parameters);
            return dsTaskList;
        }

        public static DataSet InsUp_Op_Dates(Int64 WFId, DateTime P_date, Int64 dateType, Int64 sequenceId, string date_remarks, 
            string Lang = "en", string URL = "")
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_op_opportunity_dateins_50(@P_WORKFLOWID,@P_date," +
            //    "@P_date_type,@P_date_sequence,@p_date_remarks,@p_date_Lang,@p_URL)",
            //    new MySqlParameter("P_WORKFLOWID", WFId),
            //    new MySqlParameter("P_date", P_date),
            //    new MySqlParameter("P_date_type", dateType),
            //    new MySqlParameter("P_date_sequence", sequenceId),
            //    new MySqlParameter("p_date_remarks", date_remarks),
            //    new MySqlParameter("p_date_Lang", Lang),
            //    new MySqlParameter("p_URL", URL)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
               new MySqlParameter("P_WORKFLOWID", WFId),
                new MySqlParameter("P_date", P_date),
                new MySqlParameter("P_date_type", dateType),
                new MySqlParameter("P_date_sequence", sequenceId),
                new MySqlParameter("p_date_remarks", date_remarks),
                new MySqlParameter("p_date_Lang", Lang),
                new MySqlParameter("p_URL", URL)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("sci_op_opportunity_dateins_50", parameters);

            return dsTaskList;
        }

        public static void InsRecurring(Int64 WFId, string RecurringStatus, Int64 userid)
        {
            ScivalEntities.Database.SqlQuery<change>("CALL sci_op_recurringIns(@P_WORKFLOWID,@p_recurring,@P_UserID)",
                new MySqlParameter("P_WORKFLOWID", WFId),
                new MySqlParameter("p_recurring", RecurringStatus),
                new MySqlParameter("P_UserID", userid)).ToList();            
        }

        public static DataSet get_Op_DateList(Int64 WFId)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL get_opportunity_dateins_data5(@P_WORKFLOWID)",
            //    new MySqlParameter("P_WORKFLOWID", WFId)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
               new MySqlParameter("P_WORKFLOWID", WFId)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("get_opportunity_dateins_data5", parameters);

            return dsTaskList;
        }

        public static DataSet SCI_UPD_OPP_DATE_FLG(Int64 WFId)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_UPD_OPP_DATE_FLG(@P_WORKFLOWID)",
            //    new MySqlParameter("P_WORKFLOWID", WFId)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
               new MySqlParameter("P_WORKFLOWID", WFId)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("SCI_UPD_OPP_DATE_FLG", parameters);

            return dsTaskList;
        }

        public static DataSet GetCountryList_bygroupID(Int64 countrygroupID)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL Get_GroupCountries_prc1(@p_countrygroupid)",
            //    new MySqlParameter("p_countrygroupid", countrygroupID)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
               new MySqlParameter("p_countrygroupid", countrygroupID)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("Get_GroupCountries_prc1", parameters);

            return dsTaskList;
        }

        public static DataSet Getopportunity_location(Int64 WFId)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL opportunity_location_list(@p_workflowid)",
            //    new MySqlParameter("p_workflowid", WFId)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
              new MySqlParameter("p_workflowid", WFId)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("opportunity_location_list", parameters);

            dsTaskList.Tables[0].TableName = "LocationList";
            return dsTaskList;
        }

        public static DataSet SaveUpDelLocation(Int64 WFId, Int64 insdel, string COUNTRY, string ROOM, string STREET, string CITY, string STATE, string POSTALCODE, Int64 location_id)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL opportunity_location_insupdel(@p_workflowid,@p_insdel,@p_country,@p_room," +
            //    "@p_street,@p_city,@p_state,@p_postalcode,@p_location_id)",
            //    new MySqlParameter("p_workflowid", WFId),
            //    new MySqlParameter("p_insdel", insdel),
            //    new MySqlParameter("p_country", COUNTRY),
            //    new MySqlParameter("p_room", ROOM),
            //    new MySqlParameter("p_street", STREET),
            //    new MySqlParameter("p_city", CITY),
            //    new MySqlParameter("p_state", STATE),
            //    new MySqlParameter("p_postalcode", POSTALCODE),
            //    new MySqlParameter("p_location_id", location_id)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
              new MySqlParameter("p_workflowid", WFId),
                new MySqlParameter("p_insdel", insdel),
                new MySqlParameter("p_country", COUNTRY),
                new MySqlParameter("p_room", ROOM),
                new MySqlParameter("p_street", STREET),
                new MySqlParameter("p_city", CITY),
                new MySqlParameter("p_state", STATE),
                new MySqlParameter("p_postalcode", POSTALCODE),
                new MySqlParameter("p_location_id", location_id)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("opportunity_location_insupdel", parameters);

            dsTaskList.Tables[0].TableName = "LocationList";

            if (dsTaskList.Tables["LocationList"].Rows.Count > 0)
            {
                for (int intCount = 0; intCount < dsTaskList.Tables["LocationList"].Rows.Count; intCount++)
                {
                    string UpdateFunding_ROOM = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["LocationList"].Rows[intCount]["ROOM"].ToString()));
                    if (UpdateFunding_ROOM != "")
                        dsTaskList.Tables["LocationList"].Rows[intCount]["ROOM"] = UpdateFunding_ROOM;

                    string UpdateFunding_STREET = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["LocationList"].Rows[intCount]["STREET"].ToString()));
                    if (UpdateFunding_STREET != "")
                        dsTaskList.Tables["LocationList"].Rows[intCount]["STREET"] = UpdateFunding_STREET;

                    string Update_CITY = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["LocationList"].Rows[intCount]["CITY"].ToString()));
                    if (Update_CITY != "")
                        dsTaskList.Tables["LocationList"].Rows[intCount]["CITY"] = Update_CITY;

                    string Update_STATE = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["LocationList"].Rows[intCount]["STATENAME"].ToString()));
                    if (Update_STATE != "")
                        dsTaskList.Tables["LocationList"].Rows[intCount]["STATENAME"] = Update_STATE;
                }
                dsTaskList.Tables["LocationList"].AcceptChanges();
            }
            return dsTaskList;
        }

        public static DataSet DeleteUpDelLocation(string Deletegroupid, Int64 WFId, Int64 insdel, string COUNTRY, string ROOM, string STREET, string CITY, string STATE, string POSTALCODE, Int64 location_id)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL opportunity_location_insup_DEL(@P_DELETEGROUPID,@p_workflowid," +
            //    "@p_insdel,@p_country," +
            //    "@p_room,@p_street,@p_city,@p_state,@p_postalcode,@p_location_id)",
            //    new MySqlParameter("P_DELETEGROUPID", Convert.ToInt64(Deletegroupid)),
            //    new MySqlParameter("p_workflowid", WFId),
            //    new MySqlParameter("p_insdel", insdel),
            //    new MySqlParameter("p_country", COUNTRY),
            //    new MySqlParameter("p_room", ROOM),
            //    new MySqlParameter("p_street", STREET),
            //    new MySqlParameter("p_city", CITY),
            //    new MySqlParameter("p_state", STATE),
            //    new MySqlParameter("p_postalcode", POSTALCODE),
            //    new MySqlParameter("p_location_id", location_id)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                   new MySqlParameter("P_DELETEGROUPID", Convert.ToInt64(Deletegroupid)),
                new MySqlParameter("p_workflowid", WFId),
                new MySqlParameter("p_insdel", insdel),
                new MySqlParameter("p_country", COUNTRY),
                new MySqlParameter("p_room", ROOM),
                new MySqlParameter("p_street", STREET),
                new MySqlParameter("p_city", CITY),
                new MySqlParameter("p_state", STATE),
                new MySqlParameter("p_postalcode", POSTALCODE),
                new MySqlParameter("p_location_id", location_id)

            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("opportunity_location_insup_DEL", parameters);

            dsTaskList.Tables[0].TableName = "LocationList";

            return dsTaskList;
        }
    }
}
