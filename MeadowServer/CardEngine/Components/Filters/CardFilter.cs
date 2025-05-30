using MeadowServer.CardEngine.Components.Abilities;
using MeadowServer.CardEngine.Entities;
using MeadowShared.CardEngine.Models;
using MeadowShared.Match;

namespace MeadowServer.CardEngine.Components.Filters;

public abstract class CardFilter : Filter
{
    public override bool MatchOnSource { get; set; } = false;
    public override void OnCreate(ICardComponentModel model)
    {
    }

    public abstract List<CardEntity> GetValidTargets(MatchState state, Ability parentAbility);

    public virtual bool Match(CardEntity cardModel, Ability parentAbility)
    {
        return false;
    }
}