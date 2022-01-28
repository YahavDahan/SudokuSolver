using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku
{
    class HandleMatrix
    {
        public static int[,] StringToIntegerSquareMatrix(string stringToConvert, int numOfRows)
        {
            // מקבל מחרוזת באורך ריבועי ומחזיר מטריצה ריבועית המייצגת את המחרוזת
            int[,] mat = new int[numOfRows, numOfRows];
            for (int i = 0; i < numOfRows; i++)
                for (int j = 0; j < numOfRows; j++)
                    mat[i, j] = HandleString.ConvertCharToIntegerTypeAsNumber(stringToConvert[i * numOfRows + j]);
            return mat;
        }
    }
}
