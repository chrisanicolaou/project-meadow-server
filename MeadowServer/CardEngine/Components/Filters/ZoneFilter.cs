using MeadowServer.CardEngine.Components.Abilities;
using MeadowServer.CardEngine.Entities;
using MeadowServer.ToBeMoved.Match.GameEvents;
using MeadowShared.CardEngine.Enums;
using MeadowShared.Match;

namespace MeadowServer.CardEngine.Components.Filters;

public class ZoneFilter(PlayerTarget targetPlayer, ZoneType zoneTypes) : CardFilter
{
    public PlayerTarget TargetPlayer { get; set; } = targetPlayer;
    public ZoneType ZoneTypes { get; set; } = zoneTypes;

    public override bool Match(GameEvent gameEvent, Ability parentAbility)
    {
        throw new NotImplementedException();
    }

    public override List<CardEntity> GetValidTargets(MatchState state, Ability parentAbility)
    {
        var players = GetPlayersFromTarget(state, parentAbility, TargetPlayer);
        return players.SelectMany(p => p.Zones.Where(z => ZoneTypes.HasFlag(z.ZoneType)).SelectMany(z => z.Cards)).Cast<CardEntity>().ToList();
    }
}