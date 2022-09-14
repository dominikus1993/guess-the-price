using FluentAssertions;

using GuessThePrice.Core.Model;
using GuessThePrice.Core.Model.Exceptions;
using GuessThePrice.Core.Services;

using LanguageExt;

namespace GuessThePrice.Tests.Core.Model;

public class GameTests
{
    [Fact]
    public void TestAddResponseWhenProductsListsIsEmpty()
    {
        // Arrange
        var game = Game.NewGame(new List<RossmannProduct>());
        // Act
        var result  = game.AddResponse(new Response(new ProductId(1), new PromotionalPriceResponse(21)));
        // Assert
        result.IsLeft.Should().BeTrue();
        var exc = result.IfRight(() => throw new Exception("Should be left"));
        exc.Should().BeOfType<GameIsNotStartedException>();
    }
    
    [Fact]
    public void TestAddResponseWhenProductsListsIsNotEmpty()
    {
        // Arrange
        var game = Game.NewGame(new List<RossmannProduct>() { new(1, "", 1.2, 3.2, "xD", ""), new(2, "", 1.2, 3.2, "xD", "")});
        game.AddResponse(new Response(new ProductId(1), new PromotionalPriceResponse(21)));
        // Act
        var subject = game.AddResponse(new Response(new ProductId(2), new PromotionalPriceResponse(21)));
        // Assert
        subject.IsRight.Should().BeTrue();
        game.Responses.Should().NotBeEmpty();
        game.Responses.Should().HaveCount(2);
    }
    
    [Fact]
    public void TestAddResponseWhenResponsesListWhenTryAddMoreThanPossibleResponses()
    {
        // Arrange
        var game = Game.NewGame(new List<RossmannProduct>() { new(1, "", 1.2, 3.2, "xD", ""), new(2, "", 1.2, 3.2, "xD", "")});
        game.AddResponse(new Response(new ProductId(1), new PromotionalPriceResponse(21)));
        // Act
        game.AddResponse(new Response(new ProductId(2), new PromotionalPriceResponse(21)));
        var subject = game.AddResponse(new Response(new ProductId(3), new PromotionalPriceResponse(21)));
        // Assert
        subject.IsLeft.Should().BeTrue();
        subject.IfRight(() => throw new Exception("Should be left")).Should().BeOfType<GameFinishedException>();
        game.Responses.Should().NotBeEmpty();
        game.Responses.Should().HaveCount(2);
    }
    
    [Fact]
    public void TestAddResponseWhenResponsesListWhenProductsNotExists()
    {
        // Arrange
        var game = Game.NewGame(new List<RossmannProduct>() { new(1, "", 1.2, 3.2, "xD", ""), new(2, "", 1.2, 3.2, "xD", "")});
        game.AddResponse(new Response(new ProductId(1), new PromotionalPriceResponse(21)));
        // Act
        var subject = game.AddResponse(new Response(new ProductId(3), new PromotionalPriceResponse(21)));
        // Assert
        subject.IsLeft.Should().BeTrue();
        subject.IfRight(() => throw new Exception("Should be left")).Should().BeOfType<ProductsNotExistsException>();
        game.Responses.Should().NotBeEmpty();
        game.Responses.Should().HaveCount(1);
    }
    
    [Fact]
    public void TestAddResponseWhenResponsesListWhenResponseExists()
    {
        // Arrange
        var game = Game.NewGame(new List<RossmannProduct>() { new(1, "", 1.2, 3.2, "xD", ""), new(2, "", 1.2, 3.2, "xD", "")});
        game.AddResponse(new Response(new ProductId(1), new PromotionalPriceResponse(21)));
        // Act
        var subject = game.AddResponse(new Response(new ProductId(1), new PromotionalPriceResponse(21)));
        // Assert
        subject.IsLeft.Should().BeTrue();
        subject.IfRight(() => throw new Exception("Should be left")).Should().BeOfType<ResponseExistsException>();
        game.Responses.Should().NotBeEmpty();
        game.Responses.Should().HaveCount(1);
    }
}