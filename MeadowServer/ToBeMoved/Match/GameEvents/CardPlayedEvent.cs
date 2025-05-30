using MeadowServer.CardEngine.Entities;
using MeadowShared.Match.Targets;

namespace MeadowServer.ToBeMoved.Match.GameEvents;

public class CardPlayedEvent(Player sourcePlayer, CardEntity sourceCard) : GameEvent(sourcePlayer, sourceCard)
{
    public override GameEventType EventType => GameEventType.CardPlayed;
}