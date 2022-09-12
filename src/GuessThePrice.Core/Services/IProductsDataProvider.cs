namespace GuessThePrice.Core.Services;

public record RossmannProduct(string Name);
public interface IProductsDataProvider
{
    IAsyncEnumerable<RossmannProduct> GetRandomPromotionalProducts(int take, CancellationToken cancellationToken = default);
}

public class ProductsDataProvider : IProductsDataProvider
{
    public IAsyncEnumerable<RossmannProduct> GetRandomPromotionalProducts(int take,
        CancellationToken cancellationToken = default)
    {
        return AsyncEnumerable.Empty<RossmannProduct>();
    }
}