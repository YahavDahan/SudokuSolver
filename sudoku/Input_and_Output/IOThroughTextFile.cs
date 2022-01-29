using System;
using System.IO;
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

        public async void OutputSudokuBoard(Board boardToPrint)
        {
            string guarnteedWritePath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string filePath = Path.Combine(guarnteedWritePath, "solution.txt");
            string strBoard = "";
            for (int i = 0; i < boardToPrint.GetSize(); i++)
                for (int j = 0; j < boardToPrint.GetSize(); j++)
                    strBoard = strBoard + "  " + boardToPrint.BoardMatrix[i, j];
            await File.WriteAllTextAsync(filePath, strBoard);
        }
    }
}
