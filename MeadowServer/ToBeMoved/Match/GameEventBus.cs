using MeadowServer.CardEngine.Components.Effects;
using MeadowServer.ToBeMoved.Match.GameEvents;
using MeadowShared.Match;

namespace MeadowServer.ToBeMoved.Match;

public class GameEventBus
{
    private readonly Dictionary<GameEventType, List<Func<GameEvent, MatchState, EffectResolutionRequest?>>> _eventMap =
        new();

    public void Subscribe(GameEventType type, Func<GameEvent, MatchState, EffectResolutionRequest?> handler)
    {
        if (!_eventMap.TryGetValue(type, out var subs))
        {
            subs = [];
            _eventMap.Add(type, subs);
        }

        subs.Add(handler);
    }

    public List<EffectResolutionRequest> Publish(GameEvent gameEvent, MatchState state)
    {
        if (!_eventMap.TryGetValue(gameEvent.EventType, out var subs)) return [];

        var requests = subs.Select(handler =>
            {
                var request = handler(gameEvent, state);
                if (request != null) request.TriggeringEvent = gameEvent;
                return request;
            })
            .OfType<EffectResolutionRequest>().ToList();

        return requests;
    }

    public void Clear()
    {
        _eventMap.Clear();
    }
}