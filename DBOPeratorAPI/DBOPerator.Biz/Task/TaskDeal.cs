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
        /// 构造方法
        /// </summary>
        /// <param name="task">任务</param>
        public TaskDeal(DBTask task)
        {
            this.TaskInfo = task;
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
                return res;
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error($"{e}");
                result.Msg = e.Message;
            }

            return result;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// 构建表model
        /// </summary>
        /// <param name="tables">表信息</param>
        /// <param name="conInfo">链接信息</param>
        /// <returns>结果</returns>
        private List<TableInfo> BuildTableModel(Dictionary<string, List<Table>> tables, ConString conInfo)
        {
            List<TableInfo> result = new List<TableInfo>();
            foreach (var item in tables.Keys)
            {
                foreach (var table in tables[item])
                {
                    if (result.Exists(p => table.Table_Name.StartsWith(p.TableName) && string.IsNullOrWhiteSpace(p.TableName) == false) == false)
                    {
                        TableInfo model = new TableInfo();
                        model.AddTime = DateTime.Now;
                        this.InitSplitType(table.Table_Name, tables[item], model);
                        model.CheckStatus = 2;
                        model.ConStringKeyID = conInfo.KeyID;
                        model.DatabaseName = item;
                        model.CreateStatus = 2;
                        model.IsDelete = false;
                        model.IsEnable = 1;
                        model.ModifyStatus = 1;
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
        /// <param name="tables">表信息</param>
        /// <returns>结果</returns>
        private void InitSplitType(string currentTable, List<Table> tables, TableInfo table)
        {
            string tableName = string.Empty;
            string minProfix = string.Empty;
            string maxProfix = string.Empty;
            int hashCount = 0;
            SplitType splitType = SplitType.无;
            try
            {
                var regMatch = Regex.Match(currentTable, "[0-9][0-9]*");
                if (regMatch.Success == false)
                {
                    splitType = SplitType.无;
                    return;
                }

                ////去掉数字，看看他的原型是什么
                tableName = Regex.Replace(currentTable, "[0-9][0-9]*", string.Empty);
                var sameTables = tables.FindAll(p => p.Table_Name.StartsWith(tableName));
                var list = this.GetAllValue(sameTables);
                list.Sort();
                minProfix = list[0].ToString();
                maxProfix = list[list.Count - 1].ToString();

                DateTime time = DateTime.MinValue;
                ////year>2017区分按小时分表的逻辑 yyMMddHH的场景
                if (DateTime.TryParseExact(regMatch.Value, "yyyyMM", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out time) && time.Year > 2017)
                {
                    splitType = SplitType.按月;
                    return;
                }

                if (DateTime.TryParseExact(regMatch.Value, "yyMM", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out time))
                {
                    splitType = SplitType.按月;
                    return;
                }

                if (DateTime.TryParseExact(regMatch.Value, "yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out time))
                {
                    splitType = SplitType.按年;
                    return;
                }

                ////最小表后缀是0
                if (list[0] == 0)
                {
                    hashCount = list.Count;
                    splitType = SplitType.HASH;
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
            tables.ForEach(p => result.Add(Convert.ToInt32(Regex.Match(p.Table_Name, "[0-9][0-9]*").Value)));
            return result;
        }
    }
}
