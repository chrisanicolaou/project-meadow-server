using MeadowShared.CardEngine.Enums;

namespace MeadowServer.CardEngine.Components.Filters;

public class EnemyFightersInPlayFilter : AggregateFilter
{
    public override List<CardFilter> Filters { get; set; } =
    [
        new ZoneFilter(PlayerTarget.Opponent, ZoneType.Battlefield),
        new CardTypeFilter([CardType.Fighter])
    ];
}