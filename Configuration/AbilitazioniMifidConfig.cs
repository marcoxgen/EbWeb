using EbWeb.Models.AbilitazioniMifid.Options;
using EbWeb.Models.AbilitazioniMifid.Services.Application;
using EbWeb.Models.AbilitazioniMifid.Services.Infrastructure;
using EbWeb.Models.Common.Services.Application;
using EbWeb.Models.Services.Application;
using Microsoft.EntityFrameworkCore;
//using System.Security.Cryptography;

namespace EbWeb.Configuration;

public static class AbilitazioniMifidConfig
{
    public static IServiceCollection AddAbilitazioniMifid(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AbilitazioniMifidOptions>(configuration.GetSection("AbilitazioneMifid"));
        
        services.AddDbContext<MifidDbContext>((serviceProvider, options) =>
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

            options.UseSqlServer(configuration.GetConnectionString("Abilitazioni_Mifid"))
                   .AddInterceptors(new AuditUserInterceptor(httpContextAccessor));
        });

        services.AddTransient<IAbilitazioneMifidService, EFCoreAbilitazioneMifidService>();
        services.AddTransient<IExcelExportService, ExcelExportService>();
        #pragma warning disable CA1416
        services.AddScoped<IUserService, UserService>();
        #pragma warning restore CA1416

        return services;
    }
}
