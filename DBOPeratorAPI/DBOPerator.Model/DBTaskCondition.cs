using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Model
{
    public class DBTaskCondition
    {
        /// <summary>
        /// 执行结果 0全部 1待执行 2执行中 3执行成功 4执行失败 
        /// </summary>
        public int ExecuteStatus { get; set; }

        /// <summary>
        /// 任务备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 任务类型 1根据数据库标号 执行整库建表 2根据数据库主键执行表分析 3 根据表主键执行建表 4根据表主键执行表分析
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// 执行结果msg
        /// </summary>
        public string ExecuteMsg { get; set; }

        /// <summary>
        /// 业务语句，如果此处不为空，就执行此处的语句
        /// </summary>
        public string BusinessContent { get; set; }
    }
}
