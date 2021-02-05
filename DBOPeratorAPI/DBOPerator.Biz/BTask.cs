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
        public Result AddDBTask(DBTaskModel paramIn)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            paramIn.KeyID = KeyIDHelper.Generator();
            var rt = con.Insertable<DBTaskModel>(paramIn).ExecuteCommand();
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
            var rt = con.Updateable<DBTaskModel>(new DBTaskModel() { IsDelete = true }).Where(p => p.KeyID == keyID).ExecuteCommand();
            return new Result() { Success = rt > 0 };
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="keyID">任务id</param>
        /// <returns>结果</returns>
        public DBTaskModel GetTaskByID(string keyID)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            return con.Queryable<DBTaskModel>().Where(p => p.KeyID == keyID).First();
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
            var rt = con.Updateable<DBTaskModel>(new DBTaskModel() { ExecuteStatus = 3, ExecuteMsg = message }).Where(p => p.KeyID == keyID).ExecuteCommand();
            return new Result() { Success = rt > 0 };
        }

        /// <summary>
        /// 数据库任务分页
        /// </summary>
        /// <param name="paramIn">入参</param>
        /// <returns>结果</returns>
        public PagerParamOut<DBTaskModel> PagerDBTask(PagerParamIn<DBTaskCondition> paramIn)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            var where = con.Queryable<DBTaskModel>();
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

            return new PagerParamOut<DBTaskModel>() { Success = true, Rows = data, PageSize = paramIn.PageSize, PageNo = paramIn.PageNo, Total = totalCount };
        }

        /// <summary>
        /// 更新任务
        /// </summary>
        /// <param name="model">入参</param>
        /// <returns>结果</returns>
        public bool UpdateTask(DBTaskModel model)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            return con.Updateable<DBTaskModel>(model).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 开始任务
        /// </summary>
        /// <param name="taskID">任务编号</param>
        /// <returns>结果</returns>
        public bool StartTask(string taskID)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            return con.Updateable<DBTaskModel>(new DBTaskModel() { ExecuteStatus = 2 }).Where(p => p.KeyID == taskID && p.ExecuteStatus == 1).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 增加任务执行次数
        /// </summary>
        /// <param name="taskID">任务编号</param>
        /// <returns>结果</returns>
        public bool AddTaskRunTimes(string taskID)
        {
            try
            {
                var con = ConnectionHelper.GetSqlSugarClient();
                return con.Updateable<DBTaskModel>().ReSetValue(p => p.ExecuteTimes == p.ExecuteTimes + 1).Where(p => p.KeyID == taskID && p.ExecuteStatus == 1).ExecuteCommand() > 0;
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
        public void ExecuteTask(string taskID)
        {
            try
            {
                var taskInfo = this.GetTaskByID(taskID);
                if (string.IsNullOrWhiteSpace(taskInfo?.KeyID))
                {
                    throw new Exception($"任务编号：{taskID}获取任务失败");
                }

                if (this.StartTask(taskID) == false)
                {
                    throw new Exception($"任务编号：{taskID} 开始任务失败");
                }

                ITaskDeal deal = new TaskDeal(taskInfo);
                Result result = null;
                switch (taskInfo.BusinessType)
                {
                    case BusinessType.整库建表: result = deal.ExecuteConCreateTables(); break;
                    case BusinessType.整库表分析: result = deal.ExecuteConAnalysisTables(); break;
                    case BusinessType.表建表: result = deal.ExecuteTableCreateTables(); break;
                    case BusinessType.表分析: result = deal.ExecuteTableAnalysisTables(); break;
                    default: result = new Result() { Msg = "未定义的处理方式" }; break;
                }

                this.BuildTaskResult(taskInfo, result);
                this.UpdateTask(taskInfo);
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error($"执行任务出错：{e}");
            }
            finally
            {
                ////每次执行都要增加任务执行次数
                this.AddTaskRunTimes(taskID);
            }
        }

        /// <summary>
        /// 构建任务执行结果
        /// </summary>
        /// <param name="model">任务model</param>
        /// <param name="result">结果</param>
        private void BuildTaskResult(DBTaskModel model, Result result)
        {
            model.ExecuteStatus = result.Success ? 3 : 4;
            model.ExecuteMsg = result.Success ? string.Empty : result.Msg;
            model.ModifyTime = DateTime.Now;
        }

        /// <summary>
        /// 获取最大执行次数
        /// </summary>
        /// <param name="maxRunTimes">最大执行次数</param>
        public Result<List<DBTaskModel>> GetTasks(int maxRunTimes)
        {
            Result<List<DBTaskModel>> result = new Result<List<DBTaskModel>>();
            try
            {
                var con = ConnectionHelper.GetSqlSugarClient();
                var list = con.Queryable<DBTaskModel>().Where(p => p.ExecuteTimes < maxRunTimes).ToList();
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
