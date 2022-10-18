using GuessThePrice.Core.Model;
using GuessThePrice.Core.Repositories;

using Marten;

namespace GuessThePrice.Infrastructure.Repositories;

internal class MartenGameRepository : IGameRepository
{
    private IDocumentSession _documentSession;

    public MartenGameRepository(IDocumentSession documentSession)
    {
        _documentSession = documentSession;
    }

    public async Task GetAndUpdate(Guid id, int version, Func<Game, object> handle, CancellationToken ct)
    {
        await _documentSession.Events.WriteToAggregate<Game>(id, version, stream =>
            stream.AppendOne(handle(stream.Aggregate)), ct);
    }
}