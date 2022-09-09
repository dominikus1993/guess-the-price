using FluentAssertions;

using GuessThePrice.Core.Model;
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
        game.AddResponse(new Response());
        // Assert
        game.Responses.Should().NotBeEmpty();
        game.Responses.Should().HaveCount(1);
    }
}