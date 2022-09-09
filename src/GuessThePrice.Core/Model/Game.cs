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

public class Response
{
    
}
public sealed class Game
{
    public IReadOnlyCollection<Product> Products { get; set; }
    public IReadOnlyCollection<Response> Responses { get; set; }

    public void AddResponse(Response response)
    {
        
    }
    public static Game NewGame(IReadOnlyCollection<RossmannProduct> products)
    {
        return new Game()
        {
            Products = products.Select(x => new Product(x)).ToList(), Responses = new List<Response>()
        };
    }
}