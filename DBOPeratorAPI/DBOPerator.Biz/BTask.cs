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
                where.Where(p => p.BusinessType == paramIn.Data.BusinessType);
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
    }
}
