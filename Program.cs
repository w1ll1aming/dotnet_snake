using System;
using System.Diagnostics;
using System.Threading;

namespace dotnet_snake {
    class Program {
        public struct AppInfo {
            public string AppName => "Dotnet snake";
            public double Version => 0.1;
        }

        static void Main(string[] args) => Setup(args);

        static void Setup(string[] args) {
            try {
                AppInfo info;
                Snake mysnake = new Snake(
                    _speed: 100,
                    _body: SnakeUtils.GenerateSnakeBody(length: 3),
                    _selectedkey: ConsoleKey.A,
                    _lastselectedkey: ConsoleKey.A,
                    _lastcordinates: (0, 0),
                    _direction: snake_direction.Right
                ); 
                
                /* 
                    ignore the fact that _body has three items but only two shows,
                    i cant figure out how to clear properly without it.
                */

                SnakeCoords myfood = new SnakeCoords(
                    x: 5,
                    y: 5
                );

                GameStats stats = new GameStats(
                    score: 0,
                    gameover: false,
                    paused: false
                );

                Thread snake_updater = new Thread(new ThreadStart(() => Snake.Move(
                    mysnake: ref mysnake,
                    myfood: ref myfood,
                    stats: ref stats
                )));
                snake_updater.IsBackground = true;

                Console.CursorVisible = false;
                Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) => Exit(sender: sender, eventArgs: e, exitCode: 1);
                
                Console.WriteLine($"Starting '{info.AppName}' version '{info.Version}'.");
                while (Console.ReadKey(intercept: true).Key != ConsoleKey.Enter){}
                Console.Clear();
                Start(
                    mysnake: ref mysnake,
                    stats: ref stats,
                    snake_updater: snake_updater
                );
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        // Pause menu
        static void Start(ref Snake mysnake, ref GameStats stats, Thread snake_updater) {
            var result = menu.Show(items: new string[] {"New Game", "High Scores", "Controls", "Exit"}, type: menu.Type.Main, ref stats);

            switch (result) {
                case 0:
                    Update(
                        mysnake: ref mysnake,
                        stats: ref stats,
                        snake_updater: snake_updater
                    );
                    break;
                case 1:
                    // High Scores things here
                    break;
                case 2:
                    // Controls things here
                    break;
                case 3:
                    Exit(exitCode: 0);
                    break;
            }
        }

        static void Exit(object sender=null, ConsoleCancelEventArgs eventArgs=null, int exitCode=0) {
            if (eventArgs != null) eventArgs.Cancel = true;

            Console.Clear();
            Console.CursorVisible = true;
            Environment.Exit(exitCode: exitCode);
        }

        static void Pause(ref GameStats stats) {
            stats.GamePaused = true;
            Console.Clear();
            int result = menu.Show(items: new string[] {"Continue game", "High Scores", "Controls", "Exit"}, type: menu.Type.Pause, stats: ref stats);
            switch (result) {
                case 0:
                    Console.Clear();
                    break;
                case 1:
                    // High Scores things here
                    break;
                case 2:
                    // Controls things here.
                    break;
                case 3:
                    Exit(exitCode: 0);
                    break;
                default:
                    // The user selected an invalid option in the pause menu, just return to the game.
                    Console.Clear();
                    return;
            }
            stats.GamePaused = false;
            Debug.WriteLine(stats.GamePaused);
        }

        public static void Update(ref Snake mysnake, ref GameStats stats, Thread snake_updater) {
            Debug.WriteLine("Updated.");
            ConsoleKeyInfo keyinfo = new ConsoleKeyInfo();
            Console.Clear();
            
            snake_updater.Start();
            try {
                do {
                    while (!Console.KeyAvailable);
                    keyinfo = Console.ReadKey(intercept: true);

                    switch (keyinfo.Key) {
                        case ConsoleKey.W:
                            if (mysnake.SelectedKey == ConsoleKey.S || mysnake.SelectedKey == ConsoleKey.W) break;
                            mysnake.Direction = snake_direction.Up;
                            break;
                        case ConsoleKey.A:
                            if (mysnake.SelectedKey == ConsoleKey.D || mysnake.SelectedKey == ConsoleKey.A) break;
                            mysnake.Direction = snake_direction.Left;
                            break;
                        case ConsoleKey.S:
                            if (mysnake.SelectedKey == ConsoleKey.W || mysnake.SelectedKey == ConsoleKey.S) break;
                            mysnake.Direction = snake_direction.Down;
                            break;
                        case ConsoleKey.D:
                            if (mysnake.SelectedKey == ConsoleKey.A || mysnake.SelectedKey == ConsoleKey.D) break;
                            mysnake.Direction = snake_direction.Right;
                            break;
                        case ConsoleKey.Enter:
                            break;
                        case ConsoleKey.Escape:
                            Pause(stats: ref stats);
                            break;
                    }
                    mysnake.SelectedKey = keyinfo.Key;
                } while (true);
            }
            catch (Exception ex) {
                Console.Clear();
                Console.WriteLine(ex);
            }
            finally { stats.GameOver = false; }
        }

        public static void Draw(ref Snake mysnake, ref SnakeCoords myfood, ref GameStats stats) {
            void WriteAt(int left, int top, string text) {
                (int, int) current = (Console.CursorLeft, Console.CursorTop);

                Console.SetCursorPosition(left, top);
                Console.Write(text);
                Console.SetCursorPosition(current.Item1, current.Item2);
            }
            if (!stats.GameOver) {
                Debug.WriteLine("Clearing console, and rebuilding body.");
                (int, int) oldpos = (Console.CursorLeft, Console.CursorTop);
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"Score: {stats.Score}\n{new string('-', Console.WindowWidth)}");
                Console.SetCursorPosition(oldpos.Item1, oldpos.Item2);

                string icon = "";
                Debug.WriteLine(mysnake.Body.Count);
                for (int i = 0; i < mysnake.Body.Count; i++) {
                    // select either head or body char
                    icon = (i == 0) ? "O" : "o";

                    // draw snake body and head
                    WriteAt(
                        left: mysnake.Body[i].X,
                        top: mysnake.Body[i].Y,
                        text: icon
                    );

                    // draw food
                    WriteAt(
                        left: myfood.X,
                        top: myfood.Y,
                        text: "*"
                    );

                    // set cordinates to erase body
                    mysnake.lastCordinates = (mysnake.Body[i].X, mysnake.Body[i].Y);

                }
                WriteAt(
                    left: mysnake.lastCordinates.Item1,
                    top: mysnake.lastCordinates.Item2,
                    text: " "
                );
            }
            else Snake.die(stats: ref stats);
        }
    }
}