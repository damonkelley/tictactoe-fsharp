module TicTacToe.Test

open NUnit.Framework
open FsUnit
open TestHelpers
open TestDoubles

open System.IO
open System.Text

open TicTacToe

let xWins = [1; 4; 2; 5; 3;]
let draw = [1; 3; 7; 4; 9; 5; 6; 8; 2]

let assertGameWasPresented output game move =
    let game = Game.move move game
    output.ToString()
    |> should contain (Presenter.present game)
    game

let startTicTacToe () =
    let ui = Console.Console()

    (Player.create (Strategy.human ui) "X", Player.create (Strategy.human ui) "O")
    ||> Game.create
    |> TicTacToe.create (Console.Console()) (Presenter.present)
    |> TicTacToe.start
    |> ignore

let setupFakeConsole (input:string list) =
    System.String.Join("\n", input)
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
    let input = List.map (sprintf "%d") xWins
    let input = List.append input ["n"]

    let output = setupFakeConsole input

    startTicTacToe()

    output.ToString() |> should contain "X wins!"

[<Test>]
let ``Draw is shown if there is no winner`` () =
    let input = List.map (sprintf "%d") draw
    let input = List.append input ["n"]

    let output = setupFakeConsole input

    startTicTacToe()

    output.ToString() |> should contain "Draw"
    output.ToString() |> should not' (contain "X wins!")

[<Test>]
let ``the board is presented after each move`` () =
    let input = List.map (sprintf "%d") draw
    let input = List.append input ["n"]

    let output = setupFakeConsole input

    startTicTacToe()

    let newGame =
        Game.create
        <| Player.create testStrategy "X"
        <| Player.create testStrategy "O"

    draw
    |> List.fold (assertGameWasPresented output) newGame
    |> ignore

[<Test>]
let ``the user is prompted to play again`` () =
    let xWins = List.map (sprintf "%d") xWins
    let draw = List.map (sprintf "%d") draw
    let input = List.concat [xWins; ["y"]; draw; ["n"]]

    let output = setupFakeConsole input

    startTicTacToe()

    let newGame =
        Game.create
        <| Player.create testStrategy "X"
        <| Player.create testStrategy "O"

    output.ToString() |> should contain "Play again? (y/n)"
    output.ToString() |> should contain "X wins!"
    output.ToString() |> should contain "Draw"

[<Test>]
let ``withSetup presents setup options before starting the game`` () =
    let moves = List.map (sprintf "%d") draw
    let input = List.concat [["h"; "h"; "X"]; moves; ["n"]]

    let output = setupFakeConsole input

    TicTacToe.withSetup() |> ignore

    output.ToString() |> should contain "Is X a Human or Computer?"
    output.ToString() |> should contain "Is O a Human or Computer?"
    output.ToString() |> should contain "Which player will go first?"
    output.ToString() |> should contain "Draw"
