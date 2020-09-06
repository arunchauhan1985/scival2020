using System;
using System.Collections.Generic;
using System.Web.Http;
using MySqlDal;

namespace ScivalWebApi.Controllers
{
    public class TaskController : ApiController
    {
        [HttpGet]
        [Route("api/Task/GetModuleWiseUserTasksCountByModuleName/{userId}/{userName}/{moduleName}/{reloadTasks}")]
        public int GetModuleWiseUserTasksCountByModuleName(Int64 userId, string userName, string moduleName, bool reloadTasks = false)
        {
            return TaskDataOperation.GetModuleWiseUserTasksCountByModuleName(userId, userName, moduleName, reloadTasks);
        }

        [HttpGet]
        [Route("api/Task/GetModuleWiseUserTasksCountByModuleId/{userId}/{userName}/{moduleId}/{reloadTasks}")]
        public static int GetModuleWiseUserTasksCountByModuleId(Int64 userId, string userName, Int64 moduleId, bool reloadTasks = false)
        {
            return TaskDataOperation.GetModuleWiseUserTasksCountByModuleId(userId, userName, moduleId, reloadTasks);
        }

        [HttpGet]
        [Route("api/Task/GetModuleWiseUserTasksCountByModuleIdAndTaskId/{userId}/{userName}/{moduleId}/{taskId}/{reloadTasks}")]
        public static int GetModuleWiseUserTasksCountByModuleIdAndTaskId(Int64 userId, string userName, Int64 moduleId, Int64 taskId, bool reloadTasks = false)
        {
            return TaskDataOperation.GetModuleWiseUserTasksCountByModuleIdAndTaskId(userId, userName, moduleId, taskId, reloadTasks);
        }

        [HttpGet]
        [Route("api/Task/GetModuleWiseUserTasksCountByTaskName/{userId}/{userName}/{taskName}/{reloadTasks}")]
        public static int GetModuleWiseUserTasksCountByTaskName(Int64 userId, string userName, string taskName, bool reloadTasks = false)
        {
            return TaskDataOperation.GetModuleWiseUserTasksCountByTaskName(userId, userName, taskName, reloadTasks);
        }

        [HttpGet]
        [Route("api/Task/GetModuleWiseUserTasksCount/{userId}/{userName}/{reloadTasks}")]
        public static int GetModuleWiseUserTasksCount(Int64 userId, string userName, bool reloadTasks = false)
        {
            return TaskDataOperation.GetModuleWiseUserTasksCount(userId, userName, reloadTasks);
        }

        [HttpGet]
        [Route("api/Task/GetUserTasks/{userId}/{userName}/{reloadTasks}")]
        public static List<ModuleWiseUserTask> GetUserTasks(Int64 userId, string userName, bool reloadTasks = false)
        {
            return TaskDataOperation.GetUserTasks(userId, userName, reloadTasks);
        }

        [HttpGet]
        [Route("api/Task/GetTaskForUser/{userId}")]
        public static List<UserTaskList> GetTaskForUser(Int64 userId)
        {
            return TaskDataOperation.GetTaskForUser(userId);
        }

        [HttpGet]
        [Route("api/Task/GetUserModuleId/{userId}")]
        public static Int64 GetUserModuleId(Int64 userId)
        {
            return TaskDataOperation.GetUserModuleId(userId);
        }

        [HttpGet]
        [Route("api/Task/GetDummyTaskList/{userId}/{moduleId}/{taskId}/{cycle}/{allocation}")]
        public static List<DummyTaskList> GetDummyTaskList(Int64 userId, Int64 moduleId, Int64 taskId, Int32 cycle, Int64 allocation)
        {
            return TaskDataOperation.GetDummyTaskList(userId, moduleId, taskId, cycle, allocation);
        }

        [HttpGet]
        [Route("api/Task/GetUserExpireDetails/{userId}")]
        public static List<ExpiryDetail> GetUserExpireDetails(Int64 userId)
        {
            return TaskDataOperation.GetUserExpireDetails(userId);
        }

        [HttpGet]
        [Route("api/Task/GetTabSelectionValue/{userId}")]
        public static List<Int64?> GetTabSelectionValue(Int64 userId)
        {
            return TaskDataOperation.GetTabSelectionValue(userId);
        }

        [HttpGet]
        [Route("api/Task/GetNewTaskList/{userId}/{moduleId}/{taskId}/{cycle}/{allocation}")]
        public static List<NewTaskList> GetNewTaskList(Int64 userId, Int64 moduleId, Int64 taskId, Int64 cycle, Int64 allocation)
        {
            return TaskDataOperation.GetNewTaskList(userId, moduleId, taskId, cycle, allocation);
        }

        [HttpGet]
        [Route("api/Task/BackToTaskBoard/{userId}/{Id}/{moduleId}/{taskId}/{cycle}")]
        public static void BackToTaskBoard(Int64 userId, Int64 Id, Int64 moduleId, Int64 taskId, Int64 cycle)
        {
            TaskDataOperation.BackToTaskBoard(userId, Id, moduleId, taskId, cycle);
        }
    }
}
