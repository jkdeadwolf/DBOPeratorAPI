using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBOPerator.Model
{
    /// <summary>
    /// 表信息
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        /// 表编号
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string TabelKeyID { get { return $"{DatabaseName}-{TableName}-{SplitType.ToString()}"; } }

        /// <summary>
        /// 数据库连接编号
        /// </summary>
        public string ConStringKeyID { get; set; }

        /// <summary>
        /// 数据库名字
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// 表描述
        /// </summary>
        public string TableDesc { get; set; }

        /// <summary>
        /// 分表方式  1 无 2 按年 3 按月 4按天 5按HASH
        /// </summary>
        public SplitType SplitType { get; set; }

        /// <summary>
        /// hash最大值，只有hash分表才有值
        /// </summary>
        public int HashCount { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 最小表名
        /// </summary>
        public string MinTableName { get; set; }

        /// <summary>
        /// 最大表名
        /// </summary>
        public string MaxTableName { get; set; }

        /// <summary>
        /// 建表格式
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 模板状态 1启用 2禁用
        /// </summary>
        public int IsEnable { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
