using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace CSharp.Models
{
    public class MathExpression
    {
        public double Operand1 { get; }
        public double Operand2 { get; }
        public MathOperator Operator { get; }

        public MathExpression(double operand1, double operand2, MathOperator @operator)
        {
            Operand1 = operand1;
            Operand2 = operand2;
            Operator = @operator;
        }

        public static MathExpression Parse(string expressionString)
        {
            var expressionMatch = Regex.Match(expressionString, @"^(\-?[\d,.]+)\s*([\+\-\*\/])\s*(\-?[\d,.]+)$");
            var expressionMatchGroups = expressionMatch.Groups.Cast<Group>().ToArray();
            if (!expressionMatch.Success || expressionMatchGroups.Any(g => !g.Success))
            {
                throw GetParseException();
            }

            var expressionParts = expressionMatchGroups.Select(g => g.Value).ToArray();
            return new MathExpression(
                ParseOperand(expressionParts[1]), 
                ParseOperand(expressionParts[3]),
                ParseOperator(expressionParts[2]));
        }

        public static bool TryParse(string expressionString, out MathExpression mathExpression)
        {
            try
            {
                mathExpression = Parse(expressionString);
                return true;
            }
            catch (FormatException)
            {
                mathExpression = null;
                return false;
            }
        }
        
        public double Solve()
        {
            return Operator switch
            {
                MathOperator.Plus => Operand1 + Operand2,
                MathOperator.Minus => Operand1 - Operand2,
                MathOperator.Multiply => Operand1 * Operand2,
                MathOperator.Divide => Operand1 / Operand2,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public override string ToString()
        {
            var operatorString = OperatorStringDictionary[Operator];
            return $"{Operand1} {operatorString} {Operand2}";
        }

        
        private static double ParseOperand(string operandString)
        {
            try
            {
                return double.Parse(operandString, new CultureInfo("en-US"));
            }
            catch (FormatException)
            {
                throw GetParseException();
            }
        }

        private static MathOperator ParseOperator(string operatorString)
        {
            try
            {
                return OperatorStringDictionary
                    .First(x => x.Value == operatorString).Key;
            }
            catch (InvalidOperationException)
            {
                throw GetParseException();
            }
        }

        private static Dictionary<MathOperator, string> OperatorStringDictionary => new()
        {
            { MathOperator.Plus, "+" },
            { MathOperator.Minus, "-" },
            { MathOperator.Multiply, "*" },
            { MathOperator.Divide, "/" }
        };
        
        private static FormatException GetParseException() => new("Failed to parse the expression");
    }
}