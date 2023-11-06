using System;
using System.Text;

namespace MinesweeperGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Clear();

            while (true)
            {
                Console.WriteLine("---- Minesweeper ----");
                Console.WriteLine("1. Start Game");
                Console.WriteLine("2. View Scores");
                Console.WriteLine("3. Quit Game");
                Console.Write("Enter your choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Clear();
                        var game = new Game();
                        game.Start();
                        break;
                    case "2":
                        Console.Clear();
                        ScoreManager.ViewScores();
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid choice! Try again.");
                        break;
                }
            }
        }
    }
}
