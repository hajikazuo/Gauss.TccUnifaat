using Gauss.TccUnifaat.Common.Extensions;
using Gauss.TccUnifaat.Common.Library;
using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Common.Models.Enums;
using Gauss.TccUnifaat.Common.Services;
using Gauss.TccUnifaat.Common.Services.Interfaces;
using Gauss.TccUnifaat.Common.Settings;
using Gauss.TccUnifaat.Data;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    SerilogExtension.ConfigureSeqWithSerilog(builder.Configuration, TipoSetor.mvc);

    // Add services to the container.
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.Configure<Gauss.TccUnifaat.Common.Settings.SendGridSettings>(builder.Configuration.GetSection("SendGridSettings"));
    builder.Services.AddSingleton<IEmailService, SendGridService>();
    builder.Services.AddSingleton(RT.Comb.Provider.Sql);
    builder.Services.Configure<NewsApiSettings>(builder.Configuration.GetSection("NewsApi"));
    builder.Services.AddScoped<INoticiaService, NoticiaService>();
    builder.Services.AddIdentity<Usuario, Funcao>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    builder.Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();

    builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            options.SlidingExpiration = true;
            options.AccessDeniedPath = "/Account/Acess";
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.Cookie.Name = "Gauss.Cookie";
            options.Cookie.HttpOnly = true;
        });


    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Administrador"));

        options.AddPolicy("RequireProfessorRole", policy => policy.RequireRole("Professor"));

        options.AddPolicy("RequireAdminOrProfessorRole", policy =>
            policy.RequireRole("Administrador", "Professor"));
    });

    builder.Services.AddHangfire(options =>
    {

    });
    builder.Services.AddHttpClient();

    var app = builder.Build();
    using (var scope = app.Services.CreateScope())
    {

        //migrations
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();

    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseMiddleware<SerilogMiddleware>();

    CriarPerfisUsuarios(app);

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    void CriarPerfisUsuarios(WebApplication app)
    {
        var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
        using (var scope = scopedFactory.CreateScope())
        {
            var service = scope.ServiceProvider.GetService<ISeedUserRoleInitial>();
            service.SeedRoles();
            service.SeedUsers();
        }
    }

#pragma warning disable ASP0014 // Suggest using top level route registrations
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
        );

        endpoints.MapControllerRoute(
           name: "default",
           pattern: "{controller=Home}/{action=Index}/{id?}"
       );
    });

#pragma warning restore ASP0014 // Suggest using top level route registrations


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
