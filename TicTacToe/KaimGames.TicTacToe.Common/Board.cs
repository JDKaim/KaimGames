using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KaimGames.TicTacToe.Common
{
    public class Board
    {
        public char[][] Squares { get; set; }

        public Board()
        {
            this.Squares = new char[3][];
            this.Squares[0] = new char[3] { ' ', ' ', ' ' };
            this.Squares[1] = new char[3] { ' ', ' ', ' ' };
            this.Squares[2] = new char[3] { ' ', ' ', ' ' };
        }

        public bool IsOAt(int row, int column)
        {
            return (this.Squares[row][column] == 'O');
        }

        public bool IsXAt(int row, int column)
        {
            return (this.Squares[row][column] == 'X');
        }

        public bool IsEmptyAt(int row, int column)
        {
            return (this.Squares[row][column] == ' ');
        }

        public bool IsOnBoard(int row, int column)
        {
            return !(row > 2 || row < 0 || column > 2 || column < 0);
        }

        public void VerifyIsOnBoard(int row, int column)
        {
            if (!this.IsOnBoard(row, column)) { throw new Exception($"({row}, {column}) is not on board"); }
        }

        public void VerifyIsEmptyAt(int row, int column)
        {
            this.VerifyIsOnBoard(row, column);
            if (!this.IsEmptyAt(row, column)) { throw new Exception($"The spot at ({row}, {column}) is already taken."); }
        }

        public void Mark(int row, int column, char c)
        {
            this.VerifyIsEmptyAt(row, column);
            this.Squares[row][column] = c;
        }

        public char GetAt(int row, int column)
        {
            return this.Squares[row][column];
        }
        
        public void Deserialize(string data)
        {
            if (data == null) { throw new Exception("data cannot be null"); }
            if (data.Length != 9) { throw new Exception("data must have exactly 9 squares"); }

            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    char c = data[(row * 3) + column];

                    if (c == 'X' || c == 'O' || c == ' ')
                    {
                        this.Squares[row][column] = c;
                    }
                    else { throw new Exception("Bad character found in data."); }
                }
            }
        }

        public string Serialize()
        {
            string serial = "";
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    char c = this.GetAt(row, column);
                    serial = serial + c;
                }
            }

            return serial;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(this.Serialize().Substring(0, 3));
            sb.AppendLine(this.Serialize().Substring(3, 3));
            sb.AppendLine(this.Serialize().Substring(6, 3));
            return sb.ToString();
        }
    }
}
