using System;

namespace KaimGames.Minesweeper.Common
{
    public class Board
    {
        public readonly int Rows; 
        public readonly int Columns;
        private Cell[][] _cells;

        public Board(int rows, int columns)
        {
            if(rows < 1) { throw new Exception("Rows must be positive."); }
            if(columns < 1) { throw new Exception("Columns must be positive."); }

            this.Rows = rows;
            this.Columns = columns;

            this._cells = new Cell[this.Rows][];

            for (int row = 0; row < rows; row++)
            {
                this._cells[row] = new Cell[this.Columns];

                for (int column = 0; column < columns; column++)
                {
                    this._cells[row][column] = new Cell();
                }
            }
        }

        public bool IsOnBoard(int row, int column)
        {
            if (row < 0) { return false; }
            if (row >= this.Rows) { return false; }
            if (column < 0) { return false; }
            if (column >= this.Columns) { return false; }
            return true;
        }
        public void VerifyIsOnBoard(int row, int column)
        {
            if (!IsOnBoard(row, column)) { throw new Exception($"Row {row}, Column {column} is not on the board, Einstein.");  } 
        }

        public Cell GetAt(int row, int column)
        {
            this.VerifyIsOnBoard(row, column);
            return this._cells[row][column];
        }
        
    }
}
