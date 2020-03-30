using System;
using System.Linq;
using KaimGames.Common;
using KaimGames.VideoPoker.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KaimGames.VideoPoker.Tests
{
    [TestClass]
    public class HandTests
    {
        [TestMethod]
        public void FindNothing()
        {
            CardHand hand = CardHand.FromCards(
                new Card(CardSuits.Spades, CardRanks.Ace),
                new Card(CardSuits.Hearts, CardRanks.Two),
                new Card(CardSuits.Clubs, CardRanks.Three),
                new Card(CardSuits.Diamonds, CardRanks.Four),
                new Card(CardSuits.Spades, CardRanks.Six));

            HandEvaluator evaluator = new HandEvaluator();

            BestHand bestHand = evaluator.FindBestHand(hand);

            Assert.AreEqual(VideoPokerHands.AllOther, bestHand.HandType);
        }

        [TestMethod]
        public void FindPairNotJacksOrBetter()
        {
            CardHand hand = CardHand.FromCards(
                new Card(CardSuits.Spades, CardRanks.Ace),
                new Card(CardSuits.Hearts, CardRanks.Two),
                new Card(CardSuits.Clubs, CardRanks.Two),
                new Card(CardSuits.Diamonds, CardRanks.Four),
                new Card(CardSuits.Spades, CardRanks.Six));

            HandEvaluator evaluator = new HandEvaluator();

            BestHand bestHand = evaluator.FindBestHand(hand);

            Assert.AreEqual(VideoPokerHands.AllOther, bestHand.HandType);
        }

        [TestMethod]
        public void FindPairJacksOrBetter()
        {
            CardHand hand = CardHand.FromCards(
                new Card(CardSuits.Spades, CardRanks.Two),
                new Card(CardSuits.Spades, CardRanks.Three),
                new Card(CardSuits.Spades, CardRanks.Four),
                new Card(CardSuits.Spades, CardRanks.Ace),
                new Card(CardSuits.Hearts, CardRanks.Ace));

            HandEvaluator evaluator = new HandEvaluator();

            BestHand bestHand = evaluator.FindBestHand(hand);

            Assert.AreEqual(VideoPokerHands.JacksOrBetter, bestHand.HandType);
        }

        [TestMethod]
        public void FindThreeOfAKind()
        {
            CardHand hand = CardHand.FromCards(
                new Card(CardSuits.Spades, CardRanks.Two),
                new Card(CardSuits.Spades, CardRanks.Three),
                new Card(CardSuits.Diamonds, CardRanks.Ace),
                new Card(CardSuits.Spades, CardRanks.Ace),
                new Card(CardSuits.Hearts, CardRanks.Ace));

            HandEvaluator evaluator = new HandEvaluator();

            BestHand bestHand = evaluator.FindBestHand(hand);

            Assert.AreEqual(VideoPokerHands.ThreeOfAKind, bestHand.HandType);
        }

        [TestMethod]
        public void FindFullHouse()
        {
            CardHand hand = CardHand.FromCards(
                new Card(CardSuits.Spades, CardRanks.Two),
                new Card(CardSuits.Hearts, CardRanks.Two),
                new Card(CardSuits.Diamonds, CardRanks.Ace),
                new Card(CardSuits.Spades, CardRanks.Ace),
                new Card(CardSuits.Hearts, CardRanks.Ace));

            HandEvaluator evaluator = new HandEvaluator();

            BestHand bestHand = evaluator.FindBestHand(hand);

            Assert.AreEqual(VideoPokerHands.FullHouse, bestHand.HandType);
        }

        [TestMethod]
        public void FindFourOfAKind()
        {
            CardHand hand = CardHand.FromCards(
                new Card(CardSuits.Spades, CardRanks.Two),
                new Card(CardSuits.Clubs, CardRanks.Ace),
                new Card(CardSuits.Diamonds, CardRanks.Ace),
                new Card(CardSuits.Spades, CardRanks.Ace),
                new Card(CardSuits.Hearts, CardRanks.Ace));

            HandEvaluator evaluator = new HandEvaluator();

            BestHand bestHand = evaluator.FindBestHand(hand);

            Assert.AreEqual(VideoPokerHands.FourOfAKind, bestHand.HandType);
        }

        [TestMethod]
        public void FindFlush()
        {
            CardHand hand = CardHand.FromCards(
                new Card(CardSuits.Spades, CardRanks.Two),
                new Card(CardSuits.Spades, CardRanks.Three),
                new Card(CardSuits.Spades, CardRanks.Four),
                new Card(CardSuits.Spades, CardRanks.Ace),
                new Card(CardSuits.Spades, CardRanks.King));

            HandEvaluator evaluator = new HandEvaluator();

            BestHand bestHand = evaluator.FindBestHand(hand);

            Assert.AreEqual(VideoPokerHands.Flush, bestHand.HandType);
        }

        [TestMethod]
        public void FindStraight()
        {
            CardHand hand = CardHand.FromCards(
                new Card(CardSuits.Spades, CardRanks.Two),
                new Card(CardSuits.Spades, CardRanks.Three),
                new Card(CardSuits.Hearts, CardRanks.Four),
                new Card(CardSuits.Spades, CardRanks.Five),
                new Card(CardSuits.Spades, CardRanks.Six));

            HandEvaluator evaluator = new HandEvaluator();

            BestHand bestHand = evaluator.FindBestHand(hand);

            Assert.AreEqual(VideoPokerHands.Straight, bestHand.HandType);
        }

        [TestMethod]
        public void FindStraightLowAce()
        {
            CardHand hand = CardHand.FromCards(
                new Card(CardSuits.Spades, CardRanks.Two),
                new Card(CardSuits.Spades, CardRanks.Three),
                new Card(CardSuits.Hearts, CardRanks.Four),
                new Card(CardSuits.Spades, CardRanks.Five),
                new Card(CardSuits.Spades, CardRanks.Ace));

            HandEvaluator evaluator = new HandEvaluator();

            BestHand bestHand = evaluator.FindBestHand(hand);

            Assert.AreEqual(VideoPokerHands.Straight, bestHand.HandType);
        }

        [TestMethod]
        public void FindStrightHighAce()
        {
            CardHand hand = CardHand.FromCards(
                new Card(CardSuits.Spades, CardRanks.Ace),
                new Card(CardSuits.Spades, CardRanks.King),
                new Card(CardSuits.Hearts, CardRanks.Queen),
                new Card(CardSuits.Spades, CardRanks.Jack),
                new Card(CardSuits.Spades, CardRanks.Ten));

            HandEvaluator evaluator = new HandEvaluator();

            BestHand bestHand = evaluator.FindBestHand(hand);

            Assert.AreEqual(VideoPokerHands.Straight, bestHand.HandType);
        }

        [TestMethod]
        public void FindStraightFlush()
        {
            CardHand hand = CardHand.FromCards(
                new Card(CardSuits.Spades, CardRanks.Two),
                new Card(CardSuits.Spades, CardRanks.Three),
                new Card(CardSuits.Spades, CardRanks.Four),
                new Card(CardSuits.Spades, CardRanks.Five),
                new Card(CardSuits.Spades, CardRanks.Six));

            HandEvaluator evaluator = new HandEvaluator();

            BestHand bestHand = evaluator.FindBestHand(hand);

            Assert.AreEqual(VideoPokerHands.StraightFlush, bestHand.HandType);
        }

        [TestMethod]
        public void FindRoyalFlush()
        {
            CardHand hand = CardHand.FromCards(
                new Card(CardSuits.Spades, CardRanks.Ace),
                new Card(CardSuits.Spades, CardRanks.King),
                new Card(CardSuits.Spades, CardRanks.Queen),
                new Card(CardSuits.Spades, CardRanks.Jack),
                new Card(CardSuits.Spades, CardRanks.Ten));

            HandEvaluator evaluator = new HandEvaluator();

            BestHand bestHand = evaluator.FindBestHand(hand);

            Assert.AreEqual(VideoPokerHands.RoyalFlush, bestHand.HandType);
        }
    }
}
