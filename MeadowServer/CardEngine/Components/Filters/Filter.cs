using MeadowServer.CardEngine.Components.Abilities;
using MeadowServer.ToBeMoved.Match.GameEvents;
using MeadowShared.CardEngine.Enums;
using MeadowShared.CardEngine.Models;
using MeadowShared.Match;
using MeadowShared.Match.Targets;

namespace MeadowServer.CardEngine.Components.Filters;

public abstract class Filter : ICardComponent
{
    public virtual bool MatchOnSource { get; set; } = false;
    public virtual void OnCreate(ICardComponentModel model)
    {
    }

    public abstract bool Match(GameEvent gameEvent, Ability parentAbility);

    protected virtual List<Player> GetPlayersFromTarget(MatchState state, Ability parentAbility, PlayerTarget target)
    {
        return target == PlayerTarget.Self ? [parentAbility.Owner.Owner] : state.Players.Where(p => p != parentAbility.Owner.Owner).ToList();
    }
}