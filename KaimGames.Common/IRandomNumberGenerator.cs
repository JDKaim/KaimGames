using System;
using System.Collections.Generic;
using System.Text;

namespace KaimGames.Common
{
    public interface IRandomNumberGenerator
    {
        int Next(int exclusiveMax);
        int Next(int inclusiveMin, int exclusiveMax);
        double NextDouble();
    }
}
