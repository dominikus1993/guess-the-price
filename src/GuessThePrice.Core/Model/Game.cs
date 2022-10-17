using GuessThePrice.Core.Model.Exceptions;
using GuessThePrice.Core.Services;

using LanguageExt;

using static LanguageExt.Prelude;

using Array = System.Array;

namespace GuessThePrice.Core.Model;

public class Product
{
    public ProductId Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public double Price { get; set; }
    public double PromotionalPrice { get; set; }
    public string NavigateUrl { get; set; }

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

public record Response(ProductId ProductId, PromotionalPriceResponse PromotionalPriceResponse, DateTime AddedAt);

public readonly record struct ProductId(int Value);

public readonly record struct GameId(Guid Value)
{
    public static GameId Create()
    {
        return new GameId(Guid.NewGuid());
    }
}

public record GameStarted(GameId GameId, IReadOnlyCollection<Product> Products);

public record ResponseAdded(Response Response);

public record Game(GameId GameId, IReadOnlyCollection<Product> Products, IReadOnlyCollection<Response> Responses, GameState State)
{
    public static Game Create(GameStarted evt)
    {
        return new(evt.GameId, evt.Products, Array.Empty<Response>(), GameState.New);
    }

    public Game Apply(ResponseAdded evt)
    {
        if (State == GameState.Finished)
        {
            return this;
        }
        
        var responseExists = this.Responses.Any(x => x.ProductId == evt.Response.ProductId);
        if (responseExists)
        {
            return this;
        }

        var productExists = this.Products.Any(x => x.Id == evt.Response.ProductId);

        if (productExists)
        {
            var responses = new List<Response>(Responses) { evt.Response };
            return this with { Responses = responses };
        }

        return this;
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