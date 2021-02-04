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
            return new Result();
        }

        /// <summary>
        /// 修改链接
        /// </summary>
        /// <param name="keyID">链接对应逐渐</param>
        /// <returns>添加结果</returns>
        public Result UpdateConString(string keyID, string connectionString)
        {
            return new Result();
        }

        /// <summary>
        /// 删除链接
        /// </summary>
        /// <param name="keyID">链接对应主键</param>
        /// <returns>删除结果</returns>
        public Result DelConString(string keyID)
        {
            return new Result();
        }

        /// <summary>
        /// 启用链接
        /// </summary>
        /// <param name="keyID">链接对应主键</param>
        /// <returns>启用结果</returns>
        public Result EnableConString(string keyID)
        {
            return new Result();
        }

        /// <summary>
        /// 禁用链接
        /// </summary>
        /// <param name="keyID">链接对应主键</param>
        /// <returns>删除结果</returns>
        public Result DisableConString(string keyID)
        {
            return new Result();
        }

        /// <summary>
        /// 数据库链接分页
        /// </summary>
        /// <param name="paramIn">入参</param>
        /// <returns>结果</returns>
        public PagerParamOut<ConStringModel> PagerConString(PagerParamIn<object> paramIn)
        {
            return new PagerParamOut<ConStringModel>();
        }

        /// <summary>
        /// 根据数据库链接执行连接下所有库进行创建表操作
        /// </summary>
        /// <param name="keyID">数据库链接主键</param>
        /// <param name="sql">sql语句</param>
        /// <returns>执行结果</returns>
        public Result CreateTableByConKeyID(string keyID, string sql)
        {
            return new Result();
        }

        /// <summary>
        /// 手动识别数据库建表差异
        /// </summary>
        /// <param name="keyID">数据库主键</param>
        /// <returns>结果</returns>
        public Result ReadDBTableChanges(string keyID)
        {
            return new Result() { };
        }
    }
}
