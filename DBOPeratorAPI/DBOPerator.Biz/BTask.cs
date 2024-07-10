using DBOPerator.Biz.Task;
using DBOPerator.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Biz
{
    public class BTask
    {
        /// <summary>
        /// 添加db任务
        /// </summary>
        /// <param name="paramIn">入参</param>
        /// <returns>结果</returns>
        public Result AddDBTask(DBTask paramIn)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            //paramIn.KeyID = KeyIDHelper.Generator();
            var rt = con.Insertable<DBTask>(paramIn).ExecuteCommand();
            return new Result() { Success = rt > 0 };
        }

        /// <summary>
        /// 批量添加任务
        /// </summary>
        /// <param name="paramIn">入参</param>
        /// <returns>结果</returns>
        public Result AddTasks(List<DBTask> lists)
        {
            // lists.ForEach(p => p.KeyID = KeyIDHelper.Generator());
            var con = ConnectionHelper.GetSqlSugarClient();
            var rt = con.Saveable<DBTask>(lists).UpdateIgnoreColumns(p => new { p.NextExecuteTime }).ExecuteCommand();
            return new Result() { Success = rt > 0 };
        }

        /// <summary>
        /// 删除db任务
        /// </summary>
        /// <param name="keyID">主键</param>
        /// <returns>结果</returns>
        public Result DelDBTask(string keyID)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            var rt = con.Updateable<DBTask>(new DBTask() { IsDelete = true }).Where(p => p.KeyID == keyID).ExecuteCommand();
            return new Result() { Success = rt > 0 };
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="keyID">任务id</param>
        /// <returns>结果</returns>
        public DBTask GetTaskByID(string keyID)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            return con.Queryable<DBTask>().Where(p => p.KeyID == keyID).First();
        }

        /// <summary>
        /// 完结db任务
        /// </summary>
        /// <param name="keyID">主键</param>
        /// <param name="message">完结原因</param>
        /// <returns>结果</returns>
        public Result CompleteDBTask(string keyID, string message)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            var rt = con.Updateable<DBTask>(new DBTask() { ExecuteStatus = 3, ExecuteMsg = message }).Where(p => p.KeyID == keyID).ExecuteCommand();
            return new Result() { Success = rt > 0 };
        }

        /// <summary>
        /// 数据库任务分页
        /// </summary>
        /// <param name="paramIn">入参</param>
        /// <returns>结果</returns>
        public PagerParamOut<DBTask> PagerDBTask(PagerParamIn<DBTaskCondition> paramIn)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            var where = con.Queryable<DBTask>();
            if (string.IsNullOrWhiteSpace(paramIn?.Data?.BusinessContent) == false)
            {
                where.Where(p => p.BusinessContent.Contains(paramIn.Data.BusinessContent));
            }

            if (string.IsNullOrWhiteSpace(paramIn?.Data?.ExecuteMsg) == false)
            {
                where.Where(p => p.ExecuteMsg.Contains(paramIn.Data.ExecuteMsg));
            }

            if (paramIn.Data != null && paramIn.Data.BusinessType != 0)
            {
                where.Where(p => p.BusinessType == (BusinessType)paramIn.Data.BusinessType);
            }

            if (paramIn.Data != null && paramIn.Data.ExecuteStatus != 0)
            {
                where.Where(p => p.ExecuteStatus == paramIn.Data.ExecuteStatus);
            }

            if (string.IsNullOrWhiteSpace(paramIn.Data?.Remark))
            {
                where.Where(p => p.Remark.Contains(paramIn.Data.Remark));
            }

            int totalCount = 0;
            var data = where.ToPageList(paramIn.PageNo, paramIn.PageSize, ref totalCount);

            return new PagerParamOut<DBTask>() { Success = true, Rows = data, PageSize = paramIn.PageSize, PageNo = paramIn.PageNo, Total = totalCount };
        }

        /// <summary>
        /// 更新任务
        /// </summary>
        /// <param name="model">入参</param>
        /// <returns>结果</returns>
        public bool UpdateTask(DBTask model, Result result)
        {
            try
            {
                var con = ConnectionHelper.GetSqlSugarClient();
                return con.Updateable<DBTask>(model).ExecuteCommand() > 0;
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(e);
                return false;
            }
        }

        /// <summary>
        /// 开始任务
        /// </summary>
        /// <param name="taskID">任务编号</param>
        /// <returns>结果</returns>
        public bool StartTask(string taskID)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            return con.Updateable<DBTask>().SetColumns(p => new DBTask() { ExecuteStatus = 2, ExecuteMsg = string.Empty }).Where(p => p.KeyID == taskID && p.NextExecuteTime < DateTime.Now).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 增加任务执行次数
        /// </summary>
        /// <param name="taskID">任务编号</param>
        /// <returns>结果</returns>
        public bool AddTaskRunTimes(string taskID, Result result)
        {
            if (result.Success)
            {
                return true;
            }

            try
            {
                var con = ConnectionHelper.GetSqlSugarClient();
                return con.Updateable<DBTask>().SetColumns(p => p.ExecuteTimes == p.ExecuteTimes + 1).Where(p => p.KeyID == taskID).ExecuteCommand() > 0;
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error($"增加任务执行次出错：{e}");
                return false;
            }
        }

        /// <summary>
        /// 执行task
        /// </summary>
        /// <param name="taskID">任务编号</param>
        public Result ExecuteTask(string taskID)
        {
            Result result = null;
            DBTask taskInfo = null;
            try
            {
                taskInfo = this.GetTaskByID(taskID);
                if (string.IsNullOrWhiteSpace(taskInfo?.KeyID))
                {
                    throw new Exception($"任务编号：{taskID}获取任务失败");
                }

                if (this.StartTask(taskID) == false)
                {
                    throw new Exception($"任务编号：{taskID} 开始任务失败");
                }

                ITaskDeal deal = new TaskDeal(taskInfo);

                switch (taskInfo.BusinessType)
                {
                    case BusinessType.初始化: result = deal.InitDatabaseInfo(); break;
                    case BusinessType.整库建表: result = deal.ExecuteConCreateTables(); break;
                    case BusinessType.整库表分析: result = deal.ExecuteConAnalysisTables(); break;
                    case BusinessType.表建表: result = deal.ExecuteTableCreateTables(); break;
                    case BusinessType.表分析: result = deal.ExecuteTableAnalysisTables(); break;
                    default: result = new Result() { Msg = "未定义的处理方式" }; break;
                }
            }
            catch (Exception e)
            {
                result = new Result() { Msg = e.Message };
                NLog.LogManager.GetCurrentClassLogger().Error($"执行任务出错：{e}");
            }
            finally
            {
                this.BuildTaskResult(taskInfo, result);
                this.UpdateTask(taskInfo, result);
                ////每次执行都要增加任务执行次数
                this.AddTaskRunTimes(taskID, result);
            }

            return result;
        }

        /// <summary>
        /// 构建任务执行结果
        /// </summary>
        /// <param name="model">任务model</param>
        /// <param name="result">结果</param>
        private void BuildTaskResult(DBTask model, Result result)
        {
            try
            {
                model.ExecuteStatus = result.Success ? 3 : 1;
                model.ExecuteMsg = result.Success ? string.Empty : result.Msg;
                model.ModifyTime = DateTime.Now;
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(e);
            }
        }

        /// <summary>
        /// 获取最大执行次数
        /// </summary>
        /// <param name="maxRunTimes">最大执行次数</param>
        public Result<List<DBTask>> GetTasks(int maxRunTimes)
        {
            Result<List<DBTask>> result = new Result<List<DBTask>>();
            try
            {
                var con = ConnectionHelper.GetSqlSugarClient();
                var list = con.Queryable<DBTask>().Where(p => p.ExecuteTimes < maxRunTimes && p.NextExecuteTime < DateTime.Now).ToList();
                result.Data = list;
                result.Success = true;
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error($"获取任务信息异常：{e}");
                result.Msg = e.Message;
            }

            return result;
        }
    }
}
