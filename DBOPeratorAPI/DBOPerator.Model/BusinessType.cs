using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Model
{
    /// <summary>
    /// 1根据数据库标号 执行整库建表 2根据数据库主键执行表分析 3 根据表主键执行建表 4根据表主键执行表分析
    /// </summary>
    public enum BusinessType
    {
        /// <summary>
        /// 初始化表以及任务信息
        /// </summary>
        初始化 = 0,

        /// <summary>
        /// 根据数据库id建表
        /// </summary>
        整库建表 = 1,

        /// <summary>
        /// 根据数据库id 进行整库表比对
        /// </summary>
        整库表分析 = 2,

        /// <summary>
        /// 根据表id建表
        /// </summary>
        表建表 = 3,

        /// <summary>
        /// 根据表id进行表对比
        /// </summary>
        表分析 = 4
    }
}
