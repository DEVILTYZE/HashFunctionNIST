using System;
using HashFunctionNIST.Models;
using MathNet.Numerics;

namespace HashFunctionNIST.Tests
{
    public class FrequencyTest : Test
    {
        public FrequencyTest() : base("Частотный тест", 0.01) { }

        protected override double GetPValue(string data)
        {
            var sequence = new BinarySequence(Hasher.Hash(data));
            var (zeros, ones) = sequence.GetCountOfZerosAndOnes();
            var statistic = Math.Abs(ones - zeros) / Math.Sqrt(zeros + ones);
            
            return SpecialFunctions.Erfc(statistic / Math.Sqrt(2));
        }
    }
}