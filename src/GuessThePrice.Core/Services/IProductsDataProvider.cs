namespace GuessThePrice.Core.Services;

public record RossmannProduct(string Name, int Id, double Price, double PromotionalPrice, string ImageUrl, string NavigateUrl);
public interface IProductsDataProvider
{
    IAsyncEnumerable<RossmannProduct> GetRandomPromotionalProducts(int take, CancellationToken cancellationToken = default);
}