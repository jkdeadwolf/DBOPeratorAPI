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
        private DBTaskModel TaskInfo { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="task">任务</param>
        public TaskDeal(DBTaskModel task)
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
        private List<TableModel> BuildTableModel(Dictionary<string, List<Table>> tables, ConStringModel conInfo)
        {
            foreach (var item in tables.Keys)
            {
                foreach (var table in tables[item])
                {
                    TableModel model = new TableModel();
                    model.AddTime = DateTime.Now;
                    model.SplitType =,
                model.CheckStatus =
                    model.ConStringKeyID = conInfo.ConnectionString;
                    model.DatabaseName = conInfo.ConStringName;
                    model.CreateStatus =
                        model.HashCount =
                        model.IsDelete = false;
                    model.IsEnable = 1;
                    model.MaxTableName =
                        model.MinTableName =
                        model.ModifyStatus = 1;
                    model.ModifyTime = DateTime.Now;

                    model.TableDesc = table.Table_Comment;
                }
            }
        }

        /// <summary>
        /// 获取拆分规则
        /// </summary>
        /// <param name="tables">表信息</param>
        /// <returns>结果</returns>
        private SplitType GetSplitType(string currentTable, List<Table> tables, ref string minTableProfix, ref string maxTableProfix)
        {
            var regMatch = Regex.Match(currentTable, "[0-9][0-9]*");
            if (regMatch.Success == false)
            {
                return SplitType.无;
            }

            DateTime time = DateTime.MinValue;
            ////year>2017区分按小时分表的逻辑 yyMMddHH的场景
            if (DateTime.TryParseExact(regMatch.Value, "yyyyMM", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out time) && time.Year > 2017)
            {
                return SplitType.按月;
            }

            if (DateTime.TryParseExact(regMatch.Value, "yyMM", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out time))
            {
                return SplitType.按月;
            }

            ////去掉数字，看看他的原型是什么
            var oritable = Regex.Replace(currentTable, "[0-9][0-9]*", string.Empty);
            var sameTables = tables.FindAll(p => p.Table_Name.StartsWith(oritable));
            var list = this.GetAllValue(sameTables);
            list.Sort();
            minTableProfix = list[0].ToString();
            maxTableProfix = list[list.Count - 1].ToString();

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
