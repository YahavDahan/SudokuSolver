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
				if (!(hiddenSingles || nakedSingles))
					break;
			}
			return HumanTechniques.countChangesInTheBoard;
        }

		//public static bool CompletionTechniqueToFull(Board board)
		//{
		//	bool hasChanged = false;
		//	for (int i = 0; i < board.GetSize(); i++)
		//	{
		//		ulong maskOfTheMissingNumbersInTheRow = board.RowsArr[i] ^ (((ulong)1 << board.GetSize()) - 1);
		//		if (HandleBitwise.IsPowerOfTwo(maskOfTheMissingNumbersInTheRow))
		//		{
		//			CompleteTheMissingNumberInTheRow(board, maskOfTheMissingNumbersInTheRow, i);
		//			hasChanged = true;
		//		}
		//		ulong maskOfTheMissingNumbersInTheColumn = board.ColsArr[i] ^ (((ulong)1 << board.GetSize()) - 1);
		//		if (HandleBitwise.IsPowerOfTwo(maskOfTheMissingNumbersInTheColumn))
		//		{
		//			CompleteTheMissingNumberInTheColumn(board, maskOfTheMissingNumbersInTheColumn, i);
		//			hasChanged = true;
		//		}
		//		ulong maskOfTheMissingNumbersInTheBox = board.BoxesArr[i] ^ (((ulong)1 << board.GetSize()) - 1);
		//		if (HandleBitwise.IsPowerOfTwo(maskOfTheMissingNumbersInTheBox))
		//		{
		//			CompleteTheMissingNumberInTheBox(board, maskOfTheMissingNumbersInTheBox, i);
		//			hasChanged = true;
		//		}
		//	}
		//	return hasChanged;
		//}

		//public static void CompleteTheMissingNumberInTheRow(Board board, ulong maskOfTheMissingNumbersInTheRow, int rowNum)
		//{
		//	int theMissingNumber = HandleBitwise.CreateNumberFromMask(maskOfTheMissingNumbersInTheRow);
		//	for (int col = 0; col < board.GetSize(); col++)
		//		if (board.BoardMatrix[rowNum, col] == 0)
		//		{
		//			board.UpdateValue(theMissingNumber, maskOfTheMissingNumbersInTheRow, rowNum, col);
		//			return;
		//		}
		//}

		//public static void CompleteTheMissingNumberInTheColumn(Board board, ulong maskOfTheMissingNumbersInTheColumn, int colNum)
		//{
		//	int theMissingNumber = HandleBitwise.CreateNumberFromMask(maskOfTheMissingNumbersInTheColumn);
		//	for (int row = 0; row < board.GetSize(); row++)
		//		if (board.BoardMatrix[row, colNum] == 0)
		//		{
		//			board.UpdateValue(theMissingNumber, maskOfTheMissingNumbersInTheColumn, row, colNum);
		//			return;
		//		}
		//}

		//public static void CompleteTheMissingNumberInTheBox(Board board, ulong maskOfTheMissingNumbersInTheBox, int boxNum)
		//{
		//	int theMissingNumber = HandleBitwise.CreateNumberFromMask(maskOfTheMissingNumbersInTheBox);
		//	for (int row = boxNum / board.GetSubSize() * board.GetSubSize(); row < boxNum / board.GetSubSize() * board.GetSubSize() + board.GetSubSize(); row++)
  //          {
		//		for (int col = (boxNum % board.GetSubSize()) * board.GetSubSize(); col < (boxNum % board.GetSubSize()) * board.GetSubSize() + board.GetSubSize(); col++)
  //              {
		//			if (board.BoardMatrix[row, col] == 0)
		//			{
		//				board.UpdateValue(theMissingNumber, maskOfTheMissingNumbersInTheBox, row, col);
		//				return;
		//			}
		//		}
		//	}
		//}

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

		public static ulong CheckPossibleNumbersInCurrentIndex(Board board, int row, int col)
		{
			return (board.RowsArr[row] | board.ColsArr[col] | board.BoxesArr[row - (row % board.GetSubSize()) + col / board.GetSubSize()]) ^ (((ulong)1 << board.GetSize()) - 1);
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
			int boxNumber = (row / board.GetSubSize() * board.GetSubSize()) + (col / board.GetSubSize());
			ulong resultOfAllThePossibleNumbersInTheCurrentBox = 0;
			for (int i = boxNumber / board.GetSubSize() * board.GetSubSize(); i < boxNumber / board.GetSubSize() * board.GetSubSize() + board.GetSubSize(); i++)
				for (int j = (boxNumber % board.GetSubSize()) * board.GetSubSize(); j < (boxNumber % board.GetSubSize()) * board.GetSubSize() + board.GetSubSize(); j++)
  					if (board.BoardMatrix[i, j] == 0 && (i != row || j != col))
						resultOfAllThePossibleNumbersInTheCurrentBox |= CheckPossibleNumbersInCurrentIndex(board, i, j);
			return resultOfAllThePossibleNumbersInTheCurrentBox;
		}

		public static void AddNewValueToTheBoard(Board board, ulong maskOfTheNumberToBeAddedToTheBoard, int row, int col)
        {
			board.UpdateValue(HandleBitwise.CreateNumberFromMask(maskOfTheNumberToBeAddedToTheBoard), maskOfTheNumberToBeAddedToTheBoard, row, col);
			SudokuBoardSolver.locationsOfBoardchangesStack.Push(row * board.GetSize() + col);
			HumanTechniques.countChangesInTheBoard++;
		}
	}
}
