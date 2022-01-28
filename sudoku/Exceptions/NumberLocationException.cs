using System;
using System.Collections.Generic;
using System.Text;

namespace sudoku
{
    class NumberLocationException : Exception
    {
        public NumberLocationException(string message)
        : base(message) { }
    }
}
