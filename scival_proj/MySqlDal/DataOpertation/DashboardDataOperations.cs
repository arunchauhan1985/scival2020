using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;

namespace MySqlDal
{
    public static class DashboardDataOperations
    {
        private static ScivalEntities ScivalEntities { get { return ScivalEntitiesInstance.GetInstance(); } }

        public static List<sci_language_master> GetLanguageMasters(Int64 langauageLength)
        {
            return ScivalEntities.sci_language_master.Where(l => l.CODE_LENGTH == langauageLength).OrderBy(l => l.LANGUAGE_NAME).ToList();
        }

        public static DataSet StartWork(Int64 WorkFlowId, Int64 UserId)
        {
            DataTable DT = new DataTable();
            DataRow DR = DT.NewRow();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WorkFlowId),
                new MySqlParameter("p_userid", UserId),
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("sci_inserttimesheet41", parameters);

            if (dsTaskList != null && dsTaskList.Tables.Count > 0)
            {
                if (dsTaskList.Tables.Count == 7)
                {
                    dsTaskList.Tables[1].TableName = "FundingBodyTable";
                    dsTaskList.Tables[2].TableName = "Country";
                    dsTaskList.Tables[3].TableName = "State";
                    dsTaskList.Tables[4].TableName = "FundingBodyTypes";
                    dsTaskList.Tables[5].TableName = "FundingBodySubTypes";
                    dsTaskList.Tables[6].TableName = "Keywords";
                }
                if (dsTaskList.Tables.Count == 8)
                {
                    dsTaskList.Tables[1].TableName = "Country";
                    dsTaskList.Tables[2].TableName = "State";
                    dsTaskList.Tables[3].TableName = "FundingBodyTypes";
                    dsTaskList.Tables[4].TableName = "FundingBodySubTypes";
                    dsTaskList.Tables[5].TableName = "LOIDate";
                    dsTaskList.Tables[6].TableName = "DueDate";
                    dsTaskList.Tables[7].TableName = "Keywords";
                }

                if (dsTaskList.Tables.Count == 6)
                {
                    dsTaskList.Tables[1].TableName = "Country";
                    dsTaskList.Tables[2].TableName = "State";
                    dsTaskList.Tables[3].TableName = "FundingBodyTypes";
                    dsTaskList.Tables[4].TableName = "FundingBodySubTypes";
                    dsTaskList.Tables[5].TableName = "Keywords";
                }
                DT = new DataTable();
                DT.Columns.Add("URL");
                DT.Columns.Add("NAME");
                DT.Columns.Add("TID");
                DR = DT.NewRow();
                DR[0] = dsTaskList.Tables[0].Rows[0]["url"].ToString(); //Cmd.Parameters["p_murl"].Value.ToString();
                DR[1] = dsTaskList.Tables[0].Rows[0]["fundingbodyname"].ToString();//Cmd.Parameters["p_fname"].Value.ToString();
                //DR[2] = dsTaskList.Tables[0].Columns["p_TRANSITIONALID"].ToString();
                DT.Rows.Add(DR);
                DT.TableName = "URL";
                dsTaskList.Tables.Add(DT);
            }
            
            return dsTaskList;
        }

        public static DataSet TimeSheetStopContinueForQC(Int64 WFId, Int64 userid, Int64 transId, Int64 mode, string remarks)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_timesheetstopcontinue_QA(@p_workflowid,@p_userid,@p_transitionalid,@p_remarks)",
            //       new MySqlParameter("p_workflowid", WFId), new MySqlParameter("p_userid", userid),
            //       new MySqlParameter("p_transitionalid", transId), new MySqlParameter("p_remarks", remarks)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                   new MySqlParameter("p_workflowid", WFId), 
                   new MySqlParameter("p_userid", userid),
                   new MySqlParameter("p_transitionalid", transId),
                   new MySqlParameter("p_remarks", remarks)
            };
            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("sci_timesheetstopcontinue_QA", parameters);
            dsTaskList.Tables[0].TableName = "Result";
            return dsTaskList;
        }

        public static DataSet TimeSheetStopContinue(Int64 WFId, Int64 userid, Int64 transId, Int64 mode, string remarks)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_timesheetstoP_CONTINUE(@p_workflowid,@p_userid,@p_transitionalid,@p_remarks)",
            //       new MySqlParameter("p_workflowid", WFId), new MySqlParameter("p_userid", userid),
            //       new MySqlParameter("p_transitionalid", transId), new MySqlParameter("p_remarks", remarks)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                  new MySqlParameter("p_workflowid", WFId), 
                  new MySqlParameter("p_userid", userid),
                  new MySqlParameter("p_transitionalid", transId), 
                  new MySqlParameter("p_remarks", remarks)
            };

            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("sci_timesheetstoP_CONTINUE", parameters);

            dsTaskList.Tables[0].TableName = "Result";

            return dsTaskList;
        }

        public static DataSet TimeSheetStop(Int64 WFId, Int64 userid, Int64 transId, Int64 mode, string remarks)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_timesheetstop(@p_workflowid,@p_userid,@p_transitionalid,@p_mode,@p_remarks)",
            //       new MySqlParameter("p_workflowid", WFId), new MySqlParameter("p_userid", userid), new MySqlParameter("p_transitionalid", transId),
            //       new MySqlParameter("p_mode", mode), new MySqlParameter("p_remarks", remarks)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                   new MySqlParameter("p_workflowid", WFId),
                   new MySqlParameter("p_userid", userid), 
                   new MySqlParameter("p_transitionalid", transId),
                   new MySqlParameter("p_mode", mode), 
                   new MySqlParameter("p_remarks", remarks)
            };

            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("sci_timesheetstop", parameters);

            dsTaskList.Tables[0].TableName = "Result";

            return dsTaskList;
        }

        public static DataSet getLanguageDetails(Int64 langLength)
        {
            //var estFundings = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_GetLanguageDetails(@p_langLength)",
            //new MySqlParameter("p_langLength", langLength)).FirstOrDefault();

            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_langLength", langLength)
            };

            var ds = CommonDataOperation.ExecuteStoredProcedure("sci_GetLanguageDetails", parameters);

            ds.Tables[0].TableName = "LanguageTable";

            return ds;
        }
    }
}
