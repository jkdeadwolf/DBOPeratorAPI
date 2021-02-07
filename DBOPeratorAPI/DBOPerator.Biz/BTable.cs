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
        public PagerParamOut<TableInfo> PagerTableModel(PagerParamIn<TablePagerCondition> paramIn)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            var where = con.Queryable<TableInfo>();
            if (string.IsNullOrWhiteSpace(paramIn?.Data?.DatabaseName) == false)
            {
                where.Where(p => p.DatabaseName == paramIn.Data.DatabaseName);
            }

            if (string.IsNullOrWhiteSpace(paramIn?.Data?.TableName) == false)
            {
                where.Where(p => p.TableDesc.Contains(paramIn.Data.TableName));
            }

            int totalCount = 0;
            var list = where.ToPageList(paramIn.PageNo, paramIn.PageSize, ref totalCount);
            return new PagerParamOut<TableInfo> { Success = true, Rows = list };
        }

        /// <summary>
        /// 新增模板表
        /// </summary>
        /// <param name="paramIn">入参</param>
        /// <returns>结果</returns>
        public Result AddTable(TableInfo paramIn)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            var rt = con.Insertable<TableInfo>(paramIn).ExecuteCommand();
            return new Result { Success = rt > 0 };
        }

        /// <summary>
        /// 批量模板表
        /// </summary>
        /// <param name="paramIn">入参</param>
        /// <returns>结果</returns>
        public Result AddTableBatch(List<TableInfo> list)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            var rt = con.Saveable<TableInfo>(list).ExecuteCommand();
            return new Result { Success = rt > 0 };
        }

        /// <summary>
        /// 更新表信息
        /// </summary>
        /// <param name="paramIn">入参</param>
        /// <returns>结果</returns>
        public Result UpdateTableByKeyId(TableInfo paramIn)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            var rt = con.Updateable<TableInfo>(paramIn).ExecuteCommand();
            return new Result { Success = rt > 0 };
        }

        /// <summary>
        /// 根据主键获取模板表信息
        /// </summary>
        /// <param name="keyid">主键</param>
        /// <returns>结果</returns>
        public Result<TableInfo> GetTableByKeyId(string keyid)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            var data = con.Queryable<TableInfo>().Where(p => p.TabelKeyID == keyid).First();
            return new Result<TableInfo>() { Data = data, Success = true };
        }

        /// <summary>
        /// 根据主键启用模板表信息
        /// </summary>
        /// <param name="keyid">主键</param>
        /// <returns>结果</returns>
        public Result EnableTableByKeyId(string keyid)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            var runRt = con.Updateable<TableInfo>(new TableInfo() { IsEnable = 1 }).Where(p => p.TabelKeyID == keyid).ExecuteCommand();
            return new Result<Table>() { Success = runRt > 0 };
        }

        /// <summary>
        /// 根据主键禁用模板表信息
        /// </summary>
        /// <param name="keyid">主键</param>
        /// <returns>结果</returns>
        public Result DisableTableByKeyId(string keyid)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            var runRt = con.Updateable<TableInfo>(new TableInfo() { IsEnable = 0 }).Where(p => p.TabelKeyID == keyid).ExecuteCommand();
            return new Result<TableInfo>() { Success = runRt > 0 };
        }

        /// <summary>
        /// 根据主键删除模板表信息
        /// </summary>
        /// <param name="keyid">主键</param>
        /// <returns>结果</returns>
        public Result<TableInfo> DelTableByKeyId(string keyid)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            var runRt = con.Updateable<TableInfo>(new TableInfo() { IsDelete = true }).Where(p => p.TabelKeyID == keyid).ExecuteCommand();
            return new Result<TableInfo>() { Success = runRt > 0 };
        }

        /// <summary>
        /// 获取建表模板
        /// </summary>
        /// <param name="keyid">表主键</param>
        /// <returns>结果</returns>
        public Result GetTemplateSqlCreate(string keyid)
        {
            var con = ConnectionHelper.GetSqlSugarClient();
            var tableInfo = con.Queryable<TableInfo>().Where(p => p.TabelKeyID == keyid).First();
            if (string.IsNullOrWhiteSpace(tableInfo?.TabelKeyID))
            {
                return new Result() { Msg = "表信息获取失败" };
            }

            var conInfo = new BConString().GetConString(tableInfo.ConStringKeyID);
            if (string.IsNullOrWhiteSpace(conInfo?.ConnectionString))
            {
                return new Result() { Msg = "获取数据库连接失败" };
            }

            var conCon = ConnectionHelper.GetSqlSugarClientByConString(conInfo.ConnectionString);
            string sql = $"show create table {tableInfo.MaxTableName};";
            var res = conCon.Queryable<dynamic>(sql).First();
            return new Result<Table>() { Success = true, Msg = res.CreateTable };
        }

        /// <summary>
        /// 执行表的创建
        /// </summary>
        /// <param name="tableKeyID">表主键</param>
        /// <param name="sql">待执行的sql语句</param>
        /// <returns>执行结果</returns>
        public Result ExecuteCreateTable(string tableKeyID, string sql)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sql))
                {
                    return new BTask().AddDBTask(new DBTask()
                    {
                        BusinessKeyID = tableKeyID,
                        BusinessType = BusinessType.表建表,
                        BusinessContent = sql
                    });
                }

                ////当sql为空，获取表模板的sql语句执行建表，不为空执行入参中的sql建表
                var tableInfo = this.GetTableByKeyId(tableKeyID)?.Data;
                if (string.IsNullOrWhiteSpace(tableInfo?.ConStringKeyID))
                {
                    return new Result() { Msg = "表不存在" };
                }

                var conInfo = new BConString().GetConString(tableInfo.ConStringKeyID);
                if (string.IsNullOrWhiteSpace(conInfo?.KeyID))
                {
                    return new Result() { Msg = "数据连接不存在" };
                }

                var con = ConnectionHelper.GetSqlSugarClientByConString(conInfo.ConnectionString);
                var rt = con.Ado.ExecuteCommand(sql);
                return new Result() { Success = rt > 0 };
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error($"{e}");
                return new Result() { Msg = e.Message };
            }
        }
    }
}
