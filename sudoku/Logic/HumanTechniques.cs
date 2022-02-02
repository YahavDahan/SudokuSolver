using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Logic
{
    public static class HumanTechniques
    {
		public static int countChangesInTheBoard = 0;

		public static int SolveWithHumanTechniques(Board boardToSolve)
        {
			HumanTechniques.countChangesInTheBoard = 0;
			while (true)
            {
				bool nakedSingles = NakedSinglesTechnique(boardToSolve);
				bool hiddenSingles = HiddenSinglesTechnique(boardToSolve);
				bool nakedPair = NakedPairsTechnique(boardToSolve);
				if (!(hiddenSingles || nakedSingles || nakedPair))
					break;
			}
			return HumanTechniques.countChangesInTheBoard;
        }		

		public static bool NakedSinglesTechnique(Board board)
		{  // מחזיר אמת אם התרחשו שינויים בלוח. אחרת מחזיר שקר. מחזיר שגיאה אם הלוח לא פתיר
			bool hasBoardChanged = false;
			for (int row = 0; row < board.GetSize(); row++)
				for (int col = 0; col < board.GetSize(); col++)
					if (board.BoardMatrix[row, col] == 0)
					{
						ulong maskOfThePossibleNumbers = CheckPossibleNumbersInCurrentIndex(board, row, col);
						if (maskOfThePossibleNumbers == 0)
							throw new Exceptions.UnsolvableBoardException(String.Format("No value can match the cell at location [{0}, {1}]", row, col));
						if (HandleBitwise.IsPowerOfTwo(maskOfThePossibleNumbers))
						{
							AddNewValueToTheBoard(board, maskOfThePossibleNumbers, row, col);
							hasBoardChanged = true;
						}
					}
			return hasBoardChanged;
		}

		public static bool HiddenSinglesTechnique(Board board)
        {
			bool hasBoardChanged = false;
			for (int row = 0; row < board.GetSize(); row++)
				for (int col = 0; col < board.GetSize(); col++)
					if (board.BoardMatrix[row, col] == 0)
					{
						ulong possibleNumbersOfAllTheCellsInTheCurrentBox = PossibleNumbersInCurrentBox(board, row, col);
						ulong notPossibleNumbersOfTheCurrentCell = board.RowsArr[row] | board.ColsArr[col]
							| board.BoxesArr[row - (row % board.GetSubSize()) + col / board.GetSubSize()];
						ulong maskOfThePossibleNumbers = (possibleNumbersOfAllTheCellsInTheCurrentBox | notPossibleNumbersOfTheCurrentCell) ^ (((ulong)1 << board.GetSize()) - 1);
						if (maskOfThePossibleNumbers != 0)
						{
							if (HandleBitwise.IsPowerOfTwo(maskOfThePossibleNumbers))
							{
								AddNewValueToTheBoard(board, maskOfThePossibleNumbers, row, col);
								hasBoardChanged = true;
							}
							else
								throw new Exceptions.UnsolvableBoardException(String.Format("more then one number must appear in location [{0}, {1}]", row, col));
						}
					}
			return hasBoardChanged;
		}

		public static ulong PossibleNumbersInCurrentBox(Board board, int row, int col)
		{
			int boxNumber = board.GetBoxNumberByRowAndColumn(row, col);
			ulong resultOfAllThePossibleNumbersInTheCurrentBox = 0;
			for (int i = boxNumber / board.GetSubSize() * board.GetSubSize(); i < boxNumber / board.GetSubSize() * board.GetSubSize() + board.GetSubSize(); i++)
				for (int j = (boxNumber % board.GetSubSize()) * board.GetSubSize(); j < (boxNumber % board.GetSubSize()) * board.GetSubSize() + board.GetSubSize(); j++)
  					if (board.BoardMatrix[i, j] == 0 && (i != row || j != col))
						resultOfAllThePossibleNumbersInTheCurrentBox |= CheckPossibleNumbersInCurrentIndex(board, i, j);
			return resultOfAllThePossibleNumbersInTheCurrentBox;
		}		

		public static bool NakedPairsTechnique(Board board)
        {
			bool hasBoardChanged = false;
			bool hasAChangeoccurredInRow = false, hasAChangeoccurredInColumn = false;
			for (int i = 0; i < board.GetSize(); i++)
            {
                ulong maskOfTheMissingNumbersInTheRow = board.RowsArr[i] ^ (((ulong)1 << board.GetSize()) - 1);
                if (HandleBitwise.CountOneBits(maskOfTheMissingNumbersInTheRow) >= 3)
					hasAChangeoccurredInRow = CheckNakedPairsInTheRows(board, i);
                ulong maskOfTheMissingNumbersInTheColumn = board.ColsArr[i] ^ (((ulong)1 << board.GetSize()) - 1);
                if (HandleBitwise.CountOneBits(maskOfTheMissingNumbersInTheColumn) >= 3)
					hasAChangeoccurredInColumn = CheckNakedPairsInTheColumns(board, i);
				if (hasAChangeoccurredInRow || hasAChangeoccurredInColumn)
					hasBoardChanged = true;
			}
            return hasBoardChanged;
        }

        public static bool CheckNakedPairsInTheRows(Board board, int rowNum)
        {
			bool hasBoardChanged = false, hasAChangeoccurredInRow = false, hasAChangeoccurredInBox = false;
			for (int col = 0; col < board.GetSize(); col++)
			{
				if (board.BoardMatrix[rowNum, col] != 0)
					continue;
				ulong maskOfPossibleNumbers = CheckPossibleNumbersInCurrentIndex(board, rowNum, col);
				if (HandleBitwise.CountOneBits(maskOfPossibleNumbers) == 2)
				{
					int boxNum = board.GetBoxNumberByRowAndColumn(rowNum, col);
					for (int i = col + 1; i < board.GetSize(); i++)
						if (board.BoardMatrix[rowNum, i] == 0)
							if (maskOfPossibleNumbers == CheckPossibleNumbersInCurrentIndex(board, rowNum, i))
							{
								if (boxNum == board.GetBoxNumberByRowAndColumn(rowNum, i))
									hasAChangeoccurredInBox = RemoveAllThePairsInTheBox(board, maskOfPossibleNumbers, boxNum, rowNum * board.GetSize() + col, rowNum * board.GetSize() + i);
								hasAChangeoccurredInRow = RemoveAllThePairsInTheRow(board, maskOfPossibleNumbers, rowNum, col, i);
								if (hasAChangeoccurredInRow || hasAChangeoccurredInBox)
									hasBoardChanged = true;
								break;
							}
				}
			}
			return hasBoardChanged;
        }

		public static bool RemoveAllThePairsInTheRow(Board board, ulong maskOfThePair, int rowNum, int theFirstColumnNumberOfThePairInTheRow, int theSecondColumnNumberOfThePairInTheRow)
        {
			bool hasBoardChanged = false;
			for (int col = 0; col < board.GetSize(); col++)
				if(board.BoardMatrix[rowNum, col] == 0 && col != theFirstColumnNumberOfThePairInTheRow && col != theSecondColumnNumberOfThePairInTheRow)
					if (CheckIfCanAddValueInTheBoard(board, maskOfThePair, rowNum, col))
						hasBoardChanged = true;
			return hasBoardChanged;
        }

		public static bool CheckNakedPairsInTheColumns(Board board, int colNum)
        {
			bool hasBoardChanged = false, hasAChangeoccurredInColumn = false, hasAChangeoccurredInBox = false; ;
			for (int row = 0; row < board.GetSize(); row++)
			{
				if (board.BoardMatrix[row, colNum] != 0)
					continue;
				ulong maskOfPossibleNumbers = CheckPossibleNumbersInCurrentIndex(board, row, colNum);
				if (HandleBitwise.CountOneBits(maskOfPossibleNumbers) == 2)
				{
					int boxNum = board.GetBoxNumberByRowAndColumn(row, colNum);
					for (int i = row + 1; i < board.GetSize(); i++)
						if (board.BoardMatrix[i, colNum] == 0)
							if (maskOfPossibleNumbers == CheckPossibleNumbersInCurrentIndex(board, i, colNum))
							{
								if (boxNum == board.GetBoxNumberByRowAndColumn(i, colNum))
									hasAChangeoccurredInBox = RemoveAllThePairsInTheBox(board, maskOfPossibleNumbers, boxNum, row * board.GetSize() + colNum, i * board.GetSize() + colNum);
								hasAChangeoccurredInColumn = RemoveAllThePairsInTheColumn(board, maskOfPossibleNumbers, colNum, row, i);
								if (hasAChangeoccurredInColumn || hasAChangeoccurredInBox)
									hasBoardChanged = true;
								break;
							}
				}
			}
			return hasBoardChanged;
		}

		public static bool RemoveAllThePairsInTheColumn(Board board, ulong maskOfThePair, int colNum, int theFirstRowNumberOfThePairInTheColumn, int theSecondRowNumberOfThePairInTheColumn)
		{
			bool hasBoardChanged = false;
            for (int row = 0; row < board.GetSize(); row++)
                if (board.BoardMatrix[row, colNum] == 0 && row != theFirstRowNumberOfThePairInTheColumn && row != theSecondRowNumberOfThePairInTheColumn)
					if (CheckIfCanAddValueInTheBoard(board, maskOfThePair, row, colNum))
						hasBoardChanged = true;
            return hasBoardChanged;
		}

		public static bool RemoveAllThePairsInTheBox(Board board, ulong maskOfThePair, int boxNum, int theFirstLocationOfThePairInTheBox, int theSecondLocationOfThePairInTheBox)
		{
			bool hasBoardChanged = false;
			for (int row = boxNum / board.GetSubSize() * board.GetSubSize(); row < boxNum / board.GetSubSize() * board.GetSubSize() + board.GetSubSize(); row++)
				for (int col = (boxNum % board.GetSubSize()) * board.GetSubSize(); col < (boxNum % board.GetSubSize()) * board.GetSubSize() + board.GetSubSize(); col++)
					if (board.BoardMatrix[row, col] == 0 && (row * board.GetSize() + col) != theFirstLocationOfThePairInTheBox && (row * board.GetSize() + col) != theSecondLocationOfThePairInTheBox)
						if (CheckIfCanAddValueInTheBoard(board, maskOfThePair, row, col))
							hasBoardChanged = true;
			return hasBoardChanged;
		}

		public static bool CheckIfCanAddValueInTheBoard(Board board, ulong maskOfThePair, int rowNumber, int columnNumber)
        {
			ulong maskOfThePossibleNumbers = CheckPossibleNumbersInCurrentIndex(board, rowNumber, columnNumber) & (maskOfThePair ^ (((ulong)1 << board.GetSize()) - 1));
			if (HandleBitwise.CountOneBits(maskOfThePossibleNumbers) == 0)
				throw new Exceptions.UnsolvableBoardException(String.Format("No value can match the cell at location [{0}, {1}]", rowNumber, columnNumber));
			if (HandleBitwise.CountOneBits(maskOfThePossibleNumbers) == 1)
			{
				AddNewValueToTheBoard(board, maskOfThePossibleNumbers, rowNumber, columnNumber);
				return true;
			}
			return false;
		}

		public static void AddNewValueToTheBoard(Board board, ulong maskOfTheNumberToBeAddedToTheBoard, int row, int col)
		{
			board.UpdateValue(HandleBitwise.CreateNumberFromMask(maskOfTheNumberToBeAddedToTheBoard), maskOfTheNumberToBeAddedToTheBoard, row, col);
			SudokuBoardSolver.locationsOfBoardchangesStack.Push(row * board.GetSize() + col);
			HumanTechniques.countChangesInTheBoard++;
		}

		public static ulong CheckPossibleNumbersInCurrentIndex(Board board, int row, int col)
		{
			return (board.RowsArr[row] | board.ColsArr[col] | board.BoxesArr[row - (row % board.GetSubSize()) + col / board.GetSubSize()]) ^ (((ulong)1 << board.GetSize()) - 1);
		}
	}
}
