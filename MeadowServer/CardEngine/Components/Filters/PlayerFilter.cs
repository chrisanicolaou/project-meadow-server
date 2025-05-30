using MeadowServer.CardEngine.Components.Abilities;
using MeadowServer.ToBeMoved.Match.GameEvents;
using MeadowShared.CardEngine.Enums;
using MeadowShared.CardEngine.Models;
using MeadowShared.Match;
using MeadowShared.Match.Targets;

namespace MeadowServer.CardEngine.Components.Filters;

public class PlayerFilter : Filter
{
    public List<PlayerTarget>? Targets { get; set; }

    public override void OnCreate(ICardComponentModel model)
    {
        Targets = (model as FilterModel)?.Value?.Select(v => Enum.Parse<PlayerTarget>(v)).ToList() ?? [];
    }

    public override bool Match(GameEvent gameEvent, Ability parentAbility)
    {
        var sourceAsTarget = gameEvent.SourcePlayer.Id == parentAbility.Owner.Owner.Id
            ? PlayerTarget.Self
            : PlayerTarget.Opponent;
        return Targets?.Contains(sourceAsTarget) ?? false;
    }

    public virtual List<Player> GetValidTargets(MatchState state, Ability parentAbility)
    {
        return Targets?.SelectMany(t => GetPlayersFromTarget(state, parentAbility, t))?.ToList() ?? [];
    }
}