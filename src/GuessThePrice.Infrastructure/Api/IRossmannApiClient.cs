using System.Net.Http.Json;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Refit;

namespace GuessThePrice.Infrastructure.Api;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class Attribute
{
    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("redirectUrl")] public string RedirectUrl { get; set; }
}

public class Data
{
    [JsonPropertyName("products")] public List<Product> Products { get; set; }

    [JsonPropertyName("totalPages")] public int TotalPages { get; set; }

    [JsonPropertyName("totalCount")] public int TotalCount { get; set; }
}

public class Picture
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("mini")] public string Mini { get; set; }

    [JsonPropertyName("medium")] public string Medium { get; set; }

    [JsonPropertyName("small")] public string Small { get; set; }

    [JsonPropertyName("large")] public string Large { get; set; }

    [JsonPropertyName("type")] public int Type { get; set; }

    [JsonPropertyName("alt")] public string Alt { get; set; }

    [JsonPropertyName("side")] public string Side { get; set; }
}

public class Product
{
    [JsonPropertyName("brand")] public string Brand { get; set; }

    [JsonPropertyName("brandId")] public int BrandId { get; set; }

    [JsonPropertyName("caption")] public string Caption { get; set; }

    [JsonPropertyName("unit")] public string Unit { get; set; }

    [JsonPropertyName("averageRating")] public double AverageRating { get; set; }

    [JsonPropertyName("totalReviews")] public int TotalReviews { get; set; }

    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("rossnetId")] public int RossnetId { get; set; }

    [JsonPropertyName("eanNumber")] public List<string> EanNumber { get; set; }

    [JsonPropertyName("navigateUrl")] public string NavigateUrl { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("oldPrice")] public double OldPrice { get; set; }

    [JsonPropertyName("price")] public double Price { get; set; }

    [JsonPropertyName("pricePerUnit")] public string PricePerUnit { get; set; }

    [JsonPropertyName("vat")] public int Vat { get; set; }

    [JsonPropertyName("dimensional")] public int Dimensional { get; set; }

    [JsonPropertyName("pictures")] public List<Picture> Pictures { get; set; }

    [JsonPropertyName("promotion")] public Promotion Promotion { get; set; }

    [JsonPropertyName("promotionFrom")] public DateTime PromotionFrom { get; set; }

    [JsonPropertyName("promotionTo")] public DateTime PromotionTo { get; set; }

    [JsonPropertyName("hasRichContent")] public bool HasRichContent { get; set; }

    [JsonPropertyName("availability")] public string Availability { get; set; }

    [JsonPropertyName("category")] public string Category { get; set; }

    [JsonPropertyName("attributes")] public List<Attribute> Attributes { get; set; }
}

public class Promotion
{
    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("redirectUrl")] public string RedirectUrl { get; set; }
}

public class ApiResult
{
    [JsonPropertyName("data")] public Data Data { get; set; }
}

internal class RossmannApiClient
{
    private HttpClient _httpClient;
    private ILogger<RossmannApiClient> _logger;

    public RossmannApiClient(HttpClient httpClient, ILogger<RossmannApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async IAsyncEnumerable<Product> GetMegaProducts(CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "products/v3/api/Products?ShopNumber=735&PageSize=15&Page=1&Statuses=mega");
        using var response =
            await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ApiResult>(cancellationToken: cancellationToken);

        if (result?.Data?.Products is null or { Count: 0 }) 
        {
            yield break;
        }

        foreach (var product in result.Data.Products)
        {
            yield return product;
        }
    }
}