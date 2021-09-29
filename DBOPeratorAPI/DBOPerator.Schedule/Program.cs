using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DBOPerator.Schedule
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var build = new HostBuilder().ConfigureServices((hostContext, services) =>
                {
                    ////执行维度  按分钟、按小时、按天、按月、按年
                    services.AddHostedService(p => new MinuteTimerHostExecutor(ConfigHelper.MinuteTimerSleepMinute));
                    //services.AddHostedService(p => new DayTimerHostExecutor(ConfigHelper.DayTimerSleepMinute));
                });

            await build.RunConsoleAsync();
            Console.WriteLine("Hello World!");
        }
    }
}
