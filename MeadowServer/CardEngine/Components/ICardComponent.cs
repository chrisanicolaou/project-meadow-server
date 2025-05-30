using MeadowShared.CardEngine.Models;

namespace MeadowServer.CardEngine.Components;

public interface ICardComponent
{
    public void OnCreate(ICardComponentModel model);
}