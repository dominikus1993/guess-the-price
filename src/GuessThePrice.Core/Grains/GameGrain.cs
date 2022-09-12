using GuessThePrice.Core.Model;
using GuessThePrice.Core.Services;

using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Runtime;

namespace GuessThePrice.Core.Grains;

public class GameGrain : Grain, IGameGrain
{
    private readonly ILogger<GameGrain> _logger;
    private readonly IPersistentState<Game> _state;
    private readonly IProductsDataProvider _productsDataProvider;

    public GameGrain(
        [PersistentState("games", storageName: "games")]
        IPersistentState<Game> state,
        ILogger<GameGrain> logger, IProductsDataProvider productsDataProvider)
    {
        _logger = logger;
        _productsDataProvider = productsDataProvider;
        _state = state;
    }

    public async Task<Game> StartGame()
    {
        if (_state.State.IsEmpty)
        {
            var products = await _productsDataProvider.GetRandomPromotionalProducts(5).ToListAsync();

            var game = Game.NewGame(products);
            _state.State = game;
            await _state.WriteStateAsync();
            return game;
        }

        return _state.State;
    }

    public async Task AddResponse(Response response)
    {
        if (CanAddResponse(response))
        {
            _state.State.AddResponse(response);
            await _state.WriteStateAsync();
        }
    }

    public Task<Game> GetGame()
    {
        return Task.FromResult<Game>(_state.State);
    }

    private bool CanAddResponse(Response response)
    {
        var isEmpty = _state.State.IsEmpty;
        if (isEmpty)
        {
            return false;
        }

        var responseExists = _state.State.Responses.Any(x => x.ProductId == response.ProductId);
        if (responseExists)
        {
            return false;
        }

        var productExists = _state.State.Products.Any(x => x.Id == response.ProductId);

        return productExists;
    }
}