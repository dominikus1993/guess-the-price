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
        var gameStarted = GameService.Handle(new StartGame(PlayerId.Create(), Array.Empty<Product>()));
        var game = Game.Create(gameStarted);
        // Act
        var subject = Assert.Throws<InvalidOperationException>(() => GameService.Handle(game,
            new AddResponse(new Response(new ProductId(1), new PromotionalPriceResponse(21), DateTime.Now))));

        subject.Message.Should().Be("Product not exists");
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
        var subject = game.Apply(GameService.Handle(game, new AddResponse(new Response(new ProductId(2), new PromotionalPriceResponse(21), DateTime.Now))));
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
        var subject = Assert.Throws<InvalidOperationException>(() => GameService.Handle(game,
            new AddResponse(new Response(new ProductId(1), new PromotionalPriceResponse(21), DateTime.Now))));
        
        // Assert
        subject.Message.Should().Be("Game is finished");
        game.Responses.Should().NotBeEmpty();
        game.Responses.Should().HaveCount(2);
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
        var subject = Assert.Throws<InvalidOperationException>(() => GameService.Handle(game,
            new AddResponse(new Response(new ProductId(3), new PromotionalPriceResponse(21), DateTime.Now))));
        // Assert
        subject.Message.Should().Be("Product not exists");
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
        var subject = Assert.Throws<InvalidOperationException>(() => GameService.Handle(game,
            new AddResponse(new Response(new ProductId(1), new PromotionalPriceResponse(21), DateTime.Now))));
        // Assert
        subject.Message.Should().Be("Response already exists");
    }
}