using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KaimGames.TicTacToe.Common;

namespace KaimGames.TicTacToe.Tests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void CreateGame()
        {
            Game game = new Game();
            Assert.IsTrue(game.IsXTurn);
        }

        [TestMethod]
        public void GameMove()
        {
            Game game = new Game();
            game.Mark(1, 1);
            Assert.IsTrue(game.Board.IsXAt(1, 1));
            Assert.IsFalse(game.IsXTurn);
            Assert.IsFalse(game.IsXTurn);
        }

        [TestMethod]
        public void GameOver()
        {
            Game game = new Game();
            game.Mark(1, 1);
            game.Mark(0, 0);
            game.Mark(1, 2);
            game.Mark(2, 2);
            game.Mark(1, 0);
            Assert.IsTrue(game.IsXWin);
            Assert.IsFalse(game.IsOWin);

            game.Board.Deserialize(
                "X X" +
                "OOX" +
                "OXO");
            Assert.IsFalse(game.IsXWin);
            Assert.IsFalse(game.IsOWin);

            game.Board.Deserialize(
                "XXX" +
                "   " +
                "   ");
            Assert.IsTrue(game.IsXWin);

            game.Board.Deserialize(
                "   " +
                "XXX" +
                "   ");
            Assert.IsTrue(game.IsXWin);

            game.Board.Deserialize(
                "   " +
                "   " +
                "XXX");
            Assert.IsTrue(game.IsXWin);

            game.Board.Deserialize(
                "X  " +
                " X " +
                "  X");
            Assert.IsTrue(game.IsXWin);

            game.Board.Deserialize(
                "  X" +
                " X " +
                "X  ");
            Assert.IsTrue(game.IsXWin);

            game.Board.Deserialize(
                "X  " +
                "X  " +
                "X  ");
            Assert.IsTrue(game.IsXWin);

            game.Board.Deserialize(
                " X " +
                " X " +
                " X ");
            Assert.IsTrue(game.IsXWin);

            game.Board.Deserialize(
                "  X" +
                "  X" +
                "  X");
            Assert.IsTrue(game.IsXWin);
        }
    }
}
