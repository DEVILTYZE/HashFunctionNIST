using System;
using HashFunctionNIST.Models;

namespace HashFunctionNIST.Tests
{
    public class FrequencyTest : Test
    {
        public FrequencyTest() : base("Частотный тест", 0.01) { }

        protected override void RunThread(object data)
        {
            var inputData = (string[])data;
            
            foreach (var word in inputData)
            {
                var sequence = new BinarySequence(Hasher.Hash(word));
                var (zeros, ones) = sequence.GetCountOfZerosAndOnes();
                var statistic = Math.Abs(ones - zeros);
                var pValue = MathHelper.Efrc(statistic / Math.Sqrt(2));

                if (IsSuccessed(pValue)) 
                    continue;
                
                lock (Locker)
                {
                    ++FailedCount;
                }
            }
        }
    }
}