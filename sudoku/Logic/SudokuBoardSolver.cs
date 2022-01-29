using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Logic
{
    public static class SudokuBoardSolver
    {
        public static bool Solver(Board sudokuBoardToSolve)
        {
            try
            {
                HumanTechniques.SolveWithHumanTechniques(sudokuBoardToSolve);
                return BacktrackingSolver(sudokuBoardToSolve);
            }
            catch (Exceptions.UnsolvableBoardException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static bool BacktrackingSolver(Board sudokuBoardToSolve)
        {
            int locationOfTheCellWithTheMinimumNumberOfLegalOptions = FindMinimumLocation(sudokuBoardToSolve);
            if (locationOfTheCellWithTheMinimumNumberOfLegalOptions == -1)
                return true;
            int row = locationOfTheCellWithTheMinimumNumberOfLegalOptions / sudokuBoardToSolve.GetSize();
            int col = locationOfTheCellWithTheMinimumNumberOfLegalOptions % sudokuBoardToSolve.GetSize();
            for (int i = 1; i < sudokuBoardToSolve.GetSize() + 1; i++)
            {
                ulong maskOfTheNumber = HandleBitwise.CreateMaskFromNumber(i);
                if (sudokuBoardToSolve.IsNumberValidInThisLocation(maskOfTheNumber, row, col))
                {
                    sudokuBoardToSolve.UpdateValue(i, maskOfTheNumber, row, col);
                    if (BacktrackingSolver(sudokuBoardToSolve))
                        return true;
                    sudokuBoardToSolve.RemoveValue(maskOfTheNumber, row, col);
                }
            }
            return false;
        }

        public static int CountLegalNumbersInCurrentIndex(Board board, int row, int col)
        {
            ulong theValidNumbersInTheCurrentIndex = HumanTechniques.CheckPossibleNumbersInCurrentIndex(board, row, col);
            //ulong theValidNumbersInTheCurrentIndex = (board.RowsArr[row] ^ (((ulong)1 << board.GetSize()) - 1)) & 
            //    (board.ColsArr[col] ^ (((ulong)1 << board.GetSize()) - 1)) & 
            //    (board.BoxesArr[row] ^ (((ulong)1 << board.GetSize()) - 1));
            return (HandleBitwise.BitsSetTable256[theValidNumbersInTheCurrentIndex & 0xff] 
                + HandleBitwise.BitsSetTable256[(theValidNumbersInTheCurrentIndex >> 8) & 0xff]
                + HandleBitwise.BitsSetTable256[(theValidNumbersInTheCurrentIndex >> 16) & 0xff]
                + HandleBitwise.BitsSetTable256[(theValidNumbersInTheCurrentIndex >> 24) & 0xff]
                + HandleBitwise.BitsSetTable256[(theValidNumbersInTheCurrentIndex >> 32) & 0xff]
                + HandleBitwise.BitsSetTable256[(theValidNumbersInTheCurrentIndex >> 40) & 0xff]
                + HandleBitwise.BitsSetTable256[(theValidNumbersInTheCurrentIndex >> 48) & 0xff]
                + HandleBitwise.BitsSetTable256[theValidNumbersInTheCurrentIndex >> 56]);
        }

        public static int FindMinimumLocation(Board board)
        {  // מחזיר את מיקום המשבצת בעלת מספר ההצבות החוקיות הקטן ביותר. אם הלוח מלא יוחזר מינוס אחד.
            int theMinimumNumberOfLegalOptions = board.GetSize(), locationOfTheCellWithTheMinimumNumberOfLegalOptions = -1;
            int row, col;
            for (row = 0; row < board.GetSize(); row++)
            {
                if (board.RowsArr[row] == (((ulong)1 << board.GetSize()) - 1))
                    continue;
                for (col = 0; col < board.GetSize(); col++)
                {
                    if (board.BoardMatrix[row, col] == 0)
                    {
                        int theNumberOfLegalOptionsOfTheCurrentCell = CountLegalNumbersInCurrentIndex(board, row, col);
                        if (theNumberOfLegalOptionsOfTheCurrentCell == 0)
                            return row * board.GetSize() + col;
                        if (theNumberOfLegalOptionsOfTheCurrentCell <= theMinimumNumberOfLegalOptions)
                        {
                            theMinimumNumberOfLegalOptions = theNumberOfLegalOptionsOfTheCurrentCell;
                            locationOfTheCellWithTheMinimumNumberOfLegalOptions = row * board.GetSize() + col;
                        }
                    }
                }
            }
            return locationOfTheCellWithTheMinimumNumberOfLegalOptions;
        }
    }
}
