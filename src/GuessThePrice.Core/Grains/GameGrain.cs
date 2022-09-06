using GuessThePrice.Core.Model;

using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Runtime;

namespace GuessThePrice.Core.Grains;

public class GameGrain  : Grain, IGameGrain
{
    private readonly ILogger<GameGrain> _logger;
    private readonly IPersistentState<Game> _state;

    public GameGrain(
        [PersistentState("games", storageName: "games")] IPersistentState<Game> state,
        ILogger<GameGrain> logger)
    {
        _logger = logger;
        _state = state;
    }
    
    public Task<Game> StartGame()
    {
        throw new NotImplementedException();
    }

    public Task AddResponse(Response response)
    {
        throw new NotImplementedException();
    }

    public Task<Game> GetGame()
    {
        throw new NotImplementedException();
    }
}