using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Model
{
    /// <summary>
    /// 表分页请求参数
    /// </summary>
    public class TablePagerCondition
    {
        /// <summary>
        /// 数据库连接名字
        /// </summary>
        public string ConStringName { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
    }
}
