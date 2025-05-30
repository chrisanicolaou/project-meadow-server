using MeadowServer.CardEngine.Components.Effects;
using MeadowServer.CardEngine.Components.Triggers;
using MeadowServer.CardEngine.Entities;
using MeadowServer.ToBeMoved.Match;
using MeadowServer.ToBeMoved.Match.GameEvents;
using MeadowShared.Match;

namespace MeadowServer.CardEngine.Components.Abilities;

public class TriggeredAbility : Ability
{
    public TriggeredAbility()
    {
    }

    public TriggeredAbility(CardEntity owner, Trigger trigger) : base(owner)
    {
        Trigger = trigger;
    }

    public bool ActivatesWhenPlayed => Trigger is WhenPlayed;

    public Trigger Trigger { get; set; }

    public void Register(GameEventBus eventBus)
    {
        eventBus.Subscribe(Trigger.EventType, OnEventTriggered);
    }

    public EffectResolutionRequest? OnEventTriggered(GameEvent gameEvent, MatchState gameState)
    {
        if (!Trigger.IsValid(gameEvent, gameState, this)) return null;
        Trigger.HasTriggered = true;

        return new EffectResolutionRequest { Effects = Effects.Select(e => e).ToList(), ParentAbility = this };
    }
}