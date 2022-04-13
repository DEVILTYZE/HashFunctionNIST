using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using HashFunctionNIST.Models;

namespace HashFunctionNIST.Tests
{
    public abstract class Test : ITest
    {
        public const int TestCount = 15000;
        
        protected readonly Fugue256 Hasher;
        private readonly ConsoleColor _defaultFront;
        private readonly ConsoleColor _defaultBack;
        private readonly double _alpha;
        private readonly string _name;

        private int _failedCount;
        private readonly List<string> _failed;
        private readonly object _locker = new();
        
        protected Test(string name, double alpha)
        {
            Hasher = new Fugue256();
            _failed = new List<string>();
            _failedCount = 0;
            _defaultFront = Console.ForegroundColor;
            _defaultBack = Console.BackgroundColor;
            _name = name;
            _alpha = alpha;
        }

        public void Run(string[] inputData)
        {
            const int maxCountEdges = 5;
            var listOfThreads = new List<Thread>();
            var count = inputData.Length / (maxCountEdges - 1);
            
            for (var i = 0; i < maxCountEdges - 1; ++i)
            {
                var data = inputData[(i * count)..((i + 1) * count)];
                var thread = new Thread(RunThread);
                thread.Start(data);
                listOfThreads.Add(thread);
            }

            foreach (var thread in listOfThreads)
                thread.Join();
            
            Assert(TestCount);
        }

        public void Run(string inputData)
        {
            var pValue = GetPValue(inputData);

            if (!IsSuccessed(pValue))
                ++_failedCount;
            
            Assert(1, false);
        }

        private void RunThread(object data)
        {
            var inputData = (string[])data;
            
            foreach (var word in inputData)
            {
                var pValue = GetPValue(word);

                if (IsSuccessed(pValue)) 
                    continue;
                
                lock (_locker)
                {
                    ++_failedCount;
                    _failed.Add(word);
                }
            }
        }

        protected abstract double GetPValue(string data);

        private bool IsSuccessed(double pValue) => pValue > _alpha;

        private void Assert(int testCount, bool isWrite = true)
        {
            var isSuccessed = _failedCount == 0;
            
            if (!isSuccessed && isWrite)
                SaveResults();

            lock (_locker)
            {
                Console.BackgroundColor = isSuccessed ? ConsoleColor.Green : ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("\t");
                Console.BackgroundColor = _defaultBack;
                Console.ForegroundColor = _defaultFront;
                Console.WriteLine($" {_name}: " + (isSuccessed ? "УДАЧНО" : "НЕУДАЧНО") 
                                                + $"; Тестов пройдено: {testCount}; Неудачных тестов: {_failedCount}");
            }
        }

        private void SaveResults()
        {
            using var sw = new StreamWriter(Environment.CurrentDirectory + @"\logs\log_" + 
                                            DateTime.Now.ToString("ddMMyyHHmmss") + ".txt");
            
            foreach (var word in _failed)
                sw.WriteLine(word);
        }
    }
}