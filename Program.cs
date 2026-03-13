using EbWeb.Models.Options;
using EbWeb.Models.Services.Application;
using EbWeb.Models.Services.Infrastructrure;
using EbWeb.Models.Services.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURAZIONE SERVIZI ---

var configuration = builder.Configuration;

// Services DI
builder.Services.AddTransient<IAnomaliaService, AdoNetAnomaliaService>();
builder.Services.AddTransient<IDatabaseAccessor, SqlDatabaseAccessor>();
builder.Services.AddTransient<IRevisioneService, AdoNetRevisioneService>();
builder.Services.AddTransient<IIstruttoriaService, EFCoreIstruttoriaService>();
builder.Services.AddTransient<IAgendaStipulaService, EFCoreAgendaStipulaService>();
builder.Services.AddTransient<IRichiestaPerfezionamentoService, EFCoreRichiestaPerfezionamentoService>();
builder.Services.AddTransient<ISchedaBudgetService, EFCoreSchedaBudgetService>();
builder.Services.AddTransient<IAbilitazioneMifidService, EFCoreAbilitazioneMifidService>();
#pragma warning disable CA1416
builder.Services.AddScoped<IUserService, UserService>();
#pragma warning restore CA1416

// HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// DbContext
builder.Services.AddDbContext<IstruttoriaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Cruscotto_Istruttoria")));
builder.Services.AddDbContext<ThinsoftDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Thinsoft")));
builder.Services.AddDbContext<BudgetDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Budget")));
builder.Services.AddDbContext<MifidDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Abilitazioni_Mifid")));

// Options
builder.Services.Configure<ConnectionStringsOptions>(configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<RevisioniOptions>(configuration.GetSection("Revisioni"));
builder.Services.Configure<IstruttorieOptions>(configuration.GetSection("Istruttorie"));
builder.Services.Configure<AgendaStipuleOptions>(configuration.GetSection("AgendaStipule"));
builder.Services.Configure<RichiestePerfezionamentoOptions>(configuration.GetSection("RichiestaPerfezionamento"));

// --- AUTENTICAZIONE WINDOWS ---
builder.Services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);

// --- CONTROLLERS WITH VIEWS ---
builder.Services.AddControllersWithViews();

// --- BUILD APP ---
var app = builder.Build();

// --- PIPELINE ---
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Route di default
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
