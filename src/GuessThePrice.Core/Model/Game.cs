using GuessThePrice.Core.Model.Exceptions;
using GuessThePrice.Core.Services;

using LanguageExt;

using static LanguageExt.Prelude;

using Array = System.Array;

namespace GuessThePrice.Core.Model;

public record struct PlayerId(Guid Value)
{
    public static PlayerId Create()
    {
        return new PlayerId(Guid.NewGuid());
    }
}
public class Product
{
    public ProductId Id { get; init; }
    public string Name { get; init; }
    public string? ImageUrl { get; init; }
    public double Price { get; init; }
    public double PromotionalPrice { get; init; }
    public string NavigateUrl { get; init; }

    public Product()
    {
    }

    public Product(RossmannProduct product)
    {
        Name = product.Name;
        Id = new ProductId(product.Id);
        ImageUrl = product.ImageUrl;
        Price = product.Price;
        PromotionalPrice = product.PromotionalPrice;
        NavigateUrl = product.NavigateUrl;
    }
}

public enum GameState
{
    New = 0,
    Ongoing = 1,
    Finished = 2,
}

public readonly record struct PromotionalPriceResponse(double Value);

public readonly record struct Score(double Value);

public record Response(ProductId ProductId, PromotionalPriceResponse PromotionalPriceResponse, DateTime AddedAt)
{
    public Response(ProductId productId, PromotionalPriceResponse promotionalPriceResponse) : this(productId,
        promotionalPriceResponse, DateTime.UtcNow)
    {
        
    }
}

public readonly record struct ProductId(int Value);

public readonly record struct GameId(Guid Value)
{
    public static GameId Create()
    {
        return new GameId(Guid.NewGuid());
    }
}

public record GameStarted(GameId GameId, PlayerId PlayerId, IReadOnlyCollection<Product> Products)
{
    public GameStarted(GameId id, PlayerId playerId, IReadOnlyCollection<RossmannProduct> products) : this(id, playerId,
        products.Select(x => new Product(x)).ToList())
    {
        
    }
}

public record ResponseAdded(Response Response);

public record Game(GameId GameId, IReadOnlyCollection<Product> Products, IReadOnlyCollection<Response> Responses, GameState State, long Version)
{
    public static Game Create(GameStarted evt)
    {
        return new(evt.GameId, evt.Products, Array.Empty<Response>(), GameState.New, 1);
    }

    public Game Apply(ResponseAdded evt)
    {
        var futureState = Responses.Count == 0 ? GameState.Ongoing : State;
        var responses = new List<Response>(Responses) { evt.Response };
        if (responses.Count >= Products.Count)
        {
            return this with { Responses = responses, State = GameState.Finished };
        }
        return this with { Responses = responses, State = futureState };
    }
    
    public Score CalculateScore()
    {
        var score = Responses.Join(Products, response => response.ProductId, product1 => product1.Id,
            (response, product1) =>
            {
                var difference = Math.Abs(response.PromotionalPriceResponse.Value - product1.PromotionalPrice);
                return difference switch
                {
                    < 0.5 => 1.0,
                    < 1.0 => 0.5,
                    _ => 0
                };
            }).Sum();

        return new Score(score);
    }
}