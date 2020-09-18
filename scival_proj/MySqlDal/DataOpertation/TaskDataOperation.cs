using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;

namespace MySqlDal
{
    public static class TaskDataOperation
    {
        private static ScivalEntities ScivalEntities { get { return ScivalEntitiesInstance.GetInstance(); } }

        private static IQueryable<ModuleWiseUserTask> moduleWiseUserTaskList = null;

        private static IQueryable<ModuleWiseUserTask> GetQueriableModuleWiseUserTasks(Int64 userId, string userName, bool reloadTasks)
        {
            if (moduleWiseUserTaskList == null || reloadTasks == true)
            {
                moduleWiseUserTaskList = (from sm in ScivalEntities.sci_modules
                                          join smt in ScivalEntities.sci_moduletaskconfig on sm.MODULEID equals smt.MODULEID
                                          join st in ScivalEntities.sci_tasks on smt.TASKID equals st.TASKID
                                          join sut in ScivalEntities.sci_usertask on st.TASKID equals sut.TASKID
                                          where sut.USERID == userId && sut.MODULEID == smt.MODULEID
                                          orderby smt.MODULEID, st.TASKID
                                          select new ModuleWiseUserTask
                                          {
                                              ModuleId = smt.MODULEID,
                                              ModuleName = sm.MODULENAME,
                                              TaskId = smt.TASKID,
                                              TaskName = st.TASKNAME,
                                              UserId = userId,
                                              Name = userName
                                          })
                                .OrderBy(ut => new { ut.ModuleId, ut.TaskId });
            }

            return moduleWiseUserTaskList;
        }

        public static int GetModuleWiseUserTasksCountByModuleName(Int64 userId, string userName, string moduleName, bool reloadTasks = false)
        {
            return GetQueriableModuleWiseUserTasks(userId, userName, reloadTasks).Where(ut => ut.ModuleName == moduleName).Count();
        }

        public static int GetModuleWiseUserTasksCountByModuleId(Int64 userId, string userName, Int64 moduleId, bool reloadTasks = false)
        {
            return GetQueriableModuleWiseUserTasks(userId, userName, reloadTasks).Where(ut => ut.ModuleId == moduleId).Count();
        }

        public static int GetModuleWiseUserTasksCountByModuleIdAndTaskId(Int64 userId, string userName, Int64 moduleId, Int64 taskId, bool reloadTasks = false)
        {
            return GetQueriableModuleWiseUserTasks(userId, userName, reloadTasks).Where(ut => ut.ModuleId == moduleId && ut.TaskId == taskId).Count();
        }

        public static int GetModuleWiseUserTasksCountByTaskName(Int64 userId, string userName, string taskName, bool reloadTasks = false)
        {
            return GetQueriableModuleWiseUserTasks(userId, userName, reloadTasks).Where(ut => ut.TaskName == taskName).Count();
        }

        public static int GetModuleWiseUserTasksCount(Int64 userId, string userName, bool reloadTasks = false)
        {
            return GetQueriableModuleWiseUserTasks(userId, userName, reloadTasks).Count();
        }

        public static List<ModuleWiseUserTask> GetUserTasks(Int64 userId, string userName, bool reloadTasks = false)
        {
            return GetQueriableModuleWiseUserTasks(userId, userName, reloadTasks).ToList();
        }

        public static List<UserTaskList> GetTaskForUser(Int64 userId)
        {
            var userTaskList = ScivalEntities.Database.SqlQuery<UserTaskList>("CALL sci_proc_gettaskforuser(@USER_ID)", new MySqlParameter("USER_ID", userId)).ToList();

            return userTaskList;
        }

        public static Int64 GetUserModuleId(Int64 userId)
        {
            var moduleId = ScivalEntities.userassignments.Where(u => u.USERID == userId).Select(u => u.MODULEID).Distinct().FirstOrDefault();

            if (moduleId != null)
                return moduleId.Value;
            else
                return 0;
        }

        public static List<DummyTaskList> GetDummyTaskList(Int64 userId, Int64 moduleId, Int64 taskId, Int32 cycle, Int64 allocation)
        {
            var dummyTaskList = ScivalEntities.Database.SqlQuery<DummyTaskList>("CALL dummy_tasklist_new(@pUserId,@pModuleId,@pTaskId,@pCycle,@pAllocation)", 
                new MySqlParameter("pUserId", userId), new MySqlParameter("pModuleId", moduleId), new MySqlParameter("pTaskId", taskId), new MySqlParameter("pCycle", cycle), 
                new MySqlParameter("pAllocation", allocation)).ToList();

            return dummyTaskList;
        }

        public static List<DummyTaskList> GetDummyTaskListJSON(Int64 userId, Int64 moduleId, Int64 taskId, Int32 cycle, Int64 allocation)
        {
            var dummyTaskList = ScivalEntities.Database.SqlQuery<DummyTaskList>("CALL dummy_tasklist_new(@pUserId,@pModuleId,@pTaskId,@pCycle,@pAllocation)",
                new MySqlParameter("pUserId", userId), new MySqlParameter("pModuleId", moduleId), new MySqlParameter("pTaskId", taskId), new MySqlParameter("pCycle", cycle),
                new MySqlParameter("pAllocation", allocation)).ToList();

            return dummyTaskList;
        }


        public static List<ExpiryDetail> GetUserExpireDetails(Int64 userId)
        {
            var expireDetailList = ScivalEntities.Database.SqlQuery<ExpiryDetail>("CALL sci_expiredetail(@pUserId)", new MySqlParameter("pUserId", userId)).ToList();

            return expireDetailList;
        }

        public static List<Int64?> GetTabSelectionValue(Int64 userId)
        {
            var moduleIdList = ScivalEntities.userassignments.Where(ua => ua.USERID == userId)
                .Select(ua => ua.MODULEID).Distinct().ToList();

            return moduleIdList;
        }

        public static List<NewTaskList> GetNewTaskList(Int64 userId, Int64 moduleId, Int64 taskId, Int64 cycle, Int64 allocation)
        {
            var newTaskList = ScivalEntities.Database.SqlQuery<NewTaskList>("CALL sci_proc_tasklist_new(@p_userid,@P_moduleid,@P_taskid,@P_CYCLE,@P_ALLOCATION)",
                new MySqlParameter("p_userid", userId), new MySqlParameter("P_moduleid", moduleId), new MySqlParameter("P_taskid", taskId), new MySqlParameter("P_CYCLE", cycle),
                new MySqlParameter("P_ALLOCATION", allocation)).ToList();

            return newTaskList;
        }

        public static void BackToTaskBoard(Int64 userId, Int64 Id, Int64 moduleId, Int64 taskId, Int64 cycle)
        {
            ScivalEntities.Database.ExecuteSqlCommand("sci_proc_backtask(@P_USERID,@P_ID,@P_moduleid,@P_taskid,@P_CYCLE)",
                    new MySqlParameter("P_USERID", userId),
                    new MySqlParameter("P_ID", Id),
                    new MySqlParameter("P_moduleid", moduleId),
                    new MySqlParameter("P_taskid", taskId),
                    new MySqlParameter("P_CYCLE", cycle));
            //var parameters = new List<MySqlParameter>
            //{
            //        new MySqlParameter("P_USERID", userId),
            //        new MySqlParameter("P_ID", Id),
            //        new MySqlParameter("P_moduleid", moduleId),
            //        new MySqlParameter("P_taskid", taskId),
            //        new MySqlParameter("P_CYCLE", cycle)
            //};

            //var ds = CommonDataOperation.ExecuteStoredProcedure("sci_proc_backtask", parameters);

        }
    }
}
