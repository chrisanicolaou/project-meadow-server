using MeadowShared.CardEngine.Models;
using MeadowShared.Match;
using MeadowShared.Match.Targets;

namespace MeadowServer.CardEngine.Components.Effects;

public class ValueBasedEffect<T> : TypedEffect<T> where T : ITargetable
{
    public int Value { get; set; }

    public override void OnCreate(ICardComponentModel model)
    {
        Value = (model as EffectModel)?.Value ?? 0;
    }

    public override void Apply(MatchState gameState, List<T> targets)
    {
        throw new NotImplementedException();
    }
}