using System;
using System.Collections.Generic;
using System.Text;

namespace KaimGames.Common
{
    public class CardConstants
    {
        static public CardSuits[] AllSuits = new CardSuits[] { CardSuits.Hearts, CardSuits.Diamonds, CardSuits.Spades, CardSuits.Clubs };
        static public CardRanks[] AllRanksAceLow = new CardRanks[] { CardRanks.Ace, CardRanks.Two, CardRanks.Three, CardRanks.Four, CardRanks.Five, CardRanks.Six, CardRanks.Seven, CardRanks.Eight, CardRanks.Nine, CardRanks.Ten, CardRanks.Jack, CardRanks.Queen, CardRanks.King, };
        static public CardRanks[] AllRanksAceHigh = new CardRanks[] { CardRanks.Two, CardRanks.Three, CardRanks.Four, CardRanks.Five, CardRanks.Six, CardRanks.Seven, CardRanks.Eight, CardRanks.Nine, CardRanks.Ten, CardRanks.Jack, CardRanks.Queen, CardRanks.King, CardRanks.Ace, };
    }
}
