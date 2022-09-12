using GuessThePrice.Core.Services;
using GuessThePrice.Infrastructure.Api;
using GuessThePrice.Infrastructure.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GuessThePrice.Infrastructure.Extensions;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<RossmannApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://www.rossmann.pl");
        });
        
        services.AddTransient<IProductsDataProvider, HttpProductsDataProvider>();
        return services;
    }
}