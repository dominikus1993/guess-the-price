namespace GuessThePrice.Core.Model;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public decimal PromotionalPrice { get; set; }
}

public class Response
{
    
}
public class Game
{
    public Guid GameId { get; set; }
    public IReadOnlyCollection<Product> Products { get; set; }
    public IReadOnlyCollection<Response> Responses { get; set; }
}