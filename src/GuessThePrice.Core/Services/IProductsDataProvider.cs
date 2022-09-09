namespace GuessThePrice.Core.Services;

public record RossmannProduct();
public interface IProductsDataProvider
{
    IAsyncEnumerable<RossmannProduct> GetRandomPromotionalProducts(int take, CancellationToken cancellationToken = default);
}