using System;
using KaimGames.Minesweeper.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KaimGames.Minesweeper.Tests
{
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void IsOnBoard()
        {
            Board board = new Board(5, 10);
            board.VerifyIsOnBoard(4, 9);
        }
         
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void IsNotOnBoard()
        {
            Board board = new Board(5, 10);
            board.VerifyIsOnBoard(-1, 9);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void NegativeRows()
        {
            Board board = new Board(-1, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void NegativeColumns()
        {
            Board board = new Board(5, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void VerifyBadRowIsNotOnBoard()
        {
            Board board = new Board(5, 10);
            board.VerifyIsOnBoard(-1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void VerifyColumnRowIsNotOnBoard()
        {
            Board board = new Board(5, 10);
            board.VerifyIsOnBoard(1, -1);
        }
    }
}
