using EbWeb.Controllers;
using EbWeb.Models.Options;
using EbWeb.Models.Services.Application;
using EbWeb.Models.Services.Infrastructrure;
using EbWeb.Models.Services.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Qui builder.Configuration è già un IConfiguration!
var configuration = builder.Configuration;

// Configurazione dei servizi
builder.Services.AddTransient<IAnomaliaService, AdoNetAnomaliaService>();
builder.Services.AddTransient<IDatabaseAccessor, SqlDatabaseAccessor>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IRevisioneService, AdoNetRevisioneService>();
builder.Services.AddTransient<IIstruttoriaService, EFCoreIstruttoriaService>();

// Registrazione dell'autenticazione Windows (IIS)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;
});

// Options
builder.Services.Configure<ConnectionStringsOptions>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<RevisioniOptions>(builder.Configuration.GetSection("Revisioni"));
builder.Services.Configure<IstruttorieOptions>(builder.Configuration.GetSection("Istruttorie"));

builder.Services.AddDbContext<IstruttoriaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Cruscotto_Istruttoria")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // app.UseExceptionHandler("/Home/Error");
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Middleware per l'autenticazione e l'autorizzazione
app.UseAuthentication();
app.UseAuthorization();

// Route personalizzata con parametri opzionali
app.MapControllerRoute(
    name: "ClientiFiltro",
    pattern: "Clienti/{stato?}/{anno?}",
    defaults: new { controller = "Clienti", action = "Index" }
);

// Route di default
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
