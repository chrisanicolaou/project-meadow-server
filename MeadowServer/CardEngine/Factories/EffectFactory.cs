using MeadowServer.CardEngine.Components.Effects;
using MeadowShared.CardEngine.Models;

namespace MeadowServer.CardEngine.Factories;

public class EffectFactory(FilterFactory filterFactory) : ReflectionBasedFactory<Effect>("MeadowServer.CardEngine.Components.Effects")
{
    public Effect CreateNew(EffectModel model)
    {
        var effect = base.CreateNew(model);

        if (model.Filters?.Count > 0)
        {
            effect.Filters = model.Filters.Select(filterFactory.CreateNew).ToList();
        }

        effect.TargetTriggerSource = model.TargetTriggerSource ?? false;
        effect.SelectionMode = model.SelectionMode;
        effect.SelectionAmount = model.SelectionAmount;

        return effect;
    }
}