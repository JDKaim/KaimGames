using System;
using System.Collections.Generic;
using System.Text;

namespace KaimGames.Common
{
    public class HardcodedRng : IRandomNumberGenerator
    {
        private readonly Queue<int> Ints;
        private readonly Queue<double> Doubles;

        public HardcodedRng(IEnumerable<int> ints, IEnumerable<double> doubles)
        {
            this.Ints = new Queue<int>(ints ?? new int[0]);
            this.Doubles = new Queue<double>(doubles ?? new double[0]);
        }

        static HardcodedRng CreateFromInts(params int[] values) => new HardcodedRng(values, new double[0]);

        static HardcodedRng CreateFromDoubles(params double[] values) => new HardcodedRng(new int[0], values);

        public int Next(int exclusiveMax) => this.Ints.Dequeue();
        public int Next(int inclusiveMin, int exclusiveMax) => this.Ints.Dequeue();
        public double NextDouble() => this.Doubles.Dequeue();
    }
}
