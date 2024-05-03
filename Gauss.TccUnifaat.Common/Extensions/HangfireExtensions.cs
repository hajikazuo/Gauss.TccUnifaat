using Gauss.TccUnifaat.Common.Services.Interfaces;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Gauss.TccUnifaat.Common.Extensions
{
    public static class HangfireExtensions
    {
        public static void ConfigureHangfireJobs(this IServiceProvider serviceProvider)
        {
            var recurringJobManager = serviceProvider.GetRequiredService<IRecurringJobManager>();

            recurringJobManager.AddOrUpdate("ObterNoticiasJob", () => serviceProvider.GetService<INoticiaService>().ObterNoticiasAsync(), "0 0 * * 0");

            //teste
            //recurringJobManager.AddOrUpdate("ObterNoticiasJob", () => serviceProvider.GetService<INoticiaService>().ObterNoticiasAsync(), Cron.MinuteInterval(1));
        }
    }
}
