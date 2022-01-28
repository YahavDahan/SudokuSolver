using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku
{
    interface IPrintable
    {
        string InputSudokuBoard();
        void OutputSudokuBoard(string boardToPrint);
    }
}
