using DBOPerator.Model;
using System;
using System.Collections.Generic;
using System.Text;

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

        public Result ExecuteConCreateTables()
        {
            throw new NotImplementedException();
        }

        public Result ExecuteTableAnalysisTables()
        {
            throw new NotImplementedException();
        }

        public Result ExecuteTableCreateTables()
        {
            throw new NotImplementedException();
        }
    }
}
