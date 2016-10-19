module TestHelpers

open NUnit.Framework
open System.IO

let inspect value  =
    printf "%A" value
    value

let shouldEqual (x: 'a) (y: 'a) =
    Assert.AreEqual(x, y, sprintf "Expected: %A\nActual: %A" x y)

let patchStdOut output =
    System.Console.SetOut(output) |> ignore
    output

let patchStdIn input =
    System.Console.SetIn(input) |> ignore
    input
