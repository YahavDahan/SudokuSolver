using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku
{
    interface IConsoleInputOutput
    {
        string InputSudokuBoard();
        void OutputSudokuBoard(string boardToPrint);
    }
}
