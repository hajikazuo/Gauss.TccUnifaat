using Gauss.TccUnifaat.Common.Services.Interface;
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

            recurringJobManager.AddOrUpdate("ExcluirAvisosAntigosJob", () => serviceProvider.GetService<IAvisoService>().ExcluirAvisosAntigosAsync(), "0 0 * * *");


        }
    }
}
