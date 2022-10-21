using GuessThePrice.Core.Model;

namespace GuessThePrice.Core.Services;

public record StartGame(Guid PlayerId, IReadOnlyCollection<Product> Products);
public record AddResponse(Response Response);

public static class GameService
{
    public static GameStarted Handle(StartGame cmd)
    {
        var (playerId, products) = cmd;
        return new GameStarted(Guid.NewGuid(), playerId, products);
    }

    public static ResponseAdded Handle(Game state, AddResponse cmd)
    {
        var (_, products, responses, gameState, _) = state;
        if (gameState == GameState.Finished)
        {
            throw new InvalidOperationException("Game is finished");
        }

        var responseExists = responses.Any(x => x.ProductId == cmd.Response.ProductId);
        if (responseExists)
        {
            throw new InvalidOperationException("Response already exists");
        }

        var productExists = products.Any(x => x.Id == cmd.Response.ProductId);

        if (!productExists)
        {
            throw new InvalidOperationException("Product not exists");
        }
        return new ResponseAdded(cmd.Response);
    }
}