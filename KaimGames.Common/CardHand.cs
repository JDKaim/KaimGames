using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KaimGames.Common
{
    public class CardHand
    {
        public readonly List<Card> Cards;

        public CardHand(IEnumerable<Card> cards = null)
        {
            this.Cards = new List<Card>(cards ?? new Card[0]);
        }

        static public CardHand FromCards(params Card[] cards) => new CardHand(cards);

        public void Add(IEnumerable<Card> cards)
        {
            this.Cards.AddRange(cards);
        }

        public void Discard(IEnumerable<Card> cards)
        {
            foreach(Card card in cards)
            {
                Card existing = this.Cards.First(item => item == card);
                this.Cards.Remove(existing);
            }
        }

        public void Replace(IEnumerable<Card> discardCards, IEnumerable<Card> replaceCards)
        {
            Queue<Card> toDiscard = new Queue<Card>(discardCards);
            Queue<Card> toReplace = new Queue<Card>(replaceCards);

            // Replace any as we discard from the existing.
            while (toDiscard.Any())
            {
                Card discard = toDiscard.Dequeue();
                int index = this.Cards.IndexOf(discard);
                this.Cards.RemoveAt(index);
                
                // We might have more to discard than to replace.
                if (toReplace.Any())
                {
                    this.Cards.Insert(index, toReplace.Dequeue());
                }
            }

            // We also might have more to replace than we discard.
            while (toReplace.Any())
            {
                this.Cards.Add(toReplace.Dequeue());
            }
        }
    }
}
