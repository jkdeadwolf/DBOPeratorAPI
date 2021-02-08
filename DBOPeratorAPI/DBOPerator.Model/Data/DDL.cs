using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Model
{
    public class DDL
    {
        /// <summary>
        /// 表名字
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// ddl
        /// </summary>
        [SugarColumn(ColumnName = "Create Table")]
        public string CreateTable { get; set; }
    }
}
