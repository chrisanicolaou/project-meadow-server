using MeadowServer.CardEngine.Components.Abilities;
using MeadowServer.ToBeMoved.Match;
using MeadowShared.CardEngine.Enums;
using MeadowShared.Match.Targets;

namespace MeadowServer.CardEngine.Entities;

public class CardEntity : ITargetable
{
    public CardClass Class { get; set; }
    public CardType CardType { get; set; }
    public required string Name { get; set; }

    public string? EffectDescription { get; set; }
    public string? FlavorText { get; set; }
    
    public int FocusCost { get; set; }

    public List<Ability> Abilities { get; set; } = [];
    public Player Owner { get; set; }

    // TODO: this part needs to be on server. Can the rest be shared for validation etc?
    public void Play(GameEventBus eventBus)
    {
        foreach (var ability in Abilities.Where(a => a is TriggeredAbility))
        {
            var triggeredAbility = (TriggeredAbility)ability;
            triggeredAbility.Register(eventBus);
        }
    }
}