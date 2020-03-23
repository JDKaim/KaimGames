using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KaimGames.Common
{
    public class Card
    {
        public readonly CardSuits Suit;
        public readonly CardRanks Rank;

        public string ImageUrl => Images.Card(this.Suit, this.Rank);

        public Card(CardSuits suit, CardRanks rank)
        {
            this.Suit = suit;
            this.Rank = rank;
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return (other.GetType() == GetType()) && this.Equals((Card)other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(Card other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return ((this.Rank == other.Rank) && (this.Suit == other.Suit));
        }

        public override string ToString()
        {
            return $"{(char)this.Rank}{(char)this.Suit}";
        }

        public static bool operator ==(Card first, Card second)
        {
            if (ReferenceEquals(first, second)) { return true; }
            if (ReferenceEquals(first, null)) { return false; }
            if (ReferenceEquals(second, null)) { return false; }

            return ((first.Rank == second.Rank) && (first.Suit == second.Suit));
        }

        public static bool operator !=(Card first, Card second)
        {
            return !(first == second);
        }
    }

    static public class CardExtensions
    {
        public static List<Card> Sort(this IEnumerable<Card> cards, bool aceHigh = false)
        {
            List<Card> sorted = new List<Card>();

            List<Card> existing = cards.ToList();

            foreach (CardRanks rank in aceHigh ? CardConstants.AllRanksAceHigh : CardConstants.AllRanksAceLow)
            {
                foreach (CardSuits suit in CardConstants.AllSuits)
                {
                    Card card = existing.FirstOrDefault(item => (item.Rank == rank) && (item.Suit == suit));
                    if (card != null)
                    {
                        sorted.Add(card);
                    }
                }
            }

            return sorted;
        }
    }
}
