using MeadowServer.CardEngine.Components.Abilities;
using MeadowServer.ToBeMoved.Match;
using MeadowServer.ToBeMoved.Match.GameEvents;
using MeadowShared.Match;

namespace MeadowServer.CardEngine.Components.Triggers;

public class WhenPlayed : Trigger
{
    public override GameEventType EventType { get; set; } = GameEventType.CardPlayed;
    
    public override bool IsValid(GameEvent gameEvent, MatchState gameState, Ability parentAbility)
    {
        if (!base.IsValid(gameEvent, gameState, parentAbility)) return false;
        
        if (gameEvent is CardPlayedEvent cardPlayedEvent)
        {
            return cardPlayedEvent.Source == parentAbility.Owner;
        }
        
        return false;
    }
}