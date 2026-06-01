using EbWeb.Models.AbilitazioniMifid.Options;
using EbWeb.Models.AbilitazioniMifid.Services.Application;
using EbWeb.Models.AbilitazioniMifid.Services.Infrastructure;
using EbWeb.Models.Common.Options;
using EbWeb.Models.Common.Security;
using EbWeb.Models.Common.Services.Application;
using EbWeb.Models.Services.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EbWeb.Configuration;

public static class AbilitazioniMifidConfig
{
    public static IServiceCollection AddAbilitazioniMifid(this IServiceCollection services, IConfiguration configuration, string sectionName = "AbilitazioneMifid", string policyName = "MifidAccess")
    {
        services.Configure<AbilitazioniMifidOptions>(sectionName, configuration.GetSection(sectionName));

        // Registrazione del DbContext isolato con i suoi Interceptor
        services.AddDbContext<MifidDbContext>((serviceProvider, options) =>
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            options.UseSqlServer(configuration.GetConnectionString("Abilitazioni_Mifid"))
                .AddInterceptors(new AuditUserInterceptor(httpContextAccessor));
        });

        // Registrazione dei servizi applicativi del modulo
        services.AddTransient<IAbilitazioneMifidService, EFCoreAbilitazioneMifidService>();
        services.AddTransient<IExcelExportService, ExcelExportService>();
#pragma warning disable CA1416
        services.AddScoped<IUserService, UserService>();
#pragma warning restore CA1416

        // Registrazione dell'Handler associato al tipo di opzioni di questo modulo
        services.AddScoped<IAuthorizationHandler, AdGroupHandler<AbilitazioniMifidOptions>>();

        // Append della Policy di sicurezza senza sovrascrivere le altre
        services.Configure<AuthorizationOptions>(options =>
        {
            options.AddPolicy(policyName, p =>
                p.RequireAuthenticatedUser()
                 .AddRequirements(new AdGroupRequirement(sectionName)));
        });

        return services;
    }
}