using KaimGames.CodeBreaker.Common;
using System;
using System.Linq;

namespace KaimGames.CodeBreaker.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game;

            while (true)
            {
                Console.WriteLine("Enter <code length> <code options> <code (optional)>");

                string input = Console.ReadLine();
                string[] parts = input.Split(' ');

                if ((parts.Length < 2) || (parts.Length > 3)) { continue; }

                int codeLength;
                int codeOptionsLength;
                char[] code = null;

                if (!int.TryParse(parts[0], out codeLength))
                {
                    Console.WriteLine($"Bad code length: {parts[0]}");
                    continue;
                }

                if (!int.TryParse(parts[1], out codeOptionsLength))
                {
                    Console.WriteLine($"Bad code length: {parts[1]}");
                    continue;
                }

                if (parts.Length == 3)
                {
                    code = parts[2].ToCharArray();
                }

                try
                {
                    game = new Game(codeLength, Game.GenerateCodeOptions(codeOptionsLength), code);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Game creation failed: {e.Message}");
                    continue;
                }

                break;
            }

            Console.WriteLine($"Starting game with {new string(game.Code)} ({game.CodeLength}) using {new string(game.CodeOptions)}");

            while (!game.IsGameOver)
            {
                string guess = Console.ReadLine();

                try
                {
                    game.Guess(guess.ToCharArray());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                Console.WriteLine($"Exact: {game.Guesses.Last().ExactlyCorrect}\tWrong location: {game.Guesses.Last().WrongLocation}");
            }

            Console.WriteLine("You won!");
        }
    }
}
