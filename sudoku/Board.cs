using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku
{
    public class Board
    {
        // matrix that representing the sudoku board
        private int[,] boardMatrix;

        // the number of rows in the board
        private int size;

        // the number of boxes on the board
        private int subSize;

        // מערכים להגדלת היעילות (ביט בורד)  נ
        private ulong[] rowsArr;
        
        // 
        private ulong[] colsArr;
        
        // 
        private ulong[] boxesArr;

        public Board(string strBoard)
        {
            // if the sqrt of the string length is bigger then 64 (the number of bits in ulong type)
            // or the string's length is invalid
            if (!Logic.HandleString.IsValidLengthToCreateSudokuBoard(strBoard))
                throw new System.ArgumentOutOfRangeException("The string length is incorrect for creating a sudoku board");
            this.size = (int)(Math.Sqrt(strBoard.Length));
            this.subSize = (int)(Math.Sqrt(this.size));
            if (!Logic.HandleString.AreAllTheCharactersAsciiCodeValid(strBoard, this.size))
                throw new Exceptions.AsciiCharacterOutOfRangeException(String.Format("One of the characters is invalid to create a {0}X{0} board", this.size));
            this.boardMatrix = new int[this.size, this.size];
            this.rowsArr = new ulong[this.size];
            this.colsArr = new ulong[this.size];
            this.boxesArr = new ulong[this.size];
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    int numberToSave = Logic.HandleString.ConvertCharToIntegerTypeAsNumber(strBoard[i * this.size + j]);
                    ulong maskOfTheNumber = Logic.HandleBitwise.CreateMaskFromNumber(numberToSave);
                    if (numberToSave != 0 && !IsNumberValidInThisLocation(maskOfTheNumber, i, j))
                            throw new Exceptions.NumberLocationException(String.Format("The character in location {0} in the string is invalid in this location", (i * this.size + j)));
                    UpdateValue(numberToSave, maskOfTheNumber, i, j);
                }
            }
        }

        public bool IsNumberValidInThisLocation(ulong maskOfTheNumberForChecking, int row, int col)
        {
            if ((maskOfTheNumberForChecking & this.rowsArr[row]) != 0)
                return false;
            if ((maskOfTheNumberForChecking & this.colsArr[col]) != 0)
                return false;
            if ((maskOfTheNumberForChecking & this.boxesArr[row - (row % this.subSize) + col / this.subSize]) != 0)
                return false;
            return true;
        }

        public void UpdateValue(int valueForUpdate, ulong maskOfTheValueForUpdate, int row, int col)
        {
            this.boardMatrix[row, col] = valueForUpdate;
            this.rowsArr[row] |= maskOfTheValueForUpdate;
            this.colsArr[col] |= maskOfTheValueForUpdate;
            this.boxesArr[row - (row % this.subSize) + col / this.subSize] |= maskOfTheValueForUpdate;
        }

        public void RemoveValue(ulong maskOfTheValueToRemove, int row, int col)
        {
            this.boardMatrix[row, col] = 0;
            maskOfTheValueToRemove ^= (((ulong)1 << this.size) - 1);
            this.rowsArr[row] &= maskOfTheValueToRemove;
            this.colsArr[col] &= maskOfTheValueToRemove;
            this.boxesArr[row - (row % this.subSize) + col / this.subSize] &= maskOfTheValueToRemove;
        }

        public ulong[] RowsArr {
            get { return this.rowsArr; }
            set { this.rowsArr = value; }
        }

        public ulong[] ColsArr
        {
            get { return this.colsArr; }
            set { this.colsArr = value; }
        }
        public ulong[] BoxesArr
        {
            get { return this.boxesArr; }
            set { this.boxesArr = value; }
        }

        public int GetBoxNumberByRowAndColumn(int rowNumber, int columnNumber)
        {
            return (rowNumber / this.subSize * this.subSize) + (columnNumber / this.subSize);
        }

        public int[,] BoardMatrix
        {
            get { return this.boardMatrix; }
            set { this.boardMatrix = value; }
        }

        public int GetSize()
        {
            return this.size;
        }

        public int GetSubSize()
        {
            return this.subSize;
        }
    }
}
