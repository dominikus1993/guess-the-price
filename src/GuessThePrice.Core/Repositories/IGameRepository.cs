using GuessThePrice.Core.Model;

namespace GuessThePrice.Core.Repositories;

public interface IGameRepository
{
    Task GetAndUpdate(Guid id, int version,
        Func<Game, object> handle, CancellationToken ct);
}