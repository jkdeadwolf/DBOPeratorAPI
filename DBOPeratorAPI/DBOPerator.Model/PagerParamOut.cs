using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Model
{
    /// <summary>
    /// f分页返回参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagerParamOut<T>: PagerParamIn<T>
    {
        /// <summary>
        /// 总数据
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 分页条件
        /// </summary>
        public IList<T> Rows { get; set; }
    }
}
