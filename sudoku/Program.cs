using System;

namespace sudoku
{
    class Program
    {
        //public static string InputStringBoard(int number)
        //{
        //    string strBoard;
        //    if (number == 1)
        //        strBoard = InsertBoardThroughTextFile();
        //    else
        //        strBoard = InsertBoardThroughConsole();
        //    return strBoard;
        //}

        public static Board CreateNewBoard(string strBoard)
        {
            Board sudokuBoard;
            try
            {
                sudokuBoard = new Board(strBoard);
            }
            catch (Exception e) when (e is System.ArgumentOutOfRangeException || e is Exceptions.AsciiCharacterOutOfRangeException || e is Exceptions.NumberLocationException)
            {
                Console.WriteLine("The inserted string is invalid to create a sudoku board.");
                Console.WriteLine("reason:  " + e.Message + "\nplease enter new board. \n\n");
                sudokuBoard = null;
            }
            return sudokuBoard;
        }

        //public static void OutputSudokuBoard(Board boardForOutput, int number)
        //{
        //    if (number == 1)
        //        PrintBoardThroughTextFile(boardForOutput);
        //    else
        //        PrintBoardThroughConsole(boardForOutput);
        //}

        public static void SudokuGame()
        {
            Board sudokuBoard;
            Input_and_Output.MainBoardIO inputOutputObj;
            do
            {
                inputOutputObj = new Input_and_Output.MainBoardIO();
                string strBoard = inputOutputObj.InputStringBoard();
                sudokuBoard = CreateNewBoard(strBoard);
            } while (sudokuBoard == null);
            TimeSpan theTimeBeforeTheSolving = DateTime.Now.TimeOfDay;
            if (Logic.SudokuBoardSolver.Solver(sudokuBoard))
            {
                TimeSpan theTimeAfterTheSolving = DateTime.Now.TimeOfDay;
                inputOutputObj.OutputSudokuBoard(sudokuBoard);
                Console.WriteLine(String.Format("The time it took to solve the sudoku board:  {0} \n\n", theTimeAfterTheSolving - theTimeBeforeTheSolving));
            }
            else
                Console.WriteLine("The inserted board is unsolvable \n\n");
        }

        static void Main(string[] args)
        {
            while (true)
                SudokuGame();
        }
    }
}

// שמות משמעותיים
// הערות (לראות שאין בעברית) + לרשום על כל פונקציה סיבוכיות זמן ריצה
// הרשאות גישה לפונקציות האם כל הפעולות הסטטיות הן פבליק?  מה המשמעות של פעולה פריבט סטטיק?
// טסטים
// לחבר פרויקט לגיט
// לבדוק שאין חזרות על קטעי קוד

// האם יש מחרוזות בקונסול שיכולות להקריס
