using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku
{
    interface ITextFileInputOutput
    {
        string InputSudokuBoard();
        void OutputSudokuBoard(string boardToPrint);
    }
}
