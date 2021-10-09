using System;

namespace dotnet_snake {
    static class food {
        public static void GenerateFood(ref SnakeCoords myfood) {
            Random rand = new Random();
            
            myfood = new SnakeCoords(
                x: rand.Next(0, Console.WindowWidth),
                y: rand.Next(3, Console.WindowHeight)
            );
        }
    }
}