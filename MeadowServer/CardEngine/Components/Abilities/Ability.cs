using MeadowServer.CardEngine.Components.Effects;
using MeadowServer.CardEngine.Entities;
using MeadowShared.CardEngine.Models;

namespace MeadowServer.CardEngine.Components.Abilities;

public class Ability : ICardComponent
{
    public Ability()
    {
    }

    public Ability(CardEntity owner)
    {
        Owner = owner;
    }

    public virtual List<Effect> Effects { get; set; } = [];
    
    public virtual CardEntity Owner { get; set; }

    public virtual void OnCreate(ICardComponentModel model)
    {
    }
}