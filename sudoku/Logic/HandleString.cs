using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Logic
{
    public static class HandleString
    {
        public static int ConvertCharToIntegerTypeAsNumber(char tavToConvert)
        {
            return (int)(tavToConvert - '0');
        }

        public static bool IsValidLengthToCreateSudokuBoard(string strBoard)
        {
            double sqrtOfStrBoardLength = Math.Sqrt(strBoard.Length);
            if (strBoard.Length > 4096 || sqrtOfStrBoardLength - Math.Floor(sqrtOfStrBoardLength) != 0 ||
                Math.Sqrt(sqrtOfStrBoardLength) - Math.Floor(Math.Sqrt(sqrtOfStrBoardLength)) != 0)
                return false;
            return true;
        }

        public static bool AreAllTheCharactersAsciiCodeValid(string strBoard, int theNumberOfTheValidCharacters)
        {
            for (int i = 0; i < strBoard.Length; i++)
                if (strBoard[i] < '0' || strBoard[i] > ('0' + theNumberOfTheValidCharacters))
                    return false;
            return true;
        }
    }
}
