using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Logic
{
    public static class SudokuBoardSolver
    {
        // stack for saving the location of the changed data in the sudoku board during the backtracking function
        public static Stack<int> locationsOfBoardchangesStack = new Stack<int>();
        
        public static bool Solver(Board sudokuBoardToSolve)
        {
            try
            {
                HumanTechniques.SolveWithHumanTechniques(sudokuBoardToSolve);
            }
            catch (Exceptions.UnsolvableBoardException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            bool isTheSudokuBoardResolved = BacktrackingSolver(sudokuBoardToSolve);
            // clear the stack in preparation for the next board
            locationsOfBoardchangesStack.Clear();
            return isTheSudokuBoardResolved;
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
                    int countNumOfChanges;
                    try {
                        countNumOfChanges = HumanTechniques.SolveWithHumanTechniques(sudokuBoardToSolve);
                    }
                    catch (Exceptions.UnsolvableBoardException)
                    {
                        sudokuBoardToSolve.RemoveValue(maskOfTheNumber, row, col);
                        RemoveValuesFromBoard(sudokuBoardToSolve, HumanTechniques.countChangesInTheBoard);
                        continue;
                    }
                    if (BacktrackingSolver(sudokuBoardToSolve))
                        return true;
                    sudokuBoardToSolve.RemoveValue(maskOfTheNumber, row, col);
                    RemoveValuesFromBoard(sudokuBoardToSolve, countNumOfChanges);
                    }
            }
            return false;
        }

        //public static bool BacktrackingSolver(Board sudokuBoardToSolve)
        //{
        //    int locationOfTheCellWithTheMinimumNumberOfLegalOptions = FindMinimumLocation(sudokuBoardToSolve);
        //    if (locationOfTheCellWithTheMinimumNumberOfLegalOptions == -1)
        //        return true;
        //    int row = locationOfTheCellWithTheMinimumNumberOfLegalOptions / sudokuBoardToSolve.GetSize();
        //    int col = locationOfTheCellWithTheMinimumNumberOfLegalOptions % sudokuBoardToSolve.GetSize();
        //    for (int i = 1; i < sudokuBoardToSolve.GetSize() + 1; i++)
        //    {
        //        ulong maskOfTheNumber = HandleBitwise.CreateMaskFromNumber(i);
        //        if (sudokuBoardToSolve.IsNumberValidInThisLocation(maskOfTheNumber, row, col))
        //        {
        //            sudokuBoardToSolve.UpdateValue(i, maskOfTheNumber, row, col);
        //            if (BacktrackingSolver(sudokuBoardToSolve))
        //                return true;
        //            sudokuBoardToSolve.RemoveValue(maskOfTheNumber, row, col);
        //        }
        //    }
        //    return false;
        //}

        public static int CountLegalNumbersInCurrentIndex(Board board, int row, int col)
        {
            ulong theValidNumbersInTheCurrentIndex = HumanTechniques.CheckPossibleNumbersInCurrentIndex(board, row, col);
            return HandleBitwise.CountOneBits(theValidNumbersInTheCurrentIndex);
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

        public static void RemoveValuesFromBoard(Board board, int numberOfValuesToRemove)
        {
            for (int i = 1; i <= numberOfValuesToRemove; i++)
            {
                int locationToRemove = SudokuBoardSolver.locationsOfBoardchangesStack.Pop();
                int row = locationToRemove / board.GetSize();
                int col = locationToRemove % board.GetSize();
                ulong maskOfTheValueToRemove = HandleBitwise.CreateMaskFromNumber(board.BoardMatrix[row, col]);
                board.RemoveValue(maskOfTheValueToRemove, row, col);
            }
        }
    }
}
