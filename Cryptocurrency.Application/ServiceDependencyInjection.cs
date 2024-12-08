using Cryptocurrency.Application.Crypto;
using Cryptocurrency.Application.Logger;
using Cryptocurrency.Application.User;
using Cryptocurrency.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Cryptocurrency.Application
{
    public static class ServiceDependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration, string startPath)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(startPath, rollingInterval: RollingInterval.Day)
                .Enrich.WithProperty("Application", "Cryptocurrency")
                .CreateLogger();

            services.AddScoped<ICryptoApplication, CryptoApplication>();
            services.AddScoped<ILoggerService, LoggerService>();
            services.AddScoped<IUserService, UserService>();



            services.AddInfrastructureServices(configuration);

            return services;
        }
    }
}
