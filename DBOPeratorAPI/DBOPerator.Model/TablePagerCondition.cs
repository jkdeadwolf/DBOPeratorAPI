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
        /// 数据库名字
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
    }
}
