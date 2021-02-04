using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Model
{
    /// <summary>
    /// 数据库任务
    /// </summary>
    public class DBTaskModel
    {
        /// <summary>
        /// 任务主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string KeyID { get; set; }

        /// <summary>
        /// 业务主键
        /// </summary>
        public string BusinessKeyID { get; set; }

        /// <summary>
        /// 任务类型 1根据数据库标号 执行整库建表 2根据数据库主键执行表分析 3 根据表主键执行建表 4根据表主键执行表分析
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// 业务语句，如果此处不为空，就执行此处的语句
        /// </summary>
        public string BusinessContent { get; set; }

        /// <summary>
        /// 执行结果 1待执行 2执行中 3执行成功 4执行失败 
        /// </summary>
        public int ExecuteStatus { get; set; } = 1;

        /// <summary>
        /// 执行结果为执行失败,此处不为空
        /// </summary>
        public string ExecuteMsg { get; set; } = string.Empty;

        /// <summary>
        /// 已执行次数，失败次数
        /// </summary>
        public int ExecuteTimes { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 是否删除 true 已删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 添加事件
        /// </summary>
        public DateTime AddTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; } = DateTime.Now;
    }
}
