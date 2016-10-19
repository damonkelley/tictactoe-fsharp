module TestHelpers

open NUnit.Framework
open System.IO

#nowarn "0760"

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

let resetIO () =
    let stdout =  new StreamWriter(System.Console.OpenStandardOutput())
    stdout.AutoFlush <- true
    System.Console.SetOut(stdout)

    System.Console.OpenStandardInput()
    |> StreamReader
    |> System.Console.SetIn
