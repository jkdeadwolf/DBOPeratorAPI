using DBOPerator.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Biz
{
    /// <summary>
    /// 接口定义
    /// </summary>
    public interface ITaskDeal
    {
        /// <summary>
        /// 执行整库建表
        /// </summary>
        /// <returns>结果</returns>
        Result ExecuteConCreateTables();

        /// <summary>
        /// 执行整库表分析
        /// </summary>
        /// <returns>结果</returns>
        Result ExecuteConAnalysisTables();

        /// <summary>
        /// 根据表创建表信息
        /// </summary>
        /// <returns>执行结果</returns>
        Result ExecuteTableCreateTables();

        /// <summary>
        /// 根据表分析表是否一致
        /// </summary>
        /// <returns>结果</returns>
        Result ExecuteTableAnalysisTables();
    }
}
