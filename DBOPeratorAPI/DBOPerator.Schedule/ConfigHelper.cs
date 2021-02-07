using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Schedule
{
    /// <summary>
    /// 配置帮助类
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 按分钟数执行的休眠时间
        /// </summary>
        public static int MinuteTimerSleepMinute => Convert.ToInt32(GetConfig("SleepMinute"));

        /// <summary>
        /// api地址
        /// </summary>
        public static string APIUrl => GetConfig("APIUrl");

        /// <summary>
        /// 单个任务最大执行次数
        /// </summary>
        public static int MaxRunTimes => Convert.ToInt32(GetConfig("MaxRunTimes"));

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="nodeName">节点名字</param>
        /// <param name="fileName">文件名字</param>
        /// <returns>结果</returns>
        private static string GetConfig(string nodeName, string fileName = "appsettings.json")
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile(fileName).Build();
            return configurationRoot.GetSection(nodeName).Value;
        }
    }
}
