using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku.Exceptions
{
    class UnsolvableBoardException : Exception
    {
        public UnsolvableBoardException(string message)
        : base(message) { }
    }
}
