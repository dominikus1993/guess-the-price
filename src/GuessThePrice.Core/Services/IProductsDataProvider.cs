namespace GuessThePrice.Core.Services;

public record RossmannProduct(string Name);
public interface IProductsDataProvider
{
    IAsyncEnumerable<RossmannProduct> GetRandomPromotionalProducts(int take, CancellationToken cancellationToken = default);
}