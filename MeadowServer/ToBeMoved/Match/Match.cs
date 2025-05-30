using MeadowServer.CardEngine.Components.Abilities;
using MeadowServer.CardEngine.Components.Effects;
using MeadowServer.CardEngine.Entities;
using MeadowServer.CardEngine.Factories;
using MeadowServer.ToBeMoved.Match.GameEvents;
using MeadowShared.CardEngine.Enums;
using MeadowShared.CardEngine.Models;
using MeadowShared.Extensions;
using MeadowShared.Match;
using MeadowShared.Match.Targets;

namespace MeadowServer.ToBeMoved.Match;

public class Match
{
    private CardFactory _cardFactory;
    public MatchState State { get; set; } = new();
    public GameEventBus EventBus { get; set; } = new();
    public Stack<EffectResolutionRequest> EffectResolutionStack { get; set; } = new();

    public void Initialize(params List<PlayerChallengeRequest> players)
    {
        _cardFactory = InitializeCardFactory();
        State.Players = players.Select(CreatePlayer).ToList();
    }

    public void SetPlayerOne(Player player)
    {
        SetPlayerId(player, PlayerId.PlayerOne);
    }

    public void SetPlayerTwo(Player player)
    {
        SetPlayerId(player, PlayerId.PlayerTwo);
    }

    private void SetPlayerId(Player player, PlayerId playerId)
    {
        player.Id = playerId;
    }

    public void PlayCard(CardEntity card, Player player)
    {
        card.Play(EventBus);
        var playEvent = new CardPlayedEvent(player, sourceCard: card);
        
        foreach (var ability in card.Abilities.Where(a => a is TriggeredAbility { ActivatesWhenPlayed: true }))
        {
            var onPlayAbility = (TriggeredAbility)ability;
            var effectResolutionRequest = onPlayAbility.OnEventTriggered(playEvent, State);
            foreach (var effect in effectResolutionRequest.Effects)
            {
                ResolveEffect(effect, effectResolutionRequest);
            }
        }

        PublishEvent(playEvent);
    }

    private void PublishEvent(GameEvent gameEvent)
    {
        var effectResolutionRequests = EventBus.Publish(gameEvent, State);
        effectResolutionRequests.Reverse();
        foreach (var effectResolutionRequest in effectResolutionRequests)
        {
            EffectResolutionStack.Push(effectResolutionRequest);
        }
        ResolveStack();
    }

    private void ResolveStack()
    {
        var depth = 99;
        while (EffectResolutionStack.Count > 0)
        {
            var effectResolutionRequest = EffectResolutionStack.Pop();
            foreach (var effect in effectResolutionRequest.Effects)
            {
                ResolveEffect(effect, effectResolutionRequest);
            }

            // TODO: handle this properly dumb dumb
            if (--depth < 1)
            {
                throw new StackOverflowException();
            }
        }
    }

    private void ResolveEffect(Effect effect, EffectResolutionRequest effectResolutionRequest)
    {
        if (effect.TargetTriggerSource)
        {
            effect.ApplyUntyped(State, [effectResolutionRequest.TriggeringEvent.Source]);
        }
        
        var targets = effect.GetValidTargetsUntyped(State, effectResolutionRequest.ParentAbility);
        if (targets.Count == 0) return;
        
        switch (effect)
        {
            case { SelectionMode: SelectionMode.AoE }:
                effect.ApplyUntyped(State, targets);
                break;
            // TODO: handle targeted once UI is implemented
            case { SelectionMode: SelectionMode.Targeted }:
            case { SelectionMode: SelectionMode.Random }:
                var selectedTargets = new List<object>();
                for (var i = 0; i < Math.Min(targets.Count, effect.SelectionAmount); i++)
                {
                    selectedTargets.Add(targets.PopRandom());
                };
                effect.ApplyUntyped(State, selectedTargets);
                break;
            default:
                throw new ArgumentException("Invalid selection mode");
        }
    }

    private CardFactory InitializeCardFactory()
    {
        var targetFilterFactory = new FilterFactory();
        var effectFactory = new EffectFactory(targetFilterFactory);
        var triggerFactory = new TriggerFactory(targetFilterFactory);
        var abilityFactory = new AbilityFactory(triggerFactory, effectFactory);
        return new CardFactory(abilityFactory);
    }

    private Player CreatePlayer(PlayerChallengeRequest request)
    {
        var player = new Player();
        // TODO - remove references to object cast
        player.DeckZone.Cards = request.DeckDefinition.Select(c => CreateCard(c.Item1, c.Item2, player)).Cast<object>().ToList();
        return player;
    }

    private CardEntity CreateCard(CardTemplateModel templateModel, CardPrintingModel printingModel, Player player)
    {
        return _cardFactory.CreateNew(templateModel, printingModel, player);
    }
}