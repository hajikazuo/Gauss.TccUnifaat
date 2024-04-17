using Serilog.Core;
using Serilog.Events;
using Serilog;
using System.Dynamic;
using Serilog.Exceptions;

namespace Gauss.TccUnifaat.MVC.Extensions
{
    public class SerilogExtension
    {
        public static void ConfigureSeqWithSerilog(IConfiguration configuration)
        {
            var seq_url = configuration.GetValue<String>("SEQ:URL");
            var seq_key = configuration.GetValue<String>("SEQ:URL");
            var seq_MinimumLevel = configuration.GetValue<String>("SEQ:MinimumLevel");


            var levelSwitch = new LoggingLevelSwitch
            {
                MinimumLevel = ReturnLogLevel(seq_MinimumLevel)
            };

            Serilog.Debugging.SelfLog.Enable(Console.Error);

            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(levelSwitch)
                    .Enrich.WithCorrelationId()
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails()
                     .Enrich.WithProperty("Cliente", "SIGA", false)
                  .WriteTo.Seq(seq_url, apiKey: seq_key)
                  .Destructure.ByTransforming<ExpandoObject>(e => new Dictionary<string, object>(e))
                  .CreateLogger();


        }
        public static LogEventLevel ReturnLogLevel(String valor)
        {
            return valor switch
            {
                "Verbose" => LogEventLevel.Verbose,
                "Debug" => LogEventLevel.Debug,
                "Information" => LogEventLevel.Information,
                "Warning" => LogEventLevel.Warning,
                "Error" => LogEventLevel.Error,
                "Fatal" => LogEventLevel.Fatal,
                _ => LogEventLevel.Error,
            };
        }
    }
}

