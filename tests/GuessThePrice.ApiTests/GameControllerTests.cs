using FluentAssertions;

using GuessThePrice.Core.Model;
using GuessThePrice.WebApp.Responses;

using Microsoft.VisualStudio.TestPlatform.TestHost;

using Ogooreck.API;

using static Ogooreck.API.ApiSpecification;

namespace GuessThePrice.ApiTests;

public class GameControllerTests : IClassFixture<ApiSpecification<Program>>
{
    private readonly ApiSpecification<Program> API;

    public GameControllerTests(ApiSpecification<Program> api) => API = api;

    [Fact]
    public async Task GetGameWhenIsStarted()
    {
        var gameId = Guid.NewGuid();

        await API
            .Given(URI($"/game/{gameId}"))
            .When(POST)
            .Then(OK);

        await API
            .Given(
                URI($"/game/{gameId}")
            )
            .When(GET)
            .Then(
                OK,
                RESPONSE_BODY<GameResponse>(game =>
                {
                    game.Products.Should().NotBeEmpty();
                    game.Responses.Should().BeEmpty();
                    game.IsInitialized.Should().BeTrue();
                    game.IsFinished.Should().BeFalse();
                })
            );
    }
    
    [Fact]
    public async Task GetGameWhenIsNotStarted()
    {
        var gameId = Guid.NewGuid();

        await API
            .Given(
                URI($"/game/{gameId}")
            )
            .When(GET)
            .Then(
                OK,
                RESPONSE_BODY<GameResponse>(game =>
                {
                    game.Products.Should().BeEmpty();
                    game.Responses.Should().BeEmpty();
                    game.IsInitialized.Should().BeFalse();
                })
            );
    }
}