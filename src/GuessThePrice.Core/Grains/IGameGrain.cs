using GuessThePrice.Core.Model;

using Orleans;

namespace GuessThePrice.Core.Grains;

public interface IGameGrain : IGrainWithGuidKey
{
    Task<Game> StartGame();
    Task AddResponse(Response response);
    Task<Game> GetGame();
}