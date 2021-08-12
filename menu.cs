using System;
using System.Diagnostics;

namespace dotnet_snake 
{
    static class menu 
    {
        public enum Type {
            Main,
            Pause
        }

        public static int Show(string[] items, Type type) {
            ConsoleKeyInfo keyinfo = new ConsoleKeyInfo();
            int index = 0;
            bool item_selected = false;

            string menu_name = $"{type} Menu";
            int menu_margin = 4;

            int width = (Console.WindowWidth / 2)-menu_name.Length;
            Console.SetCursorPosition(0, ((Console.WindowHeight / 2) - items.Length) - menu_margin);
            Console.WriteLine($"{new string(' ', width)}{menu_name}{new string(' ', width)}");
            Console.SetCursorPosition(0,0);

            do {
                switch (keyinfo.Key) {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        if (index > 0) index--;
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        if (index < (items.Length-1)) index++;
                        break;
                    case ConsoleKey.Enter:
                        item_selected = true;
                        break;
                }
                
                Debug.WriteLine(index);
                Console.SetCursorPosition(0, (Console.WindowHeight / 2)-items.Length);
                
                int biggest_length = 0;
                foreach (string item in items) { 
                    if (item.Length > biggest_length) { biggest_length = item.Length; } 
                }
                
                for (int i=0; i < items.Length; i++) {
                    string cur = i == index ? "> " : "  ";

                    Console.WriteLine($"{new string(' ', (Console.WindowWidth / 2) - biggest_length)}{cur}{items[i]}");
                }
                Debug.WriteLine($"Item Selected: {item_selected} - Index: {index}");
                if (!item_selected) keyinfo = Console.ReadKey(true);
            } while (!item_selected && keyinfo.Key != ConsoleKey.Escape);

            return index;
        }
    }
}