using SimpleSnake.Core.Contracts;
using SimpleSnake.Enums;
using SimpleSnake.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SimpleSnake.Core
{
    public class Engine : IEngine
    {
        private readonly Point[] pointsOfDirection;
        private Direction direction;
        private readonly Snake snake;
        private Wall wall;
        private double sleepTime;

        public Engine(Wall wall, Snake snake)
        {
            this.wall = wall;
            this.snake = snake;

            this.sleepTime = 100;
            this.pointsOfDirection = new Point[4]; 
        }

        public void Run()
        {
            this.CreateDirections();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    this.GetNextDirection();
                }

                bool isMoving = this.snake.IsMoving(this.pointsOfDirection[(int)this.direction]);

                if (!isMoving)
                {
                    this.AskUserForRestart();
                }

                this.PrintStatisticsInfo();

                //This adjust the speed of the snake
                sleepTime -= 0.01;

                //Slows the program
                //Stops the program for the given time in ms
                Thread.Sleep((int)sleepTime);
            }

        }

        private void CreateDirections()
        {
            this.pointsOfDirection[0] = new Point(1, 0);
            this.pointsOfDirection[1] = new Point(-1, 0);
            this.pointsOfDirection[2] = new Point(0, 1);
            this.pointsOfDirection[3] = new Point(0, -1);
        }

        private void GetNextDirection()
        {
            ConsoleKeyInfo userInput = Console.ReadKey();

            //Check keyboard key pressed
            if (userInput.Key == ConsoleKey.LeftArrow)
            {
                //Left Arrow
                //Check if the snake is not moving to right
                if (this.direction != Direction.Right)
                {
                    //if it is not moving to right
                    //it starts moving to left
                    this.direction = Direction.Left;
                }
            }
            else if(userInput.Key == ConsoleKey.RightArrow)
            {
                if (this.direction != Direction.Left)
                {
                    this.direction = Direction.Right;
                }
            }
            else if (userInput.Key == ConsoleKey.UpArrow)
            {
                if (this.direction != Direction.Down)
                {
                    this.direction = Direction.Up;
                }
            }
            else if (userInput.Key == ConsoleKey.DownArrow)
            {
                if (this.direction != Direction.Up)
                {
                    this.direction = Direction.Down;
                }
            }

            Console.CursorVisible = false;
        }

        private void AskUserForRestart()
        {
            int leftX = this.wall.LeftX + 1;
            int topY = 3;

            Console.SetCursorPosition(leftX, topY);
            Console.Write("Would you like to continue? y/n");

            //Move cursor to the next row
            Console.SetCursorPosition(leftX, topY + 1);
            Console.Write("Your choise: ");

            string input = Console.ReadLine();
            if (input == "y")
            {
                Console.Clear();
                StartUp.Main();
            }
            else
            {
                this.StopGame();
            }
        }

        private void StopGame()
        {
            Console.SetCursorPosition(20, 10);
            Console.Write("Game over!");
            Environment.Exit(0);
        }

        private void PrintStatisticsInfo()
        {
            int leftX = this.wall.LeftX + 1;
            int topY = 1;

            Console.SetCursorPosition(leftX, topY);
            Console.Write($"Player points: {this.snake.SnakePoints}");

            //Move cursor to the next row
            Console.SetCursorPosition(leftX, topY + 1);
            Console.Write($"Player level: {this.snake.SnakeLevel}");
        }
    }
}
