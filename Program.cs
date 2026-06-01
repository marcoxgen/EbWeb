using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using EbWeb.Configuration;
using EbWeb.Models.Options;
using EbWeb.Models.Services.Application;
using EbWeb.Models.Services.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Configura l'autenticazione Windows integrata (Kerberos/NTLM) tramite protocollo Negotiate
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

// Registra i servizi Dependency Injection (DI) globali
builder.Services.AddTransient<IAnomaliaService, AdoNetAnomaliaService>();
builder.Services.AddTransient<IDatabaseAccessor, SqlDatabaseAccessor>();
builder.Services.AddTransient<IRevisioneService, AdoNetRevisioneService>();
builder.Services.AddTransient<IIstruttoriaService, EFCoreIstruttoriaService>();
builder.Services.AddTransient<IAgendaStipulaService, EFCoreAgendaStipulaService>();
builder.Services.AddTransient<IRichiestaPerfezionamentoService, EFCoreRichiestaPerfezionamentoService>();
builder.Services.AddTransient<ISchedaBudgetService, EFCoreSchedaBudgetService>();

// Registrazione dei moduli funzionali dell'applicazione con le rispettive configurazioni e policy
builder.Services.AddAbilitazioniMifid(configuration, "AbilitazioneMifid", "MifidAccess");
builder.Services.AddAlimentazioneBudget(configuration);

// Contiene tutto ciò che riguarda una singola richiesta HTTP in corso
builder.Services.AddHttpContextAccessor();

// Registra i DbContext nel contenitore della Dependency Injection (DI) di ASP.NET Core
builder.Services.AddDbContext<IstruttoriaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Cruscotto_Istruttoria")));
builder.Services.AddDbContext<ThinsoftDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Thinsoft")));
builder.Services.AddDbContext<BudgetDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Budget")));

// Configurazione Dapper per gestire DateOnly
Dapper.SqlMapper.AddTypeHandler(new EbWeb.Models.Helpers.DateOnlyTypeHandler());

// Carica i parametri di configurazione personalizzati dal file appsettings.json
builder.Services.Configure<ConnectionStringsOptions>(configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<RevisioniOptions>(configuration.GetSection("Revisioni"));
builder.Services.Configure<IstruttorieOptions>(configuration.GetSection("Istruttorie"));
builder.Services.Configure<AgendaStipuleOptions>(configuration.GetSection("AgendaStipule"));
builder.Services.Configure<RichiestePerfezionamentoOptions>(configuration.GetSection("RichiestaPerfezionamento"));

// Abilita il supporto per i Controller e le View (Razor).
builder.Services.AddControllersWithViews();

// Finalizza la configurazione del contenitore dei servizi (Dependency Injection)
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // In sviluppo, mostra una pagina di errore dettagliata per il debugging
    app.UseDeveloperExceptionPage();
}
else
{
    // In produzione, reindirizza a una pagina di errore generica (User Friendly)
    app.UseExceptionHandler("/Error");
    // Forza l'uso di connessioni sicure HTTPS (HSTS)
    app.UseHsts();
}

// Abilita il servizio dei file fisici (CSS, JS, Immagini) contenuti nella cartella 'wwwroot'
app.UseStaticFiles();
// Analizza l'URL della richiesta in arrivo e individua il Controller/Action corrispondente.
app.UseRouting();

// I Middleware di sicurezza rimangono nella posizione corretta: prima l'autenticazione, poi l'autorizzazione
app.UseAuthentication();
app.UseAuthorization();

// Stabilisce la convenzione per interpretare gli URL del browser
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();