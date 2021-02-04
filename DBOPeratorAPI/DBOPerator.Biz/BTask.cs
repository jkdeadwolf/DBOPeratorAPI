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
            return new Result();
        }

        /// <summary>
        /// 删除db任务
        /// </summary>
        /// <param name="keyID">主键</param>
        /// <returns>结果</returns>
        public Result DelDBTask(string keyID)
        {
            return new Result();
        }

        /// <summary>
        /// 完结db任务
        /// </summary>
        /// <param name="keyID">主键</param>
        /// <param name="message">完结原因</param>
        /// <returns>结果</returns>
        public Result CompleteDBTask(string keyID, string message)
        {
            return new Result();
        }

        /// <summary>
        /// 数据库任务分页
        /// </summary>
        /// <param name="paramIn">入参</param>
        /// <returns>结果</returns>
        public PagerParamOut<DBTaskModel> PagerDBTask(PagerParamIn<DBTaskCondition> paramIn)
        {
            return new PagerParamOut<DBTaskModel>();
        }
    }
}
