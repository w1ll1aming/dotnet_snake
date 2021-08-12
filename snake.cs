using System;
using System.Threading;
using System.Collections.Generic;

namespace dotnet_snake 
{
    public class SnakeCoords
    {
        public SnakeCoords(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }

    public enum snake_direction
    {
        Up,
        Down,
        Left,
        Right
    }

    class Snake
    {
        public Snake(List<SnakeCoords> _body, int _speed, ConsoleKey _selectedkey, ConsoleKey _lastselectedkey, (int, int) _lastcordinates, snake_direction _direction)
        {
            Speed = _speed;
            Body = _body;
            SelectedKey = _selectedkey;
            LastSelectedKey = _lastselectedkey;
            lastCordinates = _lastcordinates;
            Direction = _direction;
        }
        public int Speed { get; set; }
        public List<SnakeCoords> Body { get; set; }
        public ConsoleKey SelectedKey { get; set; }
        public ConsoleKey LastSelectedKey { get; set; }
        public (int, int) lastCordinates { get; set; }
        public snake_direction Direction { get; set; }

        public static void Move(ref Snake mysnake, ref SnakeCoords myfood, ref GameStats stats)
        {
            try {
                while (!stats.GameOver)
                {
                    for (int i = mysnake.Body.Count - 1; i >= 0; i--)
                    {
                        if (i == 0)
                        {
                            switch (mysnake.Direction)
                            {
                                case snake_direction.Right:
                                    mysnake.Body[i].X++;
                                    break;
                                case snake_direction.Left:
                                    mysnake.Body[i].X--;
                                    break;
                                case snake_direction.Up:
                                    mysnake.Body[i].Y--;
                                    break;
                                case snake_direction.Down:
                                    mysnake.Body[i].Y++;
                                    break;
                            }

                            if (
                                mysnake.Body[i].X < 0 || mysnake.Body[i].Y < 2 ||
                                mysnake.Body[i].X > Console.WindowWidth || mysnake.Body[i].Y > (Console.WindowHeight) // - topbar height
                            ) die(stats: ref stats);

                            for (int j = 1; j < mysnake.Body.Count; j++)
                            {
                                if (mysnake.Body[i].X == mysnake.Body[j].X && mysnake.Body[i].Y == mysnake.Body[j].Y) die(stats: ref stats);
                            }
                            if (mysnake.Body[0].X == myfood.X && mysnake.Body[0].Y == myfood.Y) eat(
                                mysnake: ref mysnake,
                                myfood: ref myfood,
                                stats: ref stats
                            );
                        }
                        else
                        {
                            mysnake.Body[i].X = mysnake.Body[i - 1].X;
                            mysnake.Body[i].Y = mysnake.Body[i - 1].Y;
                        }
                        Program.Draw(
                            mysnake: ref mysnake,
                            myfood: ref myfood,
                            stats: ref stats
                        );
                    }
                    Thread.Sleep(mysnake.Speed);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); Environment.Exit(1); }
        }

        public static void die(ref GameStats stats) {
            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine($"Game Over\nFinal Score is {stats.Score}\nPress enter to exit.");
            Console.ReadKey(intercept: true);
            Environment.Exit(0);
        }
        
        public static void eat(ref Snake mysnake, ref SnakeCoords myfood, ref GameStats stats) {
            SnakeCoords body = new SnakeCoords(
                x: mysnake.Body[mysnake.Body.Count - 1].X,
                y: mysnake.Body[mysnake.Body.Count - 1].Y
            );
 
            mysnake.Body.Add(body); // add the part to the snakes array
            stats.Score += 1; // increase the score for the game
            food.GenerateFood(
                myfood: ref myfood
            );
        }
    }

    static class SnakeUtils {
        public static List<SnakeCoords> GenerateSnakeBody(int length) {
            List<SnakeCoords> Body = new List<SnakeCoords>{};
            for (int i=0; i < length; i++) {
                Body.Add(
                    new SnakeCoords(
                        x: (11-i),
                        y: 5
                    )
                );
            }
            return Body;
        }
    }
}