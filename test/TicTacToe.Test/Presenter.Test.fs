module Presenter.Test

open NUnit.Framework
open FsUnit
open TestHelpers
open Game.Test

[<Test>]
let ``presentFor presents the empty board`` () =
    let expected = " 1 | 2 | 3\n\
                    ---+---+---\n \
                     4 | 5 | 6\n\
                    ---+---+---\n \
                     7 | 8 | 9\n"

    Game.create "X" "O"
    |> Presenter.presentFor Presenter.Board
    |> shouldEqual expected

[<Test>]
let ``presentFor presents a board with markers`` () =
    let expected = " 1 | 2 | 3\n\
                    ---+---+---\n \
                     4 | X | 6\n\
                    ---+---+---\n \
                     7 | 8 | O\n"

    Game.create "X" "O"
    |> Game.move 5
    |> Game.move 9
    |> Presenter.presentFor Presenter.Board
    |> shouldEqual expected

[<Test>]
let ``presentFor presents player1 when it wins`` () =
    {Game.create "X" "O" with Game.Outcome = Game.Winner "X"}
    |> Presenter.presentFor Presenter.Outcome
    |> shouldEqual "X wins!"

[<Test>]
let ``presentFor presents player2 when it wins`` () =
    {Game.create "X" "O" with Game.Outcome = Game.Winner "O"}
    |> Presenter.presentFor Presenter.Outcome
    |> shouldEqual "O wins!"

[<Test>]
let ``presentFor presents the Draw outcome`` () =
    {Game.create "X" "O" with Game.Outcome = Game.Draw}
    |> Presenter.presentFor Presenter.Outcome
    |> shouldEqual "Draw"

[<Test>]
let ``presentFor presents the current players turn when the game is in progress`` () =
    {Game.create "X" "O" with Game.Outcome = Game.InProgress}
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

    Game.create "X" "O"
    |> Presenter.present
    |> shouldEqual expected
