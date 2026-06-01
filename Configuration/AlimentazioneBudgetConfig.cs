using Microsoft.EntityFrameworkCore;
using EbWeb.Models.AlimentazioneBudget.Services.Application;
using EbWeb.Models.AlimentazioneBudget.Services.Infrastructure;
using EbWeb.Models.AlimentazioneBudget.Options;

namespace EbWeb.Configuration;

public static class AlimentazioneBudgetConfig
{
    public static IServiceCollection AddAlimentazioneBudget(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AlimentazioneBudgetOptions>(configuration.GetSection("AlimentazioneBudget"));
        
        services.AddDbContext<AlimentazioneBudgetDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Alimentazione_Budget")));
        
        services.AddTransient<IAlimentazioneBudgetService, EFCoreAlimentazioneBudgetService>();
        services.AddTransient<IEsecutoreComandiService, AdoNetEsecutoreComandiService>();

        return services;
    }
}