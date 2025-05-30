using MeadowShared.CardEngine.Models;

namespace MeadowServer.ToBeMoved;

public class PlayerChallengeRequest
{
    public List<Tuple<CardTemplateModel, CardPrintingModel>> DeckDefinition { get; set; }
}