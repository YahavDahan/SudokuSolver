using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Input_and_Output
{
    public class IOThroughTextFile : IReadable, IPrintable
    {
        public IOThroughTextFile() { }

        public string InputSudokuBoard()
        {
            return "a";
        }

        public void OutputSudokuBoard(Board boardToPrint)
        {

        }
    }
}
