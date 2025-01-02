using Cryptocurrency.Application.Cache;
using Cryptocurrency.Application.Crypto;
using Cryptocurrency.Application.Helper;
using Cryptocurrency.Application.Logger;
using Cryptocurrency.Application.User;
using Cryptocurrency.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StackExchange.Redis;

namespace Cryptocurrency.Application;

public static class ServiceDependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration, string startPath)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(startPath, rollingInterval: RollingInterval.Day, outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
            .Enrich.WithProperty("Application", "Cryptocurrency")
            .Enrich.FromLogContext()
            .CreateLogger();

        services.AddScoped<ICryptoApplication, CryptoApplication>();
        services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<PasswordHasher>();

        var redisConnection = $"{configuration.GetSection("Redis:Host").Value}:{configuration.GetSection("Redis:Port").Value}";
        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(
                new ConfigurationOptions
                {
                    EndPoints = { redisConnection },
                    AbortOnConnectFail = false,
                    Ssl = false,
                    Password = configuration.GetSection("Redis:Password").Value
                }));

        services.AddSingleton<ICacheService, RedisCacheService>();

        services.AddInfrastructureServices(configuration);

        return services;
    }
}
