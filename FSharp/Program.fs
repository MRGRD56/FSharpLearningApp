module FSharp.Program

open System


let getSum num1 num2 =
    num1 + num2

let isPositive number =
    if number >= 0 then true else false //or just: number >= 0

[<EntryPoint>]
let main argv =
    let result = getSum 2 5
    printfn $"RESULT: {result}"
    0