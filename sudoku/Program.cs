using System;

namespace sudoku
{
    class Program
    {
        public static int InputNumber()
        {
            int theChosenNumber;
            try
            {
                theChosenNumber = int.Parse(Console.ReadLine());
            }
            catch (System.FormatException)
            {
                theChosenNumber = -1;
            }
            return theChosenNumber;
        }

        public static int GetNumberToIdentifyTheBoardInputWay()
        {
            Console.WriteLine("Do you want to insert a sudoku board through a text file or through the console?");
            Console.Write("Enter 1 to insert the board through a text file and 2 to insert the board through the console");
            int theChosenNumber = InputNumber();
            while (theChosenNumber != 1 && theChosenNumber != 2)
            {
                Console.WriteLine("You have to choose the number 1 or 2");
                theChosenNumber  = InputNumber();
            }
            return theChosenNumber;
        }

        public static string InputStringBoard(int number)
        {
            string strBoard;
            if (number == 1)
                strBoard = InsertBoardThroughTextFile();
            else
                strBoard = InsertBoardThroughConsole();
            return strBoard;
        }

        public static Board CreateNewBoard(string strBoard)
        {
            Board sudokuBoard;
            try
            {
                sudokuBoard = new Board(strBoard);
            }
            catch (Exception e) when (e is System.ArgumentOutOfRangeException || e is AsciiCharacterOutOfRangeException || e is NumberLocationException)
            {
                Console.WriteLine("The inserted string is invalid to create a sudoku board.");
                Console.WriteLine("reason:  " + e.Message + "\nplease enter new board. \n\n");
                sudokuBoard = null;
            }
            return sudokuBoard;
        }

        public static void OutputSudokuBoard(Board boardForOutput, int number)
        {
            if (number == 1)
                PrintBoardThroughTextFile(boardForOutput);
            else
                PrintBoardThroughConsole(boardForOutput);
        }

        public static void SudokuGame()
        {
            Board sudokuBoard;
            int number;
            do
            {
                number = GetNumberToIdentifyTheBoardInputWay();
                string strBoard = InputStringBoard(number);
                sudokuBoard = CreateNewBoard(strBoard);
            } while (sudokuBoard == null);
            if (SudokuBoardSolver.Solver(sudokuBoard))
                OutputSudokuBoard(sudokuBoard, number);
            else
                Console.WriteLine("The inserted board is unsolvable");
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
// הרשאות גישה לפונקציות
// טסטים
// לחבר פרויקט לגיט
// לבדוק שאין חזרות על קטעי קוד
// האם לכל המחלקות הסטטיות צריך להוסיף אבסטרקט?
// הדפסת זמן פתירה
// inputs and outputs

// האם צריך מחלקה סולבר או שהכל במחלקת לוח
// Git
// איפה מחלקות קלט ופלט
// מצאתם דרכים לייעל?
