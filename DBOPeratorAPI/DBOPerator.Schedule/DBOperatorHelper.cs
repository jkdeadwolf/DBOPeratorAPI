using DBOPerator.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DBOPerator.Schedule
{
    public class DBOperatorHelper
    {
        /// <summary>
        /// 扫描任务
        /// </summary>
        /// <param name="maxRunTimes">最大执行次数</param>
        /// <returns>结果</returns>
        public static Result<List<DBTask>> GetTasksAsync(int maxRunTimes)
        {
            try
            {
                return HttpClientHelper.GetWithHeader<Result<List<DBTask>>>($"{ConfigHelper.APIUrl}/DB/GetTasks?maxRunTimes={maxRunTimes}", new Dictionary<string, string>(), "GetTasks");
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(e);
                return new Result<List<DBTask>>() { Msg = e.Message };
            }
        }

        /// <summary>
        /// 扫描任务
        /// </summary>
        /// <param name="maxRunTimes">最大执行次数</param>
        /// <returns>结果</returns>
        public static  void ExecuteTaskAsync(string taskId)
        {
            try
            {
                HttpClientHelper.GetWithHeader<Result<List<DBTask>>>($"{ConfigHelper.APIUrl}/DB/ExecuteTask?taskID={taskId}", new Dictionary<string, string>(), "ExecuteTask");
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(e);
            }
        }
    }
}
