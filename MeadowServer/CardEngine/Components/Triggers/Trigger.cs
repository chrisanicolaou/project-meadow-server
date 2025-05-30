using MeadowServer.CardEngine.Components.Abilities;
using MeadowServer.CardEngine.Components.Filters;
using MeadowServer.ToBeMoved.Match;
using MeadowServer.ToBeMoved.Match.GameEvents;
using MeadowShared.CardEngine.Enums;
using MeadowShared.CardEngine.Models;
using MeadowShared.Match;

namespace MeadowServer.CardEngine.Components.Triggers;

public class Trigger : ICardComponent
{
    public virtual GameEventType EventType { get; set; }
    public virtual TriggerZone TriggerZones { get; set; }
    public virtual bool OneShot { get; set; } = true;
    public virtual List<Filter>? Filters { get; set; }
    public bool HasTriggered { get; set; }

    public virtual bool IsValid(GameEvent gameEvent, MatchState gameState, Ability parentAbility)
    {
        if (Filters?.Count > 0)
        {
            if (Filters.Any(t => !t.Match(gameEvent, parentAbility))) return false;
        }
        
        return !(OneShot && HasTriggered);
    }

    public virtual void OnCreate(ICardComponentModel model)
    {
    }
}