using System;
using CSharp.Models;

namespace CSharp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var input = Console.ReadLine();
            var expression = MathExpression.Parse(input);
            Console.WriteLine($"{expression} = {expression.Solve()}");
        }
    }
}