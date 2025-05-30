using MeadowServer.CardEngine.Components.Abilities;
using MeadowServer.ToBeMoved.Match.GameEvents;

namespace MeadowServer.CardEngine.Components.Effects;

public class EffectResolutionRequest
{
    public List<Effect> Effects { get; set; }
    public GameEvent TriggeringEvent { get; set; }
    public Ability ParentAbility { get; set; }
}