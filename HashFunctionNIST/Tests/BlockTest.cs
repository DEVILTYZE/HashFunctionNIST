using System;
using System.Linq;
using HashFunctionNIST.Models;
using MathNet.Numerics;

namespace HashFunctionNIST.Tests
{
    public class BlockTest : Test
    {
        public BlockTest() : base("Частотный блочный тест", 0.01) { }

        protected override double GetPValue(string data)
        {
            var sequence = new BinarySequence(Hasher.Hash(data));
            int m;
            int bigN;

            do
            {
                do bigN = sequence.Length is 1 or 2 ? 1 : new Random().Next(2, sequence.Length);
                while (bigN > 100);
                    
                m = sequence.Length / bigN;
            }
            while (m <= 0.01 * sequence.Length);

            var stringSequence = sequence.ToString();
            var pi = new double[bigN];
            
            for (var i = 0; i < bigN; ++i)
            {
                for (var j = 0; j < m; ++j)
                    pi[i] = int.Parse(stringSequence[i * m + j].ToString());

                pi[i] /= m;
            }

            var statistic = 4 * m * pi.Select(thisPi => Math.Pow(thisPi - 0.5, 2)).Sum();

            return SpecialFunctions.GammaLowerRegularized(bigN / 2.0, statistic / 2);
        }
    }
}