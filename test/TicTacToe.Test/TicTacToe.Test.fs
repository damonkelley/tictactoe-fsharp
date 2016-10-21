module TicTacToe.Test

open NUnit.Framework
open FsUnit
open TestHelpers
open TestDoubles

open System.IO
open System.Text

open TicTacToe

#nowarn "0760"

let xWins = [1; 4; 2; 5; 3;]
let draw = [1; 3; 7; 4; 9; 5; 6; 8; 2]

let assertGameWasPresented output game move =
    let game = Game.move move game
    output.ToString()
    |> should contain (Presenter.present game)
    game

let startTicTacToe () =
    let ui = Console.Console()

    (Player.create (Strategy.human "" ui) "X", Player.create (Strategy.human "" ui) "O")
    ||> Game.create
    |> TicTacToe.create (Console.Console()) (Presenter.present)
    |> TicTacToe.start
    |> ignore

let setupFakeConsole (moves:int list) =
    System.String.Join("\n", moves)
    |> StringReader
    |> patchStdIn
    |> ignore

    new StringWriter() |> patchStdOut

[<TearDown>]
let ``reset stdin and stdout`` () =
    resetIO()

[<Test>]
let ``create initializes a new TicTacToe record`` () =
    let presenter game = ""
    let console = Console.Console()
    let game =
        Game.create
        <| Player.create testStrategy "X"
        <| Player.create testStrategy "O"

    let expected =
        { UI = console
        ; Game = game
        ; Presenter = presenter
        }

    let actual = TicTacToe.create console presenter game

    expected.UI |> shouldEqual <| actual.UI
    expected.Game |> shouldEqual <| actual.Game

[<Test>]
let ``the winner is shown on the UI`` () =
    let output = setupFakeConsole xWins

    startTicTacToe()

    output.ToString() |> should contain "X wins!"

[<Test>]
let ``Draw is shown if there is no winner`` () =
    let output = setupFakeConsole draw

    startTicTacToe()

    output.ToString() |> should contain "Draw"
    output.ToString() |> should not' (contain "X wins!")

[<Test>]
let ``the board is presented after each move`` () =
    let output = setupFakeConsole draw

    startTicTacToe()

    let newGame =
        Game.create
        <| Player.create testStrategy "X"
        <| Player.create testStrategy "O"

    draw
    |> List.fold (assertGameWasPresented output) newGame
    |> ignore
