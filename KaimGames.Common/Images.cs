using System;
using System.Collections.Generic;
using System.Text;

namespace KaimGames.Common
{
    public class Images
    {
        const string ImageUrlBase = "https://kaimgames.blob.core.windows.net/public/";

        static public string Card(CardSuits suit, CardRanks rank)
        {
            if (rank == CardRanks.Joker)
            {
                switch(suit)
                {
                    case CardSuits.Black: return $"{Images.ImageUrlBase}cards/black_joker.png";
                    case CardSuits.Red: return $"{Images.ImageUrlBase}cards/red_joker.png";
                    default: throw new ArgumentException($"Unsupported Joker suit: {suit}");
                }
            }

            string suitName;
            switch (suit)
            {
                case CardSuits.Hearts: suitName = "hearts"; break;
                case CardSuits.Diamonds: suitName = "diamonds"; break;
                case CardSuits.Spades: suitName = "spades"; break;
                case CardSuits.Clubs: suitName = "clubs"; break;
                default:  throw new ArgumentException($"Unsupported suit: {suit}");
            }

            string rankName;
            switch (rank)
            {
                case CardRanks.Ace: rankName = "ace"; break;
                case CardRanks.Two: rankName = "2"; break;
                case CardRanks.Three: rankName = "3"; break;
                case CardRanks.Four: rankName = "4"; break;
                case CardRanks.Five: rankName = "5"; break;
                case CardRanks.Six: rankName = "6"; break;
                case CardRanks.Seven: rankName = "7"; break;
                case CardRanks.Eight: rankName = "8"; break;
                case CardRanks.Nine: rankName = "9"; break;
                case CardRanks.Ten: rankName = "10"; break;
                case CardRanks.Jack: rankName = "jack"; break;
                case CardRanks.Queen: rankName = "queen"; break;
                case CardRanks.King: rankName = "king"; break;
                default: throw new ArgumentException($"Unsupported rank: {rank}");
            }

            return $"{Images.ImageUrlBase}cards/{rankName}_of_{suitName}.png";
        }
    }
}
