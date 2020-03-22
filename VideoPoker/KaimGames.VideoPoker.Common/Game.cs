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
        public CardHand CurrentHand { get; set; }

        public bool IsOver => this.Round == 10;

        private HandEvaluator _handEvaluator;

        public Game()
        {
            this._handEvaluator = new HandEvaluator();

            this.Score = 10;
            this.Round = 0;
            this.Deck = Deck.CreateStandardDeck();
            this.Deal();
        }

        public void Deal()
        {
            if (this.IsOver) { return; }

            this.Score--;
            this.Round++;

            this.Deck.Shuffle();
            this.CurrentHand = this.Deck.DrawHand(5);
        }

        public BestHand Keep(IEnumerable<Card> cards)
        {
            var discardCards = this.CurrentHand.Cards.Where(item => !cards.Any(card => card.ToString() == item.ToString())).ToArray();

            this.Deck.Discard(discardCards);

            this.CurrentHand.Replace(discardCards, this.Deck.Draw(discardCards.Count()));

            BestHand bestHand = this._handEvaluator.FindBestHand(this.CurrentHand);

            this.Score += bestHand.Points;
            
            return bestHand;
        }
    }
}
