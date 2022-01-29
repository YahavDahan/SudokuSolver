using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Logic
{
    public static class HumanTechniques
    {
		public static bool CompletionTechniqueToFull(Board board)
		{
			bool hasChanged = false;
			for (int i = 0; i < board.GetSize(); i++)
			{
				ulong theMissingNumbersInTheRow = board.RowsArr[i] ^ (((ulong)1 << board.GetSize()) - 1);
				if (IsPowerOfTwo(theMissingNumbersInTheRow))
				{
					// CreateNumberFromMask(theMissingNumbersInTheRow);
					hasChanged = true;
				}
			}
			return hasChanged;
		}

		public static bool HiddenSinglesTechnique(Board board)
		{  // מחזיר אמת אם התרחשו שינויים בלוח. אחרת מחזיר שקר. מחזיר שגיאה אם הלוח לא פתיר
			bool hasChanged = false;
			for (int i = 0; i < board.GetSize(); i++)
				for (int j = 0; j < board.GetSize(); j++)
					if (board.BoardMatrix[i, j] == 0)
					{
						ulong possibleNumbers = CheckPossibleNumbersInCurrentIndex(board, i, j);
						if (possibleNumbers == 0)
							throw;
						if (IsPowerOfTwo(possibleNumbers))
						{
							board.UpdateValue(CreateNumberFromMask(possibleNumbers), possibleNumbers, i, j);
							hasChanged = true;
						}
					}
			return hasChanged;
		}

		public static bool IsPowerOfTwo(ulong number)
		{
			return (number != 0) && ((number & (number - 1)) == 0);
		}

		public static ulong CheckPossibleNumbersInCurrentIndex(Board board, int row, int col)
		{
			return (board.RowsArr[row] | board.ColsArr[col] | board.BoxesArr[row - (row % board.GetSubSize()) + col / board.GetSubSize()]) ^ (((ulong)1 << board.GetSize()) - 1);
		}

		public static int CreateNumberFromMask(ulong maskForCreatingNumber)
		{
			return Log2ToNumber(maskForCreatingNumber) + 1;
		}

		public static int Log2ToNumber(ulong number)
		{
			return (number > 1) ? 1 + Log2ToNumber(number / 2) : 0;
		}

	}
}
