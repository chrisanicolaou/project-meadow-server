using MeadowServer.CardEngine.Entities;
using MeadowShared.Match;

namespace MeadowServer.CardEngine.Components.Effects;

public class Silence : TypedEffect<CardEntity>
{
    public override void Apply(MatchState gameState, List<CardEntity> targets)
    {
        Console.WriteLine($"Applying {nameof(DrawCard)}!");
    }
}