using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleSnake.GameObjects
{
    public class Snake
    {
        private const char snakeSymbol = '\u25CF';
        private const char EmptySpaceSymbol = ' ';

        private readonly Queue<Point> snakeElements;
        private readonly Food[] foods;
        private readonly Wall wall;

        private int nextLeftX;
        private int nextTopY;
        private int foodIndex;

        private bool isFoodSpawned;
        private int snakePoints;
        private int levelCount;

        public Snake(Wall wall)
        {
            this.snakeElements = new Queue<Point>();
            this.foods = new Food[3];
            this.wall = wall;
            this.foodIndex = RandomFoodNumber;

            this.isFoodSpawned = false;
            this.snakePoints = 6;
            this.levelCount = 100;

            this.GetFoods();
            this.CreateSnake();
        }

        public int SnakePoints => this.snakePoints;

        public int SnakeLevel => this.levelCount / 100;

        private int RandomFoodNumber =>
            new Random().Next(0, this.foods.Length);

        public bool IsMoving(Point direction)
        {
            Point currentSnakeHead = this.snakeElements.Last();
            //Sets next LeftX and next TopY
            GetNextPoint(direction, currentSnakeHead);

            //Check if snake have bitten itself
            bool isPointOfSnake = snakeElements.Any(p => p.LeftX == this.nextLeftX &&
            p.TopY == this.nextTopY);

            if (isPointOfSnake)
            {
                return false;
            }

            Point newSnakeHead = new Point(this.nextLeftX, this.nextTopY);

            //Check if snake hits one of the walls
            if (this.wall.IsPointOfWall(newSnakeHead))
            {
                return false;
            }

            this.snakeElements.Enqueue(newSnakeHead);
            newSnakeHead.Draw(snakeSymbol);

            if (!this.isFoodSpawned)
            {
                this.foods[foodIndex].SetRandomPosition(this.snakeElements);
                this.isFoodSpawned = true;
            }

            if (foods[this.foodIndex].IsFoodPoint(newSnakeHead))
            {
                this.Eat(direction, currentSnakeHead);
            }

            Point snakeTail = this.snakeElements.Dequeue();
            snakeTail.Draw(EmptySpaceSymbol);

            this.levelCount++;

            return true;
        }

        private void Eat(Point direction, Point currentSnakeHead)
        {
            int length = this.foods[foodIndex].FoodPoints;

            for (int i = 0; i < length; i++)
            {
                this.snakeElements.Enqueue(new Point(this.nextLeftX, this.nextTopY));
                GetNextPoint(direction, currentSnakeHead);
            }

            this.snakePoints += length;

            //Spawn new food
            this.foodIndex = RandomFoodNumber;
            this.foods[foodIndex].SetRandomPosition(this.snakeElements);
        }

        private void GetNextPoint(Point direction, Point snakeHead)
        {
            this.nextLeftX = direction.LeftX + snakeHead.LeftX;
            this.nextTopY = direction.TopY + snakeHead.TopY;
        }

        private void CreateSnake()
        {
            for (int topY = 1; topY <= 6; topY++)
            {
                this.snakeElements.Enqueue(new Point(2, topY));
            }
        }

        private void GetFoods()
        {
            this.foods[0] = new FoodHash(this.wall);
            this.foods[1] = new FoodDollar(this.wall);
            this.foods[2] = new FoodAsterisk(this.wall);
        }


    }
}
