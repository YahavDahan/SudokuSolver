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
            Console.WriteLine("Enter the string that representing the sudoku board: ");
            string strBoard = Console.ReadLine();
            return strBoard;
        }

        public void OutputSudokuBoard(Board boardToPrint)
        {
            for (int i = 0; i < boardToPrint.GetSize(); i++)
                Console.Write("_____");
            Console.WriteLine();
            for (int i = 0; i < boardToPrint.GetSize(); i++)
            {
                Console.Write('|');
                for (int j = 0; j < boardToPrint.GetSize(); j++)
                    Console.Write(String.Format(" {0,2} |", boardToPrint.BoardMatrix[i, j]));
                Console.Write("\n|");
                for (int j = 0; j < boardToPrint.GetSize(); j++)
                    Console.Write("____|");
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
