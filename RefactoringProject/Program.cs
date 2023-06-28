using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;
using System.Threading;

namespace RefactoringProject
{
    struct Position
    {
        public int row;
        public int col;
        public Position(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            int lastFoodTime = 0;
            int FoodDisspearTime = 8000;
            int negativePoints = 0;



            Position[] directions = new Position[] // block positions
            {
                new Position(0, 1), // right
                new Position(0, -1), // left
                new Position(1, 0), //down
                new Position(-1, 0), //up
            };
            double sleeptime = 100;
            int direction = 0; //0
            Random randomNubersGenerator = new Random();
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
            Position food = new Position(randomNubersGenerator.Next(0, Console.WindowHeight),
                randomNubersGenerator.Next(0, Console.WindowWidth));

            lastFoodTime = Environment.TickCount;
            Console.SetCursorPosition(food.col, food.row);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("@");

            List<Position> obstacles = new List<Position>();
            {
                new Position(12, 21);
                new Position(12, 21);
                new Position(21, 6);
            }
            foreach (Position obstacle in obstacles)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.SetCursorPosition(obstacle.col, obstacle.row);
                Console.Write("M");
            }

            Queue<Position> snakeelements = new Queue<Position>();
            for (int i = 0; i <= 5; i++)
            {
                snakeelements.Enqueue(new Position(0, i));
            }
            foreach (Position position in snakeelements)
            {
                Console.SetCursorPosition(position.col, position.row);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("G");
            }

            while (true)
            {
                negativePoints++;

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();
                    if (userInput.Key == ConsoleKey.LeftArrow)
                    {
                        if (direction != 0) direction = 1;
                    }
                    if (userInput.Key == ConsoleKey.RightArrow)
                    {
                        if (direction != 1) direction = 0;
                    }
                    if (userInput.Key == ConsoleKey.UpArrow)
                    {
                        if (direction != 2) direction = 3;
                    }
                    if (userInput.Key == ConsoleKey.DownArrow)
                    {
                        if (direction != 3) direction = 2;
                    }
                }
                Position snakeHead = snakeelements.Last();
                Position nextDirections = directions[direction];
                Position snakeNewHead = new Position(snakeHead.row + nextDirections.row,
                    snakeHead.col + nextDirections.col);

                if (snakeNewHead.col < 0) snakeNewHead.col = Console.WindowWidth - 1;
                if (snakeNewHead.row < 0) snakeNewHead.row = Console.WindowWidth - 1;
                if (snakeNewHead.row >= Console.WindowHeight) snakeNewHead.row = 0;
                if (snakeNewHead.col >= Console.WindowWidth) snakeNewHead.col = 0;

                if (snakeelements.Contains(snakeNewHead))
                {
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Game olver!");
                    int usersPoints = ((snakeelements.Count - 6) * 100) - negativePoints;
                    usersPoints = Math.Max(usersPoints, 0); // взима по голямата стойност
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Your points are : {0}", usersPoints);
                    return;
                }

                Console.SetCursorPosition(snakeHead.col, snakeHead.row);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("G");

                snakeelements.Enqueue(snakeNewHead);
                Console.SetCursorPosition(snakeNewHead.col, snakeNewHead.row);
                //if (direction == 0) Console.Write(">");
                // if (direction == 1) Console.Write("<");
                // if (direction == 3) Console.Write("^");
                // if (direction == 2) Console.Write("v");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("G");

                if (snakeNewHead.col == food.col && snakeNewHead.row == food.row)
                {
                    do
                    {
                        // feeding yhe snake 
                        food = new Position(randomNubersGenerator.Next(0, Console.WindowHeight),
                        randomNubersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeelements.Contains(food));
                    lastFoodTime = Environment.TickCount;
                    Console.SetCursorPosition(food.col, food.row);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("@");
                    sleeptime--;
                }
                else
                {
                    // moving
                    Position last = snakeelements.Dequeue();
                    Console.SetCursorPosition(last.col, last.row);
                    Console.Write(" ");
                }

                if (Environment.TickCount - lastFoodTime >= FoodDisspearTime)
                {
                    negativePoints = negativePoints + 50;
                    Console.SetCursorPosition(food.col, food.row);
                    Console.Write(" ");
                    do
                    {
                        // feeding yhe snake 
                        food = new Position(randomNubersGenerator.Next(0, Console.WindowHeight),
                        randomNubersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeelements.Contains(food));
                    lastFoodTime = Environment.TickCount;
                }

                Console.SetCursorPosition(food.col, food.row);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("@");

                sleeptime -= 0.01;

                Thread.Sleep((int)sleeptime);
            }

        }
    }
}