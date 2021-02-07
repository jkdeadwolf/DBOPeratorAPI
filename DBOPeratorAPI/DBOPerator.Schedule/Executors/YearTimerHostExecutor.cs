using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Schedule
{
    /// <summary>
    /// 按年执行
    /// </summary>
    public class YearTimerHostExecutor : BaseTimerHostService
    {
        public YearTimerHostExecutor(int sleepMinute) : base(sleepMinute)
        {
        }

        protected override void DoWork(object args)
        {
            throw new NotImplementedException();
        }
    }
}
