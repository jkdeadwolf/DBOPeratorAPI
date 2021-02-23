using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Model
{
    public class Table
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string Table_Name { get; set; }

        /// <summary>
        /// 表备注
        /// </summary>
        public string Table_Comment { get; set; }

        /// <summary>
        /// 表类型
        /// </summary>
        public string Table_Type { get; set; }
    }
}
