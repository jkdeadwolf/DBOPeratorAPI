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
        /// 执行结果 1待执行 2执行成功 3执行失败 
        /// </summary>
        public int ExecuteStatus { get; set; }

        /// <summary>
        /// 执行结果为执行失败,此处不为空
        /// </summary>
        public int ExecuteMsg { get; set; }

        /// <summary>
        /// 是否删除 true 已删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 添加事件
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }
    }
}
