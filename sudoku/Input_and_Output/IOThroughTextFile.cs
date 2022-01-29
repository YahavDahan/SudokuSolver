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
            string textFilePath = this.InputTextFilePath();
            string strBoard;
            try
            {
                strBoard = System.IO.File.ReadAllText(textFilePath);
                strBoard = strBoard.Replace("\n", "").Replace("\r", "");
            }
            catch (Exception e) when (e is System.IO.FileNotFoundException || e is System.IO.PathTooLongException || e is System.Security.SecurityException)
            {
                Console.WriteLine("cannot open file with this path, enter other path");
                strBoard = InputSudokuBoard();
            }
            return strBoard;
        }

        private string InputTextFilePath()
        {
            Console.Write("Enter the text file path: ");
            string filePath = Console.ReadLine();
            while (!filePath.EndsWith(".txt"))
            {
                Console.Write("Enter the path to text file with the string that representing the sudoku board: ");
                filePath = Console.ReadLine();
            }
            return filePath;
        }

        public void OutputSudokuBoard(Board boardToPrint)
        {

        }
    }
}
