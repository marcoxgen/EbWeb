using Microsoft.EntityFrameworkCore;
using EbWeb.Models.ControlliThanos2.Services.Application;
using EbWeb.Models.ControlliThanos2.Services.Infrastructure;
using EbWeb.Models.ControlliThanos2.Options;

namespace EbWeb.Configuration;

public static class ControlliThanos2Config
{
    public static IServiceCollection AddControlliThanos2(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ControlliThanos2Options>(configuration.GetSection("ControlliThanos2"));
        
        services.AddDbContext<ControlliDWHDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Controlli_DWH")));
        
        services.AddTransient<IControlloThanos2Service, EFCoreControlloThanos2Service>();

        return services;
    }
}