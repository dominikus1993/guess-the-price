namespace GuessThePrice.Tests.Core.Grains;
using Orleans;
using Orleans.TestingHost;

public class GameGrainTests
{
    [Fact]
    public Task TestStartNewGame()
    {
        var builder = new TestClusterBuilder();
        var cluster = builder.Build();
        cluster.Deploy();
    }
}