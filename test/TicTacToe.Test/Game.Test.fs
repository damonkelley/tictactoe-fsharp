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

let draw game =
    game
    |> move 1 "X"
    |> move 3 "O"
    |> move 7 "X"
    |> move 4 "O"
    |> move 9 "X"
    |> move 5 "O"
    |> move 6 "X"
    |> move 8 "O"
    |> move 2 "X"

[<Test>]
let ``create makes a new Game record`` () =
    let expectedGame =
        { Outcome = InProgress
        ; Board = Board.create()
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
let ``updateOutcome checks for a draw`` () =
    let game =
        game
        |> draw
        |> updateOutcome

    game.Outcome |> should equal Draw

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
        |> xWins
        |> findWinner
        |> should equal <| Some "X"

    Game.create()
        |> move 4 "O"
        |> move 5 "O"
        |> move 6 "O"
        |> findWinner
        |> should equal <| Some "O"

[<Test>]
let ``findWinner finds none when there is no winner`` () =
    Game.create()
        |> move 1 "X"
        |> move 4 "O"
        |> move 2 "X"
        |> move 5 "O"
        |> move 9 "X"
        |> findWinner
        |> should equal None

[<Test>]
let ``findWinner finds none with an empty board`` () =
    Game.create()
        |> findWinner
        |> should equal None


[<Test>]
let ``availableSpaces collects all spaces when the board is empty`` () =
    Game.create()
    |> Game.availableSpaces
    |> shouldEqual <| [1; 2; 3; 4; 5; 6; 7; 8; 9]

[<Test>]
let ``availableSpaces collects only the empty spaces`` () =
    Game.create()
    |> move 4 "X"
    |> move 5 "O"
    |> Game.availableSpaces
    |> shouldEqual <| [1; 2; 3; 6; 7; 8; 9]
