using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Biz
{
    public class ConnectionHelper
    {
        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <param name="name">节点名字</param>
        /// <returns>结果</returns>
        public static SqlSugarClient GetSqlSugarClient(string name = "DBOPerator")
        {
            return new SqlSugarClient(new ConnectionConfig()
            {
                //必填, 数据库连接字符串
                ConnectionString = GetConnecttionString($"{name}Write"),
                SlaveConnectionConfigs = new List<SlaveConnectionConfig>()
                {
                    new SlaveConnectionConfig()
                    {
                        ConnectionString = GetConnecttionString($"{name}Read"),
                    }
                },

                //必填, 数据库类型
                DbType = DbType.MySql,
                //默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
        }

        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <param name="name">节点名字</param>
        /// <returns>结果</returns>
        public static SqlSugarClient GetSqlSugarClientByConString(string connectiongString)
        {
            return new SqlSugarClient(new ConnectionConfig()
            {
                //必填, 数据库连接字符串
                ConnectionString = connectiongString,
                //必填, 数据库类型
                DbType = DbType.MySql,
                //默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
        }

        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <param name="nodeName">数据链接节点名字</param>
        /// <param name="fileName">文件名字</param>
        /// <returns>结果</returns>
        private static string GetConnecttionString(string nodeName, string fileName = "appsettings.json")
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile(fileName).Build();
            return configurationRoot.GetConnectionString(nodeName);
        }
    }
}
