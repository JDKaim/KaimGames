using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KaimGames.TicTacToe.Common;

namespace KaimGames.TicTacToe.Tests
{
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void CreateBoard()
        {
            Board board = new Board();

            Assert.IsTrue(board.IsEmptyAt(0, 0));
            Assert.IsFalse(board.IsXAt(0, 0));
            Assert.IsFalse(board.IsOAt(0, 0));
        }

        [TestMethod]
        public void VerifyIsOnBoard()
        {
            Board board = new Board();

            Assert.IsTrue(board.IsOnBoard(0, 0));
            Assert.IsTrue(board.IsOnBoard(1, 1));
            Assert.IsTrue(board.IsOnBoard(2, 2));
            Assert.IsFalse(board.IsOnBoard(3, 0));
            Assert.IsFalse(board.IsOnBoard(-1, 0));
            Assert.IsFalse(board.IsOnBoard(0, 3));
            Assert.IsFalse(board.IsOnBoard(0, -1));
        }

        [TestMethod]
        public void MoveOnBoard()
        {
            Board board = new Board();

            board.Mark(0, 0, 'X');

            Assert.IsFalse(board.IsEmptyAt(0, 0));
            Assert.IsTrue(board.IsXAt(0, 0));
            Assert.IsFalse(board.IsOAt(0, 0));
        }

        [TestMethod]
        public void BoardGetAt()
        {
            Board board = new Board();

            board.Mark(0, 0, 'X');

            Assert.AreEqual('X', board.GetAt(0, 0));
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void MoveOnBoardBadSpot()
        {
            Board board = new Board();

            board.Mark(3, 0, 'X');
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void MoveOnBoardUsedPlace()
        {
            Board board = new Board();

            board.Mark(1, 0, 'X');
            board.Mark(0, 0, 'O');
            board.Mark(1, 0, 'X');
        }

        [TestMethod]
        public void DeserializeBoard()
        {
            Board board = new Board();

            board.Deserialize(
                "X X" +
                "OXO" +
                "   ");

            Assert.AreEqual('X', board.GetAt(0, 0));
            Assert.AreEqual(' ', board.GetAt(0, 1));
            Assert.AreEqual('X', board.GetAt(0, 2));
            Assert.AreEqual('O', board.GetAt(1, 0));
            Assert.AreEqual('X', board.GetAt(1, 1));
            Assert.AreEqual('O', board.GetAt(1, 2));
            Assert.AreEqual(' ', board.GetAt(2, 0));
            Assert.AreEqual(' ', board.GetAt(2, 1));
            Assert.AreEqual(' ', board.GetAt(2, 2));
        }

        [TestMethod]
        public void SerializeBoard()
        {
            Board board = new Board();

            board.Mark(0, 0, 'X');
            board.Mark(0, 2, 'X');
            board.Mark(1, 1, 'X');
            board.Mark(1, 0, 'O');
            board.Mark(1, 2, 'O');
            
            Assert.AreEqual(
                "X X" +
                "OXO" +
                "   ", board.Serialize());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void DeserializeBadBoard()
        {
            Board board = new Board();

            board.Deserialize(
                "X!X" +
                "OXO" +
                "   ");
        }
        }
    }
