using KaimGames.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KaimGames.VideoPoker.Common
{
    public class Game
    {
        public int Score { get; set; }
        public Deck Deck { get; set; }
        public int Round { get; set; }
        public int TotalRounds { get; set; }
        public CardHand CurrentHand { get; set; }

        public bool IsGameOver { get; set; }
        public string Name => "VideoPoker";
        public string SubGame => $"{this.HandSize}-{this.TotalRounds}";

        private HandEvaluator _handEvaluator;

        public int HandSize { get; set; }

        public Game(int handSize = 5, int totalRounds = 10)
        {
            this._handEvaluator = new HandEvaluator();

            this.HandSize = handSize;
            this.Score = totalRounds;
            this.Round = 0;
            this.TotalRounds = totalRounds;
            this.Deck = Deck.CreateStandardDeck();
            this.Deal();
        }

        public void Deal()
        {
            if (this.IsGameOver) { return; }

            this.Score--;
            this.Round++;

            if (this.CurrentHand != null)
            {
                this.Deck.Discard(this.CurrentHand.Cards);
                this.CurrentHand.Discard(this.CurrentHand.Cards);
            }

            this.Deck.Shuffle();

            this.CurrentHand = this.Deck.DrawHand(this.HandSize);
        }

        public BestHand Keep(IEnumerable<Card> cards)
        {
            var discardCards = this.CurrentHand.Cards.Where(item => !cards.Any(card => card.ToString() == item.ToString())).ToArray();

            this.Deck.Discard(discardCards);

            this.CurrentHand.Replace(discardCards, this.Deck.Draw(discardCards.Count()));

            BestHand bestHand = this._handEvaluator.FindBestHand(this.CurrentHand);

            this.Score += bestHand.Points;

            if (this.Round == this.TotalRounds)
            {
                this.IsGameOver = true;
            }

            return bestHand;
        }
    }
}
