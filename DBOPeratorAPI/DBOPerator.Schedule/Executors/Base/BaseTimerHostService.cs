using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBOPerator.Schedule
{
    /// <summary>
    /// 休眠时间
    /// </summary>
    public abstract class BaseTimerHostService : BackgroundService
    {
        private Timer _timer;

        private int _sleepMinute;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="sleepMinute">休眠时间</param>
        public BaseTimerHostService(int sleepMinute)
        {
            this._sleepMinute = sleepMinute;
        }

        /// <summary>
        /// 主执行方法
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(this._sleepMinute));
            await Task.CompletedTask;
        }

        /// <summary>
        /// 业务执行类
        /// </summary>
        /// <param name="args">timer回调参数</param>
        protected abstract void DoWork(object args);

        /// <summary>
        /// 消息提示
        /// </summary>
        /// <param name="content">内容</param>
        protected void ShowMessage(string content)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {content}");
        }
    }
}
