using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku
{
    interface IReadable
    {
        string InputSudokuBoard();
        void OutputSudokuBoard(string boardToPrint);
    }
}
