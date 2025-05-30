using MeadowServer.CardEngine.Components.Abilities;
using MeadowServer.CardEngine.Entities;
using MeadowServer.ToBeMoved.Match.GameEvents;
using MeadowShared.CardEngine.Enums;
using MeadowShared.CardEngine.Models;
using MeadowShared.Match;

namespace MeadowServer.CardEngine.Components.Filters;

public class CardTypeFilter(List<CardType> cardTypes) : CardFilter
{
    public CardTypeFilter() : this([])
    {
    }

    public List<CardType> CardTypes { get; set; } = cardTypes;
    
    public override void OnCreate(ICardComponentModel model)
    {
        CardTypes = (model as FilterModel)?.Value?.Select(s => Enum.Parse<CardType>(s)).ToList() ?? [];
    }

    public override bool Match(GameEvent gameEvent, Ability parentAbility)
    {
        var matchTarget = MatchOnSource ? gameEvent.Source : gameEvent.Target;
        if (matchTarget == null) return false;
        return CardTypes.Contains(matchTarget.CardType);
    }

    public override bool Match(CardEntity cardModel, Ability parentAbility) => CardTypes.Contains(cardModel.CardType);

    public override List<CardEntity> GetValidTargets(MatchState state, Ability parentAbility)
    {
        return [];
    }
}