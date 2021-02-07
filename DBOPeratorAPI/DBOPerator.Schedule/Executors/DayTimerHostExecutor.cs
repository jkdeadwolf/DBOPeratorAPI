using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Schedule
{
    /// <summary>
    /// 按天执行的
    /// </summary>
    public class DayTimerHostExecutor : BaseTimerHostService
    {
        public DayTimerHostExecutor(int sleepMinute) : base(sleepMinute)
        {
        }

        protected override void DoWork(object args)
        {
            throw new NotImplementedException();
        }
    }
}
