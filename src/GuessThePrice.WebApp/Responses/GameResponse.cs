using GuessThePrice.Core.Model;

namespace GuessThePrice.WebApp.Responses;

using GuessThePrice.Core.Model.Exceptions;
using GuessThePrice.Core.Services;

using LanguageExt;

using static LanguageExt.Prelude;

using Array = System.Array;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public double Price { get; set; }
    public double PromotionalPrice { get; set; }
    public string NavigateUrl { get; set; }
    
    public static ProductDto FromProduct(Product product)
    {
        return new ProductDto()
        {
            Id = product.Id.Value,
            Name = product.Name,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            NavigateUrl = product.NavigateUrl,
            PromotionalPrice = product.PromotionalPrice,
        };
    }
}

public class ResponseDto
{
    public int ProductId { get; set; }
    public double PromotionalPriceResponse { get; set; }
    
    public static ResponseDto FromResponse(Response response)
    {
        return new ResponseDto()
        {
            ProductId = response.ProductId.Value,
            PromotionalPriceResponse = response.PromotionalPriceResponse.Value,
        };
    }
}

public readonly record struct ProductId(int Value);

public sealed class GameResponse
{
    public IReadOnlyCollection<ProductDto> Products { get; init; }
    public IReadOnlyCollection<ResponseDto> Responses { get; init; }
    public bool IsInitialized { get; set; }
    public bool IsFinished { get; set; }

    public static GameResponse FromGame(Game game)
    {
        return new GameResponse()
        {
            Products = game.Products.OfType<Product>().Select(product => ProductDto.FromProduct(product)).ToList(),
            Responses = game.Responses.Select(response => ResponseDto.FromResponse(response)).ToList(),
            IsFinished = game.IsFinished,
            IsInitialized = game.IsInitialized,
        };
    }
}