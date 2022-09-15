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

public readonly record struct PromotionalPriceResponse(double Value);

public readonly record struct Score(double Value);

public record Response(ProductId ProductId, PromotionalPriceResponse PromotionalPriceResponse);

public readonly record struct ProductId(int Value);

public sealed class Game
{
    private const int MaximumAnswersQuantity = 5;
    private List<Response> _responses;
    public IReadOnlyCollection<Product> Products { get; }
    public IReadOnlyCollection<Response> Responses => _responses;

    public bool IsInitialized => Products.Count > 0;
    public bool IsFinished => Responses.Count == Products.Count;

    public Game()
    {
        Products = Array.Empty<Product>();
        _responses = new List<Response>();
    }

    private Game(IReadOnlyCollection<Product> products)
    {
        _responses = new List<Response>();
        Products = products;
    }

    public Either<Exception, Unit> AddResponse(Response response)
    {
        ArgumentNullException.ThrowIfNull(response);
        return TryAddResponse(response);
    }

    public static Game NewGame(IEnumerable<RossmannProduct> products)
    {
        return new Game(products.Select(x => new Product(x)).ToList());
    }

    public Score CalculateScore()
    {
        var score = _responses.Join(Products, response => response.ProductId, product1 => product1.Id,
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

    private Either<Exception, Unit> TryAddResponse(Response response)
    {
        if (!IsInitialized)
        {
            return Left<Exception>(new GameIsNotStartedException("Game is not started"));
        }

        if (IsFinished)
        {
            return Left<Exception>(new GameFinishedException("game is finished"));
        }

        var responseExists = this.Responses.Any(x => x.ProductId == response.ProductId);
        if (responseExists)
        {
            return Left<Exception>(new ResponseExistsException($"response of id: {response.ProductId} exists"));
        }

        var productExists = this.Products.Any(x => x.Id == response.ProductId);

        if (productExists)
        {
            _responses.Add(response);
            return Right<Unit>(Unit.Default);
        }

        return Left<Exception>(new ProductsNotExistsException($"Product of id: {response.ProductId} not exists"));
    }
}