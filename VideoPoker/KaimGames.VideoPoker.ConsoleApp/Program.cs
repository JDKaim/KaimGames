using KaimGames.Common;
using KaimGames.VideoPoker.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KaimGames.VideoPoker.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            while (!game.IsGameOver)
            {
                Console.WriteLine($"Round: {game.Round} - Score: {game.Score}");

                for (int lcv = 0; lcv < game.CurrentHand.Cards.Count; lcv++)
                {
                    Card card = game.CurrentHand.Cards[lcv];
                    Console.Write($"({lcv + 1}) {card}\t");
                }
                Console.WriteLine();
                Console.WriteLine($"Select cards to keep as \"1 2\"");

                while (true)
                {
                    string keepList = Console.ReadLine();

                    try
                    {
                        int[] keepIndexes = keepList.Split(" ").Select(item => int.Parse(item)).ToArray();
                        List<Card> keepCards = keepIndexes.Select(item => game.CurrentHand.Cards[item - 1]).Distinct().ToList();
                        BestHand result = game.Keep(keepCards);

                        Console.WriteLine("Final hand:");
                        for (int lcv = 0; lcv < game.CurrentHand.Cards.Count; lcv++)
                        {
                            Card card = game.CurrentHand.Cards[lcv];
                            Console.Write($"    {card}\t");
                        }
                        Console.WriteLine();

                        Console.WriteLine($"Your best hand was a {result.HandType} worth {result.Points} points: {string.Join(' ', result.Cards.Select(item => item.ToString()))}");
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                    }
                }

                Console.WriteLine();

                if (!game.IsGameOver)
                {
                    game.Deal();
                }
            }
        }
    }
}
