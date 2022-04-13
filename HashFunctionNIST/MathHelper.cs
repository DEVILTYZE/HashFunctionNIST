using System;
using MathNet.Numerics;

namespace HashFunctionNIST
{
    public static class MathHelper
    {
        public static double Efrc(double value)
        {
            const int maxCount = 10;
            var result = 0.0;
        
            for (int factorial = 0, evenNumber = 1; factorial < maxCount; ++factorial, evenNumber += 2)
                result += (-1 ^ factorial) * (1 / SpecialFunctions.Factorial(factorial)) * 
                    Math.Pow(value, evenNumber) / evenNumber;
        
            return 1 - result * (2 / Math.Sqrt(Math.PI));
        }

        public static double Igmc(double a, double x)
        {
            var gx = Integral(t => Math.Pow(t, a - 1) * Math.Pow(Math.E, -t), 0, x);
            var g = Math.Pow(2, a + 1) / a * Integral(t => t * Math.Pow(-Math.Log(t, Math.E), a), 0, 1);
        
            return 1 - gx / g;
        }

        private static double Integral(Func<double, double> function, double a, double b)
        {
            const int n = 10000;
            
            var sum = 0.0;
            var h = (b - a) / n;
        
            for (var i = 0; i < n; ++i)
                sum += 0.5 * h * (function.Invoke(a + i * h) + function.Invoke(a + (i + 1) * h));
        
            return sum;
        }
    }
}