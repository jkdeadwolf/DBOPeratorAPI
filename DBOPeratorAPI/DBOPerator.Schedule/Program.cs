using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DBOPerator.Schedule
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var build = new HostBuilder()
                 .ConfigureServices((hostContext, services) =>
                 {
                     services.AddHostedService(p => new MinuteTimerHostExecutor(ConfigHelper.MinuteTimerSleepMinute));
                 });

            await build.RunConsoleAsync();
            Console.WriteLine("Hello World!");
        }
    }
}
