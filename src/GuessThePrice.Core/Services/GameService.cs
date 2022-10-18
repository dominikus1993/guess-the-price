using GuessThePrice.Core.Model;

namespace GuessThePrice.Core.Services;

public record StartGame(PlayerId PlayerId, IReadOnlyCollection<Product> Products);
public record AddResponse(Response Response);

public class GameService
{
    public static GameStarted Handle(StartGame cmd)
    {
        var (playerId, products) = cmd;
        return new GameStarted(GameId.Create(), playerId, products);
    }

    public static ResponseAdded Handle(Game state, AddResponse cmd)
    {
        if (state.State == GameState.Finished)
        {
            throw new InvalidOperationException("Game is finished");
        }

        var responseExists = state.Responses.Any(x => x.ProductId == cmd.Response.ProductId);
        if (responseExists)
        {
            throw new InvalidOperationException("Response already exists");
        }

        var productExists = state.Products.Any(x => x.Id == cmd.Response.ProductId);

        if (productExists)
        {
            throw new InvalidOperationException("Product not exists");
        }
        return new ResponseAdded(cmd.Response);
    }
}