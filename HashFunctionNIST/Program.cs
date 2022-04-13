using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using HashFunctionNIST.Models;
using HashFunctionNIST.Tests;

namespace HashFunctionNIST
{
    internal static class Program
    {
        private static void Main()
        {
            //RandomStrings();
            RunTests();
            //TestOne(",>A_<ZMG@J6~r6dTARZ~g?DisK");
            
            Console.WriteLine("Нажмите Enter для завершения программы...");
            Console.ReadKey();
        }

        private static void RunTests()
        {
            Console.WriteLine("Тестирование...\r\n");
            
            string[] data;
            var threadList = new List<Thread>();
            
            if (File.Exists(Environment.CurrentDirectory + "\\data.txt"))
            {
                using var sr = new StreamReader("data.txt");
                data = sr.ReadToEnd().Split(char.MinValue);
            }
            else return;
            
            ITest[] tests =
            {
                new FrequencyTest(),
                new BlockTest(),
                new HoleTest(),
                new DisjointPatternTest(),
                new JointPatternTest()
            };

            foreach (var test in tests)
            {
                var thread = new Thread(RunOne);
                thread.Start((data, test));
                threadList.Add(thread);
            }

            foreach (var thread in threadList)
                thread.Join();
        }

        private static void TestOne(string data)
        {
            Console.WriteLine("String: " + data);
            Console.WriteLine("Array: " + string.Join(" ", new Fugue256().Hash(data)) + Environment.NewLine);

            ITest[] tests =
            {
                new FrequencyTest(),
                new BlockTest(),
                new HoleTest(),
                new DisjointPatternTest(),
                new JointPatternTest()
            };

            foreach (var test in tests)
                test.Run(data);
        }


        private static void RunOne(object obj)
        {
            var (data, test) = ((string[], ITest))obj;
            test.Run(data);
        }
        
        private static void RandomStrings()
        {
            Console.WriteLine("Создание данных...");
            
            var random = new Random();
            var set = new HashSet<string>();
            const int start = 32;
            const int end = 127;

            for (var t = 0; t < Test.TestCount; ++t)
            {
                string word;
                
                do
                {
                    var length = random.Next(1, 250);
                    var array = new byte[length];

                    for (var i = 0; i < length; ++i)
                        array[i] = (byte)random.Next(start, end);

                    word = Encoding.UTF8.GetString(array);
                } 
                while (set.Contains(word));

                set.Add(word);

                if ((t + 1) % 1000 == 0)
                    Console.WriteLine($"{t + 1} строк готово...");
            }

            using var sw = new StreamWriter("data.txt");
            
            foreach (var word in set)
                sw.Write(word + char.MinValue);
            
            Console.WriteLine("Данные созданы");
        }
    }
}