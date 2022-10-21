using GuessThePrice.Core.Model;

namespace GuessThePrice.Core.Repositories;

public interface IGameRepository
{
    Task Add(Guid id, object @event, CancellationToken ct);
    Task GetAndUpdate(Guid id, int version,
        Func<Game, object> handle, CancellationToken ct);
}