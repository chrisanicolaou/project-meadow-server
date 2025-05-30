using MeadowServer.CardEngine.Entities;
using MeadowShared.Match;

namespace MeadowServer.CardEngine.Components.Effects;

public class DealDamage : ValueBasedEffect<CardEntity>
{
    public override void Apply(MatchState gameState, List<CardEntity> targets)
    {
        Console.WriteLine($"Applying {nameof(DealDamage)}!");
    }
}