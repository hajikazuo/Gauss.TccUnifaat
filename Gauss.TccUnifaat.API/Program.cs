using Gauss.TccUnifaat.Common.Extensions;
using Gauss.TccUnifaat.Common.Library;
using Gauss.TccUnifaat.Common.Models.Enums;
using Gauss.TccUnifaat.Common.Services;
using Gauss.TccUnifaat.Common.Services.Interfaces;
using Gauss.TccUnifaat.Common.Settings;
using Gauss.TccUnifaat.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Gauss.TccUnifaat.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                SerilogExtension.ConfigureSeqWithSerilog(builder.Configuration, TipoSetor.api);

                // Add services to the container.
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString));
                builder.Services.Configure<NewsApiSettings>(builder.Configuration.GetSection("NewsApi"));
                builder.Services.AddScoped<INoticiaService, NoticiaService>();

                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseMiddleware<SerilogMiddleware>();

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Gauss Fatal exception");
            }
            finally
            {
                Log.Information("Gauss Shutdown complete");
                Log.CloseAndFlush();
            }

        }
    }
}
