using MeadowServer.CardEngine.Components.Abilities;
using MeadowServer.CardEngine.Entities;
using MeadowShared.CardEngine.Models;

namespace MeadowServer.CardEngine.Factories;

public class AbilityFactory(
    TriggerFactory triggerFactory,
    EffectFactory effectFactory) : ReflectionBasedFactory<Ability>("MeadowServer.CardEngine.Components.Abilities")
{
    
    public Ability CreateNew(AbilityModel model, CardEntity owner)
    {
        var ability = CreateNew(model);
        ability.Owner = owner;

        if (model.Trigger != null)
        {
            (ability as TriggeredAbility).Trigger = triggerFactory.CreateNew(model.Trigger);
        }

        ability.Effects = model.Effects
            .Select(effectFactory.CreateNew)
            .ToList();

        return ability;
    }
}