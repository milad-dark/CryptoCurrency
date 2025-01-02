using Cryptocurrency.Domain.Interfaces;
using Cryptocurrency.Infrastructure.Data;
using Cryptocurrency.Infrastructure.Repositories;
using Cryptocurrency.Infrastructure.ServiceProxy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Cryptocurrency.Infrastructure;

public static class ServiceDependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
            services.AddDbContextFactory<ApplicationContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        // Register Refit interfaces
        services.AddRefitClient<ICoinMarketCapApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(configuration["CoinMarketCapApiUrl"]);
                c.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", configuration["CoinMarketCapApiKey"]);
            });

        services.AddRefitClient<IExchangeRatesApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration["ExchangeRatesApiUrl"]));

        services.AddScoped<ICryptoRepository, CryptoRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
