using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Exceptions
{
    public class AsciiCharacterOutOfRangeException : Exception
    {
        public AsciiCharacterOutOfRangeException(string message)
        : base(message) { }

        //public override string ToString()
        //{
        //    return Message;
        //}
    }
}
