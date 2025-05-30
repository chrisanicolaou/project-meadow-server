using MeadowShared.Match;
using MeadowShared.Match.Targets;

namespace MeadowServer.CardEngine.Components.Effects;

public class DrawCard : ValueBasedEffect<Player>
{
    public override void Apply(MatchState gameState, List<Player> targets)
    {
        Console.WriteLine($"Applying {nameof(DrawCard)} to {targets[0].Id}!");
    }
}