using GuessThePrice.Core.Model;

using Marten.Events.Aggregation;

namespace GuessThePrice.Infrastructure.Projections;

internal sealed class GameShortInfoProjection : SingleStreamAggregation<GameShortInfo>
{
    public static GameShortInfo Create(GameStarted started) =>
        new(started.PlayerId, started.GameId, );
}