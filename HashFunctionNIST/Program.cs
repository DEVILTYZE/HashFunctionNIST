using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HashFunctionNIST.Tests;

namespace HashFunctionNIST
{
    internal static class Program
    {
        private static void Main()
        {
            //RandomStrings();
            RunTests();
            
            Console.WriteLine("Нажмите Enter для завершения программы...");
            Console.ReadKey();
        }

        private static void RunTests()
        {
            Console.WriteLine("Тестирование...\r\n");
            
            string[] inputData;
            
            if (File.Exists(Environment.CurrentDirectory + "\\data.txt"))
            {
                using var sr = new StreamReader("data.txt");
                inputData = sr.ReadToEnd().Split(char.MinValue);
            }
            else return;
            
            ITest[] tests = { /*new FrequencyTest(),*/ new DisjointPatternTest() };

            foreach (var test in tests)
                test.Run(inputData);
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

                if (t % 10000 == 0)
                    Console.WriteLine($"{t} строк готово...");
            }

            using var sw = new StreamWriter("data.txt");
            
            foreach (var word in set)
                sw.Write(word + char.MinValue);
            
            Console.WriteLine("Данные созданы");
        }
    }
}