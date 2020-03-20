using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KaimGames.CodeBreaker.Common
{
    public class Game
    {
        const int MaxCodeLength = 10;
        const int MinCodeLength = 1;
        const int MaxCodeOptions = 10;
        const int MinCodeOptions = 1;

        public readonly int CodeLength;
        public readonly char[] CodeOptions;

        public readonly char[] Code;
        public readonly List<Guess> Guesses;

        public bool IsGameOver => this.Guesses.Any() && this.Guesses.Last().IsWon;

        public string Name => "CodeBreaker";
        public string SubGame => $"{this.CodeLength}-{this.CodeOptions.Length}";

        static public char[] GenerateCodeOptions(int codeOptionsLength)
        {
            // Assumes we don't support more than 10. Will need to update if we support more.
            return (new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }).Take(codeOptionsLength).ToArray();
        }

        static public Game Create(int codeLength, int codeOptionsLength)
        {
            return new Game(codeLength, Game.GenerateCodeOptions(codeOptionsLength));
        }

        public Game(int codeLength, char[] codeOptions, char[] code = null)
        {
            // Validate parameters.
            if (codeOptions == null) { throw new ArgumentException("You must provide code options"); }
            if ((codeLength < Game.MinCodeLength) || (codeLength > Game.MaxCodeLength)) { throw new ArgumentException($"Code length must be between {Game.MinCodeLength} and {Game.MinCodeLength}"); }
            if ((codeOptions.Length < Game.MinCodeOptions) || (codeOptions.Length > Game.MaxCodeOptions)) { throw new ArgumentException($"Code options must be between {Game.MinCodeOptions} and {Game.MaxCodeOptions} characters"); }
            if (codeOptions.Distinct().Count() != codeOptions.Length) { throw new ArgumentException("You cannot have duplicates in your code options"); }

            this.CodeLength = codeLength;
            this.CodeOptions = codeOptions;
            this.Guesses = new List<Guess>();

            if (code != null)
            {
                // Validate the code, if provided.
                if (code.Length != this.CodeLength) { throw new ArgumentException("Provided code did not match provided length"); }
                if (code.Any(item => !this.CodeOptions.Contains(item))) { throw new ArgumentException("Your provided code cannot contain characters that are not in your code options"); }
                this.Code = code;
            }
            else
            {
                // Generate a random code if not provided.
                this.Code = new char[this.CodeLength];
                Random random = new Random();
                for (int lcv = 0; lcv < this.CodeLength; lcv++)
                {
                    this.Code[lcv] = this.CodeOptions[random.Next(0, this.CodeOptions.Length)];
                }
            }
        }

        public void Guess(char[] code)
        {
            if (this.IsGameOver) { return; }
            if (code.Length != this.CodeLength) { throw new ArgumentException("The guess was not the correct length"); }
            if (code.Any(item => !this.CodeOptions.Contains(item))) { throw new ArgumentException("The guess contained characters not in the options"); }

            int exactlyCorrect = 0;
            int wrongLocation = 0;

            Dictionary<char, int> missesFromCode = new Dictionary<char, int>();
            List<char> leftoverFromGuess = new List<char>();

            for (int lcv = 0; lcv < this.CodeLength; lcv++)
            {
                if (this.Code[lcv] == code[lcv])
                {
                    exactlyCorrect++;
                }
                else
                {
                    if (!missesFromCode.ContainsKey(this.Code[lcv]))
                    {
                        missesFromCode.Add(this.Code[lcv], 0);
                    }
                    missesFromCode[this.Code[lcv]]++;
                    leftoverFromGuess.Add(code[lcv]);
                }
            }

            foreach(char c in leftoverFromGuess)
            {
                if (missesFromCode.ContainsKey(c) && (missesFromCode[c] > 0))
                {
                    missesFromCode[c]--;
                    wrongLocation++;
                }
            }

            this.Guesses.Add(new Guess(code, exactlyCorrect, wrongLocation));
        }
    }
}
