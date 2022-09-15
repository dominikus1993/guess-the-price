using FluentAssertions;

using GuessThePrice.Core.Grains;
using GuessThePrice.Core.Model;
using GuessThePrice.Tests.Core.Fixtures;

namespace GuessThePrice.Tests.Core.Grains;
using Orleans;
using Orleans.TestingHost;

public class GameGrainTests : IClassFixture<OrleansGrainFixture>
{
    private OrleansGrainFixture _fixture;

    public GameGrainTests(OrleansGrainFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task TestStartNewGame()
    {
        var id = Guid.NewGuid();
        var grain = _fixture.Cluster.GrainFactory.GetGrain<IGameGrain>(id);

        var game = await grain.StartGame();

        game.IsInitialized.Should().BeTrue();
    }
    
    
    [Fact]
    public async Task TestGetGameWhenIsNotInitialized()
    {
        var id = Guid.NewGuid();
        var grain = _fixture.Cluster.GrainFactory.GetGrain<IGameGrain>(id);

        var game = await grain.GetGame();

        game.IsInitialized.Should().BeFalse();
    }
    
    [Fact]
    public async Task TestGetGameWhenIsInitialized()
    {
        var id = Guid.NewGuid();
        var grain = _fixture.Cluster.GrainFactory.GetGrain<IGameGrain>(id);
        await grain.StartGame();
        var game = await grain.GetGame();

        game.IsInitialized.Should().BeTrue();
    }
    
            
    [Fact]
    public async Task TestAddOneResponseWhenGameIsNotInitialized()
    {
        var id = Guid.NewGuid();
        var grain = _fixture.Cluster.GrainFactory.GetGrain<IGameGrain>(id);
        await grain.AddResponse(new Response(new ProductId(1), new PromotionalPriceResponse(2)));
        var subject = await grain.GetGame();
        subject.IsInitialized.Should().BeFalse();
        subject.Responses.Should().BeEmpty();
        subject.Products.Should().BeEmpty();
    }
        
    [Fact]
    public async Task TestAddOneResponseWhenGameIsInitialized()
    {
        var id = Guid.NewGuid();
        var grain = _fixture.Cluster.GrainFactory.GetGrain<IGameGrain>(id);
        var game = await grain.StartGame();
        await grain.AddResponse(new Response(game.Products.First().Id, new PromotionalPriceResponse(2)));
        var subject = await grain.GetGame();
        subject.IsInitialized.Should().BeTrue();
        subject.Responses.Should().NotBeEmpty();
        subject.Responses.Should().HaveCount(1);
        subject.Products.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task TestAddMoreResponsesThanProductsResponseWhenGameIsInitialized()
    {
        var id = Guid.NewGuid();
        var grain = _fixture.Cluster.GrainFactory.GetGrain<IGameGrain>(id);
        var game = await grain.StartGame();
        await grain.AddResponse(new Response(game.Products.First().Id, new PromotionalPriceResponse(2)));
        await grain.AddResponse(new Response(game.Products.First().Id, new PromotionalPriceResponse(2)));
        var subject = await grain.GetGame();
        subject.IsInitialized.Should().BeTrue();
        subject.Responses.Should().NotBeEmpty();
        subject.Responses.Should().HaveCount(1);
        subject.Products.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task TestAddResponseForGameThatNotExists()
    {
        var id = Guid.NewGuid();
        var grain = _fixture.Cluster.GrainFactory.GetGrain<IGameGrain>(id);
        var game = await grain.StartGame();
        await grain.AddResponse(new Response(new ProductId(2137), new PromotionalPriceResponse(2)));
        var subject = await grain.GetGame();
        subject.IsInitialized.Should().BeTrue();
        subject.Responses.Should().BeEmpty();
        subject.Products.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task TestCalculateScore()
    {
        // Arrange
        var id = Guid.NewGuid();
        var grain = _fixture.Cluster.GrainFactory.GetGrain<IGameGrain>(id);
        var game = await grain.StartGame();
        await grain.AddResponse(new Response(new ProductId(1), new PromotionalPriceResponse(4.1)));

        // Act
        var subject = await grain.GetGameScore();
        // Assert
        subject.Value.Should().Be(1);
    }
}