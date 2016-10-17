module Game.Test

open NUnit.Framework
open FsUnit
open TestHelpers

let game = Game.create()

let xWins game =
    game
    |> move 0 "X"
    |> move 3 "O"
    |> move 1 "X"
    |> move 4 "O"
    |> move 2 "X"

[<Test>]
let ``create makes a new Game record`` () =
    let expectedGame = {
        Outcome = InProgress;
        Board = Board.create()
    }
    Game.create() |> should equal expectedGame

[<Test>]
let ``updateOutcome checks for a winner`` () =
    let game =
        game
        |> xWins
        |> updateOutcome

    game.Outcome |> should equal Winner

[<Test>]
let ``move updates the outcome`` () =
    game.Outcome |> should equal InProgress

    let game = game |> xWins

    game.Outcome |> should equal Winner


[<Test>]
let ``move updates the board`` () =
    {game with Board = Board.move 0 "X" game.Board}
        |> shouldEqual <| Game.move 0 "X" game

    {game with Board = Board.move 0 "O" game.Board}
        |> shouldEqual <| Game.move 0 "O" game

[<Test>]
let ``findWinner finds some winner`` () =
    Game.create()
        |> move 0 "X"
        |> move 3 "O"
        |> move 1 "X"
        |> move 4 "O"
        |> move 2 "X"
        |> findWinner
        |> should equal <| Some "X"

    Game.create()
        |> move 3 "O"
        |> move 4 "O"
        |> move 5 "O"
        |> findWinner
        |> should equal <| Some "O"

[<Test>]
let ``or findWinner finds none`` () =
    Game.create()
        |> findWinner
        |> should equal None

    Game.create()
        |> move 0 "X"
        |> move 3 "O"
        |> move 1 "X"
        |> move 4 "O"
        |> move 8 "X"
        |> findWinner
        |> should equal None
