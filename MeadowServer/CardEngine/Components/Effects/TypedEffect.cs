using MeadowServer.CardEngine.Components.Abilities;
using MeadowServer.CardEngine.Components.Filters;
using MeadowServer.CardEngine.Entities;
using MeadowShared.CardEngine.Enums;
using MeadowShared.Match;
using MeadowShared.Match.Targets;

namespace MeadowServer.CardEngine.Components.Effects;

public abstract class TypedEffect<T> : Effect where T : ITargetable
{
    public abstract void Apply(MatchState gameState, List<T> targets);
    public virtual List<T> GetValidTargets(MatchState gameState, Ability parentAbility)
    {
        if (Filters == null || Filters.Count == 0)
            return [];
        
        if (typeof(T) == typeof(CardEntity))
        {
            var aggregateFilter = new AggregateFilter { Filters = Filters.Cast<CardFilter>().ToList() };
            return aggregateFilter.GetValidTargets(gameState, parentAbility).Cast<T>().ToList();
        }
        else
        {
            return Filters.Cast<PlayerFilter>().SelectMany(f => f.GetValidTargets(gameState, parentAbility)).Cast<T>().ToList();
        }
    }
    
    protected virtual List<Player> GetPlayersFromTarget(MatchState state, Ability parentAbility, PlayerTarget target)
    {
        return target == PlayerTarget.Self ? [parentAbility.Owner.Owner] : state.Players.Where(p => p != parentAbility.Owner.Owner).ToList();
    }

    public override List<object> GetValidTargetsUntyped(MatchState gameState, Ability parentAbility) =>
        GetValidTargets(gameState, parentAbility).Cast<object>().ToList();

    public override void ApplyUntyped(MatchState gameState, List<object> targets) =>
        Apply(gameState, targets.Cast<T>().ToList());
}