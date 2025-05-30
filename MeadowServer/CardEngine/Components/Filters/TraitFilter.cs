using MeadowServer.CardEngine.Components.Abilities;
using MeadowServer.CardEngine.Entities;
using MeadowServer.ToBeMoved.Match.GameEvents;
using MeadowShared.CardEngine.Enums;
using MeadowShared.Match;

namespace MeadowServer.CardEngine.Components.Filters;

public class TraitFilter(Trait traits) : CardFilter
{
    public Trait Traits { get; set; } = traits;

    public override bool Match(GameEvent gameEvent, Ability parentAbility)
    {
        throw new NotImplementedException();
    }

    public override List<CardEntity> GetValidTargets(MatchState state, Ability parentAbility)
    {
        throw new NotImplementedException();
    }

    public override bool Match(CardEntity cardModel, Ability parentAbility)
    {
        throw new NotImplementedException();
    }
}