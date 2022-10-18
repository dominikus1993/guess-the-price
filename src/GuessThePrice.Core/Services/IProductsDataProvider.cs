namespace GuessThePrice.Core.Services;

public record RossmannProduct(int Id, string Name, double Price, double PromotionalPrice, string? ImageUrl, string NavigateUrl);
public interface IProductsDataProvider
{
    IAsyncEnumerable<RossmannProduct> GetRandomPromotionalProducts(int take, CancellationToken cancellationToken = default);
}