module FSharp.Program

open System
open FSharp.Models

[<EntryPoint>]
let main args =
    let input = Console.ReadLine()
    let expression, success = MathExpression.tryParse input
    let result = 
        if success then
            $"{expression} = {expression.solve}"
        else 
            "Failed to parse the expression"
    printf "%s" result
    0