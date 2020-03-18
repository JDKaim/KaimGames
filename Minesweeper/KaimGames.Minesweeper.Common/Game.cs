using System;
using System.Collections.Generic;
using System.Text;

namespace KaimGames.Minesweeper.Common
{
    public class Game
    {
        public Board Board { get; set; }
        public int Mines { get; set; }
        public string SubGame
        {
            get
            {
                return Board.Rows + "-" + Board.Columns + "-" + Mines;
            }
        }

        public bool IsPristine
        {
            get
            {
                for (int row = 0; row < this.Board.Rows; row++)
                {
                    for (int column = 0; column < this.Board.Columns; column++)
                    {
                        Cell cell = Board.GetAt(row, column);
                        if (cell.State != CellState.Pristine) { return false; }
                    }
                }
                return true;
            }
        }
        public int FlagsLeft
        {
            get
            {
                int flagsLeft = this.Mines;
                for (int row = 0; row < this.Board.Rows; row++)
                {
                    for (int column = 0; column < this.Board.Columns; column++)
                    {
                        Cell cell = Board.GetAt(row, column);
                        if (cell.State == CellState.Flagged) { flagsLeft -= 1; }
                    }
                }
                return flagsLeft;
            }
        }

        public int FlagAttempts { get; set; }
        public int Score
        {
            get
            {
                int score = 0;
                for (int row = 0; row < this.Board.Rows; row++)
                {
                    for (int column = 0; column < this.Board.Columns; column++)
                    {
                        Cell cell = Board.GetAt(row, column);
                        if (cell.State == CellState.Revealed && !cell.IsMine) { score += 1; }
                    }
                }
                score -= FlagAttempts;
                return score;
            }
        }

        public int Moves { get; set; }
        public bool IsGameOver
        {
            get
            {
                return this.IsWon || this.IsLost;
            }
        }

        public bool IsWon
        {
            get
            {
                for (int row = 0; row < this.Board.Rows; row++)
                {
                    for (int column = 0; column < this.Board.Columns; column++)
                    {
                        Cell cell = Board.GetAt(row, column);
                        if (cell.IsMine) { continue; }
                        switch (cell.State)
                        {
                            case CellState.Pristine:
                            case CellState.Flagged:
                                return false;
                        }
                    }
                }
                return true;
            }
        }

        public bool IsLost
        {
            get
            {
                for (int row = 0; row < this.Board.Rows; row++)
                {
                    for (int column = 0; column < this.Board.Columns; column++)
                    {
                        Cell cell = Board.GetAt(row, column);
                        if (!cell.IsMine) { continue; }
                        if (cell.State == CellState.Revealed) { return true; }
                    }
                }
                return false;
            }
        }

        public Game()
        {
        }

        public Game(int rows, int columns, int mines)
        {
            this.Board = new Board(rows, columns);
            this.Mines = mines;

            if (mines < 0) { throw new Exception("Mine amount must zero or greater."); }
            if (mines > (rows * columns)) { throw new Exception("Mine amount must be less than game board room."); }

            List<Cell> cells = new List<Cell>();
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    cells.Add(this.Board.GetAt(row, column));
                }
            }

            Random random = new Random();
            for (int mine = 0; mine < mines; mine++)
            {
                int index = random.Next(0, cells.Count);
                cells[index].IsMine = true;
                cells.RemoveAt(index);
            }

            this.UpdateNumbers();
        }

        public void UpdateNumbers()
        {
            for (int row = 0; row < this.Board.Rows; row++)
            {
                for (int column = 0; column < this.Board.Columns; column++)
                {
                    Cell cell = Board.GetAt(row, column);
                    if (cell.IsMine) { continue; }

                    int mineCount = 0;
                    if (this.Board.IsOnBoard((row - 1), (column - 1)) && this.Board.GetAt((row - 1), (column - 1)).IsMine)
                    {
                        mineCount++;
                    }
                    if (this.Board.IsOnBoard((row - 1), (column)) && this.Board.GetAt((row - 1), (column)).IsMine)
                    {
                        mineCount += 1;
                    }
                    if (this.Board.IsOnBoard((row - 1), (column + 1)) && this.Board.GetAt((row - 1), (column + 1)).IsMine)
                    {
                        mineCount += 1;
                    }
                    if (this.Board.IsOnBoard((row), (column - 1)) && this.Board.GetAt((row), (column - 1)).IsMine)
                    {
                        mineCount += 1;
                    }
                    if (this.Board.IsOnBoard((row), (column + 1)) && this.Board.GetAt((row), (column + 1)).IsMine)
                    {
                        mineCount += 1;
                    }
                    if (this.Board.IsOnBoard((row + 1), (column - 1)) && this.Board.GetAt((row + 1), (column - 1)).IsMine)
                    {
                        mineCount += 1;
                    }
                    if (this.Board.IsOnBoard((row + 1), (column)) && this.Board.GetAt((row + 1), (column)).IsMine)
                    {
                        mineCount += 1;
                    }
                    if (this.Board.IsOnBoard((row + 1), (column + 1)) && this.Board.GetAt((row + 1), (column + 1)).IsMine)
                    {
                        mineCount += 1;
                    }
                    cell.SurroundingMines = mineCount;
                }
            }
        }

        public void Mark(int row, int column, bool click = false)
        {
            if (!this.Board.IsOnBoard(row, column)) { return; }

            Cell cell = Board.GetAt(row, column);

            if (cell.State != CellState.Pristine) { return; }

            cell.State = CellState.Revealed;
            if (click == true) { Moves += 1; }
            if (cell.IsMine)
            {
                if (this.Moves > 1) { return; }

                // Make a list of cells that aren't mines.
                List<Cell> cells = new List<Cell>();
                for (int rowIndex = 0; rowIndex < this.Board.Rows; rowIndex++)
                {
                    for (int columnIndex = 0; columnIndex < this.Board.Columns; columnIndex++)
                    {
                        Cell indexCell = this.Board.GetAt(rowIndex, columnIndex);
                        if (!indexCell.IsMine)
                        {
                            cells.Add(indexCell);
                        }
                    }
                }

                // Swap with a random space that isn't a mine.
                Random random = new Random();
                int index = random.Next(0, cells.Count);
                cells[index].IsMine = true;
                cell.IsMine = false;
                this.UpdateNumbers();
            }

            if (cell.SurroundingMines == 0)
            {
                Mark((row + 1), column);
                Mark((row - 1), column);
                Mark((row + 1), (column + 1));
                Mark((row + 1), (column - 1));
                Mark((row - 1), (column + 1));
                Mark((row - 1), (column - 1));
                Mark(row, (column + 1));
                Mark(row, (column - 1));
            }
        }

        public void RevealSurroundings(int row, int column)
        {
            Cell cell = Board.GetAt(row, column);

            if (cell.State != CellState.Revealed) { return; }

            if (cell.IsMine) { return; }

            int surroundingFlags = 0;

            if (Board.IsOnBoard(row - 1, column - 1) && Board.GetAt(row - 1, column - 1).State == CellState.Flagged) { surroundingFlags += 1; }
            if (Board.IsOnBoard(row - 1, column) && Board.GetAt(row - 1, column).State == CellState.Flagged) { surroundingFlags += 1; }
            if (Board.IsOnBoard(row - 1, column + 1) && Board.GetAt(row - 1, column + 1).State == CellState.Flagged) { surroundingFlags += 1; }
            if (Board.IsOnBoard(row, column - 1) && Board.GetAt(row, column - 1).State == CellState.Flagged) { surroundingFlags += 1; }
            if (Board.IsOnBoard(row, column + 1) && Board.GetAt(row, column + 1).State == CellState.Flagged) { surroundingFlags += 1; }
            if (Board.IsOnBoard(row + 1, column - 1) && Board.GetAt(row + 1, column - 1).State == CellState.Flagged) { surroundingFlags += 1; }
            if (Board.IsOnBoard(row + 1, column) && Board.GetAt(row + 1, column).State == CellState.Flagged) { surroundingFlags += 1; }
            if (Board.IsOnBoard(row + 1, column + 1) && Board.GetAt(row + 1, column + 1).State == CellState.Flagged) { surroundingFlags += 1; }

            if (surroundingFlags == cell.SurroundingMines)
            {
                Mark((row + 1), column);
                Mark((row - 1), column);
                Mark((row + 1), (column + 1));
                Mark((row + 1), (column - 1));
                Mark((row - 1), (column + 1));
                Mark((row - 1), (column - 1));
                Mark(row, (column + 1));
                Mark(row, (column - 1));
                Moves += 1;
            }

            surroundingFlags = 8;

            if (!Board.IsOnBoard(row - 1, column - 1) || Board.GetAt(row - 1, column - 1).State == CellState.Revealed) { surroundingFlags -= 1; }
            if (!Board.IsOnBoard(row - 1, column) || Board.GetAt(row - 1, column).State == CellState.Revealed) { surroundingFlags -= 1; }
            if (!Board.IsOnBoard(row - 1, column + 1) || Board.GetAt(row - 1, column + 1).State == CellState.Revealed) { surroundingFlags -= 1; }
            if (!Board.IsOnBoard(row, column - 1) || Board.GetAt(row, column - 1).State == CellState.Revealed) { surroundingFlags -= 1; }
            if (!Board.IsOnBoard(row, column + 1) || Board.GetAt(row, column + 1).State == CellState.Revealed) { surroundingFlags -= 1; }
            if (!Board.IsOnBoard(row + 1, column - 1) || Board.GetAt(row + 1, column - 1).State == CellState.Revealed) { surroundingFlags -= 1; }
            if (!Board.IsOnBoard(row + 1, column) || Board.GetAt(row + 1, column).State == CellState.Revealed) { surroundingFlags -= 1; }
            if (!Board.IsOnBoard(row + 1, column + 1) || Board.GetAt(row + 1, column + 1).State == CellState.Revealed) { surroundingFlags -= 1; }

            if (surroundingFlags == cell.SurroundingMines)
            {
                SetFlag((row + 1), column);
                SetFlag((row - 1), column);
                SetFlag((row + 1), (column + 1));
                SetFlag((row + 1), (column - 1));
                SetFlag((row - 1), (column + 1));
                SetFlag((row - 1), (column - 1));
                SetFlag(row, (column + 1));
                SetFlag(row, (column - 1));
            }
        }

        public void SetFlag(int row, int column)
        {
            if (!this.Board.IsOnBoard(row, column)) { return; }
            Cell cell = Board.GetAt(row, column);
            if (cell.State == CellState.Pristine) { cell.State = CellState.Flagged; Moves += 1; FlagAttempts += 1; }
        }

        public void ClearFlag(int row, int column)
        {
            Cell cell = Board.GetAt(row, column);
            if (cell.State == CellState.Flagged) { cell.State = CellState.Pristine; Moves += 1; FlagAttempts += 1; }
        }

    }
}
