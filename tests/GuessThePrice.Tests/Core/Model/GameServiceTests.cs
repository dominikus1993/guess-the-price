using FluentAssertions;

using GuessThePrice.Core.Model;
using GuessThePrice.Core.Services;

namespace GuessThePrice.Tests.Core.Model;

public class GameServiceTests
{
        [Fact]
    public void TestAddResponseWhenProductsListsIsEmpty()
    {
        // Arrange
        var game = Game.Create(new GameStarted(GameId.Create(), PlayerId.Create(), Array.Empty<Product>()));
        // Act
        var result  = game.Apply(new ResponseAdded(new Response(new ProductId(1), new PromotionalPriceResponse(21), DateTime.Now)));
        // Assert
        result.Responses.Should().BeEmpty();
    }
    
    [Fact]
    public void TestAddResponseWhenProductsListsIsNotEmpty()
    {
        // Arrange
        var products =
            new List<RossmannProduct>() { new(1, "", 1.2, 3.2, "xD", ""), new(2, "", 1.2, 3.2, "xD", "") }.Select(x =>
                new Product(x));
        var game = Game.Create(new GameStarted(GameId.Create(), PlayerId.Create(), products.ToArray()));
        game = game.Apply(new ResponseAdded(new Response(new ProductId(1), new PromotionalPriceResponse(21), DateTime.Now)));
        // Act
        var subject = game.Apply(new ResponseAdded(new Response(new ProductId(2), new PromotionalPriceResponse(21), DateTime.Now)));
        // Assert
        subject.Responses.Should().NotBeEmpty();
        subject.Responses.Should().HaveCount(2);
    }
    
    [Fact]
    public void TestAddResponseWhenResponsesListWhenTryAddMoreThanPossibleResponses()
    {
        // Arrange
        var products =
            new List<RossmannProduct>() { new(1, "", 1.2, 3.2, "xD", ""), new(2, "", 1.2, 3.2, "xD", "") }.Select(x =>
                new Product(x));
        var game = Game.Create(new GameStarted(GameId.Create(), PlayerId.Create(), products.ToArray()));
        game = game.Apply(new ResponseAdded(new Response(new ProductId(1), new PromotionalPriceResponse(21), DateTime.Now)));
        // Act
        game = game.Apply(new ResponseAdded(new Response(new ProductId(2), new PromotionalPriceResponse(21), DateTime.Now)));
        var subject = game.Apply(new ResponseAdded(new Response(new ProductId(3), new PromotionalPriceResponse(21), DateTime.Now)));
        // Assert
        subject.Responses.Should().NotBeEmpty();
        subject.Responses.Should().HaveCount(2);
    }
    
    [Fact]
    public void TestAddResponseWhenResponsesListWhenProductsNotExists()
    {
        // Arrange
        var products =
            new List<RossmannProduct>() { new(1, "", 1.2, 3.2, "xD", ""), new(2, "", 1.2, 3.2, "xD", "") }.Select(x =>
                new Product(x));
        var game = Game.Create(new GameStarted(GameId.Create(), PlayerId.Create(), products.ToArray()));
        game = game.Apply(new ResponseAdded(new Response(new ProductId(1), new PromotionalPriceResponse(21), DateTime.Now)));
        // Act
        var subject = game.Apply(new ResponseAdded(new Response(new ProductId(3), new PromotionalPriceResponse(21), DateTime.Now)));
        // Assert
        subject.Responses.Should().NotBeEmpty();
        subject.Responses.Should().HaveCount(1);
    }
    
    [Fact]
    public void TestAddResponseWhenResponsesListWhenResponseExists()
    {
        // Arrange
        var products =
            new List<RossmannProduct>() { new(1, "", 1.2, 3.2, "xD", ""), new(2, "", 1.2, 3.2, "xD", "") }.Select(x =>
                new Product(x));
        var game = Game.Create(new GameStarted(GameId.Create(), PlayerId.Create(), products.ToArray()));
        var priceResponse = new PromotionalPriceResponse(21);
        game = game.Apply(new ResponseAdded(new Response(new ProductId(1), priceResponse, DateTime.Now)));
        // Act
        var subject =  game.Apply(new ResponseAdded(new Response(new ProductId(1), new PromotionalPriceResponse(212), DateTime.Now)));
        // Assert
        subject.Responses.Should().NotBeEmpty();
        subject.Responses.Should().HaveCount(1);
        subject.Responses.Should().Contain(x => x.PromotionalPriceResponse == priceResponse);
    }
    
    
    [Fact]
    public void TestCalculateScore()
    {
        // Arrange
        var products =
            new List<RossmannProduct>() { new(1, "", 1.2, 3.2, "xD", ""), new(2, "", 1.2, 3.2, "xD", "") }.Select(x =>
                new Product(x));
        var game = Game.Create(new GameStarted(GameId.Create(), PlayerId.Create(), products.ToArray()));
        game = game.Apply(new ResponseAdded(new Response(new ProductId(1), new PromotionalPriceResponse(3.2), DateTime.Now)));
        game = game.Apply(new ResponseAdded(new Response(new ProductId(2), new PromotionalPriceResponse(3.2), DateTime.Now)));
        // Act
        var subject = game.CalculateScore();
        // Assert
        subject.Value.Should().Be(2);
    }
    
    [Fact]
    public void TestCalculateScore2()
    {
        // Arrange
        var products =
            new List<RossmannProduct>() { new(1, "", 1.2, 3, "xD", ""), new(2, "", 1.2, 3, "xD", "") }.Select(x =>
                new Product(x));
        var game = Game.Create(new GameStarted(GameId.Create(), PlayerId.Create(), products.ToArray()));
        game = game.Apply(new ResponseAdded(new Response(new ProductId(1), new PromotionalPriceResponse(3.6), DateTime.Now)));
        game = game.Apply(new ResponseAdded(new Response(new ProductId(2), new PromotionalPriceResponse(2.4), DateTime.Now)));
        // Act
        var subject = game.CalculateScore();
        // Assert
        subject.Value.Should().Be(1);
    }
}