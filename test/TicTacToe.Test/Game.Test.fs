module Game.Test

open NUnit.Framework
open FsUnit
open TestHelpers

let player1 = Player.create "X"
let player2 = Player.create "O"

let game = Game.create player1 player2

let xWins game =
    game
    |> move 1 // X
    |> move 4 // O
    |> move 2
    |> move 5
    |> move 3

let oWins game =
    game
    |> move 1 // X
    |> move 4 // O
    |> move 3
    |> move 5
    |> move 7
    |> move 6

let draw game =
    game
    |> move 1 // X
    |> move 3 // O
    |> move 7
    |> move 4
    |> move 9
    |> move 5
    |> move 6
    |> move 8
    |> move 2

[<Test>]
let ``create makes a new Game record`` () =
    let expectedGame =
        { Outcome = InProgress
        ; Board   = Board.create()
        ; Players = player1, player2
        ; Turn    = player1
        }
    Game.create player1 player2 |> should equal expectedGame

[<Test>]
let ``updateOutcome checks for a winner`` () =
    let game =
        game
        |> xWins
        |> updateOutcome

    game.Outcome |> should equal <| Winner player1

[<Test>]
let ``updateOutcome checks for a draw`` () =
    let game =
        game
        |> draw
        |> updateOutcome

    game.Outcome |> shouldEqual Game.Draw

[<Test>]
let ``swapTurn swaps the Turn`` () =
    game
    |> swapTurn
    |> shouldEqual <| {game with Turn = player2}

    game
    |> swapTurn
    |> swapTurn
    |> shouldEqual <| {game with Turn = player1}

[<Test>]
let ``move updates the outcome`` () =
    let game = game |> xWins
    game.Outcome |> should equal <| Winner player1

[<Test>]
let ``move updates the turn`` () =
    let game = game |> move 1
    game.Turn |> should equal <| player2

[<Test>]
let ``move updates the board`` () =
    {game with Board = Board.move 1 player1 game.Board; Turn = player2}
        |> shouldEqual <| Game.move 1 game

    let game = Game.create player2 player1
    {game with Board = Board.move 1 player2 game.Board; Turn = player1}
        |> shouldEqual <| Game.move 1 game

[<Test>]
let ``findWinner finds some winner`` () =
    game
    |> xWins
    |> findWinner
    |> shouldEqual <| Some player1

    game
    |> oWins
    |> findWinner
    |> shouldEqual <| Some player2

[<Test>]
let ``findWinner finds none when there is no winner`` () =
    game
    |> move 1
    |> move 4
    |> move 2
    |> move 5
    |> move 9
    |> findWinner
    |> should equal None

[<Test>]
let ``findWinner finds none with an empty board`` () =
    game
    |> findWinner
    |> should equal None

[<Test>]
let ``availableSpaces collects all spaces when the board is empty`` () =
    game
    |> Game.availableSpaces
    |> shouldEqual <| [1; 2; 3; 4; 5; 6; 7; 8; 9]

[<Test>]
let ``availableSpaces collects only the empty spaces`` () =
    game
    |> move 4
    |> move 5
    |> Game.availableSpaces
    |> shouldEqual <| [1; 2; 3; 6; 7; 8; 9]
