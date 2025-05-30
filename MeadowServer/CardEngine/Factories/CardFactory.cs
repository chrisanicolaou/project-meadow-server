using MeadowServer.CardEngine.Entities;
using MeadowShared.CardEngine.Models;
using MeadowShared.Match.Targets;

namespace MeadowServer.CardEngine.Factories;

public class CardFactory(AbilityFactory abilityFactory)
{
    public CardEntity CreateNew(CardTemplateModel model, CardPrintingModel printingModel, Player owner)
    {
        var card = new CardEntity
        {
            Name = model.Name,
            CardType = model.TypeName,
            Class = model.Class,
            FocusCost = model.FocusCost,
            EffectDescription = model.EffectDescription,
            FlavorText = model.FlavorText,
            Owner = owner
        };
        
        card.Abilities = model.Abilities?.Select(a => abilityFactory.CreateNew(a, card)).ToList() ?? [];
        
        return card;
    }
}