using MeadowServer.CardEngine.Components.Filters;
using MeadowShared.CardEngine.Models;

namespace MeadowServer.CardEngine.Factories;

public class FilterFactory() :  ReflectionBasedFactory<Filter>("MeadowServer.CardEngine.Components.Filters")
{
    public Filter CreateNew(FilterModel model)
    {
        var filter = base.CreateNew(model);
        filter.MatchOnSource = model.MatchOnSource;
        return filter;
    }
}