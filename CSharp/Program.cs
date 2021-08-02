using System;
using CSharp.Models;

namespace CSharp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var input = Console.ReadLine();
            if (MathExpression.TryParse(input, out var expression))
            {
                Console.WriteLine($"{expression} = {expression.Solve()}");
            }
            else
            {
                Console.WriteLine("Failed to parse the expression");
            }
        }
    }
}