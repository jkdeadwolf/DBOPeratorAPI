using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Model
{
    /// <summary>
    /// 拆分规则
    /// </summary>
    public enum SplitType
    {
        /// <summary>
        /// 没有分表
        /// </summary>
        无 = 1,

        /// <summary>
        /// 按年分表
        /// </summary>
        按年 = 2,

        /// <summary>
        /// 按月
        /// </summary>
        按月 = 3,

        /// <summary>
        /// 按天
        /// </summary>
        按天 = 4,

        /// <summary>
        /// Hash
        /// </summary>
        HASH = 5,
    }
}
