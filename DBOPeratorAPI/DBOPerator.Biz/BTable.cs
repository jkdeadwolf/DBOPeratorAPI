using DBOPerator.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Biz
{
    public class BTable
    {
        /// <summary>
        /// 表设计分页
        /// </summary>
        /// <returns>结果</returns>
        public PagerParamOut<TableModel> PagerTableModel(PagerParamIn<TablePagerCondition> paramIn)
        {
            return new PagerParamOut<TableModel>();
        }

        /// <summary>
        /// 新增模板表
        /// </summary>
        /// <param name="paramIn">入参</param>
        /// <returns>结果</returns>
        public Result AddTable(TableModel paramIn)
        {
            return new Result();
        }

        /// <summary>
        /// 更新表信息
        /// </summary>
        /// <param name="paramIn">入参</param>
        /// <returns>结果</returns>
        public Result UpdateTableByKeyId(TableModel paramIn)
        {
            return new Result();
        }

        /// <summary>
        /// 根据主键获取模板表信息
        /// </summary>
        /// <param name="keyid">主键</param>
        /// <returns>结果</returns>
        public Result<TableModel> GetTableByKeyId(string keyid)
        {
            return new Result<TableModel>();
        }

        /// <summary>
        /// 根据主键启用模板表信息
        /// </summary>
        /// <param name="keyid">主键</param>
        /// <returns>结果</returns>
        public Result EnableTableByKeyId(string keyid)
        {
            return new Result();
        }

        /// <summary>
        /// 根据主键禁用模板表信息
        /// </summary>
        /// <param name="keyid">主键</param>
        /// <returns>结果</returns>
        public Result DisableTableByKeyId(string keyid)
        {
            return new Result();
        }

        /// <summary>
        /// 根据主键删除模板表信息
        /// </summary>
        /// <param name="keyid">主键</param>
        /// <returns>结果</returns>
        public Result<TableModel> DelTableByKeyId(string keyid)
        {
            return new Result<TableModel>();
        }

        /// <summary>
        /// 获取建表模板
        /// </summary>
        /// <param name="keyid">表主键</param>
        /// <returns>结果</returns>
        public Result GetTemplateSqlCreate(string keyid)
        {
            return new Result();
        }

        /// <summary>
        /// 执行表的创建
        /// </summary>
        /// <param name="tableKeyID">表主键</param>
        /// <param name="sql">待执行的sql语句</param>
        /// <returns>执行结果</returns>
        public Result ExecuteCreateTable(string tableKeyID, string sql)
        {
            ////当sql为空，获取表模板的sql语句执行建表，不为空执行入参中的sql建表
            return new Result();
        }
    }
}
