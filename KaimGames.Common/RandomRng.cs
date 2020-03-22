using System;
using System.Collections.Generic;
using System.Text;

namespace KaimGames.Common
{
    public class RandomRng : IRandomNumberGenerator
    {
        private Random _random;

        public RandomRng()
        {
            this._random = new Random();
        }

        public int Next(int exclusiveMax) => this._random.Next(exclusiveMax);
        public int Next(int inclusiveMin, int exclusiveMax) => this._random.Next(inclusiveMin, exclusiveMax);
        public double NextDouble() => this._random.NextDouble();
    }
}
