//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Threading.Tasks;
//using MySqlDal;

//namespace ScivalApiCall
//{
//    public static class TaskApiCall
//    {
//        public static async Task<int> GetModuleWiseUserTasksCountByModuleName(Int64 userId, string userName, string moduleName, bool reloadTasks = false)
//        {
//            string requestUri = string.Format("api/Task/GetModuleWiseUserTasksCountByModuleName/{0}/{1}/{2}/{3}", userId, userName, moduleName, reloadTasks);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<int>();
//            else
//                return 0;
//        }

//        public static async Task<int> GetModuleWiseUserTasksCountByModuleId(Int64 userId, string userName, Int64 moduleId, bool reloadTasks = false)
//        {
//            string requestUri = string.Format("api/Task/GetModuleWiseUserTasksCountByModuleId/{0}/{1}/{2}/{3}", userId, userName, moduleId, reloadTasks);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<int>();
//            else
//                return 0;
//        }

//        public static async Task<int> GetModuleWiseUserTasksCountByModuleIdAndTaskId(Int64 userId, string userName, Int64 moduleId, Int64 taskId, bool reloadTasks = false)
//        {
//            string requestUri = string.Format("api/Task/GetModuleWiseUserTasksCountByModuleIdAndTaskId/{0}/{1}/{2}/{3}/{4}", userId, userName, moduleId, taskId, reloadTasks);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<int>();
//            else
//                return 0;
//        }

//        public static async Task<int> GetModuleWiseUserTasksCountByTaskName(Int64 userId, string userName, string taskName, bool reloadTasks = false)
//        {
//            string requestUri = string.Format("api/Task/GetModuleWiseUserTasksCountByTaskName/{0}/{1}/{2}/{3}", userId, userName, taskName, reloadTasks);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<int>();
//            else
//                return 0;
//        }

//        public static async Task<int> GetModuleWiseUserTasksCount(Int64 userId, string userName, bool reloadTasks = false)
//        {
//            string requestUri = string.Format("api/Task/GetModuleWiseUserTasksCount/{0}/{1}/{2}", userId, userName, reloadTasks);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<int>();
//            else
//                return 0;
//        }

//        public static async Task<List<ModuleWiseUserTask>> GetUserTasks(Int64 userId, string userName, bool reloadTasks = false)
//        {
//            string requestUri = string.Format("api/Task/GetUserTasks/{0}/{1}/{2}", userId, userName, reloadTasks);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<ModuleWiseUserTask>>();
//            else
//                return null;
//        }

//        public static async Task<List<UserTaskList>> GetTaskForUser(Int64 userId)
//        {
//            string requestUri = string.Format("api/Task/GetTaskForUser/{0}", userId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<UserTaskList>>();
//            else
//                return null;
//        }

//        public static async Task<Int64> GetUserModuleId(Int64 userId)
//        {
//            string requestUri = string.Format("api/Task/GetUserModuleId/{0}", userId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<Int64>();
//            else
//                return 0;
//        }

//        public static async Task<List<DummyTaskList>> GetDummyTaskList(Int64 userId, Int64 moduleId, Int64 taskId, Int32 cycle, Int64 allocation)
//        {
//            string requestUri = string.Format("api/Task/GetDummyTaskList/{0}/{1}/{2}/{3}/{4}", userId, moduleId, taskId, cycle, allocation);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<DummyTaskList>>();
//            else
//                return null;
//        }

//        public static async Task<List<ExpiryDetail>> GetUserExpireDetails(Int64 userId)
//        {
//            string requestUri = string.Format("api/Task/GetUserExpireDetails/{0}", userId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<ExpiryDetail>>();
//            else
//                return null;
//        }

//        public static async Task<List<Int64?>> GetTabSelectionValue(Int64 userId)
//        {
//            string requestUri = string.Format("api/Task/GetTabSelectionValue/{0}", userId);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<Int64?>>();
//            else
//                return null;
//        }

//        public static async Task<List<NewTaskList>> GetNewTaskList(Int64 userId, Int64 moduleId, Int64 taskId, Int64 cycle, Int64 allocation)
//        {
//            string requestUri = string.Format("api/Task/GetNewTaskList/{0}/{1}/{2}/{3}/{4}", userId, moduleId, taskId, cycle, allocation);
//            var response = HttpClientInstance.GetCall(requestUri);

//            if (response.Result != null)
//                return await response.Result.Content.ReadAsAsync<List<NewTaskList>>();
//            else
//                return null;
//        }

//        public static async void BackToTaskBoard(Int64 userId, Int64 Id, Int64 moduleId, Int64 taskId, Int64 cycle)
//        {
//            string requestUri = string.Format("api/Task/BackToTaskBoard/{0}/{1}/{2}/{3}/{4}", userId, Id, moduleId, taskId, cycle);
//            await HttpClientInstance.GetCall(requestUri);
//        }
//    }
//}
