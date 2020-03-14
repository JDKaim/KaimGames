using System;

namespace KaimGames.CodeBreaker.Common
{
    public class Guess
    {
        public readonly int ExactlyCorrect;
        public readonly int WrongLocation;
        public readonly char[] Code;

        public bool IsWon => this.ExactlyCorrect == this.Code.Length;

        public Guess(char[] code, int exactlyCorrect, int wrongLocation)
        {
            this.Code = code;
            this.ExactlyCorrect = exactlyCorrect;
            this.WrongLocation = wrongLocation;
        }
    }
}
