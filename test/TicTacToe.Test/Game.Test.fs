module Game.Test

open NUnit.Framework
open FsUnit
open TestHelpers

let game = Game.create()

[<Test>]
let ``create makes a new Game record`` () =
    let expectedGame = {
        Outcome = InProgress;
        Board = Board.create()
    }
    Game.create() |> should equal expectedGame

[<Test>]
let ``move updates the board`` () =
    let expectedGame = {game with Board = Board.move 0 "X" game.Board}
    expectedGame |> shouldEqual <| Game.move 0 game
