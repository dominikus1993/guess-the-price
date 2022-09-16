using System.Net.Http.Json;

using FluentAssertions;

using GuessThePrice.Core.Model;
using GuessThePrice.WebApp.Requests;
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
    
    [Fact]
    public async Task GetGameWhenAddOneResponse()
    {

        var gameId = Guid.NewGuid();

        await API
            .Given(URI($"/game/{gameId}"))
            .When(POST)
            .Then(OK);

        var resp = await API
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

        var game = await resp.Content.ReadFromJsonAsync<GameResponse>();
        await API
            .Given(URI($"/game/{gameId}/responses"), BODY(new AddResponseRequest(game.Products.First().Id, new PromotionalPriceResponseRequest(1.2))))
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
                    game.Responses.Should().NotBeEmpty();
                    game.Responses.Should().HaveCount(1);
                    game.IsInitialized.Should().BeTrue();
                    game.IsFinished.Should().BeFalse();
                })
            );
    }
    
    [Fact]
    public async Task GetGameWhenAddAllResponses()
    {

        var gameId = Guid.NewGuid();

        await API
            .Given(URI($"/game/{gameId}"))
            .When(POST)
            .Then(OK);

        var resp = await API
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

        var game = await resp.Content.ReadFromJsonAsync<GameResponse>();

        foreach (var product in game!.Products)
        {
            await API
                .Given(URI($"/game/{gameId}/responses"), BODY(new AddResponseRequest(product.Id, new PromotionalPriceResponseRequest(product.PromotionalPrice))))
                .When(POST)
                .Then(OK);
        }

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
                    game.Responses.Should().NotBeEmpty();
                    game.Responses.Should().HaveCount(5);
                    game.IsInitialized.Should().BeTrue();
                    game.IsFinished.Should().BeTrue();
                })
            );
        
        await API
            .Given(
                URI($"/game/{gameId}/score")
            )
            .When(GET)
            .Then(
                OK,
                RESPONSE_BODY<ScoreResponse>(game =>
                {
                    game.Value.Should().Be(5);
                })
            );
    }
}