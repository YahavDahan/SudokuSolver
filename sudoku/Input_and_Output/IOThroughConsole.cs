using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Input_and_Output
{
    public class IOThroughConsole : IReadable, IPrintable
    {
        public IOThroughConsole() { }

        public string InputSudokuBoard()
        {
            return "a";
        }

        public void OutputSudokuBoard(Board boardToPrint)
        {

        }
    }
}
