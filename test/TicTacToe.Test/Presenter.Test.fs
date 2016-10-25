module Presenter.Test

open NUnit.Framework
open FsUnit
open TestHelpers
open TestDoubles

let game =
    Game.create
    <| Player.create testStrategy "X"
    <| Player.create testStrategy "O"

[<Test>]
let ``presentFor presents the empty board`` () =
    let expected = " 1 | 2 | 3\n\
                    ---+---+---\n \
                     4 | 5 | 6\n\
                    ---+---+---\n \
                     7 | 8 | 9\n"

    game
    |> Presenter.presentFor Presenter.Board
    |> shouldEqual expected

[<Test>]
let ``presentFor presents a board with markers`` () =
    let (player1, player2) = game.Players
    let expected =
        sprintf " 1 | 2 | 3\n---+---+---\n 4 | %s | 6\n---+---+---\n 7 | 8 | %s\n"
        <| colorize player1.Marker
        <| colorize player2.Marker

    game
    |> Game.move 5
    |> Game.move 9
    |> Presenter.presentFor Presenter.Board
    |> shouldEqual expected

[<Test>]
let ``presentFor presents player1 when it wins`` () =
    {game with Game.Outcome = Winner <| Player.create (fun _ -> 1) "X"}
    |> Presenter.presentFor Presenter.Outcome
    |> shouldEqual "X wins!"

[<Test>]
let ``presentFor presents player2 when it wins`` () =
    {game with Game.Outcome = Winner <| Player.create (fun _ -> 1) "O"}
    |> Presenter.presentFor Presenter.Outcome
    |> shouldEqual "O wins!"

[<Test>]
let ``presentFor presents the Draw outcome`` () =
    {game with Game.Outcome = Draw}
    |> Presenter.presentFor Presenter.Outcome
    |> shouldEqual "Draw"

[<Test>]
let ``presentFor presents the current players turn when the game is in progress`` () =
    {game with Game.Outcome = InProgress}
    |> Presenter.presentFor Presenter.Outcome
    |> shouldEqual "X is up!"

[<Test>]
let ``present presents the outcome and the game`` () =
    let expected = " 1 | 2 | 3\n\
                    ---+---+---\n \
                     4 | 5 | 6\n\
                    ---+---+---\n \
                     7 | 8 | 9\n\
                     \n\
                     X is up!\n"

    game
    |> Presenter.present
    |> shouldEqual expected
