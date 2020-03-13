using System;
using KaimGames.Minesweeper.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KaimGames.Minesweeper.Tests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void Win1x1x1Game()
        {
            Game game = new Game(1, 1, 1);
            Assert.IsTrue(game.Board.GetAt(0, 0).IsMine);

            Assert.IsTrue(game.IsGameOver);
            Assert.IsTrue(game.IsWon);
            Assert.IsFalse(game.IsLost);
        }

        [TestMethod]
        public void Win1x1x0Game()
        {
            Game game = new Game(1, 1, 0);
            Assert.IsFalse(game.Board.GetAt(0, 0).IsMine);

            Assert.IsFalse(game.IsGameOver);
            Assert.IsFalse(game.IsWon);
            Assert.IsFalse(game.IsLost);

            game.Mark(0, 0, true);
            Assert.AreEqual(game.Board.GetAt(0, 0).State, CellState.Revealed);

            Assert.IsTrue(game.IsGameOver);
            Assert.IsTrue(game.IsWon);
            Assert.IsFalse(game.IsLost);
        }

        [TestMethod]
        public void Win2x1x1Game()
        {
            Game game = new Game(1, 1, 0);
            if (game.Board.GetAt(0, 0).IsMine)
            {
                game.Mark(0, 1, true);
            }
            else
            {
                game.Mark(0, 0, true);
            }

            Assert.IsTrue(game.IsGameOver);
            Assert.IsTrue(game.IsWon);
            Assert.IsFalse(game.IsLost);
        }

        [TestMethod]
        public void Lose2x2x2Game()
        {
            Game game = new Game(2, 2, 2);
            game.Mark(0, 0, true);
            if (game.Board.GetAt(1, 0).IsMine)
            {
                game.Mark(1, 0, true);
            }
            else
            {
                game.Mark(0, 1, true);
            }

            Assert.IsTrue(game.IsGameOver);
            Assert.IsFalse(game.IsWon);
            Assert.IsTrue(game.IsLost);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void NegativeMines()
        {
            Game game = new Game(5, 10, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TooManyMines()
        {
            Game game = new Game(5, 10, 51);
        }

        [TestMethod]
        public void MarkSpread()
        {
            Game game = new Game(10, 10, 0);
            game.Board.GetAt(0, 0).IsMine = true;
            game.Board.GetAt(9, 0).IsMine = true;
            game.Board.GetAt(0, 9).IsMine = true;
            game.Board.GetAt(9, 9).IsMine = true;
            game.UpdateNumbers();

            game.Mark(2, 2, true);

            Assert.IsTrue(game.IsGameOver);
            Assert.IsTrue(game.IsWon);
            Assert.IsFalse(game.IsLost);
        }
    }
}
