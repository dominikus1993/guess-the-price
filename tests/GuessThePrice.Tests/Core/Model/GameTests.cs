using FluentAssertions;

using GuessThePrice.Core.Model;
using GuessThePrice.Core.Model.Exceptions;
using GuessThePrice.Core.Services;

using LanguageExt;

namespace GuessThePrice.Tests.Core.Model;

public class GameTests
{
    [Fact]
    public void TestApplyResponseAdded()
    {
        // Arrange
        var products =
            new List<RossmannProduct>() { new(1, "", 1.2, 3.2, "xD", ""), new(2, "", 1.2, 3.2, "xD", "") }.Select(x =>
                new Product(x)).ToList();
        var game = Game.Create(new GameStarted(Guid.NewGuid(), Guid.NewGuid(), products));
        // Act
        var result  = game.Apply(new ResponseAdded(new Response(new ProductId(1), new PromotionalPriceResponse(21), DateTime.Now)));
        // Assert
        result.Responses.Should().HaveCount(1);
        result.State.Should().Be(GameState.Ongoing);
    }
    
    [Fact]
    public void TestApplyResponseAddedUntilIsFinished()
    {
        // Arrange
        var products =
            new List<RossmannProduct>() { new(1, "", 1.2, 3.2, "xD", ""), new(2, "", 1.2, 3.2, "xD", "") }.Select(x =>
                new Product(x)).ToList();
        var game = Game.Create(new GameStarted(Guid.NewGuid(), Guid.NewGuid(), products));
        // Act
        game  = game.Apply(new ResponseAdded(new Response(new ProductId(1), new PromotionalPriceResponse(21), DateTime.Now)));
        var result  = game.Apply(new ResponseAdded(new Response(new ProductId(2), new PromotionalPriceResponse(21), DateTime.Now)));
        // Assert
        result.Responses.Should().HaveCount(2);
        result.State.Should().Be(GameState.Finished);
    }

    [Fact]
    public void TestCalculateScore()
    {
        // Arrange
        var products =
            new List<RossmannProduct>() { new(1, "", 1.2, 3.2, "xD", ""), new(2, "", 1.2, 3.2, "xD", "") }.Select(x =>
                new Product(x));
        var game = Game.Create(new GameStarted(Guid.NewGuid(), Guid.NewGuid(), products.ToArray()));
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
        var game = Game.Create(new GameStarted(Guid.NewGuid(), Guid.NewGuid(), products.ToArray()));
        game = game.Apply(new ResponseAdded(new Response(new ProductId(1), new PromotionalPriceResponse(3.6), DateTime.Now)));
        game = game.Apply(new ResponseAdded(new Response(new ProductId(2), new PromotionalPriceResponse(2.4), DateTime.Now)));
        // Act
        var subject = game.CalculateScore();
        // Assert
        subject.Value.Should().Be(1);
    }
}