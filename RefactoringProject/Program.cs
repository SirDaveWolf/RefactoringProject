using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;
using System.Threading;

namespace RefactoringProject
{
    struct Pos
    {
        public int row;
        public int col;
        public Pos(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            int lft = 0; // lastFoodTime
            int FoodDisspearTime = 8000;
            int negativePoints = 0;



            Pos[] directions = new Pos[] // block positions
            {
                new Pos(0, 1), // right
                new Pos(0, -1), // left
                new Pos(1, 0), //down
                new Pos(-1, 0), //up
            };
            double sleeptime = 100;
            int dir = 0; //dir = 0
            Random rng = new Random();
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
            Pos f = new Pos(rng.Next(0, Console.WindowHeight),
                rng.Next(0, Console.WindowWidth)); // food

            lft = Environment.TickCount;
            Console.SetCursorPosition(f.col, f.row);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("@");

            List<Pos> obs = new List<Pos>();
            {
                new Pos(12, 21);
                new Pos(12, 21);
                new Pos(21, 6);
            }
            foreach (Pos o in obs)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.SetCursorPosition(o.col, o.row);
                Console.Write("M");
            }

            Queue<Pos> se = new Queue<Pos>(); // snake elements
            for (int i = 0; i <= 5; i++)
            {
                se.Enqueue(new Pos(0, i));
            }
            foreach (Pos p in se)
            {
                Console.SetCursorPosition(p.col, p.row);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("G");
            }

            while (true)
            {
                negativePoints++;

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo ui = Console.ReadKey();
                    if (ui.Key == ConsoleKey.LeftArrow)
                    {
                        if (dir != 0) dir = 1;
                    }
                    if (ui.Key == ConsoleKey.RightArrow)
                    {
                        if (dir != 1) dir = 0;
                    }
                    if (ui.Key == ConsoleKey.UpArrow)
                    {
                        if (dir != 2) dir = 3;
                    }
                    if (ui.Key == ConsoleKey.DownArrow)
                    {
                        if (dir != 3) dir = 2;
                    }
                }
                Pos sh = se.Last(); //snake head
                Pos nDir = directions[dir]; // next direction
                Pos snh = new Pos(sh.row + nDir.row,
                    sh.col + nDir.col); // snake new head

                if (snh.col < 0) snh.col = Console.WindowWidth - 1;
                if (snh.row < 0) snh.row = Console.WindowWidth - 1;
                if (snh.row >= Console.WindowHeight) snh.row = 0;
                if (snh.col >= Console.WindowWidth) snh.col = 0;

                if (se.Contains(snh))
                {
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Game olver!");
                    int usersPoints = ((se.Count - 6) * 100) - negativePoints;
                    usersPoints = Math.Max(usersPoints, 0); // взима по голямата стойност
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Your points are : {0}", usersPoints);
                    return;
                }

                Console.SetCursorPosition(sh.col, sh.row);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("G");

                se.Enqueue(snh);
                Console.SetCursorPosition(snh.col, snh.row);
                //if (direction == 0) Console.Write(">");
                // if (direction == 1) Console.Write("<");
                // if (direction == 3) Console.Write("^");
                // if (direction == 2) Console.Write("v");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("G");

                if (snh.col == f.col && snh.row == f.row)
                {
                    do
                    {
                        // feeding yhe snake 
                        f = new Pos(rng.Next(0, Console.WindowHeight),
                        rng.Next(0, Console.WindowWidth));
                    }
                    while (se.Contains(f));
                    lft = Environment.TickCount;
                    Console.SetCursorPosition(f.col, f.row);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("@");
                    sleeptime--;
                }
                else
                {
                    // moving
                    Pos last = se.Dequeue();
                    Console.SetCursorPosition(last.col, last.row);
                    Console.Write(" ");
                }

                if (Environment.TickCount - lft >= FoodDisspearTime)
                {
                    negativePoints = negativePoints + 50;
                    Console.SetCursorPosition(f.col, f.row);
                    Console.Write(" ");
                    do
                    {
                        // feeding yhe snake 
                        f = new Pos(rng.Next(0, Console.WindowHeight),
                        rng.Next(0, Console.WindowWidth));
                    }
                    while (se.Contains(f));
                    lft = Environment.TickCount;
                }

                Console.SetCursorPosition(f.col, f.row);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("@");

                sleeptime -= 0.01;

                Thread.Sleep((int)sleeptime);
            }

        }
    }
}