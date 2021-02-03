using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Model
{
    /// <summary>
    /// 分页入参
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagerParamIn<T>
    {
        /// <summary>
        /// 当前页面
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// 每页数据量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 分页条件
        /// </summary>
        public T Data { get; set; }
    }
}
