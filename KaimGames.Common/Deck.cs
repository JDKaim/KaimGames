using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KaimGames.Common
{
    public class Deck
    {
        public readonly Queue<Card> Cards;
        public readonly Stack<Card> DiscardPile;

        public Deck(IEnumerable<Card> cards = null, IEnumerable<Card> discardPile = null)
        {
            this.Cards = new Queue<Card>(cards ?? new Card[0]);
            this.DiscardPile = new Stack<Card>(discardPile ?? new Card[0]);
        }

        static public Deck CreateStandardDeck()
        {
            return new Deck(
                new Card[]
                {
                    new Card(CardSuits.Hearts, CardRanks.Ace),
                    new Card(CardSuits.Hearts, CardRanks.Two),
                    new Card(CardSuits.Hearts, CardRanks.Three),
                    new Card(CardSuits.Hearts, CardRanks.Four),
                    new Card(CardSuits.Hearts, CardRanks.Five),
                    new Card(CardSuits.Hearts, CardRanks.Six),
                    new Card(CardSuits.Hearts, CardRanks.Seven),
                    new Card(CardSuits.Hearts, CardRanks.Eight),
                    new Card(CardSuits.Hearts, CardRanks.Nine),
                    new Card(CardSuits.Hearts, CardRanks.Ten),
                    new Card(CardSuits.Hearts, CardRanks.Jack),
                    new Card(CardSuits.Hearts, CardRanks.Queen),
                    new Card(CardSuits.Hearts, CardRanks.King),
                    new Card(CardSuits.Diamonds, CardRanks.Ace),
                    new Card(CardSuits.Diamonds, CardRanks.Two),
                    new Card(CardSuits.Diamonds, CardRanks.Three),
                    new Card(CardSuits.Diamonds, CardRanks.Four),
                    new Card(CardSuits.Diamonds, CardRanks.Five),
                    new Card(CardSuits.Diamonds, CardRanks.Six),
                    new Card(CardSuits.Diamonds, CardRanks.Seven),
                    new Card(CardSuits.Diamonds, CardRanks.Eight),
                    new Card(CardSuits.Diamonds, CardRanks.Nine),
                    new Card(CardSuits.Diamonds, CardRanks.Ten),
                    new Card(CardSuits.Diamonds, CardRanks.Jack),
                    new Card(CardSuits.Diamonds, CardRanks.Queen),
                    new Card(CardSuits.Diamonds, CardRanks.King),
                    new Card(CardSuits.Spades, CardRanks.Ace),
                    new Card(CardSuits.Spades, CardRanks.Two),
                    new Card(CardSuits.Spades, CardRanks.Three),
                    new Card(CardSuits.Spades, CardRanks.Four),
                    new Card(CardSuits.Spades, CardRanks.Five),
                    new Card(CardSuits.Spades, CardRanks.Six),
                    new Card(CardSuits.Spades, CardRanks.Seven),
                    new Card(CardSuits.Spades, CardRanks.Eight),
                    new Card(CardSuits.Spades, CardRanks.Nine),
                    new Card(CardSuits.Spades, CardRanks.Ten),
                    new Card(CardSuits.Spades, CardRanks.Jack),
                    new Card(CardSuits.Spades, CardRanks.Queen),
                    new Card(CardSuits.Spades, CardRanks.King),
                    new Card(CardSuits.Clubs, CardRanks.Ace),
                    new Card(CardSuits.Clubs, CardRanks.Two),
                    new Card(CardSuits.Clubs, CardRanks.Three),
                    new Card(CardSuits.Clubs, CardRanks.Four),
                    new Card(CardSuits.Clubs, CardRanks.Five),
                    new Card(CardSuits.Clubs, CardRanks.Six),
                    new Card(CardSuits.Clubs, CardRanks.Seven),
                    new Card(CardSuits.Clubs, CardRanks.Eight),
                    new Card(CardSuits.Clubs, CardRanks.Nine),
                    new Card(CardSuits.Clubs, CardRanks.Ten),
                    new Card(CardSuits.Clubs, CardRanks.Jack),
                    new Card(CardSuits.Clubs, CardRanks.Queen),
                    new Card(CardSuits.Clubs, CardRanks.King),
                });
        }

        static public Deck CreateStandardDeckWithJokers()
        {
            Deck deck = Deck.CreateStandardDeck();
            deck.Cards.Enqueue(new Card(CardSuits.Red, CardRanks.Joker));
            deck.Cards.Enqueue(new Card(CardSuits.Black, CardRanks.Joker));
            return deck;
        }

        public void Shuffle(IRandomNumberGenerator rng = null)
        {
            if (rng == null) { rng = new RandomRng(); }

            List<Card> existing = this.Cards.Concat(this.DiscardPile).ToList();
            this.Cards.Clear();
            this.DiscardPile.Clear();

            while (existing.Any())
            {
                int index = rng.Next(existing.Count);
                this.Cards.Enqueue(existing.ElementAt(index));
                existing.RemoveAt(index);
            }
        }

        public List<Card> Draw(int count)
        {
            List<Card> cards = new List<Card>();
            for (int lcv = 0; lcv < count; lcv++)
            {
                if (!this.Cards.Any())
                {
                    this.Shuffle();
                }

                cards.Add(this.Cards.Dequeue());
            }
            return cards;
        }

        public Card DrawFromDiscardPile()
        {
            return this.DiscardPile.Pop();
        }

        public void Discard(IEnumerable<Card> cards)
        {
            foreach (Card card in cards)
            {
                this.DiscardPile.Push(card);
            }
        }

        public CardHand DrawHand(int count)
        {
            return new CardHand(this.Draw(count));
        }
    }
}
