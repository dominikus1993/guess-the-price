using FluentAssertions;

using GuessThePrice.Core.Grains;
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
}