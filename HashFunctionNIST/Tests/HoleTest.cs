using System;
using HashFunctionNIST.Models;
using MathNet.Numerics;

namespace HashFunctionNIST.Tests
{
    public class HoleTest : Test
    {
        public HoleTest() : base("Тест дырок", 0.01) { }

        protected override double GetPValue(string data)
        {
            var sequence = new BinarySequence(Hasher.Hash(data));
            var pi = (double)sequence.GetCountOfZerosAndOnes().Item2 / sequence.Length;

            if (Math.Abs(pi - 0.5) >= 2 / Math.Sqrt(sequence.Length))
                return double.Epsilon;

            var statistic = 0;
            var stringSequence = sequence.ToString();

            for (var i = 0; i < stringSequence.Length - 1; ++i)
                statistic += stringSequence[i] == stringSequence[i + 1] ? 0 : 1;

            return SpecialFunctions.Erfc(Math.Abs(statistic + 1 - 2 * sequence.Length * pi * (1 - pi)) /
                                         (2 * Math.Sqrt(2 * sequence.Length) * pi * (1 - pi)));
        }
    }
}