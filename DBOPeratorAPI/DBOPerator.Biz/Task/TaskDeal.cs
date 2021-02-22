using DBOPerator.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace DBOPerator.Biz.Task
{
    /// <summary>
    /// 任务处理类
    /// </summary>
    public class TaskDeal : ITaskDeal
    {
        /// <summary>
        /// 任务信息
        /// </summary>
        private DBTask TaskInfo { get; set; }

        /// <summary>
        /// 表名后缀正则表达式
        /// </summary>
        private static string Pattern = "[0-9][0-9]*$";

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="task">任务</param>
        public TaskDeal(DBTask task)
        {
            this.TaskInfo = task;
        }

        /// <summary>
        /// 初始化数据库信息
        /// </summary>
        /// <returns>结果</returns>
        public Result InitDatabaseInfo()
        {
            Result result = new Result();
            try
            {
                var conInfo = new BConString().GetConString(this.TaskInfo.BusinessKeyID);
                if (string.IsNullOrWhiteSpace(conInfo?.KeyID))
                {
                    result.Msg = "获取当前任务信息失败";
                    return result;
                }

                IDBOperator op = new DBOperator();
                ////获取数据库连接
                SqlSugarClient client = null;
                ///根据链接获取此链接下所有数据库
                var dataBases = op.GetAllDataBase(conInfo.ConnectionString, ref client);
                ///根据数据库获取所有表
                if (dataBases == null || dataBases.Count == 0)
                {
                    result.Msg = "当前链接没有数据库";
                    result.Success = true;
                    return result;
                }

                Dictionary<string, List<Table>> dBAndTables = new Dictionary<string, List<Table>>();
                ////这里表多了会不会超时，待处理
                foreach (var item in dataBases)
                {
                    Thread.Sleep(100);
                    dBAndTables.Add(item, op.GetAllTables(item, client));
                }

                ///根据规则判断表的生成规则，并生成tables的表信息
                var tables = this.BuildTableModel(dBAndTables, conInfo);
                var res = new BTable().AddTableBatch(tables);
                ////筛选需要定期建表的
                var cycleTables = tables.FindAll(p => p.SplitType != SplitType.None && p.SplitType != SplitType.HASH);
                List<DBTask> tasks = new List<DBTask>();
                cycleTables.ForEach(p => tasks.Add(new DBTask() { BusinessKeyID = p.TabelKeyID, BusinessType = BusinessType.表建表 }));
                var insertRes = new BTask().AddTasks(tasks);
                return insertRes;
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error($"{e}");
                result.Msg = e.Message;
            }

            return result;
        }

        /// <summary>
        /// 执行表分析
        /// </summary>
        /// <returns>结果</returns>
        public Result ExecuteConAnalysisTables()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据数据库连接建表
        /// </summary>
        /// <returns>创建结果</returns>
        public Result ExecuteConCreateTables()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 执行分析表
        /// </summary>
        /// <returns>结果</returns>
        public Result ExecuteTableAnalysisTables()
        {
            throw new NotImplementedException();
        }

        public Result ExecuteTableCreateTables()
        {
            Result result = new Result();
            try
            {
                var tableInfo = new BTable().GetTableByKeyId(this.TaskInfo.BusinessKeyID);
                if (tableInfo?.Success == false || tableInfo.Data == null || string.IsNullOrWhiteSpace(tableInfo?.Data?.TabelKeyID))
                {
                    result.Msg = "获取表信息失败";
                    return result;
                }

                var conInfo = new BConString().GetConString(tableInfo?.Data.ConStringKeyID);
                if (string.IsNullOrWhiteSpace(conInfo?.KeyID) || string.IsNullOrWhiteSpace(conInfo?.ConnectionString))
                {
                    result.Msg = "获取链接信息失败";
                    return result;
                }

                IDBOperator op = new DBOperator();
                var con = ConnectionHelper.GetSqlSugarClientByConString(conInfo.ConnectionString);
                ////获取当前数据库所有的表
                var tables = op.GetAllTables(tableInfo.Data.DatabaseName, con);
                if (tableInfo.Data.SplitType == SplitType.None)
                {
                    result.Success = true;
                    result.Msg = "无建表规则，无需处理";
                    return result;
                }

                var ddl = op.GetTableDDL(tableInfo.Data.DatabaseName, tableInfo.Data.MaxTableName, con);
                string nexttable = this.GetNextTableName(tableInfo.Data.SplitType, tableInfo.Data.Format);
                string use = $" use {tableInfo.Data.DatabaseName};";
                string createRes = use + ddl.Replace(tableInfo.Data.MaxTableName, $"{tableInfo.Data.TableName}{nexttable}");
                var res = op.ExecuteSql(createRes, con);
                if (res)
                {
                    tableInfo.Data.MaxTableName = $"{ tableInfo.Data.TableName}{ nexttable}";
                    var upRes = new BTable().UpdateTableByKeyId(tableInfo.Data);
                    if (upRes.Success == false)
                    {
                        result.Msg = "更新最大表失败";
                        return result;
                    }
                }

                result.Success = res;
                return result;
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(e);
                result.Msg = e.Message;
                return result;
            }
        }

        /// <summary>
        /// 构建表model
        /// </summary>
        /// <param name="tables">表信息</param>
        /// <param name="conInfo">链接信息</param>
        /// <returns>结果</returns>
        private List<TableInfo> BuildTableModel(Dictionary<string, List<Table>> tables, ConString conInfo)
        {
            List<string> selfDatabases = new List<string>() { "information_schema", "mysql", "performance_schema", "test", "sys" };
            List<TableInfo> result = new List<TableInfo>();
            foreach (var item in tables.Keys)
            {
                if (selfDatabases.Contains(item))
                {
                    continue;
                }

                foreach (var table in tables[item])
                {
                    if (result.Exists(p => ((table.Table_Name.StartsWith(p.TableName) && p.SplitType != SplitType.None) || (table.Table_Name == p.TableName && p.SplitType == SplitType.None)) && string.IsNullOrWhiteSpace(p.TableName) == false && p.DatabaseName == item) == false)
                    {
                        TableInfo model = new TableInfo();
                        model.AddTime = DateTime.Now;
                        this.InitSplitType(table.Table_Name, tables[item], model);
                        model.ConStringKeyID = conInfo.KeyID;
                        model.DatabaseName = item;
                        model.IsDelete = false;
                        model.IsEnable = 1;
                        model.ModifyTime = DateTime.Now;
                        model.TableDesc = table.Table_Comment;
                        result.Add(model);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取拆分规则
        /// </summary>
        /// <param name="currentTable">当前表</param>
        /// <param name="tables">所有表</param>
        /// <param name="table">表信息</param>
        private void InitSplitType(string currentTable, List<Table> tables, TableInfo table)
        {
            string tableName = currentTable;
            string minProfix = string.Empty;
            string maxProfix = string.Empty;
            string format = string.Empty;
            int hashCount = 0;
            SplitType splitType = SplitType.None;
            try
            {
                var regMatch = Regex.Match(currentTable, Pattern);
                if (regMatch.Success == false)
                {
                    splitType = SplitType.None;
                    return;
                }

                ////仅有数字组成的表
                if (regMatch.Value == tableName)
                {
                    splitType = SplitType.None;
                    return;
                }

                ////去掉数字，看看他的原型是什么
                tableName = Regex.Replace(currentTable, Pattern, string.Empty);


                var sameTables = tables.FindAll(p => p.Table_Name.StartsWith(tableName));
                var list = this.GetAllValue(sameTables);
                list.Sort();
                minProfix = list[0].ToString();
                maxProfix = list[list.Count - 1].ToString();

                DateTime time = DateTime.MinValue;
                if (DateTime.TryParseExact(regMatch.Value, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out time) && time.Year > 2017)
                {
                    format = "yyyyMMdd";
                    splitType = SplitType.Day;
                    return;
                }

                ////year>2017区分按小时分表的逻辑 yyMMddHH的场景
                if (DateTime.TryParseExact(regMatch.Value, "yyyyMM", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out time) && time.Year > 2017)
                {
                    format = "yyyyMM";
                    splitType = SplitType.Month;
                    return;
                }

                if (DateTime.TryParseExact(regMatch.Value, "yyMM", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out time))
                {
                    format = "yyMM";
                    splitType = SplitType.Month;
                    return;
                }

                if (DateTime.TryParseExact(regMatch.Value, "yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out time) && time.Year > 2017)
                {
                    format = "yyyy";
                    splitType = SplitType.Year;
                    return;
                }

                ////最小表后缀是0
                if (list[0] == 0)
                {
                    hashCount = list.Count;
                    splitType = SplitType.HASH;
                    format = list.Count.ToString();
                    return;
                }
            }
            finally
            {
                table.HashCount = hashCount;
                table.MaxTableName = $"{tableName}{maxProfix}";
                table.MinTableName = $"{tableName}{minProfix}";
                table.SplitType = splitType;
                table.TableName = $"{tableName}";
                table.Format = format;
            }
        }

        /// <summary>
        /// 获取所有表后缀
        /// </summary>
        /// <param name="tables">同名表</param>
        /// <returns>结果</returns>
        private List<int> GetAllValue(List<Table> tables)
        {
            List<int> result = new List<int>();
            foreach (var item in tables)
            {
                int tableProfix = 0;
                if (int.TryParse(Regex.Match(item.Table_Name, Pattern).Value, out tableProfix))
                {
                    result.Add(tableProfix);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取下个表名
        /// </summary>
        /// <param name="type">分表类型</param>
        /// <param name="format">分表模板</param>
        /// <returns>结果</returns>
        private string GetNextTableName(SplitType type, string format)
        {
            switch (type)
            {
                case SplitType.Day: return DateTime.Now.AddDays(1).ToString(format);
                case SplitType.Month: return DateTime.Now.AddMonths(1).ToString(format);
                case SplitType.Year: return DateTime.Now.AddYears(1).ToString(format);
                default: return string.Empty;
            }
        }
    }
}
