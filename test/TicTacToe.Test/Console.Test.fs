module Console.Test

open NUnit.Framework
open FsUnit
open TestHelpers

open System.IO
open System.Text

open UI
open Console

let console = new Console() :> UI

[<Test>]
let ``ReadLine reads from stdin`` () =
    new StringReader("A\nB\n") |> patchStdIn |> ignore

    console.ReadLine()
    |> should equal <| Some "A"

    console.ReadLine()
    |> should equal <| Some "B"

[<Test>]
let ``ReadLine returns None when there is no more input`` () =
    new StringReader("") |> patchStdIn |> ignore

    console.ReadLine()
    |> should equal <| None

[<Test>]
let ``Write writes to stdout`` () =
    let output = new StringWriter() |> patchStdOut

    console.Write "A" |> ignore
    output.ToString() |> should equal "A"

    console.Write "B" |> ignore
    output.ToString() |> should equal "AB"

[<Test>]
let ``Prompt will prompt for input from stdin`` () =
    let output = new StringWriter() |> patchStdOut
    new StringReader("5\n") |> patchStdIn |> ignore

    console.Prompt "How old are you" id
    |> should equal "5"

[<Test>]
let ``Prompt uses a transformer to validate and parse the input`` () =
    let output = new StringWriter() |> patchStdOut
    new StringReader("5\n") |> patchStdIn |> ignore

    let transformer input =
        Some "tranformed"

    console.Prompt "How old are you" transformer
    |> should equal <| Some "tranformed"