using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Exceptions
{
    public class NumberLocationException : Exception
    {
        public NumberLocationException(string message)
        : base(message) { }
    }
}
