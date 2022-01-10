namespace SimpleSnake
{
    using SimpleSnake.Core;
    using SimpleSnake.Core.Contracts;
    using SimpleSnake.GameObjects;
    using Utilities;

    public class StartUp
    {
        public static void Main()
        {
            ConsoleWindow.CustomizeConsole();

            //First argument is length, second is width
            Wall wall = new Wall(60, 20);
            Snake snake = new Snake(wall);

            IEngine engine = new Engine(wall, snake);
            engine.Run();
        }
    }
}
