using GuessThePrice.Core.Services;
using GuessThePrice.Infrastructure.Api;

namespace GuessThePrice.Infrastructure.Services;

internal class HttpProductsDataProvider : IProductsDataProvider
{
    private RossmannApiClient _rossmannApiClient;

    public HttpProductsDataProvider(RossmannApiClient rossmannApiClient)
    {
        _rossmannApiClient = rossmannApiClient;
    }

    public IAsyncEnumerable<RossmannProduct> GetRandomPromotionalProducts(int take,
        CancellationToken cancellationToken = default)
    {
        return _rossmannApiClient.GetMegaProducts(cancellationToken).Take(take).Select(x => new RossmannProduct(x.Id, $"{x.Name} {x.Caption}", x.OldPrice, x.Price, x.Pictures[0].Medium, x.NavigateUrl));
    }
}