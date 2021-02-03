using DBOPerator.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBOPeratorAPI.Filters
{
    public class GlobalExceptionsFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                context.ExceptionHandled = true;//设置异常已被处理
                if (string.IsNullOrWhiteSpace(context.Exception?.Message) == false && context.Exception.Message.Contains("未登录"))
                {
                    context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                }
                else
                {
                    context.Result = new JsonResult(new Result() { Msg = $"系统异常：{context.Exception?.Message}" });
                }

                NLog.LogManager.GetCurrentClassLogger().Error(context.Exception.ToString());
            }
        }
    }
}
