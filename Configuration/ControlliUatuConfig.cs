using Microsoft.EntityFrameworkCore;
using EbWeb.Models.ControlliUatu.Services.Application;
using EbWeb.Models.ControlliUatu.Services.Infrastructure;
using EbWeb.Models.ControlliUatu.Options;

namespace EbWeb.Configuration;

public static class ControlliUatuConfig
{
    public static IServiceCollection AddControlliUatu(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ControlliUatuOptions>(configuration.GetSection("ControlliUatu"));
        
        services.AddDbContext<ControlliDWHDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Controlli_DWH")));
        
        services.AddTransient<IControlloUatuService, EFCoreControlloUatuService>();

        return services;
    }
}