module Game.Test

open NUnit.Framework
open FsUnit
open TestHelpers

let game = Game.create()

let xWins game =
    game
    |> move 1 "X"
    |> move 4 "O"
    |> move 2 "X"
    |> move 5 "O"
    |> move 3 "X"

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

    game.Outcome |> should equal <| Winner "X"

[<Test>]
let ``move updates the outcome`` () =
    game.Outcome |> should equal InProgress

    let game = game |> xWins

    game.Outcome |> should equal <| Winner "X"


[<Test>]
let ``move updates the board`` () =
    {game with Board = Board.move 1 "X" game.Board}
        |> shouldEqual <| Game.move 1 "X" game

    {game with Board = Board.move 1 "O" game.Board}
        |> shouldEqual <| Game.move 1 "O" game

[<Test>]
let ``findWinner finds some winner`` () =
    Game.create()
        |> move 1 "X"
        |> move 4 "O"
        |> move 2 "X"
        |> move 5 "O"
        |> move 3 "X"
        |> findWinner
        |> should equal <| Some "X"

    Game.create()
        |> move 4 "O"
        |> move 5 "O"
        |> move 6 "O"
        |> findWinner
        |> should equal <| Some "O"

[<Test>]
let ``or findWinner finds none`` () =
    Game.create()
        |> findWinner
        |> should equal None

    Game.create()
        |> move 1 "X"
        |> move 4 "O"
        |> move 2 "X"
        |> move 5 "O"
        |> move 9 "X"
        |> findWinner
        |> should equal None
