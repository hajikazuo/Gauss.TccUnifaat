using Serilog.Core;
using Serilog.Events;
using Serilog;
using System.Dynamic;
using Serilog.Exceptions;
using Microsoft.Extensions.Configuration;
using Gauss.TccUnifaat.Common.Models.Enums;

namespace Gauss.TccUnifaat.Common.Extensions
{
    public class SerilogExtension
    {
        public static void ConfigureSeqWithSerilog(IConfiguration configuration, TipoSetor tipo)
        {
            var seq_url = configuration.GetSection("SEQ:URL").Value;
            var seq_key = configuration.GetSection("SEQ:Key").Value;
            var seq_MinimumLevel = configuration.GetSection("SEQ:MinimumLevel").Value;


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
                     .Enrich.WithProperty("Setor", tipo, false)
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

