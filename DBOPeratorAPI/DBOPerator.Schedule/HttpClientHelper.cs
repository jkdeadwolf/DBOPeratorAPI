using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DBOPerator.Schedule
{
    public class HttpClientHelper
    {
        /// <summary>
        /// 带header的get请求
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="url">请求url</param>
        /// <param name="headers">header</param>
        /// <param name="methodName">方法名字，记录日志</param>
        /// <returns>结果</returns>
        public static T GetWithHeader<T>(string url, Dictionary<string, string> headers, string methodName = "Get")
        {
            DateTime startTime = DateTime.Now;
            string result = string.Empty;
            try
            {
                HeaderDeal(headers);

                ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;
                HttpClient client = new HttpClient();
                client.Timeout = new TimeSpan(0, 5, 0);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (headers != null && headers.Count > 0)
                {
                    foreach (var item in headers.Keys)
                    {
                        client.DefaultRequestHeaders.TryAddWithoutValidation(item, headers[item]);
                    }
                }
                HttpResponseMessage responseMessage = client.GetAsync(url).Result;
                responseMessage.EnsureSuccessStatusCode();
                result = responseMessage.Content.ReadAsStringAsync().Result;
                if (typeof(T) == typeof(string))
                {
                    return (T)Convert.ChangeType(result, typeof(T));
                }

                return JsonConvert.DeserializeObject<T>(result);
            }
            finally
            {
                WriteLog(url, methodName, startTime, $"{string.Empty}{Environment.NewLine}", result, headers);
            }
        }

        private static void HeaderDeal(Dictionary<string, string> dic)
        {
            if (dic.ContainsKey("Host") || dic.ContainsKey("HOST"))
            {
                dic.Remove("Host");
                dic.Remove("HOST");
            }

            if (dic.ContainsKey("Accept-Encoding"))
            {
                dic.Remove("Accept-Encoding");
            }
        }

        /// <summary>
        /// 记录日志,需要引入NLOG，没有的话无法记录
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="method">方法名字</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="paramaters">请求参数</param>
        /// <param name="result">结果</param>
        public static void WriteLog(string url, string method, DateTime startTime, string paramaters, string result, Dictionary<string, string> headers = null)
        {
            try
            {
                StringBuilder build = new StringBuilder();
                build.Append($"{Environment.NewLine}");
                build.Append($"RequestUri:{url}{Environment.NewLine}");
                build.Append($"ActionName:{method}{Environment.NewLine}");
                build.Append($"EnterTime:{startTime.ToString("yyyy-MM-dd HH:mm:ss")}{Environment.NewLine}");
                build.Append($"CostTime:{(DateTime.Now.Subtract(startTime)).TotalMilliseconds}{Environment.NewLine}");
                if (headers != null && headers.Count > 0)
                {
                    foreach (var item in headers.Keys)
                    {
                        build.AppendLine($"{item}:{headers[item]}");
                    }
                }
                build.Append($"Paramaters:{paramaters}{Environment.NewLine}");
                build.Append($"Result:{result}{Environment.NewLine}");
                build.Append($"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}");
                NLog.LogManager.GetCurrentClassLogger().Trace(build.ToString());
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// https请求处理
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="cert">cert</param>
        /// <param name="chain">chain</param>
        /// <param name="error">error</param>
        /// <returns>总是返回true</returns>
        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }
    }
}
