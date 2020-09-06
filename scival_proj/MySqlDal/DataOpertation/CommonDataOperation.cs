using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Linq;
using MySql.Data.MySqlClient;

namespace MySqlDal
{
    public static class CommonDataOperation
    {
        private static ScivalEntities ScivalEntities { get { return ScivalEntitiesInstance.GetInstance(); } }

        public static string GetDatabaseVersion()
        {
            var version = ScivalEntities.sci_version.FirstOrDefault();

            return version.VERSION_ID;
        }

        public static bool ValidateLogin(string username, string password, out string validationMessage, out sci_usermaster user)
        {
            validationMessage = string.Empty;
            user = null;

            var users = ScivalEntities.sci_usermaster.Where(u => u.USERNAME == username).ToList();

            if (users == null || users.Count == 0)
            {
                validationMessage = "No user found, please enter valid Username.";
                return false;
            }

            if (users.Count > 1)
            {
                validationMessage = "Multiple user found, please contact Administrator.";
                return false;
            }

            user = users[0];

            if (user.ISACTIVE != "Y")
            {
                validationMessage = "User is not active, please contact Administrator.";
                return false;
            }

            if (user.PASSWORD != password)
            {
                validationMessage = "Invalid Password, please enter valid Password.";
                return false;
            }

            return true;
        }

        public static int GetExpireAlertsCount()
        {
            return ScivalEntities.sci_expire_alert.Where(a => a.FLAG.Value == 0).Count();
        }

        public static void UpdateExpireAlert()
        {
            ScivalEntities.Database.ExecuteSqlCommandAsync("sci_proc_updateexpirealert");
        }

        public static DataSet AddAndDeletePageURL(Int64 WFId, string PageName, string pageURL, Int64 UserId, Int64 mode)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_pageurls(@p_workflowid,@p_pagename,@p_url,@p_userid,@p_mode)",
            //    new MySqlParameter("p_workflowid", WFId), new MySqlParameter("p_pagename", PageName), new MySqlParameter("p_url", pageURL), 
            //    new MySqlParameter("p_userid", UserId), new MySqlParameter("p_mode", mode)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFId), 
                new MySqlParameter("p_pagename", PageName), 
                new MySqlParameter("p_url", pageURL),
                new MySqlParameter("p_userid", UserId), 
                new MySqlParameter("p_mode", mode)
            };

            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("sci_pageurls", parameters);

            dsTaskList.Tables[0].TableName = "URL";

            return dsTaskList;
        }

        public static DataSet GetURL(Int64 WFId, string pagename)
        {
            //var dsTaskList = ScivalEntities.Database.SqlQuery<DataSet>("CALL sci_urllinks(@p_workflowid,@p_pagename)",
            //    new MySqlParameter("p_workflowid", WFId), new MySqlParameter("p_pagename", pagename)).FirstOrDefault();
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("p_workflowid", WFId),
                new MySqlParameter("p_pagename", pagename)
            };

            var dsTaskList = CommonDataOperation.ExecuteStoredProcedure("sci_urllinks", parameters);

            dsTaskList.Tables[0].TableName = "URL";

            return dsTaskList;
        }

        public static DataSet ExecuteStoredProcedure(string storedProcedureName, IEnumerable<MySqlParameter> parameters)
        {
            var connectionString = ScivalEntities.Database.Connection.ConnectionString;
            var ds = new DataSet();
            using (var conn = new MySqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var parameter in parameters)
                    {                        
                        cmd.Parameters.Add(parameter);
                    }

                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(ds);
                    }
                }
            }

            return ds;
        }

        public static DataSet ExecuteStoredProcedureWithoutParam(string storedProcedureName)
        {
            var connectionString = ScivalEntities.Database.Connection.ConnectionString;
            var ds = new DataSet();
            using (var conn = new MySqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }
    }
}
