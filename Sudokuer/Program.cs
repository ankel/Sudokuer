using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Sudokuer
{
    class Sudoku
    {
        private int x;
        private int y;
        public int[,] arr;
        private int startingLine;

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        public int currentLoc
        {
            get { return arr[x,y];}
            set { arr[x,y] = value; }
        }

        public Sudoku()
        {
            x = 0;
            y = 0;
            arr = new int[9, 9];
        }

        public bool Advance()
        {
            y++;
            if (y == 9)
            {
                y = 0;
                x++;
            }
            if (x == 9)
            {
                x = 0;
            }
            if (x == startingLine && y == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

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
    }

    class Program
    {
        static Sudoku loc = new Sudoku();
        static Point starting;
        static bool visited;

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

        static bool Solve(int[,] arr, Point loc)
        {
            if (loc == starting && visited)
            {
                PrintArr(arr);
                return true;
            }
            if (loc == starting && !visited)
            {
                visited = true;
            }
            if (arr[loc.X, loc.Y] != 0)
            {
                if (Solve(arr, Sudoku.Advance(loc)))
                {
                    return true;
                }
            }
            else
            {
                for (int i = 1; i <= 9; ++i)
                {
                    if (CheckCol(arr, loc.X, loc.Y, i) &&
                        CheckRow(arr, loc.X, loc.Y, i) &&
                        CheclSq(arr, loc.X, loc.Y, i))
                    {
                        arr[loc.X, loc.Y] = i;
                        if (Solve(arr, Sudoku.Advance(loc)))
                        {
                            return true;
                        }
                        else
                        {
                            arr[loc.X, loc.Y] = 0;
                        }
                    }
                }
            }
            return false;
        }

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

        static void Main(string[] args)
        {
            int[,] arr = new int[9, 9];

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

                System.IO.StreamReader file = new System.IO.StreamReader(args[1]);
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

                Console.WriteLine(Sudoku.Check(arr));

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
                System.IO.StreamReader file = new System.IO.StreamReader(args[1]);
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

                Console.WriteLine("File " + args[1] + " read successfully!");
                PrintArr(arr);

                Console.WriteLine("Solve mode: " + args[1]);

                DateTime start = DateTime.Now;
                Console.WriteLine(SolveSudoku(arr));
                Console.WriteLine("Runtime: " + (DateTime.Now - start));
            }

            if (args[0] == "g")
            {
                int diffLvl = Convert.ToInt32(args[1]);
                Console.WriteLine("Generate mode. Difficulty level: " + diffLvl);
                if (diffLvl < 1 || diffLvl > 30)
                {
                    throw new Exception("Difficult lvl can only be between 1(hardest) and 30(easiest) inclusively");
                }

                arr = new int[9, 9];
                Random rand = new Random();
                for (int i = 0; i < 18; ++i)
                {
                    int x,y,t;
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
                Console.WriteLine("Solution: ");
                Solve(arr, new Point(0, 0));

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

        private static bool SolveSuperSudoku(int[,] src)
        {
            IntStack iStack = new IntStack(81);
            IntStack jStack = new IntStack(81);

            int[,] dest = new int[9, 9];
            int i,j,k;

            for (i = 0; i < 9; ++i)
            {
                for (j = 0; j < 9; ++j)
                {
                    dest[i, j] = src[i, j];
                }
            }

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