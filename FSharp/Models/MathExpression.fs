namespace FSharp.Models

open System
open System.Globalization
open System.Linq
open System.Text.RegularExpressions

[<AllowNullLiteral>]
type MathExpression(operand1: double, operand2: double, operator: MathOperator) = class
    static member private getParseException =
        new FormatException("Failed to parse the expression")

    static member private operatorStringDictionary = 
        dict [
            MathOperator.Plus, "+";
            MathOperator.Minus, "-";
            MathOperator.Multiply, "*";
            MathOperator.Divide, "/"
        ]

    static member private parseOperand operandString =
        try
            Double.Parse(operandString, new CultureInfo("en-US"))
        with
            | :? InvalidOperationException -> raise MathExpression.getParseException

    static member private parseOperator operatorString =
        try
            MathExpression.operatorStringDictionary.First(fun x -> x.Value.Equals(operatorString)).Key
        with
            | :? InvalidOperationException -> raise MathExpression.getParseException

    override this.ToString() =
        let operatorString = MathExpression.operatorStringDictionary.[operator];
        $"{operand1} {operatorString} {operand2}"

    static member public parse expressionString =
        let expressionMatch = Regex.Match(expressionString, @"^(\-?[\d,.]+)\s*([\+\-\*\/])\s*(\-?[\d,.]+)$");
        let expressionMatchGroups = expressionMatch.Groups.Cast<Group>().ToArray();
        if not expressionMatch.Success || expressionMatchGroups.Any(fun g -> not g.Success) 
        then raise MathExpression.getParseException else
        let expressionParts = expressionMatchGroups.Select(fun g -> g.Value).ToArray();
        new MathExpression(
            MathExpression.parseOperand expressionParts.[1], 
            MathExpression.parseOperand expressionParts.[3],
            MathExpression.parseOperator expressionParts.[2]);

    static member public tryParse expressionString =
        try
            (MathExpression.parse expressionString, true)
        with
        | :? FormatException -> (null, false)

    member public this.solve =
        match operator with
        | MathOperator.Plus     -> operand1 + operand2
        | MathOperator.Minus    -> operand1 - operand2
        | MathOperator.Multiply -> operand1 * operand2
        | MathOperator.Divide   -> operand1 / operand2
end