using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Logic
{
    public static class HumanTechniques
    {
		public static bool SolveWithHumanTechniques(Board boardToSolve)
        {
			bool hasChanged = false;
			while (true)
            {
				bool completionToFull = CompletionTechniqueToFull(boardToSolve);
				bool nakedSingles = NakedSinglesTechnique(boardToSolve);
				bool hiddenSingles = HiddenSinglesTechnique(boardToSolve);
				if (completionToFull || hiddenSingles || nakedSingles)
					hasChanged = true;
				if (!(completionToFull || hiddenSingles || nakedSingles))
					break;
			}
			return hasChanged;
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
			bool hasChanged = false;
			for (int row = 0; row < board.GetSize(); row++)
				for (int col = 0; col < board.GetSize(); col++)
					if (board.BoardMatrix[row, col] == 0)
					{
						ulong possibleNumbers = CheckPossibleNumbersInCurrentIndex(board, row, col);
						if (possibleNumbers == 0)
							throw new Exceptions.UnsolvableBoardException(String.Format("No value can match the cell at location [{0}, {1}]", row, col));
						if (HandleBitwise.IsPowerOfTwo(possibleNumbers))
						{
							board.UpdateValue(HandleBitwise.CreateNumberFromMask(possibleNumbers), possibleNumbers, row, col);
							hasChanged = true;
						}
					}
			return hasChanged;
		}

		public static ulong CheckPossibleNumbersInCurrentIndex(Board board, int row, int col)
		{
			return (board.RowsArr[row] | board.ColsArr[col] | board.BoxesArr[row - (row % board.GetSubSize()) + col / board.GetSubSize()]) ^ (((ulong)1 << board.GetSize()) - 1);
		}

		public static bool HiddenSinglesTechnique(Board board)
        {
			bool hasChanged = false;
			for (int row = 0; row < board.GetSize(); row++)
				for (int col = 0; col < board.GetSize(); col++)
					if (board.BoardMatrix[row, col] == 0)
					{
						ulong possibleNumbersOfAllTheCellsInTheCurrentBox = PossibleNumbersInCurrentBox(board, row, col);
						ulong notPossibleNumbersOfTheCurrentCell = board.RowsArr[row] | board.ColsArr[col]
							| board.BoxesArr[row - (row % board.GetSubSize()) + col / board.GetSubSize()];
						ulong result = (possibleNumbersOfAllTheCellsInTheCurrentBox | notPossibleNumbersOfTheCurrentCell) ^ (((ulong)1 << board.GetSize()) - 1);
						if (result != 0)
						{
							if (HandleBitwise.IsPowerOfTwo(result))
							{
								board.UpdateValue(HandleBitwise.CreateNumberFromMask(result), result, row, col);
								hasChanged = true;
							}
							else
								throw new Exceptions.UnsolvableBoardException(String.Format("more then one number must appear in location [{0}, {1}]", row, col));
						}
					}
			return hasChanged;
		}

		public static ulong PossibleNumbersInCurrentBox(Board board, int row, int col)
        {  // o(n)  לבדוק את הפונקציה ולשנות את מספר הקופסה
			int boxNumber = 1;
			ulong resultOfAllThePossibleNumbersInTheCurrentBox = 0;
			for (int i = boxNumber / board.GetSubSize() * board.GetSubSize(); i < boxNumber / board.GetSubSize() * board.GetSubSize() + board.GetSubSize(); i++)
				for (int j = (boxNumber % board.GetSubSize()) * board.GetSubSize(); j < (boxNumber % board.GetSubSize()) * board.GetSubSize() + board.GetSubSize(); j++)
  					if (board.BoardMatrix[i, j] == 0 && (i != row || j != col))
						resultOfAllThePossibleNumbersInTheCurrentBox |= CheckPossibleNumbersInCurrentIndex(board, i, j);
			return resultOfAllThePossibleNumbersInTheCurrentBox;
		}
	}
}
