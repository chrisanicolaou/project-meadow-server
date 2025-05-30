using MeadowServer.CardEngine.Entities;
using MeadowShared.Match.Targets;

namespace MeadowServer.ToBeMoved.Match.GameEvents;

public class GameEvent(Player sourcePlayer, CardEntity sourceCard, CardEntity? targetCard = null)
{
    public virtual GameEventType EventType { get; set; }
    public CardEntity? Source { get; set; } = sourceCard;
    public CardEntity? Target { get; set; } = targetCard;
    public Player SourcePlayer { get; } = sourcePlayer;
}