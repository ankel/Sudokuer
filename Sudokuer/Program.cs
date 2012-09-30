using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using NUnit.Framework;

namespace Sudokuer
{
    class Sudoku
    {
        private int x;
        private int y;
        public int[,] arr;

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
                x = 8;
                y = 8;
                return false;
            }
            else
                return true;
        }

        static public Point Advance(Point pt)
        {
            if (pt.Y < 8)
            {
                return new Point(pt.X, pt.Y + 1);
            }
            else
            {
                return new Point(pt.X + 1, 0);
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

    [TestFixture]
    public class SudokuTest
    {
        Sudoku board;

        [SetUp]
        public void SetUp()
        {
            board = new Sudoku();
        }

        [Test]
        public void TestAdvance()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Assert.AreEqual(board.X, i, "i {0} j {1} X {2} Y {3}", i, j, board.X, board.Y);
                    Assert.AreEqual(board.Y, j, "i {0} j {1} X {2} Y {3}", i, j, board.X, board.Y);
                    Assert.AreEqual(board.currentLoc, 0);
                    if (i == 8 && j == 8)
                    {
                        Assert.IsFalse(board.Advance(), "i {0} j {1} X {2} Y {3}", i, j, board.X, board.Y);
                    }
                    else
                    {
                        Assert.IsTrue(board.Advance(), "i {0} j {1} X {2} Y {3}", i, j, board.X, board.Y);
                    }
                }
            }

            for (int i = 0; i < 10; i++)
            {
                Assert.IsFalse(board.Advance());
            }
        }
    }

    class Program
    {
        static Sudoku loc = new Sudoku();

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
            if (loc.X == 9)
            {
                PrintArr(arr);
                return true;
            }
            else if (arr[loc.X, loc.Y] != 0)
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
                return;
            }

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

            if (args[0] == "c")
            {
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
                Console.WriteLine("Solve mode: " + args[1]);

                Console.WriteLine(Solve(arr, new Point(0, 0)));
            }
            Console.ReadLine();
        }
    }
}
