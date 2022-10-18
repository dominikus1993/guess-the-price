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

        return new ResponseAdded(cmd.Response);
    }
}