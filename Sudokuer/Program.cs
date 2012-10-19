using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Sudokuer
{
    //class Sudoku
    //{
    //    private int x;
    //    private int y;
    //    public int[,] arr;
    //    private int startingLine;

    //    public int X
    //    {
    //        get { return x; }
    //    }

    //    public int Y
    //    {
    //        get { return y; }
    //    }

    //    public int currentLoc
    //    {
    //        get { return arr[x,y];}
    //        set { arr[x,y] = value; }
    //    }

    //    public Sudoku()
    //    {
    //        x = 0;
    //        y = 0;
    //        arr = new int[9, 9];
    //    }

    //    public bool Advance()
    //    {
    //        y++;
    //        if (y == 9)
    //        {
    //            y = 0;
    //            x++;
    //        }
    //        if (x == 9)
    //        {
    //            x = 0;
    //        }
    //        if (x == startingLine && y == 0)
    //        {
    //            return false;
    //        }
    //        else
    //        {
    //            return true;
    //        }
    //    }

    //    static public Point Advance(Point pt)
    //    {
    //        if (pt.Y < 8)
    //        {
    //            return new Point(pt.X, pt.Y + 1);
    //        }
    //        else
    //        {
    //            return new Point((pt.X + 1) % 9, 0);
    //        }
    //    }

    //    static public bool Check(int[,] intArr)
    //    {
    //        bool[] array;
    //        for (int i = 0; i < 9; ++i)
    //        {
    //            array = new bool[10];
    //            for (int j = 0; j < 9; ++j)
    //            {
    //                if (!array[intArr[i, j]])
    //                {
    //                    array[intArr[i, j]] = true;
    //                }
    //                else
    //                {
    //                    Console.WriteLine(String.Format("x {0} y {1}", i, j));
    //                    return false;
    //                }
    //            }
    //        }

    //        for (int i = 0; i < 9; ++i)
    //        {
    //            array = new bool[10];
    //            for (int j = 0; j < 9; ++j)
    //            {
    //                if (!array[intArr[i, j]])
    //                {
    //                    array[intArr[i, j]] = true;
    //                }
    //                else
    //                {
    //                    Console.WriteLine(String.Format("x {0} y {1}", i, j));
    //                    return false;
    //                }
    //            }
    //        }

    //        for (int i1 = 0; i1 < 9; i1 += 3)
    //            {
    //                for (int j1 = 0; j1 < 9; j1 += 3)
    //                {
    //                    array = new bool[10];
    //                    for (int i = i1; i < i1 + 3; ++i)
    //                    {
    //                        for (int j = j1; j < j1 + 3; ++j)
    //                        {
    //                            if (!array[intArr[i, j]])
    //                            {
    //                                array[intArr[i, j]] = true;
    //                            }
    //                            else
    //                            {
    //                                Console.WriteLine(String.Format("x {0} y {1}", i, j));
    //                                return false;
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        return true;
    //    }
    //}

    class Program
    {
        //static Sudoku loc = new Sudoku();
        static Point starting;
        static bool visited;
        static int[,] solution;
        static DateTime deadLine;
        static bool genMode;

        /// <summary>
        /// Move pt forward (top down, left right) and wrap around at the lower right corner
        /// </summary>
        /// <param name="pt">current location</param>
        /// <returns>next location</returns>
        static public Point Advance(Point pt)
        {
            if (pt.Y < 8)
            {
                return new Point(pt.X, pt.Y + 1);
            }
            else
            {
                return new Point((pt.X + 1) % 9, 0);
            }
        }

        /// <summary>
        /// Check a 9x9 grid whether it's a valid sudoku solution or not
        /// </summary>
        /// <param name="intArr">9x9 grid</param>
        /// <returns>true if valid, false if not</returns>
        static public bool Check(int[,] intArr)
        {
            bool[] array;
            for (int i = 0; i < 9; ++i)
            {
                array = new bool[10];
                for (int j = 0; j < 9; ++j)
                {
                    if (!array[intArr[i, j]])
                    {
                        array[intArr[i, j]] = true;
                    }
                    else
                    {
                        Console.WriteLine(String.Format("x {0} y {1}", i, j));
                        return false;
                    }
                }
            }

            for (int i = 0; i < 9; ++i)
            {
                array = new bool[10];
                for (int j = 0; j < 9; ++j)
                {
                    if (!array[intArr[i, j]])
                    {
                        array[intArr[i, j]] = true;
                    }
                    else
                    {
                        Console.WriteLine(String.Format("x {0} y {1}", i, j));
                        return false;
                    }
                }
            }

            for (int i1 = 0; i1 < 9; i1 += 3)
            {
                for (int j1 = 0; j1 < 9; j1 += 3)
                {
                    array = new bool[10];
                    for (int i = i1; i < i1 + 3; ++i)
                    {
                        for (int j = j1; j < j1 + 3; ++j)
                        {
                            if (!array[intArr[i, j]])
                            {
                                array[intArr[i, j]] = true;
                            }
                            else
                            {
                                Console.WriteLine(String.Format("x {0} y {1}", i, j));
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Copy array
        /// </summary>
        /// <param name="inArr">source</param>
        /// <param name="outArr">destination</param>
        static void CopyArr(int[,] inArr, ref int[,] outArr)
        {
            if (outArr == null)
            {
                outArr = new int[9, 9];
            }

            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    outArr[i, j] = inArr[i, j];
                }
            }
        }

        /// <summary>
        /// Check if a number is valid for a row
        /// </summary>
        /// <param name="arr">array to check against</param>
        /// <param name="x">x coordinate of the point to check</param>
        /// <param name="y">y coordinate of the point to check</param>
        /// <param name="val">value to check</param>
        /// <returns>true if a valid move, false if not a valid move</returns>
        static bool CheckRow(int[,] arr, int x, int y, int val)
        {
            if (arr.Length != 81)
            {
                throw new Exception("Array size mismatch");
            }

            for (int i = 0; i < 9; ++i)
            {
                if (arr[x,i] == val)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check if a number is valid for a collumn
        /// </summary>
        /// <param name="arr">array to check against</param>
        /// <param name="x">x coordinate of the point to check</param>
        /// <param name="y">y coordinate of the point to check</param>
        /// <param name="val">value to check</param>
        /// <returns>true if a valid move, false if not a valid move</returns>
        static bool CheckCol(int[,] arr, int x, int y, int val)
        {
            if (arr.Length != 81)
            {
                throw new Exception("Array size mismatch");
            }

            for (int i = 0; i < 9; ++i)
            {
                if (arr[i, y] == val)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check if a value is valid for a smaller square
        /// </summary>
        /// <param name="arr">array to check against</param>
        /// <param name="x">x coordinate of the point to check</param>
        /// <param name="y">y coordinate of the point to check</param>
        /// <param name="val">value to check</param>
        /// <returns>true if a valid move, false if not a valid move</returns>
        static bool CheclSq(int[,] arr, int x, int y, int val)
        {
            if (arr.Length != 81)
            {
                throw new Exception("Array size mismatch");
            }

            int n = ((int)Math.Floor((double)x / 3)) * 3;
            int m = ((int)Math.Floor((double)y / 3)) * 3;

            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (arr[n + i, m + j] == val)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Recursive function used to find solution
        /// </summary>
        /// <param name="arr">(in)complete 9x9 array</param>
        /// <param name="loc">location to look at</param>
        /// <returns>true if solved, false if can not be solved</returns>
        static bool Solve(int[,] arr, Point loc)
        {
            if (genMode && DateTime.Now > deadLine)
            {
                return false;
            }
            if (loc == starting && visited)
            {
                CopyArr(arr, ref solution);
                return true;
            }
            if (loc == starting && !visited)
            {
                visited = true;
            }
            while (arr[loc.X, loc.Y] != 0)
            {
                loc = Advance(loc);
            }
            for (int i = 1; i <= 9; ++i)
                {
                    if (CheckCol(arr, loc.X, loc.Y, i) &&
                        CheckRow(arr, loc.X, loc.Y, i) &&
                        CheclSq(arr, loc.X, loc.Y, i))
                    {
                        arr[loc.X, loc.Y] = i;
                        if (Solve(arr, Advance(loc)))
                        {
                            return true;
                        }
                        else
                        {
                            arr[loc.X, loc.Y] = 0;
                        }
                    }
                }
            
            return false;
        }

        /// <summary>
        /// Display the array onto console
        /// </summary>
        /// <param name="arr">array to display</param>
        static void PrintArr(int[,] arr)
        {
            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    Console.Write(arr[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Read file for 9x9 array of (in)complete sudoku puzzle
        /// </summary>
        /// <param name="path">path to file</param>
        /// <returns>9x9 array contains (in)complete sudoku puzzle</returns>
        static int[,] ReadFile(string path)
        {
            int[,] arr = new int[9, 9];

            System.IO.StreamReader file = new System.IO.StreamReader(path);
            for (int i = 0; i < 9; ++i)
            {
                string str = file.ReadLine();
                string[] splitted = str.Split(' ');
                for (int j = 0; j < 9; ++j)
                {
                    int n = Convert.ToInt32(splitted[j]);
                    if ((n < 0) || (n > 9))
                    {
                        throw new Exception("File contain non-digit number");
                    }
                    arr[i, j] = n;
                }
            }
            file.Close();

            return arr;
        }

        /// <summary>
        /// Main code
        /// </summary>
        /// <param name="args">arguments</param>
        static void Main(string[] args)
        {
            int[,] arr = new int[9, 9];
            genMode = false;

            if (args.Length != 2)
            {
                Console.WriteLine("arguments:");
                Console.WriteLine("c <input file> to check");
                Console.WriteLine("s <input file> to solve");
                Console.WriteLine("g <difficult lvl> to generate");
                return;
            }

            if (args[0] == "c")
            {
                arr = ReadFile(args[1]);

                Console.WriteLine("File " + args[1] + " read successfully!");
                PrintArr(arr);

                Console.WriteLine("Check mode: " + args[1]);

                for (int i = 0; i < 9; ++i)
                {
                    for (int j = 0; j < 9; ++j)
                    {
                        Console.Write(arr[i, j] + " ");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine(Check(arr));

                for (int i = 0; i < 9; ++i)
                {
                    for (int j = 0; j < 9; ++j)
                    {
                        int n = arr[i, j];
                        arr[i, j] = 0;
                        Console.WriteLine(String.Format("x{0}y{1} {2}", i, j, CheclSq(arr, i, j, n)));
                        arr[i, j] = n;
                    }
                }
                        
                
            }

            if (args[0] == "s")
            {
                arr = ReadFile(args[1]);

                Console.WriteLine("File " + args[1] + " read successfully!");
                PrintArr(arr);

                Console.WriteLine("Solve mode: " + args[1]);

                DateTime start = DateTime.Now;
                Console.WriteLine(SolveSudoku(arr));
                PrintArr(solution);
                Console.WriteLine("Runtime: " + (DateTime.Now - start));
            }

            if (args[0] == "g")
            {
                genMode = true;
                int diffLvl = Convert.ToInt32(args[1]);
                Console.WriteLine("Generate mode. Difficulty level: " + diffLvl);
                if (diffLvl < 1 || diffLvl > 30)
                {
                    throw new Exception("Difficult lvl can only be between 1(hardest) and 30(easiest) inclusively");
                }

                arr = new int[9, 9];
                Random rand = new Random();
                bool pass;
                do
                {
                    rand = new Random();
                    arr = new int[9, 9];
                    deadLine = DateTime.Now.AddSeconds(5);
                    for (int i = 0; i < 17; ++i)
                    {
                        int x, y, t;
                        do
                        {
                            x = rand.Next(9);
                            y = rand.Next(9);
                        } while (arr[x, y] != 0);
                        do
                        {
                            t = 1 + rand.Next(9);
                        } while (!(CheckCol(arr, x, y, t) && CheckRow(arr, x, y, t) && CheclSq(arr, x, y, t)));
                        arr[x, y] = t;
                    }

                    PrintArr(arr);
                    pass = SolveSudoku(arr);
                } while (!pass);

                Console.WriteLine("Solution: ");
                PrintArr(solution);
                    
                    diffLvl += 17;
                    for (int i = 0; i < diffLvl; ++i)
                    {
                        int x, y;
                        do
                        {
                            x = rand.Next(9);
                            y = rand.Next(9);
                        } while (arr[x, y] < 0);
                        arr[x, y] = -arr[x, y];
                    }

                Console.WriteLine("Puzzle:");

                for (int i = 0; i < 9; ++i)
                {
                    for (int j = 0; j < 9; ++j)
                    {
                        Console.Write((arr[i, j] < 0 ? -arr[i,j] : 0) + " ");
                    }
                    Console.WriteLine();
                }

            }
            Console.ReadLine();
        }

        private static bool SolveSudoku(int[,] arr)
        {
            int max = 0;
            int maxLine = -1;
            for (int i = 0; i < 9; ++i)
            {
                int cnt = 0;
                for (int j = 0; j < 9; ++j)
                {
                    if (arr[i, j] != 0)
                    {
                        cnt++;
                    }
                }
                if (cnt > max)
                {
                    max = cnt;
                    maxLine = i;
                }
            }

            for (int i = 0; i < 9; ++i)
            {
                if (arr[maxLine, i] != 0)
                {
                    starting = new Point(maxLine, i);
                    break;
                }
            }
            visited = false;
            return Solve(arr, starting);
        }

        /// <summary>
        /// Addapted from algorithm at http://en.wikipedia.org/wiki/Talk:Sudoku_algorithms#worst_case_scenario_compute_timing
        /// by unsigned user as a benchmark. 
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        private static bool SolveSuperSudoku(int[,] src)
        {
            IntStack iStack = new IntStack(81);
            IntStack jStack = new IntStack(81);

            int[,] dest = new int[9, 9];
            int i,j,k;

            CopyArr(src, ref dest);

            for (i = 0; i < 9; ++i)
            {
                for (j = 0; j < 9; ++j)
                {
                    if (src[i,j] == 0)
                    {
                        for (k = dest[i,j] + 1; k < 10; ++k)
                        {
                            if (CheckCol(dest, i, j, k) && CheckRow(dest, i, j, k) && CheclSq(dest, i, j, k))
                            {
                                dest[i, j] = k;
                                break;
                            }
                        }
                        if (k == 10)
                        {
                            dest[i, j] = 0;
                            i = iStack.Pop();
                            j = jStack.Pop();
                            j--;
                        }
                        else
                        {
                            iStack.Push(i);
                            jStack.Push(j);
                        }
                    }
                }
            }
            PrintArr(dest);
            return true;
        }
    }
}