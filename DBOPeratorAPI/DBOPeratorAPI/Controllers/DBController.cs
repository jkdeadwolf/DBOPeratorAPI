using DBOPerator.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DBOPeratorAPI.Controllers
{
    /// <summary>
    /// db操作类
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("cors")]
    public class DBController : Controller
    {
        #region 数据库连接操作
        /// <summary>
        /// 添加链接
        /// </summary>
        /// <param name="connectionString">connectionstring</param>
        /// <returns>添加结果</returns>
        [HttpPost("AddConString")]
        public Result AddConString([FromBody] string connectionString)
        {
            return new Result();
        }

        /// <summary>
        /// 修改链接
        /// </summary>
        /// <param name="keyID">链接对应逐渐</param>
        /// <returns>添加结果</returns>
        [HttpPost("UpdateConString")]
        public Result UpdateConString(string keyID)
        {
            return new Result();
        }

        /// <summary>
        /// 删除链接
        /// </summary>
        /// <param name="keyID">链接对应主键</param>
        /// <returns>删除结果</returns>
        [HttpPost("DelConString")]
        public Result DelConString(string keyID)
        {
            return new Result();
        }

        /// <summary>
        /// 启用链接
        /// </summary>
        /// <param name="keyID">链接对应主键</param>
        /// <returns>启用结果</returns>
        [HttpPost("EnableConString")]
        public Result EnableConString(string keyID)
        {
            return new Result();
        }

        /// <summary>
        /// 禁用链接
        /// </summary>
        /// <param name="keyID">链接对应主键</param>
        /// <returns>删除结果</returns>
        [HttpPost("DisableConString")]
        public Result DisableConString(string keyID)
        {
            return new Result();
        }

        /// <summary>
        /// 数据库链接分页
        /// </summary>
        /// <param name="paramIn">入参</param>
        /// <returns>结果</returns>
        [HttpPost("PagerConString")]
        public PagerParamOut<ConStringModel> PagerConString(PagerParamIn<object> paramIn)
        {
            return new PagerParamOut<ConStringModel>();
        }

        /// <summary>
        /// 根据数据库链接执行连接下所有库进行创建表操作
        /// </summary>
        /// <param name="keyID">数据库链接主键</param>
        /// <param name="sql">sql语句</param>
        /// <returns>执行结果</returns>
        [HttpPost("CreateTableByConKeyID/keyID")]
        public Result CreateTableByConKeyID(string keyID, [FromBody] string sql)
        {
            return new Result();
        }

        /// <summary>
        /// 手动识别数据库建表差异
        /// </summary>
        /// <param name="keyID">数据库主键</param>
        /// <returns>结果</returns>
        [HttpGet("ReadDBTableChanges/keyID")]
        public Result ReadDBTableChanges(string keyID)
        {
            return new Result() { };
        }

        #endregion

        #region 数据库表,用户输入连接后自动生成此表的数据，但是可能会需要人工排查表规则是否正确

        /// <summary>
        /// 表设计分页
        /// </summary>
        /// <returns>结果</returns>
        [HttpPost("PagerTableModel")]
        public PagerParamOut<TableModel> PagerTableModel(PagerParamIn<TablePagerCondition> paramIn)
        {
            return new PagerParamOut<TableModel>();
        }

        /// <summary>
        /// 新增模板表
        /// </summary>
        /// <param name="paramIn">入参</param>
        /// <returns>结果</returns>
        [HttpPost("AddTable")]
        public Result AddTable(TableModel paramIn)
        {
            return new Result();
        }

        /// <summary>
        /// 更新表信息
        /// </summary>
        /// <param name="paramIn">入参</param>
        /// <returns>结果</returns>
        [HttpPost("UpdateTableByKeyId")]
        public Result UpdateTableByKeyId(TableModel paramIn)
        {
            return new Result();
        }

        /// <summary>
        /// 根据主键获取模板表信息
        /// </summary>
        /// <param name="keyid">主键</param>
        /// <returns>结果</returns>
        [HttpGet("GetTableByKeyId/keyid")]
        public Result<TableModel> GetTableByKeyId(string keyid)
        {
            return new Result<TableModel>();
        }

        /// <summary>
        /// 根据主键启用模板表信息
        /// </summary>
        /// <param name="keyid">主键</param>
        /// <returns>结果</returns>
        [HttpGet("EnableTableByKeyId/keyid")]
        public Result<TableModel> EnableTableByKeyId(string keyid)
        {
            return new Result<TableModel>();
        }

        /// <summary>
        /// 根据主键禁用模板表信息
        /// </summary>
        /// <param name="keyid">主键</param>
        /// <returns>结果</returns>
        [HttpGet("DisableTableByKeyId/keyid")]
        public Result<TableModel> DisableTableByKeyId(string keyid)
        {
            return new Result<TableModel>();
        }

        /// <summary>
        /// 根据主键删除模板表信息
        /// </summary>
        /// <param name="keyid">主键</param>
        /// <returns>结果</returns>
        [HttpGet("DelTableByKeyId/keyid")]
        public Result<TableModel> DelTableByKeyId(string keyid)
        {
            return new Result<TableModel>();
        }

        /// <summary>
        /// 获取建表模板
        /// </summary>
        /// <param name="keyid">表主键</param>
        /// <returns>结果</returns>
        [HttpGet("GetTemplateSqlCreate/keyid")]
        public Result GetTemplateSqlCreate(string keyid)
        {
            return new Result();
        }

        /// <summary>
        /// 执行表的创建
        /// </summary>
        /// <param name="tableKeyID">表主键</param>
        /// <param name="sql">待执行的sql语句</param>
        /// <returns>执行结果</returns>
        [HttpPost("ExecuteCreateTable/tableKeyID")]
        public Result ExecuteCreateTable(string tableKeyID, [FromBody] string sql)
        {
            ////当sql为空，获取表模板的sql语句执行建表，不为空执行入参中的sql建表
            return new Result();
        }
        #endregion

        #region 任务信息

        /// <summary>
        /// 添加db任务
        /// </summary>
        /// <param name="paramIn">入参</param>
        /// <returns>结果</returns>
        [HttpPost("AddDBTask")]
        public Result AddDBTask(DBTaskModel paramIn)
        {
            return new Result();
        }

        /// <summary>
        /// 删除db任务
        /// </summary>
        /// <param name="keyID">主键</param>
        /// <returns>结果</returns>
        [HttpPost("DelDBTask/keyID")]
        public Result DelDBTask(string keyID)
        {
            return new Result();
        }

        /// <summary>
        /// 完结db任务
        /// </summary>
        /// <param name="keyID">主键</param>
        /// <param name="message">完结原因</param>
        /// <returns>结果</returns>
        [HttpPost("CompleteDBTask/keyID")]
        public Result CompleteDBTask(string keyID, [FromBody] string message)
        {
            return new Result();
        }

        /// <summary>
        /// 数据库任务分页
        /// </summary>
        /// <param name="paramIn">入参</param>
        /// <returns>结果</returns>
        [HttpPost("PagerDBTask")]
        public PagerParamOut<DBTaskModel> PagerDBTask(PagerParamIn<DBTaskCondition> paramIn)
        {
            return new PagerParamOut<DBTaskModel>();
        }
        #endregion
    }
}
