using KaimGames.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KaimGames.VideoPoker.Common
{
    public class HandEvaluator
    {
        static public VideoPokerHands[] AllHands = new VideoPokerHands[] { VideoPokerHands.StraightFlush, VideoPokerHands.FourOfAKind, VideoPokerHands.FullHouse, VideoPokerHands.Flush, VideoPokerHands.Straight, VideoPokerHands.ThreeOfAKind, VideoPokerHands.TwoPair, VideoPokerHands.JacksOrBetter, VideoPokerHands.AllOther };

        /// <summary>
        /// Finds the best Video Poker hand from the provided cards. Note that this code does not assume 5 cards,
        /// although it does assume one deck.
        /// </summary>
        /// <param name="cardHand"></param>
        /// <returns></returns>
        public BestHand FindBestHand(CardHand cardHand)
        {
            if (cardHand == null) { throw new ArgumentNullException("cardHand"); }
            if (cardHand.Cards.Count() < 2) { throw new ArgumentException("This method requires at least two cards"); }

            // Group the cards by their rank. This will help us whenever we're dealing with pairs, threes, etc.
            var groupsByRank = cardHand.Cards.GroupBy(item => item.Rank);

            // Group the cards by their suit. This will help us whenever we're dealing with flushes.
            var groupsBySuit = cardHand.Cards.GroupBy(item => item.Suit);

            // Check for stright/royal flushes first. Go suit by suit.
            foreach (var suitCards in groupsBySuit.Where(item => item.Count() >= 5))
            {
                List<Card> straightFlush = this.FindStraight(suitCards);
                if (straightFlush != null)
                {
                    return new BestHand(straightFlush, (straightFlush.Last().Rank == CardRanks.Ace) ? VideoPokerHands.RoyalFlush : VideoPokerHands.StraightFlush);
                }
            }

            // Check to see if we have four of the same cards of a given rank. Shouldn't be more than four, but why not include that potential scenario?
            if (groupsByRank.Any(item => item.Count() >= 4))
            {
                // We don't really care if there are multiple at this time. Just grab one.
                return new BestHand(groupsByRank.First(item => item.Count() >= 4).Take(5), VideoPokerHands.FourOfAKind);
            }

            // Check to see if we have a full house (at least three cards of one rank and at least two of another).
            var threeOfAKind = groupsByRank.FirstOrDefault(item => item.Count() == 3);
            if (threeOfAKind != null)
            {
                var fullHousePair = groupsByRank.FirstOrDefault(item => ((item.Key != threeOfAKind.Key) && (item.Count() >= 2)));
                if (fullHousePair != null)
                {
                    return new BestHand(threeOfAKind.Concat(fullHousePair.Take(2)), VideoPokerHands.FullHouse);
                }
            }

            // Check for a flush.
            if (groupsBySuit.Any(item => item.Count() >= 5))
            {
                return new BestHand(groupsBySuit.First(item => item.Count() >= 5).Take(5), VideoPokerHands.Flush);
            }

            // Check for a straight.
            List<Card> straight = this.FindStraight(cardHand.Cards);
            if (straight != null)
            {
                return new BestHand(straight, VideoPokerHands.Straight);
            }

            if (threeOfAKind != null)
            {
                return new BestHand(threeOfAKind, VideoPokerHands.ThreeOfAKind);
            }

            // Check to see if we have a pair or two.
            var onePair = groupsByRank.FirstOrDefault(item => item.Count() == 2);
            if (onePair != null)
            {
                var twoPair = groupsByRank.FirstOrDefault(item => ((item.Key != onePair.Key) && (item.Count() == 2)));
                if (twoPair != null)
                {
                    return new BestHand(onePair.Concat(twoPair), VideoPokerHands.TwoPair);
                }

                // We only support single pairs if they are Jacks or better (Q, K, A).
                switch(onePair.Key)
                {
                    case CardRanks.Ace:
                    case CardRanks.King:
                    case CardRanks.Queen:
                    case CardRanks.Jack:
                        return new BestHand(onePair, VideoPokerHands.JacksOrBetter);
                }
            }

            return new BestHand(new Card[0], VideoPokerHands.AllOther);
        }

        private List<Card> FindStraight(IEnumerable<Card> cards)
        {
            // Map the indexes of the ranks we have. This will make it easier to check for straights later.
            Dictionary<int, Card> cardIndexMap = new Dictionary<int, Card>();
            
            foreach(Card card in cards)
            {
                cardIndexMap[Array.FindIndex(CardConstants.AllRanksAceLow, item => card.Rank == item)] = card;
            }

            if (cardIndexMap.Keys.Count < 5) { return null; }

            // If there's an Ace (will be at index 0), then add a copy of it to the end (index 13) so we can test it for both high and low.
            if (cardIndexMap.ContainsKey(0))
            {
                cardIndexMap.Add(13, cards.First());
            }

            // Start with the highest possible card (Ace high) and check to see if it exists in the hand, as well as the four previous cards required.
            for (int highCardIndex = 13; highCardIndex >= 4; highCardIndex--)
            {
                if (!cardIndexMap.ContainsKey(highCardIndex)) { continue; }

                bool isStraight = true;
                for (int previousCardIndex = highCardIndex - 1; previousCardIndex > (highCardIndex - 5); previousCardIndex--)
                {
                    if (!cardIndexMap.ContainsKey(previousCardIndex)) 
                    {
                        isStraight = false;
                        break;
                    }
                }

                if (isStraight)
                {
                    return new List<Card>()
                    {
                        cardIndexMap[highCardIndex - 4],
                        cardIndexMap[highCardIndex - 3],
                        cardIndexMap[highCardIndex - 2],
                        cardIndexMap[highCardIndex - 1],
                        cardIndexMap[highCardIndex]
                    };
                }
            }

            return null;
        }
    }
}
