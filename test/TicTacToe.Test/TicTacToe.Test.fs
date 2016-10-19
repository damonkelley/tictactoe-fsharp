module TicTacToe.Test

open NUnit.Framework
open FsUnit
open TestHelpers

open System.IO
open System.Text

open TicTacToe

#nowarn "0760"

[<TearDown>]
let ``reset stdin and stdout`` () =
    resetIO()

[<Test>]
let ``create initializes a new TicTacToe record`` () =
    let presenter game = ""
    let console = Console.Console()

    let expected =
        { UI = console
        ; Game = Game.create "X" "O"
        ; Presenter = presenter
        }

    let actual = TicTacToe.create console presenter "X" "O"

    expected.UI |> shouldEqual <| actual.UI
    expected.Game |> shouldEqual <| actual.Game

[<Test>]
let ``the winner is shown on the UI`` () =
    let output = new StringWriter() |> patchStdOut

    System.String.Join("\n", [1; 4; 2; 5; 3;])
    |> StringReader
    |> patchStdIn
    |> ignore

    TicTacToe.start <| new Console.Console() <| Game.create "X" "O" |> ignore

    output.ToString() |> should contain "X wins!"

[<Test>]
let ``Draw is shown if there is no winner`` () =
    let output = new StringWriter() |> patchStdOut

    System.String.Join("\n", [1; 3; 7; 4; 9; 5; 6; 8; 2])
    |> StringReader
    |> patchStdIn
    |> ignore

    TicTacToe.start <| new Console.Console() <| Game.create "X" "O" |> ignore

    output.ToString() |> should contain "Draw"
    output.ToString() |> should not' (contain "X wins!")

[<Test>]
let ``the board is presented after each move`` () =
    let output = new StringWriter() |> patchStdOut

    System.String.Join("\n", [1; 3; 7; 4; 9; 5; 6; 8; 2])
    |> StringReader
    |> patchStdIn
    |> ignore

    TicTacToe.start <| new Console.Console() <| Game.create "X" "O" |> ignore

    output.ToString() |> should contain "Draw"
    output.ToString() |> should not' (contain "X wins!")
