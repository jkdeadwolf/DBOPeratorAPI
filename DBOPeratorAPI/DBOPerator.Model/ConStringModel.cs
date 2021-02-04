using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBOPerator.Model
{
    /// <summary>
    /// 数据库链接model
    /// </summary>
    public class ConStringModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string KeyID { get; set; }

        /// <summary>
        /// 链接名字
        /// </summary>
        public string ConStringName { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否启用 true 启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否删除 true 删除  false未删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
