module Strategy.Test

open NUnit.Framework
open FsUnit
open FsCheck
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

[<Test>]
let ``computer strategy will find the winning move`` () =
    let game =
        game
        |> Game.move 1
        |> Game.move 4
        |> Game.move 2
        |> Game.move 5

    Strategy.computer game |> should equal 3

[<Test>]
let ``computer strategy will prevent a loss`` () =
    let game =
        game
        |> Game.move 1
        |> Game.move 5
        |> Game.move 7
        |> Game.move 4

    Strategy.computer game |> should equal 6

[<Test>]
let ``computer never loses`` () =
    let game =
        Game.create
        <| Player.create Strategy.randomSpace "X"
        <| Player.create Strategy.computer "O"

    let hook game = printfn "%s" (Presenter.present game); game
    let game = Game.play (id) game

    match game with
    | {Outcome = Winner {Marker = "O"}} -> ()
    | {Outcome = Draw} -> ()
    | _ -> failwith (sprintf "Unexpected loss: %s" (Presenter.present game))


type ComputerStrategyProperties =
    static member ``computer strategy never loses as player 1 against random player`` () =
        let game =
            Game.create
            <| Player.create Strategy.computer "O"
            <| Player.create Strategy.randomSpace "X"

        match Game.play (id) game with
        | {Outcome = Draw} -> true
        | {Outcome = Winner {Marker = "O"}} -> true
        | _ -> false

    static member ``computer strategy never loses as player 2 against random player`` () =
        let game =
            Game.create
            <| Player.create Strategy.randomSpace "X"
            <| Player.create Strategy.computer "O"

        match Game.play (id) game with
        | {Outcome = Draw} -> true
        | {Outcome = Winner {Marker = "O"}} -> true
        | _ -> false

[<Test>]
[<Category("Long")>]
let ``check properties of computer strategy`` () =
    let config = {Config.QuickThrowOnFailure with MaxTest = 50}
    Check.All<ComputerStrategyProperties> config
