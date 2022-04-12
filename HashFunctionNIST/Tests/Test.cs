using System;
using System.Collections.Generic;
using System.Threading;
using HashFunctionNIST.Models;

namespace HashFunctionNIST.Tests
{
    public abstract class Test : ITest
    {
        public const int TestCount = 25000;
        
        protected readonly Fugue256 Hasher;
        private readonly ConsoleColor _defaultFront;
        private readonly ConsoleColor _defaultBack;
        private readonly double _alpha;
        private readonly string _name;

        protected int FailedCount = 0;
        protected readonly object Locker = new();
        
        protected Test(string name, double alpha)
        {
            Hasher = new Fugue256();
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
            
            Assert();
        }

        protected abstract void RunThread(object data);

        protected bool IsSuccessed(double pValue) => pValue > _alpha;

        private void Assert()
        {
            var isSuccessed = FailedCount == 0;

            Console.BackgroundColor = isSuccessed ? ConsoleColor.Green : ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("\t");
            Console.BackgroundColor = _defaultBack;
            Console.ForegroundColor = _defaultFront;
            Console.WriteLine($" {_name}: " + (isSuccessed ? "УДАЧНО" : "НЕУДАЧНО") 
                + $"; Тестов пройдено: {TestCount}; Неудачных тестов: {FailedCount}");
        }
    }
}