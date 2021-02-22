using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Schedule
{
    /// <summary>
    /// 主要是执行
    /// </summary>
    public class MinuteTimerHostExecutor : BaseTimerHostService
    {
        public MinuteTimerHostExecutor(int sleepMinute) : base(sleepMinute)
        {
        }

        /// <summary>
        /// 主执行方法
        /// </summary>
        /// <param name="args">入参</param>
        protected override void DoWork(object args)
        {
            try
            {
                var tasks = DBOperatorHelper.GetTasksAsync(ConfigHelper.MaxRunTimes);
                if (tasks == null || tasks.Data == null || tasks.Data.Count == 0)
                {
                    base.ShowMessage("未获取到待处理任务");
                    return;
                }

                foreach (var item in tasks.Data)
                {
                    base.ShowMessage($"开始处理任务：{item.KeyID}");
                    var res = DBOperatorHelper.ExecuteTaskAsync(item.KeyID);
                    base.ShowMessage($"结束处理任务：{item.KeyID},{res.Success} {res.Msg}");
                }
            }
            catch (Exception e)
            {
                base.ShowMessage($"系统出错：{e.ToString()}");
                NLog.LogManager.GetCurrentClassLogger().Error(e);
            }
            finally
            {
                base.ShowMessage($"一轮执行完毕，休眠：{ConfigHelper.MinuteTimerSleepMinute}分钟");
            }
        }
    }
}
