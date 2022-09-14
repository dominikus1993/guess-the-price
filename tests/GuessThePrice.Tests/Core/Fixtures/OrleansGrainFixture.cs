using GuessThePrice.Core.Grains;
using GuessThePrice.Core.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Orleans.Hosting;
using Orleans.TestingHost;

namespace GuessThePrice.Tests.Core.Fixtures;

public class OrleansGrainFixture : IDisposable
{
    public OrleansGrainFixture()
    {
        var builder = new TestClusterBuilder();
        builder.AddSiloBuilderConfigurator<TestSiloConfigurations>();
        Cluster = builder.Build();
        Cluster.Deploy();
    }

    public void Dispose()
    {
        Cluster.StopAllSilos();
    }

    public TestCluster Cluster { get; private set; }
}

public class TestSiloConfigurations : ISiloConfigurator
{
    public void Configure(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMemoryGrainStorage("games");
        siloBuilder.ConfigureServices(services =>
        {
            services.AddTransient<IProductsDataProvider, FakeProductsDataProvider>();
            services.AddSingleton<ILogger<GameGrain>>(_ =>
            {
                return LoggerFactory.Create(l => l.AddConsole()).CreateLogger<GameGrain>();
            });
        });
    }
}

public class FakeProductsDataProvider : IProductsDataProvider
{
    public IAsyncEnumerable<RossmannProduct> GetRandomPromotionalProducts(int take, CancellationToken cancellationToken = default)
    {
        return new[] { new RossmannProduct(1, "product", 4.3, 4.1, "", "") }.ToAsyncEnumerable();
    }
}