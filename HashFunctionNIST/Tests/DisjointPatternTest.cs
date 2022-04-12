using System;
using System.Collections.Generic;
using System.Linq;
using HashFunctionNIST.Models;

namespace HashFunctionNIST.Tests
{
    public class DisjointPatternTest : Test
    {
        public DisjointPatternTest() : base("Тест непересекающихся шаблонов", 0.01) { }

        protected override void RunThread(object data)
        {
            var inputData = (string[])data;

            foreach (var word in inputData)
            {
                var sequence = new BinarySequence(Hasher.Hash(word));
                int bigM;
                int bigN;

                do
                {
                    do bigN = sequence.Length is 1 or 2 ? 1 : new Random().Next(2, sequence.Length);
                    while (bigN > 100);
                    
                    bigM = sequence.Length / bigN;
                }
                while (bigM <= 0.01 * sequence.Length);
                
                var subsequences = GetSubsequences(sequence, bigM);
                var length = new Random().Next(2, bigM + 1);
                var statistic = subsequences.Select((subsequence, j) 
                    => Math.Pow(ScanPattern(sequence[..length], subsequence, bigM, bigN, j, length) - U(bigM, length), 2) 
                       / O(bigM, length)).Sum();
                var pValue = MathHelper.Igmc(bigN / 2.0, statistic / 2);

                if (IsSuccessed(pValue))
                    continue;
                
                lock (Locker)
                {
                    ++FailedCount;
                }
            }
        }

        private static IEnumerable<string> GetSubsequences(BinarySequence sequence, int n)
        {
            var subsequences = new string[n - 1];
            var coef = sequence.Length / n;

            for (var i = 0; i < n - 1; ++i)
                if ((i + 1) * coef < sequence.Length - 1)
                    subsequences[i] = sequence[(i * coef)..((i + 1) * coef)];

            return subsequences;
        }

        private static int ScanPattern(string pattern, string subsequence, int bigM, int bigN, int j, int length)
        {
            int coef = 0, coef2 = 0, result = 0;
            
            while (true)
            {
                for (var k = 0; k < bigM - length + 1; ++k)
                {
                    var start = bigN * j + k + coef * length + coef2;
                    var end = bigN * j + k + (coef + 1) * length + coef2;

                    if (start > subsequence.Length || end > subsequence.Length)
                        return result;
                    
                    var currentSequence = subsequence[start..end];

                    if (string.CompareOrdinal(pattern, currentSequence) == 0)
                    {
                        coef2 = 0;
                        ++coef;
                        ++result;
                    }
                    else ++coef2;
                }
            }
        }

        private static double U(int bigM, int length) => (bigM - length + 1) / Math.Pow(2.0, length);

        private static double O(int bigM, int length)
            => bigM * (1 / Math.Pow(2.0, length) - (2 * length - 1) / Math.Pow(2.0, 2 * length));
    }
}