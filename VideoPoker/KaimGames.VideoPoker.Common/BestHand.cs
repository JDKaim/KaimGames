using KaimGames.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KaimGames.VideoPoker.Common
{
    public class BestHand
    {
        public readonly VideoPokerHands HandType;
        public readonly List<Card> Cards;
        public readonly int Points;

        public string HandName
        {
            get
            {
                switch (this.HandType)
                {
                    case VideoPokerHands.RoyalFlush: return "Royal Flush";
                    case VideoPokerHands.StraightFlush: return "Stright Flush";
                    case VideoPokerHands.FourOfAKind: return "4 Of A Kind";
                    case VideoPokerHands.FullHouse: return "Full House";
                    case VideoPokerHands.Flush: return "Flush";
                    case VideoPokerHands.Straight: return "Straight";
                    case VideoPokerHands.ThreeOfAKind: return "3 Of A Kind";
                    case VideoPokerHands.TwoPair: return "Two Pair";
                    case VideoPokerHands.JacksOrBetter: return "Jacks or Better";
                }

                return "Insignificant Hand";
            }
        }

        public BestHand(IEnumerable<Card> cards, VideoPokerHands handType)
        {
            this.Cards = cards.ToList();
            this.HandType = handType;

            switch (this.HandType)
            {
                case VideoPokerHands.RoyalFlush: this.Points = 800; break;
                case VideoPokerHands.StraightFlush: this.Points = 50; break;
                case VideoPokerHands.FourOfAKind: this.Points = 25; break;
                case VideoPokerHands.FullHouse: this.Points = 9; break;
                case VideoPokerHands.Flush: this.Points = 6; break;
                case VideoPokerHands.Straight: this.Points = 4; break;
                case VideoPokerHands.ThreeOfAKind: this.Points = 3; break;
                case VideoPokerHands.TwoPair: this.Points = 2; break;
                case VideoPokerHands.JacksOrBetter: this.Points = 1; break;
                default: this.Points = 0; break;
            }
        }
    }
}
