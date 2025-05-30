using MeadowServer.CardEngine.Entities;
using MeadowServer.ToBeMoved;
using MeadowServer.ToBeMoved.Match;
using MeadowShared.CardEngine.Models;
using Newtonsoft.Json;

var json = File.ReadAllText("001.json");

// Deserialize JSON into a List of CardDefinition
var cards = JsonConvert.DeserializeObject<List<CardTemplateModel>>(json);
var cardTuples = cards.Select(c => new Tuple<CardTemplateModel, CardPrintingModel>(c, new CardPrintingModel())).ToList();
var playerOneChallengeRequest = new PlayerChallengeRequest { DeckDefinition = cardTuples };
var playerTwoChallengeRequest = new PlayerChallengeRequest { DeckDefinition = cardTuples };
var game = new Match();

game.Initialize(playerOneChallengeRequest, playerTwoChallengeRequest);

// Simulate coin flip
game.SetPlayerOne(game.State.Players[0]);
game.SetPlayerTwo(game.State.Players[1]);

game.PlayCard(game.State.Players[1].DeckZone.Cards.Cast<CardEntity>().First(c => c.Name == "Caltrops"), game.State.Players[1]);
game.PlayCard(game.State.Players[1].DeckZone.Cards.Cast<CardEntity>().First(c => c.Name == "Young Student"), game.State.Players[1]);
game.PlayCard(game.State.Players[0].DeckZone.Cards[0] as CardEntity, game.State.Players[0]);