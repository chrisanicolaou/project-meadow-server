using MeadowServer.CardEngine.Components.Abilities;
using MeadowServer.CardEngine.Entities;
using MeadowServer.ToBeMoved.Match.GameEvents;
using MeadowShared.Match;

namespace MeadowServer.CardEngine.Components.Filters;

public class AggregateFilter : CardFilter
{
    public virtual List<CardFilter> Filters { get; set; } = [];

    public override bool Match(GameEvent gameEvent, Ability parentAbility)
    {
        throw new NotImplementedException();
    }

    public override List<CardEntity> GetValidTargets(MatchState state, Ability parentAbility)
    {
        if (Filters.Count == 0)
            return [];

        var baseSet = Filters[0].GetValidTargets(state, parentAbility);

        var remainingFilters = Filters.Skip(1);

        return baseSet
            .Where(card => remainingFilters.All(f => f.Match(card, parentAbility)))
            .ToList();
    }
}