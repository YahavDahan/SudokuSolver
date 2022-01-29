using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Input_and_Output
{
    interface IPrintable
    {
        void OutputSudokuBoard(Board boardToPrint);
    }
}
