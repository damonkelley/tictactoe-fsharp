module Console.Test

open NUnit.Framework
open FsUnit

open UI
open Console
open System.IO
open System.Text

let console = new Console() :> UI

[<Test>]
let ``console reads from stdin`` () =
    let console = new Console() :> UI
    System.Console.SetIn(new StringReader("A\nB\n")) |> ignore

    console.ReadLine()
    |> should equal <| Some "A"

    console.ReadLine()
    |> should equal <| Some "B"

[<Test>]
let ``console writes to stdout`` () =
    let output = new StringWriter()
    System.Console.SetOut(output) |> ignore

    console.Write "A" |> ignore
    output.ToString() |> should equal "A"

    console.Write "B" |> ignore
    output.ToString() |> should equal "AB"

[<Test>]
let ``console prompts can prompt for input`` () =
    let output = new StringWriter()
    System.Console.SetOut(output) |> ignore
    System.Console.SetIn(new StringReader("5\n")) |> ignore

    console.Prompt "How old are you" id
    |> should equal "5"

[<Test>]
let ``prompt uses a transformer to validate and parse the input`` () =
    let output = new StringWriter()
    System.Console.SetOut(output) |> ignore
    System.Console.SetIn(new StringReader("5\n")) |> ignore

    let transformer input =
        Some "tranformed"

    console.Prompt "How old are you" transformer
    |> should equal <| Some "tranformed"
