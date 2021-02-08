using DBOPerator.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Biz
{
    /// <summary>
    /// 操作
    /// </summary>
    public interface IDBOperator
    {
        /// <summary>
        /// 获取数据库列表
        /// </summary>
        /// <param name="connectionString">数据库连接</param>
        /// <returns>结果</returns>
        List<string> GetAllDataBase(string connectionString, ref SqlSugarClient client);

        /// <summary>
        /// 获取数据库表信息
        /// </summary>
        /// <param name="dataBaseName">数据库名字</param>
        /// <param name="client">客户端</param>
        /// <returns>结果</returns>
        List<Table> GetAllTables(string dataBaseName, SqlSugarClient client);

        /// <summary>
        /// 获取数据库表信息
        /// </summary>
        /// <param name="tableName">数据库名字</param>
        /// <param name="client">客户端</param>
        /// <returns>结果</returns>
        string GetTableDDL(string databaseName, string tableName, SqlSugarClient client);

        /// <summary>
        /// 执行sql，主要是检表语句这些
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="client">链接</param>
        /// <returns>结果</returns>
        bool ExecuteSql(string sql, SqlSugarClient client);
    }
}
