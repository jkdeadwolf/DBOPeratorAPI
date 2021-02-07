using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Schedule
{
    /// <summary>
    /// 按小时执行的
    /// </summary>
    public class HourTimerHostExecutor : BaseTimerHostService
    {
        public HourTimerHostExecutor(int sleepMinute) : base(sleepMinute)
        {
        }

        protected override void DoWork(object args)
        {
            throw new NotImplementedException();
        }
    }
}
