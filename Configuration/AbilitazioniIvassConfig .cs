using EbWeb.Models.AbilitazioniIvass.Services.Application;
using EbWeb.Models.AbilitazioniIvass.Options;
using EbWeb.Models.AbilitazioniIvass.Services.Infrastructure;
using EbWeb.Models.Common.Security;
using EbWeb.Models.Common.Services.Application;
using EbWeb.Models.Services.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EbWeb.Configuration;

public static class AbilitazioniIvassConfig
{
    public static IServiceCollection AddAbilitazioniIvass(this IServiceCollection services, IConfiguration configuration, string sectionName = "AbilitazioniIvass", string policyName = "IvassAccess")
    {
        services.Configure<AbilitazioniIvassOptions>(sectionName, configuration.GetSection(sectionName));

        // Registrazione del DbContext isolato con i suoi Interceptor
        services.AddDbContext<IvassDbContext>((serviceProvider, options) =>
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            options.UseSqlServer(configuration.GetConnectionString("Abilitazioni_Ivass"))
                .AddInterceptors(new AuditUserInterceptor(httpContextAccessor));
        });

        // Registrazione dei servizi applicativi del modulo
        services.AddTransient<IAbilitazioneIvassService, EFCoreAbilitazioneIvassService>();
        services.AddTransient<IExportAbilitazioneIvassService, ExportAbilitazioneIvassService>();
#pragma warning disable CA1416
        services.AddScoped<IUserService, UserService>();
#pragma warning restore CA1416

        // Registrazione dell'Handler associato al tipo di opzioni di questo modulo
        services.AddScoped<IAuthorizationHandler, AdGroupHandler<AbilitazioniIvassOptions>>();

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