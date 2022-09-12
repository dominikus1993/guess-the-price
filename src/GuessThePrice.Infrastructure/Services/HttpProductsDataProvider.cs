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
        return _rossmannApiClient.GetMegaProducts(cancellationToken).Take(take).Select(x => new RossmannProduct($"{x.Name} {x.Caption}", x.Id, x.Price, x.PromotionalPrice, x.ImageUrl, x.NavigateUrl));
    }
}