using DBOPerator.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Biz
{
    /// <summary>
    /// 数据库连接操作类
    /// </summary>
    public class BConString
    {
        /// <summary>
        /// 添加链接
        /// </summary>
        /// <param name="connectionString">connectionstring</param>
        /// <returns>添加结果</returns>
        public Result AddConString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return new Result() { Msg = "入参为空" };
            }

            var con = ConnectionHelper.GetSqlSugarClient();
            var data = new ConString()
            {
                KeyID = KeyIDHelper.Generator(),
                ConStringName = string.Empty,
                ConnectionString = connectionString,
                Remark = string.Empty,
                IsEnable = true,
            };

            int runRt = con.Insertable<ConString>(data).ExecuteCommand();
            if (runRt > 0)
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    try
                    {
                        new BTask().AddDBTask(new DBTask()
                        {
                            BusinessKeyID = data.KeyID,
                            BusinessType = BusinessType.初始化,
                            NextExecuteTime = DateTime.Now,
                        });
                    }
                    catch (Exception e)
                    {
                        NLog.LogManager.GetCurrentClassLogger().Error(e);
                    }
                });
            }

            return new Result() { Success = runRt > 0, Msg = runRt > 0 ? string.Empty : "写入失败" };
        }


        /// <summary>
        /// 删除链接
        /// </summary>
        /// <param name="keyID">链接对应主键</param>
        /// <returns>删除结果</returns>
        public ConString GetConString(string keyID)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            return con.Queryable<ConString>().Where(p => p.KeyID == keyID).First();
        }

        /// <summary>
        /// 修改链接
        /// </summary>
        /// <param name="keyID">链接对应逐渐</param>
        /// <returns>添加结果</returns>
        public Result UpdateConString(string keyID, ConString paramIn)
        {
            if (string.IsNullOrWhiteSpace(keyID))
            {
                return new Result() { Msg = "主键不能为空" };
            }

            var con = ConnectionHelper.GetSqlSugarClient();
            var count = con.Updateable<ConString>(new ConString() { ConStringName = paramIn.ConStringName, Remark = paramIn.Remark }).Where(p => p.KeyID == keyID).ExecuteCommand();
            return new Result() { Success = count > 0 };
        }

        /// <summary>
        /// 删除链接
        /// </summary>
        /// <param name="keyID">链接对应主键</param>
        /// <returns>删除结果</returns>
        public Result DelConString(string keyID)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            var count = con.Updateable<ConString>(new ConString() { IsDelete = true }).Where(p => p.KeyID == keyID).ExecuteCommand();
            return new Result() { Success = count > 0 };
        }

        /// <summary>
        /// 启用链接
        /// </summary>
        /// <param name="keyID">链接对应主键</param>
        /// <returns>启用结果</returns>
        public Result EnableConString(string keyID)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            var count = con.Updateable<ConString>(new ConString() { IsEnable = true }).ExecuteCommand();
            return new Result() { Success = count > 0 };
        }

        /// <summary>
        /// 禁用链接
        /// </summary>
        /// <param name="keyID">链接对应主键</param>
        /// <returns>删除结果</returns>
        public Result DisableConString(string keyID)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            var count = con.Updateable<ConString>(new ConString() { IsEnable = false }).ExecuteCommand();
            return new Result() { Success = count > 0 };
        }

        /// <summary>
        /// 数据库链接分页
        /// </summary>
        /// <param name="paramIn">入参</param>
        /// <returns>结果</returns>
        public PagerParamOut<ConString> PagerConString(PagerParamIn<ConStringCondition> paramIn)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            int totalCount = 0;
            var where = con.Queryable<ConString>();
            if (string.IsNullOrWhiteSpace(paramIn.Data.ConName) == false)
            {
                where.Where(p => p.ConStringName.Contains(paramIn.Data.ConName));
            }

            var list = where.ToPageList(paramIn.PageNo, paramIn.PageSize, ref totalCount);
            return new PagerParamOut<ConString>() { Success = true, Rows = list };
        }

        /// <summary>
        /// 根据数据库链接执行连接下所有库进行创建表操作
        /// </summary>
        /// <param name="keyID">数据库链接主键</param>
        /// <param name="sql">sql语句</param>
        /// <returns>执行结果</returns>
        public Result CreateTableByConKeyID(string keyID, string sql)
        {
            try
            {
                var data = this.GetConString(keyID);
                if (string.IsNullOrWhiteSpace(data?.ConnectionString))
                {
                    return new Result() { Msg = "查无此数据库配置" };
                }

                ////整库时间较久，下发任务,做异步执行
                if (string.IsNullOrWhiteSpace(sql) == false)
                {
                    return new BTask().AddDBTask(new DBTask()
                    {
                        BusinessKeyID = keyID,
                        BusinessType = BusinessType.整库建表,
                    });
                }

                var con = ConnectionHelper.GetSqlSugarClientByConString(data.ConnectionString);
                int runRt = con.Ado.ExecuteCommand(sql);
                return new Result() { Success = runRt > 0 };
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(e);
                return new Result() { Msg = $"根据数据库执行sql出错,{e.Message}" };
            }
        }

        /// <summary>
        /// 手动识别数据库建表差异
        /// </summary>
        /// <param name="keyID">数据库主键</param>
        /// <returns>结果</returns>
        public Result ReadDBTableChanges(string keyID)
        {
            try
            {
                var data = this.GetConString(keyID);
                if (string.IsNullOrWhiteSpace(data?.ConnectionString))
                {
                    return new Result() { Msg = "查无此数据库配置" };
                }

                ////整库时间较久，下发任务
                return new BTask().AddDBTask(new DBTask()
                {
                    BusinessKeyID = keyID,
                    BusinessType = BusinessType.整库表分析,
                });
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(e);
                return new Result() { Msg = $"根据数据库识别表出错,{e.Message}" };
            }
        }
    }
}
