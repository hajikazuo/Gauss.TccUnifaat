using Gauss.TccUnifaat.Common.Models;
using Gauss.TccUnifaat.Data;
using Gauss.TccUnifaat.MVC.Services;
using Gauss.TccUnifaat.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Gauss.TccUnifaat.Migrations;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using System.Drawing;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSingleton<RT.Comb.ICombProvider>(RT.Comb.Provider.Sql);
builder.Services.AddIdentity<Usuario, Funcao>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

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

// Configuração do Hangfire usando o SQL Server
var hangfireConnectionString = builder.Configuration.GetConnectionString("HangfireConnection");
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)  // Define o nível de compatibilidade dos dados.
    .UseSimpleAssemblyNameTypeSerializer()  // Utiliza um serializador simples de nomes de assemblies.
    .UseRecommendedSerializerSettings()  // Configurações recomendadas para o serializador.
    .UseSqlServerStorage(hangfireConnectionString));  // Armazenamento no SQL Server.
builder.Services.AddHangfireServer();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("RequireProfessorRole", policy => policy.RequireRole("Professor"));

    options.AddPolicy("RequireAdminOrProfessorRole", policy =>
        policy.RequireRole("Administrador", "Professor"));
});

builder.Services.AddHttpClient();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{

    //migrations
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();

}

// Configuração do painel de controle do Hangfire
app.UseHangfireDashboard();

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

CriarPerfisUsuarios(app);

app.UseRouting();
app.MapHangfireDashboard(); // Endpoint do painel de controle: /hangfire
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
