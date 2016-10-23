module Strategy.Test

open NUnit.Framework
open FsUnit
open TestHelpers
open TestDoubles

let game =
    Game.create
    <| Player.create testStrategy "X"
    <| Player.create testStrategy "O"

let pickRandom (options:int list) =
    let random = System.Random()
    options.Item (random.Next(options.Length))

[<Test>]
let ``random strategy will choose a random available board space`` () =
    let moves = [for _ in [1..4] -> pickRandom (Game.availableSpaces game)]
    let game = List.fold (fun game s -> Game.move s game) game moves

    let invalidChoices  =
        [1..9]
        |> List.fold (fun choices _ -> (Strategy.randomSpace game)::choices) []
        |> List.filter (fun s -> List.contains s moves)

    invalidChoices |> shouldEqual []

[<Test>]
let ``human strategy will get the input from stdin`` () =
    Strategy.human (TestUI("1")) game
        |> should equal 1

    Strategy.human (TestUI("2")) game
        |> should equal 2
