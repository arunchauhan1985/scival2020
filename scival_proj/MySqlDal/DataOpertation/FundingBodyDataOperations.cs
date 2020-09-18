using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace MySqlDal
{
    public static class FundingBodyDataOperations
    {
        private static ScivalEntities ScivalEntities { get { return ScivalEntitiesInstance.GetInstance(); } }
        static Replace r = new Replace();
        static List<DashboardTask> dashBoardDetailsTaskList = null;
        static List<DashboardRemark> dashBoardDetailsRemarkList = null;


        private static int GetUserAssignmentCount(Int64 userId, Int64 moduleId)
        {
            return ScivalEntities.userassignments.Where(ua => ua.USERID == userId && ua.MODULEID == moduleId).Count();
        }

        public static List<UserFunding> GetUserFundingLists(Int64 userId, Int64 moduleId)
        {
            int userAssignmentCount = GetUserAssignmentCount(userId, moduleId);

            IQueryable<UserFunding> userFundingListQuery;

            if (userAssignmentCount == 0)
            {
                userFundingListQuery = (from fm in ScivalEntities.fundingbody_master
                                        join fb in ScivalEntities.fundingbodies on fm.FUNDINGBODY_ID equals fb.FUNDINGBODY_ID
                                        where fm.HIDDEN_FLAG == 0 && fm.STATUSCODE != 3
                                        orderby fm.DUEDATE, fm.FUNDINGBODYNAME
                                        select new UserFunding
                                        {
                                            FundingBodyId = fm.FUNDINGBODY_ID,
                                            FundingBodyName = fm.FUNDINGBODYNAME,
                                            DueDate = fm.DUEDATE
                                        });
            }
            else
            {
                userFundingListQuery = (from fm in ScivalEntities.fundingbody_master
                                        join ua in ScivalEntities.userassignments on fm.FUNDINGBODY_ID equals ua.FUNDINGBODYID
                                        where fm.HIDDEN_FLAG == 0 && ua.USERID == userId
                                        orderby fm.DUEDATE, fm.FUNDINGBODYNAME
                                        select new UserFunding
                                        {
                                            FundingBodyId = fm.FUNDINGBODY_ID,
                                            FundingBodyName = fm.FUNDINGBODYNAME,
                                            DueDate = fm.DUEDATE
                                        });
            }

            return userFundingListQuery.ToList();
        }

        public static List<UserFunding> GetUserFundingListsByTask(Int64 userId, Int64 moduleId, Int64 taskId)
        {
            var userFundingList = ScivalEntities.Database.SqlQuery<UserFunding>("CALL sci_opaw_tasklist(@pUserId,@pModuleId,@pTaskId,@pAllocation)",
                new MySqlParameter("pUserId", userId), new MySqlParameter("pModuleId", moduleId), new MySqlParameter("pTaskId", taskId), new MySqlParameter("pAllocation", 1))
                .ToList();

            return userFundingList;
        }

        public static List<DashboardUserFunding> GetDashBoardDetails(Int64 userId, string userName, Int64 id, Int64 moduleId, Int64 taskId, Int64 cycle)
        {
            dashBoardDetailsTaskList = null;
            dashBoardDetailsRemarkList = null;

            var maxCycle = ScivalEntities.sci_workflow.Where(wf => wf.MODULEID == moduleId && wf.ID == id).Max(wf => wf.CYCLE);

            if (cycle > maxCycle)
                maxCycle = cycle;

            var c1 = ScivalEntities.sci_workflow.Where(wf => wf.ID == id).Max(wf => wf.CYCLE);

            var statusIds = new List<Int64> { 8, 4 };

            var count = ScivalEntities.sci_workflow.Where(wf => wf.ID == id && wf.TASKID == taskId && wf.CYCLE == c1 && statusIds.Contains(wf.STATUSID.Value)).Count();

            if (count > 0)
                throw new ScivalDataException("Critical Error.");

            count = ScivalEntities.sci_workflow.Where(wf => wf.ID == id && wf.MODULEID == moduleId && wf.TASKID == taskId && wf.CYCLE == maxCycle && wf.STATUSID.Value == 7 && wf.STARTBY == userId).Count();

            List<sci_workflow> workflows = null;

            if (count == 0)
                workflows = ScivalEntities.sci_workflow.Where(wf => wf.ID == id && wf.MODULEID == moduleId && wf.TASKID == taskId && wf.CYCLE == maxCycle && wf.STATUSID.Value != 7).ToList();
            else
                workflows = ScivalEntities.sci_workflow.Where(wf => wf.ID == id && wf.MODULEID == moduleId && wf.TASKID == taskId && wf.CYCLE == maxCycle).ToList();

            foreach (sci_workflow workflow in workflows)
            {
                workflow.STARTBY = userId;
                workflow.STARTDATE = DateTime.Today;
                workflow.STATUSID = 7;
            }

            var rowCount = ScivalEntities.SaveChanges();

            if (count == 0 && rowCount == 0)
                throw new ScivalDataException("Oops! Task Already Started.");

            var dashboardDetailUserFunding = new List<DashboardUserFunding>();

            var countbouts = ScivalEntities.abouts.Where(ab => ab.FUNDINGBODY_ID == id).Count();
            var countfunding = ScivalEntities.fundingbodies.Where(ab => ab.FUNDINGBODY_ID == id).Count();
            var countrelated = ScivalEntities.relatedorgs.Where(ab => ab.FUNDINGBODY_ID == id).Count();
            var countestInfo = ScivalEntities.establishmentinfoes.Where(ab => ab.FUNDINGBODY_ID == id).Count();
            var countfunprtypes = ScivalEntities.fundedprogramstypes.Where(ab => ab.FUNDINGBODY_ID == id).Count();
            var countcontTypes = ScivalEntities.contactinfoes.Where(ab => ab.FUNDINGBODY_ID == id).Count();
            var countcontacts = ScivalEntities.contacts.Where(ab => ab.FUNDINGBODY_ID == id).Count();
            var countfpolicy = ScivalEntities.fundingpolicies.Where(ab => ab.FUNDINGBODY_ID == id).Count();
            var countclassgroup = ScivalEntities.classificationgroups.Where(ab => ab.FUNDINGBODY_ID == id).Count();

            dashboardDetailUserFunding.Add(new DashboardUserFunding { Tab = "About", Count = ScivalEntities.abouts.Where(ab => ab.FUNDINGBODY_ID == id).Count() });
            dashboardDetailUserFunding.Add(new DashboardUserFunding { Tab = "Fundingbody", Count = ScivalEntities.fundingbodies.Where(ab => ab.FUNDINGBODY_ID == id).Count() });
            dashboardDetailUserFunding.Add(new DashboardUserFunding { Tab = "Related Orgs", Count = ScivalEntities.relatedorgs.Where(ab => ab.FUNDINGBODY_ID == id).Count() });
            dashboardDetailUserFunding.Add(new DashboardUserFunding { Tab = "Establishment Info", Count = ScivalEntities.establishmentinfoes.Where(ab => ab.FUNDINGBODY_ID == id).Count() });
            dashboardDetailUserFunding.Add(new DashboardUserFunding { Tab = "Funded Program Types", Count = ScivalEntities.fundedprogramstypes.Where(ab => ab.FUNDINGBODY_ID == id).Count() });
            dashboardDetailUserFunding.Add(new DashboardUserFunding { Tab = "Contact Info", Count = ScivalEntities.contactinfoes.Where(ab => ab.FUNDINGBODY_ID == id).Count() });
            dashboardDetailUserFunding.Add(new DashboardUserFunding { Tab = "Contacts", Count = ScivalEntities.contacts.Where(ab => ab.FUNDINGBODY_ID == id).Count() });
            dashboardDetailUserFunding.Add(new DashboardUserFunding { Tab = "Funding Policy", Count = ScivalEntities.fundingpolicies.Where(ab => ab.FUNDINGBODY_ID == id).Count() });
            dashboardDetailUserFunding.Add(new DashboardUserFunding { Tab = "Classification Group", Count = ScivalEntities.classificationgroups.Where(ab => ab.FUNDINGBODY_ID == id).Count() });

            dashBoardDetailsTaskList = (from sw in ScivalEntities.sci_workflow
                                        join st in ScivalEntities.sci_tasks on sw.TASKID equals st.TASKID
                                        join fm in ScivalEntities.fundingbody_master on sw.ID equals fm.FUNDINGBODY_ID
                                        where sw.MODULEID == moduleId && sw.ID == id && sw.CYCLE == maxCycle
                                        orderby sw.SEQUENCE
                                        select new DashboardTask { TaskName = st.TASKNAME, TaskId = st.TASKID, WorkFlowId = sw.WORKFLOWID, FundingBodyName = fm.FUNDINGBODYNAME, Sequence = sw.SEQUENCE.Value })
                         .ToList();

            dashBoardDetailsRemarkList = (from str in ScivalEntities.sci_timesheetremarks
                                          join sw in ScivalEntities.sci_workflow on str.WORKFLOWID equals sw.WORKFLOWID
                                          where sw.MODULEID == moduleId && sw.ID == id && sw.CYCLE == cycle
                                          orderby str.CREATEDDATE
                                          select new DashboardRemark
                                          {
                                              Remark = str.REMARKS,
                                              Task = (sw.TASKID == 1) ? "Collection" : "Quality Check",
                                              UserName = userName,
                                              CreatedDate = str.CREATEDDATE.Value
                                          }
                    )
                    .ToList();

            foreach (DashboardRemark remark in dashBoardDetailsRemarkList)
                remark.CreatedDate1 = remark.CreatedDate.ToString("DD-Mon-YYYY HH:MI AM");

            return dashboardDetailUserFunding;
        }

        public static string GetFundingBodyMainJson(long FBID)
        {
            string MyConnection2 = "datasource=localhost;username=root;password=root";
            MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
            DataTable dt = new DataTable();
            try
            {
                string Query = "select DATAJSON from scival.fundingbody_main where FUNDINGBODY_ID='" + FBID + "';";
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand2;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                dt = dTable;
                return Convert.ToString(dt.Rows[0][0]);
                // MyConn2.Close();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                MyConn2.Close();
            }
        }



        public static List<DashboardTask> GetDashBoardDetailsTaskList()
        {
            return dashBoardDetailsTaskList;
        }

        public static List<DashboardRemark> GetDashBoardDetailsRemarkList()
        {
            return dashBoardDetailsRemarkList;
        }

        public static List<sci_language_master> GetLanguageMasters(Int64 langauageLength)
        {
            return ScivalEntities.sci_language_master.Where(l => l.CODE_LENGTH == langauageLength).OrderBy(l => l.LANGUAGE_NAME).ToList();
        }

        public static DataSet SaveAndupdateAwardlist5(Int64 WFId, Int64 pagemode, string Status, string LASTVISITED, string URl, string p_lang, string name, string Frequency,
            string date_captureStart, string date_captureEnd, string Comment)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_url", URl),
                new MySqlParameter("p_status", Status),
                new MySqlParameter("p_lastvisited", LASTVISITED),
                new MySqlParameter("p_Workflow_id", WFId),
                new MySqlParameter("p_lang", p_lang),
                new MySqlParameter("p_name", name),
                new MySqlParameter("p_Frequency", Frequency),
                new MySqlParameter("p_date_captureStart", date_captureStart),
                new MySqlParameter("p_date_captureEnd", date_captureEnd),
                new MySqlParameter("p_Comment", Comment),
                new MySqlParameter("p_mode", pagemode),
                new MySqlParameter("p_award_source_id", null),

            };

            var estFundings = CommonDataOperation.ExecuteStoredProcedure("awardssource_dml_prc_5", parameters);


            estFundings.Tables[0].TableName = "ItemListDisplay";

            return estFundings;
        }

        public static DataSet GetAwardSource(Int64 WFId, Int64 pagemode)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_Workflow_id", WFId),
                 new MySqlParameter("p_mode", pagemode)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("awardssource_dml_prc_5", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet updateAwardlist(Int64 WFId, Int64 pagemode, Int64 AWARD_SOURCE_ID, string Status, string LASTVISITED, string URl, string p_lang)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_url", URl),
                new MySqlParameter("p_status", Status),
                new MySqlParameter("p_lastvisited", LASTVISITED),
                new MySqlParameter("p_Workflow_id", WFId),
                new MySqlParameter("p_lang", p_lang),
               new MySqlParameter("p_mode", pagemode),
               new MySqlParameter("p_award_source_id", pagemode),
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("awardssource_dml_prc", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet updateAwardlist5(Int64 WFId, Int64 pagemode, Int64 AWARD_SOURCE_ID, string Status, string LASTVISITED,
            string URl, string p_lang, string name, string Frequency, string date_captureStart, string date_captureEnd, string Comment)
        {

            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_url", URl),
                new MySqlParameter("p_status", Status),
                new MySqlParameter("p_lastvisited", LASTVISITED),
                new MySqlParameter("p_Workflow_id", WFId),
                new MySqlParameter("p_lang", p_lang),
                new MySqlParameter("p_name", name),
                new MySqlParameter("p_Frequency", Frequency),
                new MySqlParameter("p_date_captureStart", date_captureStart),
                new MySqlParameter("p_date_captureEnd", date_captureEnd),
                new MySqlParameter("p_Comment", Comment),
                new MySqlParameter("p_mode", pagemode),
                new MySqlParameter("p_award_source_id", null),
            };

            var estFundings = CommonDataOperation.ExecuteStoredProcedure("awardssource_dml_prc_5", parameters);

            estFundings.Tables[0].TableName = "ItemListDisplay";

            return estFundings;
        }

        public static DataSet GetAwardCurrency()
        {
            //var estFundings = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_CURRENCYTYPElist").FirstOrDefault();
            var estFundings = CommonDataOperation.ExecuteStoredProcedureWithoutParam("SCI_CURRENCYTYPElist");//, parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet GetAwardStatistics(Int64 WFid)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFid)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_AWARDSTATISTICSlist", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet SaveAndUpdateAwardSta(Int64 WFID, Int64 mode, string currency, string amount, string url, string linkText)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFID),
                new MySqlParameter("p_insdel", mode),
                new MySqlParameter("p_currency", currency),
                new MySqlParameter("p_totalfunding_text", amount),
                new MySqlParameter("p_url", url),
                new MySqlParameter("p_link_text", linkText),
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_AWARDSTATISTICSinsert", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet GetClassiFicatilList(Int64 WFid)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFid)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_classificationlist", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet GetSubASJCTypeList()
        {
            var estFundings = CommonDataOperation.ExecuteStoredProcedureWithoutParam("asjc_description_prc");
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet SaveClassiFication(Int64 WFId, string type, Int64 frequency, string[] code, string[] ClaasText)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFId),
                new MySqlParameter("p_type", type),
                new MySqlParameter("p_FREQUENCY", frequency),
                new MySqlParameter("p_CODE", code),
                new MySqlParameter("p_CLASSIFICATION_TEXT", ClaasText)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_classificationinsert", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet UpdateClassification(Int64 WFId, Int64 mode, string type, Int64 frequency, string code, string ClaasText, string ClassFcID)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("P_WORKFLOWID", WFId),
                new MySqlParameter("P_INSDEL", mode),
                new MySqlParameter("P_TYPE", type),
                new MySqlParameter("P_FREQUENCY", frequency),
                new MySqlParameter("P_CODE", code),
                new MySqlParameter("P_CLASSIFICATION_TEXT", ClaasText),
                new MySqlParameter("P_CLASSIFICATIONS_ID", ClassFcID),
                new MySqlParameter("O_FREQUENCY", null),
                new MySqlParameter("O_CODE", null)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("SCI_CLASSIFICATIONUPDEL", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        //public static DataSet DeleteClassification(Int64 WFId, Int64 mode, string type, Int64 frequency, string code, string ClaasText, string ClassFcID)
        //{
        //    //var estFundings = ScivalEntities.Database.SqlQuery<DataSet>("CALL SCI_CLASSIFICATIONUPDEL(@P_WORKFLOWID,@P_INSDEL,@P_TYPE," +
        //    //    "@P_FREQUENCY,@P_CODE,@P_CLASSIFICATION_TEXT,@P_CLASSIFICATIONS_ID,@O_FREQUENCY,@O_CODE)",
        //    //    new MySqlParameter("P_WORKFLOWID", WFId),
        //    //    new MySqlParameter("P_INSDEL", mode),
        //    //    new MySqlParameter("P_TYPE", type),
        //    //    new MySqlParameter("P_FREQUENCY", frequency),
        //    //    new MySqlParameter("P_CODE", code),
        //    //    new MySqlParameter("P_CLASSIFICATION_TEXT", ClaasText),
        //    //    new MySqlParameter("P_CLASSIFICATIONS_ID", ClassFcID),
        //    //    new MySqlParameter("O_FREQUENCY", null),
        //    //    new MySqlParameter("O_CODE", null)).FirstOrDefault();
        //    var parameters = new List<MySqlParameter>
        //    {
        //        new MySqlParameter("P_WORKFLOWID", WFId),
        //        new MySqlParameter("P_INSDEL", mode),
        //        new MySqlParameter("P_TYPE", type),
        //        new MySqlParameter("P_FREQUENCY", frequency),
        //        new MySqlParameter("P_CODE", code),
        //        new MySqlParameter("P_CLASSIFICATION_TEXT", ClaasText),
        //        new MySqlParameter("P_CLASSIFICATIONS_ID", ClassFcID),
        //        new MySqlParameter("O_FREQUENCY", null),
        //        new MySqlParameter("O_CODE", null)
        //    };
        //    var estFundings = CommonDataOperation.ExecuteStoredProcedure("SCI_CLASSIFICATIONUPDEL", parameters);

        //    estFundings.Tables[0].TableName = "ItemListDisplay";

        //    return estFundings;
        //}

        public static DataSet GetContactsList(Int64 WFId, Int64 pagemode)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFId),
                new MySqlParameter("p_mode", pagemode)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_contactlist", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet SaveAndDeleteContactsLIst(Int64 WFId, Int64 pagemode, Int64 WorkMode, Int64 Contact_Id, string TYPE,
            string TITLE, string TELEPHONE, string FAX, string EMAIL, string url, string WEBSITE_TEXT, string COUNTRY, string ROOM,
            string STREET, string STATE, string CITY, string POSTALCODE, string PREFIX, string GIVENNAME, string MIDDLENAME,
            string SURNAME, string SUFFIX, string Lang)
        {
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
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_contactinsert_s2", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet GetEstablishInfo(Int64 WFId)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFId)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_establishmentlist", parameters);
            estFundings.Tables[0].TableName = "Country";
            estFundings.Tables[1].TableName = "State";
            estFundings.Tables[2].TableName = "DisplayData";
            return estFundings;
        }

        public static DataSet SaveEstablishInfo(Int64 WFID, string date, string city, string state, string CountryCode, string desc, string lang = "en")
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFID),
                new MySqlParameter("p_ESTABLISHMENTDATE", date),
                new MySqlParameter("p_ESTABLISHMENTCITY", city),
                new MySqlParameter("p_ESTABLISHMENTSTATE", state),
                new MySqlParameter("p_ESTABLISHMENTCOUNTRYCODE", CountryCode),
                new MySqlParameter("p_ESTABLISHMENTDESCRIPTION", desc),
                new MySqlParameter("p_lang", lang)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_establishmentinsert", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet getTextIdsForddl()
        {
            var estFundings = CommonDataOperation.ExecuteStoredProcedureWithoutParam("sci_gettaxidtype");
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet AddandUpdateTexIDS(Int64 WFid, string texId, string TexText, Int64 mode, Int64 userid, Int64 FinancialInfo)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFid),
                new MySqlParameter("p_insdel", mode),
                new MySqlParameter("p_type", TexText),
                new MySqlParameter("p_taxids_text", texId),
                new MySqlParameter("p_userid", userid),
                new MySqlParameter("p_FINANCIALINFO_ID", FinancialInfo)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_taxids", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet AddandUpdateFiscalDate(Int64 WFid, DateTime FinancialDate, Int64 mode, Int64 userid, Int64 FinancialInfo)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFid),
                new MySqlParameter("p_insdel", mode),
                new MySqlParameter("p_userid", userid),
                new MySqlParameter("p_FINANCIALINFO_ID", FinancialInfo),
                new MySqlParameter("p_fiscalclosedate_column", FinancialDate)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_fiscalclosedate", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet getTex(Int64 WFid)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFid)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_gettaxids", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet getFiscal(Int64 WFid)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFid)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_getfiscalclosedate", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet GetFundedProgramType(Int64 WFid)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFid)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_FundedProgramTypelist", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet SaveAndDeleteFundedProType_For_Window(Int64 WFId, string TypesId, string NewTypeId, string OldTypeId, Int64 mode)
        {
            var parameters = new List<MySqlParameter>
            {
                 new MySqlParameter("p_workflowid", WFId),
                new MySqlParameter("p_id", TypesId),
                new MySqlParameter("p_typeid", NewTypeId),
                new MySqlParameter("p_mode", mode),
                new MySqlParameter("p_oldtypeid", OldTypeId)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sciFundedProgramType_win", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet SaveAndDeleteFundedProType(Int64 WFId, string TypesId, string NewTypeId, string OldTypeId, Int64 mode)
        {
            var parameters = new List<MySqlParameter>
            {
               new MySqlParameter("p_workflowid", WFId),
                new MySqlParameter("p_id", TypesId),
                new MySqlParameter("p_typeid", NewTypeId),
                new MySqlParameter("p_mode", mode),
                new MySqlParameter("p_oldtypeid", OldTypeId)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sciFundedProgramType", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet SaveFundingBodyLang(DataTable FundingBodyLang)
        {
            //try
            //{
            DataSet dsTaskList = new DataSet();
            for (Int32 x = 0; x < FundingBodyLang.Rows.Count; x++)
            {
                var parameters = new List<MySqlParameter>
            {
               new MySqlParameter("tran_id_in", FundingBodyLang.Rows[x]["tran_id"]),
                            new MySqlParameter("scival_id_in", FundingBodyLang.Rows[x]["FUNDINGBODY_ID"]),
                            new MySqlParameter("column_desc_in", FundingBodyLang.Rows[x]["COLUMN_DESC"]),
                            new MySqlParameter("column_id_in", FundingBodyLang.Rows[x]["COLUMN_ID"]),
                            new MySqlParameter("moduleid_in", FundingBodyLang.Rows[x]["MODE_ID"]),
                            new MySqlParameter("language_id_in", FundingBodyLang.Rows[x]["LANGUAGE_ID"]),
                            new MySqlParameter("tran_type_id_in", FundingBodyLang.Rows[x]["TRAN_TYPE_ID"]),
                            new MySqlParameter("flag_in", FundingBodyLang.Rows[x]["FLAG_IN"])
            };
                dsTaskList = CommonDataOperation.ExecuteStoredProcedure("sci_language_detail_dml_prc", parameters);
            }
            //}
            //catch (Exception error)
            //{ 

            //}
            return dsTaskList;
        }
        public static DataSet SaveFundingBody(DataTable FundingBody)
        {
            DataSet dsTaskList = new DataSet();
            DataTable DT = new DataTable();
            if (FundingBody.Rows.Count > 0)
            {
                for (Int32 x = 0; x < FundingBody.Rows.Count; x++)
                {
                    try
                    {
                        var parameters = new List<MySqlParameter>
                         {
                        new MySqlParameter("p_FUNDINGBODYID", FundingBody.Rows[x]["FUNDINGBODY_ID"].ToString()),
                           new MySqlParameter("p_ORGDBID", FundingBody.Rows[x]["ORGDBID"].ToString()),
                           new MySqlParameter("p_TYPE", FundingBody.Rows[x]["type"].ToString()),
                           new MySqlParameter("p_TRUSTING", FundingBody.Rows[x]["TRUSTING"].ToString()),
                           new MySqlParameter("p_COUNTRY", FundingBody.Rows[x]["country"].ToString()),
                           new MySqlParameter("p_STATE", r.WieredChar_ReplacementHexValue(FundingBody.Rows[x]["state"].ToString())),
                           new MySqlParameter("p_COLLECTIONCODE", r.WieredChar_ReplacementHexValue(FundingBody.Rows[x]["COLLECTIONCODE"].ToString())),
                           new MySqlParameter("p_HIDDEN", FundingBody.Rows[x]["HIDDEN"].ToString()),
                           new MySqlParameter("p_CANONICALNAME", r.WieredChar_ReplacementHexValue(FundingBody.Rows[x]["CANONICALNAME"].ToString())),
                           new MySqlParameter("p_PREFERREDORGNAME", r.WieredChar_ReplacementHexValue(FundingBody.Rows[x]["PREFERREDORGNAME"].ToString())),
                           new MySqlParameter("p_CONTEXTNAME", r.WieredChar_ReplacementHexValue(FundingBody.Rows[x]["CONTEXTNAME"].ToString())),
                           new MySqlParameter("p_ABBREVNAME", r.WieredChar_ReplacementHexValue(FundingBody.Rows[x]["ABBREVNAME"].ToString())),
                           new MySqlParameter("p_ELIGIBILITYDESCRIPTION", r.WieredChar_ReplacementHexValue(FundingBody.Rows[x]["ELIGIBILITYDESCRIPTION"].ToString())),
                           new MySqlParameter("p_IDsubtypeid", FundingBody.Rows[x]["SUBTYPEID"].ToString()),
                           new MySqlParameter("p_SUBTYPE_TEXT", FundingBody.Rows[x]["WFID"].ToString()),
                           new MySqlParameter("p_workflowid", FundingBody.Rows[x]["FUNDINGBODY_ID"].ToString()),
                           new MySqlParameter("p_userid", FundingBody.Rows[x]["LOGINId"].ToString()),
                           new MySqlParameter("p_projectname", FundingBody.Rows[x]["PAGENAME"].ToString()),
                           new MySqlParameter("p_recordsource", r.WieredChar_ReplacementHexValue(FundingBody.Rows[x]["RECORDSOURCE"].ToString())),
                           new MySqlParameter("p_awardsuccesrate", r.WieredChar_ReplacementHexValue(FundingBody.Rows[x]["AWARDSUCCESSRATE"].ToString())),
                           new MySqlParameter("p_comment_desc", r.WieredChar_ReplacementHexValue(FundingBody.Rows[x]["COMMENT"].ToString())),
                           new MySqlParameter("p_defunct", FundingBody.Rows[x]["DEFUNCT"].ToString()),
                           new MySqlParameter("p_crossrefid",  r.WieredChar_ReplacementHexValue(FundingBody.Rows[x]["CROSSREFID"].ToString())),
                           new MySqlParameter("p_extendedRecord", FundingBody.Rows[x]["extendedRecord"].ToString()),
                           new MySqlParameter("p_CapOp", r.WieredChar_ReplacementHexValue(FundingBody.Rows[x]["CapOpp"].ToString())),
                           new MySqlParameter("p_OPPSup", r.WieredChar_ReplacementHexValue(FundingBody.Rows[x]["OPPSup"].ToString())),
                           new MySqlParameter("p_CapAwards", FundingBody.Rows[x]["CapAwards"].ToString()),
                           new MySqlParameter("p_AwardsSup", r.WieredChar_ReplacementHexValue(FundingBody.Rows[x]["AwardsSup"].ToString())),
                           new MySqlParameter("p_TierInfo", FundingBody.Rows[x]["TierInfo"].ToString()),
                           new MySqlParameter("p_profit", FundingBody.Rows[x]["profit"].ToString()),
                           new MySqlParameter("p_oppFrequency", FundingBody.Rows[x]["opportunitiesFrequency"].ToString()),
                           new MySqlParameter("p_awFrequency", FundingBody.Rows[x]["awardsFrequency"].ToString()),
                            new MySqlParameter("p_status", FundingBody.Rows[x]["status"].ToString())
                        };
                        dsTaskList = CommonDataOperation.ExecuteStoredProcedure("sci_fundingbase_41", parameters);
                        dsTaskList.Tables[0].TableName = "FundingBodyTable";
                    }
                    catch (Exception ex)
                    {
                        //oErrorLog.WriteErrorLog(exp);
                    }
                }
            }
            dsTaskList.Tables.Add(DT);
            if (dsTaskList.Tables.Count > 0)
            {
                if (dsTaskList.Tables["FundingBodyTable"].Rows.Count > 0)
                {
                    for (int intCount = 0; intCount < dsTaskList.Tables["FundingBodyTable"].Rows.Count; intCount++)
                    {
                        string UpdateFunding_STATE = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["STATE"].ToString()));
                        if (UpdateFunding_STATE != "")
                        {
                            dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["STATE"] = UpdateFunding_STATE;
                        }
                        string UpdateFunding_COLLECTIONCODE = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["COLLECTIONCODE"].ToString()));
                        if (UpdateFunding_COLLECTIONCODE != "")
                        {
                            dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["COLLECTIONCODE"] = UpdateFunding_COLLECTIONCODE;
                        }
                        string UpdateFunding_ELIGIBILITYDESCRIPTION = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["ELIGIBILITYDESCRIPTION"].ToString()));
                        if (UpdateFunding_ELIGIBILITYDESCRIPTION != "")
                        {
                            dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["ELIGIBILITYDESCRIPTION"] = UpdateFunding_ELIGIBILITYDESCRIPTION;
                        }
                        string UpdateFunding_RECORDSOURCE = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["RECORDSOURCE"].ToString()));
                        if (UpdateFunding_RECORDSOURCE != "")
                        {
                            dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["RECORDSOURCE"] = UpdateFunding_RECORDSOURCE;
                        }
                        string UpdateFunding_AWARDSUCCESRATE = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["AWARDSUCCESRATE"].ToString()));
                        if (UpdateFunding_AWARDSUCCESRATE != "")
                        {
                            dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["AWARDSUCCESRATE"] = UpdateFunding_AWARDSUCCESRATE;
                        }
                        string UpdateFunding_COMMENT = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["comment_desc"].ToString()));
                        if (UpdateFunding_COMMENT != "")
                        {
                            dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["comment_desc"] = UpdateFunding_COMMENT;
                        }

                        string UpdateFunding_CapOpp = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["captureopportunities"].ToString()));
                        if (UpdateFunding_CapOpp != "")
                        {
                            dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["captureopportunities"] = UpdateFunding_CapOpp;
                        }

                        string UpdateFunding_OPPSup = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["opportunitiessupplier"].ToString()));
                        if (UpdateFunding_OPPSup != "")
                        {
                            dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["opportunitiessupplier"] = UpdateFunding_OPPSup;
                        }

                        string UpdateFunding_AwardsSup = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["awardssupplier"].ToString()));
                        if (UpdateFunding_AwardsSup != "")
                        {
                            dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["awardssupplier"] = UpdateFunding_AwardsSup;
                        }

                        string UpdateFunding_CROSSREFID = r.Return_WieredChar_Original(Convert.ToString(dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["CROSSREFID"].ToString()));
                        if (UpdateFunding_CROSSREFID != "")
                        {
                            dsTaskList.Tables["FundingBodyTable"].Rows[intCount]["CROSSREFID"] = UpdateFunding_CROSSREFID;
                        }
                    }
                    dsTaskList.Tables["FundingBodyTable"].AcceptChanges();
                }
            }

            return dsTaskList;
        }
        public static DataSet GetProgress(Int64 FundingBodyId)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("P_ID", FundingBodyId)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_proc_fundingbodyprogres_s5", parameters);
            estFundings.Tables[0].TableName = "Progress";
            return estFundings;
        }

        public static string CheckHiddenStatus_fb(Int64 wkId)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_WorkId", wkId)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_GetHiddenStatus", parameters);
            if (estFundings != null && estFundings.Tables.Count > 0)
            {
                estFundings.Tables[0].TableName = "CheckHiddenStatus";
                return estFundings.Tables["CheckHiddenStatus"].Rows[0][0].ToString();
            }
            else
                return "";
        }

        public static DataSet Mandatorysectionfunction(Int32 Wfid)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_WorkId", Wfid)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_GetMandatorySection", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet SaveAndupdateFundingbodySource5(Int64 WFId, Int64 pagemode, string Status, string URl, string p_lang,
            string name, string Frequency, string date_captureStart, string date_captureEnd, string Comment)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_url", URl),
                new MySqlParameter("p_status", Status),
                new MySqlParameter("p_Workflow_id", WFId),
                new MySqlParameter("p_lang", p_lang),
                new MySqlParameter("p_name", name),
                new MySqlParameter("p_Frequency", Frequency),
                new MySqlParameter("p_date_captureStart", date_captureStart),
                new MySqlParameter("p_date_captureEnd", date_captureEnd),
                new MySqlParameter("p_Comment", Comment),
                new MySqlParameter("p_mode", pagemode),
                new MySqlParameter("p_opp_source_id", null)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("fundingbodysource_dml_prc_5", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet GetFundingbodiesSource(Int64 WFId, Int64 pagemode)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_Workflow_id", WFId),
                new MySqlParameter("p_mode", pagemode)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("fundingbodysource_dml_prc_5", parameters);
            estFundings.Tables[0].TableName = "ItemListDDLDisplay";
            return estFundings;
        }

        public static DataSet updateFundingbodySource5(Int64 WFId, Int64 pagemode, Int64 AWARD_SOURCE_ID, string Status,
            string LASTVISITED, string URl, string p_lang, string name, string Frequency, string date_captureStart,
            string date_captureEnd, string Comment)
        {
            var parameters = new List<MySqlParameter>
            {
                    new MySqlParameter("p_url", URl),
                    new MySqlParameter("p_status", Status),
                    new MySqlParameter("p_Workflow_id", WFId),
                    new MySqlParameter("p_lang", p_lang),
                    new MySqlParameter("p_name", name),
                    new MySqlParameter("p_Frequency", Frequency),
                    new MySqlParameter("p_date_captureStart", date_captureStart),
                    new MySqlParameter("p_date_captureEnd", date_captureEnd),
                    new MySqlParameter("p_Comment", Comment),
                    new MySqlParameter("p_mode", pagemode),
                    new MySqlParameter("p_opp_source_id", AWARD_SOURCE_ID)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("fundingbodysource_dml_prc_5", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet GetItemsList(Int64 WFId, Int64 pagemode)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFId),
                new MySqlParameter("p_mode", pagemode)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_itemlist5", parameters);
            estFundings.Tables[1].TableName = "ItemListDisplay";
            return estFundings;
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

            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_iteminserttemp5", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet SaveAndupdateOpportunitieslist5(Int64 WFId, Int64 pagemode, string Status, string URl, string p_lang,
            string name, string Frequency, string date_captureStart, string date_captureEnd, string Comment)
        {
            var parameters = new List<MySqlParameter>
            {
                    new MySqlParameter("p_url", URl),
                    new MySqlParameter("p_status", Status),
                    new MySqlParameter("p_Workflow_id", WFId),
                    new MySqlParameter("p_lang", p_lang),
                    new MySqlParameter("p_name", name),
                    new MySqlParameter("p_Frequency", Frequency),
                    new MySqlParameter("p_date_captureStart", date_captureStart),
                    new MySqlParameter("p_date_captureEnd", date_captureEnd),
                    new MySqlParameter("p_Comment", Comment),
                    new MySqlParameter("p_mode", pagemode),
                    new MySqlParameter("p_opp_source_id", null)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("opportunitiessource_dml_prc_5", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet GetOpportunitiesSource(Int64 WFId, Int64 pagemode)
        {
            var parameters = new List<MySqlParameter>
            {
                  new MySqlParameter("p_Workflow_id", WFId),
                  new MySqlParameter("p_mode", pagemode)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("opportunitiessource_dml_prc_5", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet updateOpportuntiteslist(Int64 WFId, Int64 pagemode, Int64 AWARD_SOURCE_ID, string Status, string LASTVISITED, string URl, string p_lang,
            string name, string Frequency, string date_captureStart, string date_captureEnd, string Comment)
        {
            var parameters = new List<MySqlParameter>
            {
                    new MySqlParameter("p_url", URl),
                    new MySqlParameter("p_status", Status),
                    new MySqlParameter("p_Workflow_id", WFId),
                    new MySqlParameter("p_lang", p_lang),
                    new MySqlParameter("p_name", name),
                    new MySqlParameter("p_Frequency", Frequency),
                    new MySqlParameter("p_date_captureStart", date_captureStart),
                    new MySqlParameter("p_date_captureEnd", date_captureEnd),
                    new MySqlParameter("p_Comment", Comment),
                    new MySqlParameter("p_mode", pagemode),
                    new MySqlParameter("p_opp_source_id", AWARD_SOURCE_ID)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("opportunitiessource_dml_prc_5", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet SaveAndupdatePublicationSource5(Int64 WFId, Int64 pagemode, string Status, string URl, string p_lang,
            string name, string Frequency, string date_captureStart, string date_captureEnd, string Comment)
        {
            var parameters = new List<MySqlParameter>
            {
                    new MySqlParameter("p_url", URl),
                    new MySqlParameter("p_status", Status),
                    new MySqlParameter("p_Workflow_id", WFId),
                    new MySqlParameter("p_lang", p_lang),
                    new MySqlParameter("p_name", name),
                    new MySqlParameter("p_Frequency", Frequency),
                    new MySqlParameter("p_date_captureStart", date_captureStart),
                    new MySqlParameter("p_date_captureEnd", date_captureEnd),
                    new MySqlParameter("p_Comment", Comment),
                    new MySqlParameter("p_mode", pagemode),
                    new MySqlParameter("p_opp_source_id", null)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("publicationsource_dml_prc_5", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet GetPublicationsSource(Int64 WFId, Int64 pagemode)
        {
            var parameters = new List<MySqlParameter>
            {
                    new MySqlParameter("p_Workflow_id", WFId),
                    new MySqlParameter("p_mode", pagemode)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("publicationsource_dml_prc_5", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet updatePublicationSource5(Int64 WFId, Int64 pagemode, Int64 AWARD_SOURCE_ID, string Status, string LASTVISITED,
            string URl, string p_lang, string name, string Frequency, string date_captureStart, string date_captureEnd, string Comment)
        {
            var parameters = new List<MySqlParameter>
            {
                    new MySqlParameter("p_url", URl),
                    new MySqlParameter("p_status", Status),
                    new MySqlParameter("p_Workflow_id", WFId),
                    new MySqlParameter("p_lang", p_lang),
                    new MySqlParameter("p_name", name),
                    new MySqlParameter("p_Frequency", Frequency),
                    new MySqlParameter("p_date_captureStart", date_captureStart),
                    new MySqlParameter("p_date_captureEnd", date_captureEnd),
                    new MySqlParameter("p_Comment", Comment),
                    new MySqlParameter("p_mode", pagemode),
                    new MySqlParameter("p_opp_source_id", AWARD_SOURCE_ID)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("publicationsource_dml_prc_5", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static DataSet GetRelatedOrgs(Int64 WFId)
        {
            var parameters = new List<MySqlParameter>
            {
                   new MySqlParameter("p_workflowid", WFId)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_relatedorglist", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            estFundings.Tables[2].TableName = "RelatedOrgs";
            estFundings.Tables[3].TableName = "FundingBody";
            return estFundings;
        }

        public static DataSet GetVendorFundingbody(int? rel_orgs_vendorid, int? VendorId, string FBName, string mode)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_rel_orgs_vendorid", rel_orgs_vendorid),
                new MySqlParameter("p_vendor_id", VendorId),
                new MySqlParameter("p_vendor_fundingbody_name", FBName),
                new MySqlParameter("p_mode", mode)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_related_vendor_dml_prc", parameters);
            estFundings.Tables[0].TableName = "VendorFundingbody";// "ItemListDisplay";
            return estFundings;
        }

        public static DataSet SaveAndDeleteRelatedOrgs(Int64 WFID, Int64 mode, string Hieracrhy, string ORgDbId, string reltype)
        {
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFID),
                new MySqlParameter("p_insdel", mode),
                new MySqlParameter("p_HIERARCHY", Hieracrhy),
                new MySqlParameter("p_orgdbid", ORgDbId),
                new MySqlParameter("p_RELTYPE", reltype)
            };
            var estFundings = CommonDataOperation.ExecuteStoredProcedure("sci_relatedorgsinsert", parameters);
            estFundings.Tables[0].TableName = "ItemListDisplay";
            return estFundings;
        }

        public static void saveandUpdateJSONinTable(string P_fundingBodyProjectId, string P_json, string createdby, string createdTime, string modifiedby, string ModifiedTime, int trigger)
        {
            string MyConnection2 = "datasource=localhost;username=root;password=root";
            MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
            DataTable dt = new DataTable();
            try
            {
                string Query = "select FUNDINGBODY_ID from scival.fundingbody_main where FUNDINGBODY_ID='" + P_fundingBodyProjectId + "';";
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand2;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                dt = dTable;
                // MyConn2.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                MyConn2.Close();
            }
            if (dt.Rows.Count > 0)
            {
                trigger = 2;
                modifiedby = createdby;
                ModifiedTime = createdTime;
            }
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("P_fundingBodyProjectId", P_fundingBodyProjectId),
                new MySqlParameter("P_json", P_json),
                new MySqlParameter("P_createdby", createdby),
                new MySqlParameter("P_createddate", createdTime),
                new MySqlParameter("P_modifiedby", modifiedby),
                new MySqlParameter("P_modifieddate", ModifiedTime),
                new MySqlParameter("P_trigger", trigger),

            };

            CommonDataOperation.ExecuteStoredProcedure("savefundingbody_json", parameters);
        }

        public static DataTable GetIdentifiers(string P_fundingBodyProjectId)
        {
            string MyConnection2 = "datasource=localhost;username=root;password=root";
            MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
            DataTable dt = new DataTable();
            try
            {
                string Query = "select type,value from scival.identifier where FUNDINGBODY_ID='" + P_fundingBodyProjectId + "';";
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand2;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                dt = dTable;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                MyConn2.Close();
            }
            return dt;
        }

        public static DataTable GetEstablishmentInfoData(string P_fundingBodyProjectId)
        {
            string MyConnection2 = "datasource=localhost;username=root;password=root";
            MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
            DataTable dt = new DataTable();
            try
            {
                string Query = "select ESTABLISHMENTDATE,ESTABLISHMENTCOUNTRYCODE,ESTABLISHMENTDESCRIPTION, LANG from scival.identifier where FUNDINGBODY_ID='" + P_fundingBodyProjectId + "';";
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand2;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                dt = dTable;
                // MyConn2.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                MyConn2.Close();
            }
            return dt;
        }

        public static DataTable GetAwardSucessData(string P_fundingBodyProjectId)
        {
            string MyConnection2 = "datasource=localhost;username=root;password=root";
            MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
            DataTable dt = new DataTable();
            try
            {
                string Query = "select LANG,DESCRIPTION,SOURCE from scival.awardsuccessratedesc where FUNDINGBODY_ID='" + P_fundingBodyProjectId + "';";
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand2;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                dt = dTable;
                dt.Columns[2].ColumnName = "URL";
                // MyConn2.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                MyConn2.Close();
            }
            return dt;
        }
        public static DataTable GetRevisionHostory(string P_fundingBodyProjectId)
        {
            string MyConnection2 = "datasource=localhost;username=root;password=root";
            MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
            DataTable dt = new DataTable();
            try
            {
                string Query = "select status from scival.revisionhistory where FUNDINGBODY_ID='" + P_fundingBodyProjectId + "';";
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand2;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                dt = dTable;
                // MyConn2.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                MyConn2.Close();
            }
            return dt;
        }
        public static DataTable GetCreateDateData(string P_fundingBodyProjectId)
        {
            string MyConnection2 = "datasource=localhost;username=root;password=root";
            MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
            DataTable dt = new DataTable();
            try
            {
                string Query = "SELECT it.CREATEDDATE_TEXT, it.VERSION FROM scival.createddate it, scival.revisionhistory ch WHERE it.REVISIONHISTORY_ID = ch.REVISIONHISTORY_ID AND ch.FUNDINGBODY_ID ='" + P_fundingBodyProjectId + "';";
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand2;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                dt = dTable;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                MyConn2.Close();
            }
            return dt;
        }

        public static DataTable GetDescription(string P_fundingBodyProjectId)
        {
            string MyConnection2 = "datasource=localhost;username=root;password=root";
            MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
            DataTable dt = new DataTable();
            try
            {

                string Query = "SELECT it.RELTYPE, it.DESCRIPTION, it.ITEM_ID, it.ABOUT_ID id, ch.FUNDINGBODY_ID, l.URL, l.LINK_TEXT, AWARDSTATISTICS_ID, IT.LANG FROM   scival.item it, scival.about ch, scival.link l WHERE it.about_id = ch.about_id AND l.item_id = it.item_id AND ch.FUNDINGBODY_ID ='" + P_fundingBodyProjectId + "';";

                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand2;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                DataTable items = new DataTable();
                items.Columns.Add("LANG");
                items.Columns.Add("DESCRIPTION");
                items.Columns.Add("URL");
                if (dTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dTable.Rows)
                    {
                        items.Rows.Add();
                        items.Rows[items.Rows.Count - 1][0] = row[8].ToString();
                        items.Rows[items.Rows.Count - 1][1] = row[1].ToString();
                        items.Rows[items.Rows.Count - 1][2] = row[5].ToString();
                    }
                }
                dt = dTable;
                // MyConn2.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                MyConn2.Close();
            }
            return dt;
        }

        public static DataTable GetAddress(string P_fundingBodyProjectId)
        {
            string MyConnection2 = "datasource=localhost;username=root;password=root";
            MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
            DataTable dt = new DataTable();
            try
            {
                string Query = "SELECT it.COUNTRYTEST, it.ROOM, it.STREET, it.CITY id, it.STATE, it.POSTALCODE FROM scival.address it, scival.contacts ch WHERE it.CONTACT_ID = ch.CONTACTS_ID AND ch.FUNDINGBODY_ID ='" + P_fundingBodyProjectId + "';";
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand2;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                dt = dTable;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                MyConn2.Close();
            }
            return dt;
        }
    }
}
