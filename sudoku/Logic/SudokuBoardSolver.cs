using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku
{
    public static class SudokuBoardSolver
    {
        public static int[] BitsSetTable256;
        static SudokuBoardSolver()
        {
            BitsSetTable256 = new int[256];
            BitsSetTable256[0] = 0;
            for (int i = 0; i < 256; i++)
            {
                BitsSetTable256[i] = (i & 1) + BitsSetTable256[i / 2];
            }
        }

        public static bool Solver(Board sudokuBoardToSolve)
        {
            int locationOfTheCellWithTheMinimumNumberOfLegalOptions = FindMinimumLocation(sudokuBoardToSolve);
            if (locationOfTheCellWithTheMinimumNumberOfLegalOptions == -1)
                return true;
            int row = locationOfTheCellWithTheMinimumNumberOfLegalOptions / sudokuBoardToSolve.GetSize();
            int col = locationOfTheCellWithTheMinimumNumberOfLegalOptions % sudokuBoardToSolve.GetSize();
            for (int i = 1; i < sudokuBoardToSolve.GetSize(); i++)
            {
                ulong maskOfTheNumber = Board.createMaskForNumber(i);
                if (sudokuBoardToSolve.IsNumberValidInThisLocation(maskOfTheNumber, row, col))
                {
                    sudokuBoardToSolve.UpdateValue(i, maskOfTheNumber, row, col);
                    if (Solver(sudokuBoardToSolve))
                        return true;
                    sudokuBoardToSolve.RemoveValue(maskOfTheNumber, row, col);
                }
            }
            return false;
        }

        public static int CountLegalNumbersInCurrentIndex(Board board, int row, int col)
        {
            ulong theValidNumbersInTheCurrentIndex = (board.RowsArr[row] ^ (((ulong)1 << board.GetSize()) - 1)) & 
                (board.ColsArr[col] ^ (((ulong)1 << board.GetSize()) - 1)) & 
                (board.BoxesArr[row] ^ (((ulong)1 << board.GetSize()) - 1));
            return (BitsSetTable256[theValidNumbersInTheCurrentIndex & 0xff] 
                + BitsSetTable256[(theValidNumbersInTheCurrentIndex >> 8) & 0xff]
                + BitsSetTable256[(theValidNumbersInTheCurrentIndex >> 16) & 0xff]
                + BitsSetTable256[(theValidNumbersInTheCurrentIndex >> 24) & 0xff]
                + BitsSetTable256[(theValidNumbersInTheCurrentIndex >> 32) & 0xff]
                + BitsSetTable256[(theValidNumbersInTheCurrentIndex >> 40) & 0xff]
                + BitsSetTable256[(theValidNumbersInTheCurrentIndex >> 48) & 0xff]
                + BitsSetTable256[theValidNumbersInTheCurrentIndex >> 56]);
        }

        public static int FindMinimumLocation(Board board)
        {  // מחזיר את מיקום המשבצת בעלת מספר ההצבות החוקיות הקטן ביותר. אם הלוח מלא יוחזר מינוס אחד.
            int theMinimumNumberOfLegalOptions = board.GetSize(), locationOfTheCellWithTheMinimumNumberOfLegalOptions = -1;
            for (int row = 0; row < board.GetSize(); row++)
            {
                if (board.RowsArr[row] == (((ulong)1 << board.GetSize()) - 1))
                    continue;
                for (int col = 0; col < board.GetSize(); col++)
                {
                    if (board.BoardMatrix[row, col] == 0)
                    {
                        int theNumberOfLegalOptionsOfTheCurrentCell = CountLegalNumbersInCurrentIndex(board, row, col);
                        if (theNumberOfLegalOptionsOfTheCurrentCell == 0)
                            return row * board.GetSize() + col;
                        if (theNumberOfLegalOptionsOfTheCurrentCell < theMinimumNumberOfLegalOptions)
                        {
                            theMinimumNumberOfLegalOptions = theNumberOfLegalOptionsOfTheCurrentCell;
                            locationOfTheCellWithTheMinimumNumberOfLegalOptions = row * board.getSize() + col;
                        }
                    }
                }
            }
            return locationOfTheCellWithTheMinimumNumberOfLegalOptions;
        }
    }
}
