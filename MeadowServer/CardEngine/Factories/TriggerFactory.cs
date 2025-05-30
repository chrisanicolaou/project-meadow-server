using MeadowServer.CardEngine.Components.Triggers;
using MeadowShared.CardEngine.Models;

namespace MeadowServer.CardEngine.Factories;

public class TriggerFactory(FilterFactory filterFactory) : ReflectionBasedFactory<Trigger>("MeadowServer.CardEngine.Components.Triggers")
{
    public Trigger CreateNew(TriggerModel model)
    {
        var trigger = base.CreateNew(model);
        trigger.TriggerZones = model.TriggerZones;
        trigger.OneShot = model.OneShot;

        if (model.Filters?.Count > 0)
        {
            trigger.Filters = model.Filters.Select(filterFactory.CreateNew).ToList();
        }
        
        return trigger;
    }
}