using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBOPerator.Model
{
    /// <summary>
    /// 结果
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        public int Code { get; set; }

        public string Msg { get; set; }
    }

    /// <summary>
    /// 带内容的 result
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public class Result<T> : Result
    {
        public T Data { get; set; }
    }
}
