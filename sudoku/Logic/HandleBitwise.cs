﻿using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Logic
{
    public static class HandleBitwise
    {
        public static int[] BitsSetTable256;
        static HandleBitwise()
        {
            BitsSetTable256 = new int[256];
            BitsSetTable256[0] = 0;
            for (int i = 0; i < 256; i++)
            {
                BitsSetTable256[i] = (i & 1) + BitsSetTable256[i / 2];
            }
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