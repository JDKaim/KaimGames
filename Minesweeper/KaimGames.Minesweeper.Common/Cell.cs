using System;
using System.Collections.Generic;
using System.Text;

namespace KaimGames.Minesweeper.Common
{
    public class Cell
    {
        public CellState State { get; set; }
        public bool IsMine { get; set; }
        public int SurroundingMines { get; set; }

        public Cell()
        {
            this.State = CellState.Pristine;
            this.IsMine = false;
            this.SurroundingMines = 0;
        }
    }
}
