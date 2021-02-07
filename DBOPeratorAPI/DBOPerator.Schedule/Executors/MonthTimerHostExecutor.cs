using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Schedule
{
    /// <summary>
    /// 按月执行
    /// </summary>
    public class MonthTimerHostExecutor : BaseTimerHostService
    {
        public MonthTimerHostExecutor(int sleepMinute) : base(sleepMinute)
        {
        }

        protected override void DoWork(object args)
        {
            throw new NotImplementedException();
        }
    }
}
