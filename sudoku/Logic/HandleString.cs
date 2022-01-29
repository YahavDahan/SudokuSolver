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
    }
}
