using MeadowServer.CardEngine.Components.Abilities;
using MeadowServer.CardEngine.Components.Filters;
using MeadowShared.CardEngine.Enums;
using MeadowShared.CardEngine.Models;
using MeadowShared.Match;

namespace MeadowServer.CardEngine.Components.Effects;

public abstract class Effect : ICardComponent
{
    public virtual List<Filter>? Filters { get; set; }
    public virtual bool TargetTriggerSource { get; set; }
    public virtual SelectionMode SelectionMode { get; set; }
    public virtual int SelectionAmount { get; set; }
    public virtual void OnCreate(ICardComponentModel model)
    {
    }

    public abstract List<object> GetValidTargetsUntyped(MatchState gameState, Ability parentAbility);
    
    public abstract void ApplyUntyped(MatchState gameState, List<object> targets);
}