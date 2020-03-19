using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaimGames.TicTacToe.Common
{
    public class Game
    {       
        public Board Board { get; set; }

        public bool IsXTurn
        {
            get
            {
                int index = 0;
                if (!this.Board.IsEmptyAt(0, 0)) { index++; }
                if (!this.Board.IsEmptyAt(0, 1)) { index++; }
                if (!this.Board.IsEmptyAt(0, 2)) { index++; }
                if (!this.Board.IsEmptyAt(1, 0)) { index++; }
                if (!this.Board.IsEmptyAt(1, 1)) { index++; }
                if (!this.Board.IsEmptyAt(1, 2)) { index++; }
                if (!this.Board.IsEmptyAt(2, 0)) { index++; }
                if (!this.Board.IsEmptyAt(2, 1)) { index++; }
                if (!this.Board.IsEmptyAt(2, 2)) { index++; }
                if (index % 2 == 0) { return true; }
                return false;
            }
        }

        public void Mark(int row, int column)
        {
            if (this.IsXTurn)
            {
                this.Board.Mark(row, column, 'X');
            }
            else
            {
                this.Board.Mark(row, column, 'O');
            }
        }

        public bool IsWin(char c)
        {
            if (this.Board.GetAt(1, 1) == c)
            {
                if (this.Board.GetAt(0, 0) == c && this.Board.GetAt(2, 2) == c) { return true; }
                if (this.Board.GetAt(1, 0) == c && this.Board.GetAt(1, 2) == c) { return true; }
                if (this.Board.GetAt(0, 1) == c && this.Board.GetAt(2, 1) == c) { return true; }
                if (this.Board.GetAt(0, 2) == c && this.Board.GetAt(2, 0) == c) { return true; }
            }

            if (this.Board.GetAt(0, 0) == c)
            {
                if (this.Board.GetAt(0, 1) == c && this.Board.GetAt(0, 2) == c) { return true; }
                if (this.Board.GetAt(1, 0) == c && this.Board.GetAt(2, 0) == c) { return true; }
            }

            if (this.Board.GetAt(2, 2) == c)
            {
                if (this.Board.GetAt(0, 2) == c && this.Board.GetAt(1, 2) == c) { return true; }
                if (this.Board.GetAt(2, 1) == c && this.Board.GetAt(2, 0) == c) { return true; }
            }

            return false;
        }

        public bool IsXWin
        {
            get { return this.IsWin('X'); }
        }

        public bool IsOWin
        {
            get { return this.IsWin('O'); }
        }

        public bool IsTie
        {
            get
            {
                if (this.IsXWin || this.IsOWin) { return false; }
                return !this.Board.Serialize().Contains(" ");
            }
        }

        public bool IsOver => this.IsXWin || this.IsOWin || this.IsTie;

        public Game()
        {
            this.Board = new Board();
        }

        public Game Copy()
        {
            Game copy = new Game();
            copy.Board.Deserialize(this.Board.Serialize());
            return copy;
        }

    }
}
