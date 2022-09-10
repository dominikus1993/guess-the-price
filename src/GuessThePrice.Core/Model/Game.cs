using GuessThePrice.Core.Model.Exceptions;
using GuessThePrice.Core.Services;

namespace GuessThePrice.Core.Model;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public decimal PromotionalPrice { get; set; }

    public Product()
    {
        
    }

    public Product(RossmannProduct product)
    {
        Name = product.Name;
    }
}

public readonly record struct PromotionalPriceResponse(decimal Value);

public record Response(Product Product, PromotionalPriceResponse PromotionalPriceResponse);

public sealed class Game
{
    private const int MaximumAnswersQuantity = 5;
    private List<Response> _responses;
    public IReadOnlyCollection<Product> Products { get; }
    public IReadOnlyCollection<Response> Responses => _responses;

    private Game(IReadOnlyCollection<Product> products)
    {
        _responses = new List<Response>();
        Products = products;
    }
    public void AddResponse(Response response)
    {
        ArgumentNullException.ThrowIfNull(response);
        
        if (_responses.Count >= MaximumAnswersQuantity)
        {
            throw new ToManyAnswersException($"Number of responses should be {MaximumAnswersQuantity}");
        }
        
        _responses.Add(response);    
    }
    
    public static Game NewGame(IEnumerable<RossmannProduct> products)
    {
        return new Game(products.Select(x => new Product(x)).ToList());
    }
}