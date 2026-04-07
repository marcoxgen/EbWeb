using EbWeb.Models.Options;
using EbWeb.Models.Services.Application;
using EbWeb.Models.Services.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Models.Options;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Registra i servizi Dependency Injection (DI)
builder.Services.AddTransient<IAnomaliaService, AdoNetAnomaliaService>();
builder.Services.AddTransient<IDatabaseAccessor, SqlDatabaseAccessor>();
builder.Services.AddTransient<IRevisioneService, AdoNetRevisioneService>();
builder.Services.AddTransient<IIstruttoriaService, EFCoreIstruttoriaService>();
builder.Services.AddTransient<IAgendaStipulaService, EFCoreAgendaStipulaService>();
builder.Services.AddTransient<IRichiestaPerfezionamentoService, EFCoreRichiestaPerfezionamentoService>();
builder.Services.AddTransient<ISchedaBudgetService, EFCoreSchedaBudgetService>();
builder.Services.AddTransient<IAbilitazioneMifidService, EFCoreAbilitazioneMifidService>();
builder.Services.AddTransient<IExcelExportService, ExcelExportService>();
#pragma warning disable CA1416
builder.Services.AddScoped<IUserService, UserService>();
#pragma warning restore CA1416

// Contiene tutto ciò che riguarda una singola richiesta HTTP in corso
builder.Services.AddHttpContextAccessor();

// Registra i DbContext nel contenitore della Dependency Injection (DI) di ASP.NET Core
builder.Services.AddDbContext<IstruttoriaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Cruscotto_Istruttoria")));
builder.Services.AddDbContext<ThinsoftDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Thinsoft")));
builder.Services.AddDbContext<BudgetDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Budget")));
builder.Services.AddDbContext<MifidDbContext>((serviceProvider, options) =>
{
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    
    options.UseSqlServer(builder.Configuration.GetConnectionString("Abilitazioni_Mifid"))
           .AddInterceptors(new AuditUserInterceptor(httpContextAccessor));
});

// Configurazione Dapper per gestire DateOnly
Dapper.SqlMapper.AddTypeHandler(new EbWeb.Models.Helpers.DateOnlyTypeHandler());

// Carica i parametri di configurazione personalizzati dal file appsettings.json
builder.Services.Configure<ConnectionStringsOptions>(configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<RevisioniOptions>(configuration.GetSection("Revisioni"));
builder.Services.Configure<IstruttorieOptions>(configuration.GetSection("Istruttorie"));
builder.Services.Configure<AgendaStipuleOptions>(configuration.GetSection("AgendaStipule"));
builder.Services.Configure<RichiestePerfezionamentoOptions>(configuration.GetSection("RichiestaPerfezionamento"));
builder.Services.Configure<AbilitazioniMifidOptions>(configuration.GetSection("AbilitazioneMifid"));

// Configura l'app per accettare l'identità dell'utente passata da IIS.
// Questo permette di recuperare automaticamente l'utente AD (Dominio\Utente)
// senza richiedere una maschera di login manuale.
builder.Services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);

// Abilita il supporto per i Controller e le View (Razor).
// Include il 'Model Binding' (conversione automatica dei dati da Web a C#)
// e il 'Fluent Validation' se configurato, per gestire le interfacce utente.
builder.Services.AddControllersWithViews();

// Finalizza la configurazione del contenitore dei servizi (Dependency Injection)
// e inizializza l'istanza dell'applicazione (Web Host).
// Dopo questa riga non è più possibile registrare nuovi servizi.
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
// Definisce 'dove' la richiesta deve andare, permettendo ai successivi Middleware 
// (come Autenticazione e Autorizzazione) di applicare le regole specifiche per quella destinazione.
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Stabilisce la convenzione per interpretare gli URL del browser
// Se l'utente non specifica nulla, il sistema carica automaticamente 
// la Action 'Index' del 'HomeController'
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();