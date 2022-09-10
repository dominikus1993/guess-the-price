using FluentAssertions;

using GuessThePrice.Core.Model;
using GuessThePrice.Core.Model.Exceptions;
using GuessThePrice.Core.Services;

namespace GuessThePrice.Tests.Core.Model;

public class GameTests
{
    [Fact]
    public void TestAddResponseWhenResponsesListIsEmpty()
    {
        // Arrange
        var game = Game.NewGame(new List<RossmannProduct>());
        // Act
        game.AddResponse(new Response(new Product() { Id = 1, Name = "xD", Price = 12, PromotionalPrice = 21, ImageUrl = "xd.pl"}, new PromotionalPriceResponse(21)));
        // Assert
        game.Responses.Should().NotBeEmpty();
        game.Responses.Should().HaveCount(1);
    }
    
    [Fact]
    public void TestAddResponseWhenResponsesListIsNotEmpty()
    {
        // Arrange
        var game = Game.NewGame(new List<RossmannProduct>());
        game.AddResponse(new Response(new Product() { Id = 1, Name = "xD", Price = 12, PromotionalPrice = 21, ImageUrl = "xd.pl"}, new PromotionalPriceResponse(21)));
        // Act
        game.AddResponse(new Response(new Product() { Id = 1, Name = "xD", Price = 12, PromotionalPrice = 21, ImageUrl = "xd.pl"}, new PromotionalPriceResponse(21)));
        // Assert
        game.Responses.Should().NotBeEmpty();
        game.Responses.Should().HaveCount(2);
    }
    
    [Fact]
    public void TestAddResponseWhenResponsesListWhenTryAddMoreThanFiveResponses()
    {
        // Arrange
        var game = Game.NewGame(new List<RossmannProduct>());
        // Act
        game.AddResponse(new Response(new Product() { Id = 1, Name = "xD", Price = 12, PromotionalPrice = 21, ImageUrl = "xd.pl"}, new PromotionalPriceResponse(21)));
        game.AddResponse(new Response(new Product() { Id = 1, Name = "xD", Price = 12, PromotionalPrice = 21, ImageUrl = "xd.pl"}, new PromotionalPriceResponse(21)));
        game.AddResponse(new Response(new Product() { Id = 1, Name = "xD", Price = 12, PromotionalPrice = 21, ImageUrl = "xd.pl"}, new PromotionalPriceResponse(21)));
        game.AddResponse(new Response(new Product() { Id = 1, Name = "xD", Price = 12, PromotionalPrice = 21, ImageUrl = "xd.pl"}, new PromotionalPriceResponse(21)));
        game.AddResponse(new Response(new Product() { Id = 1, Name = "xD", Price = 12, PromotionalPrice = 21, ImageUrl = "xd.pl"}, new PromotionalPriceResponse(21)));

        Assert.Throws<ToManyAnswersException>(() =>
            game.AddResponse(new Response(new Product()
            {
                Id = 1,
                Name = "xD",
                Price = 12,
                PromotionalPrice = 21,
                ImageUrl = "xd.pl"
            }, new PromotionalPriceResponse(21))));
    }
}