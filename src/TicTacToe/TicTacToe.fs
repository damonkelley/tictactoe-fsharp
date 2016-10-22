module TicTacToe

open UI
open Game
open InputTransformer

type TicTacToe =
    { UI : UI
    ; Game : Game
    ; Presenter : Game -> string
    }

let create ui presenter game =
    { UI = ui
    ; Game = game
    ; Presenter = presenter
    }

let private write ttt output =
    ttt.UI.Write(output) |> ignore
    ttt

let private present ttt =
    ttt.UI.Update() |> ignore
    ttt.Presenter ttt.Game
    |> write ttt

let private move ttt =
    let game = ttt.Game
    let {Turn = player } = game
    {ttt with Game = Game.move <| player.Strategy(game) <| game}

let private doTurn = move >> present

let rec private loop ttt =
    match doTurn ttt with
    | {Game = {Outcome = InProgress}} as ttt -> loop ttt
    | ttt -> ttt

let start = present >> loop

let private exitCode _ =
    0

let withSetup () =
    let config = Setup.run(Console.Console())

    Game.create config.Player1 config.Player2
    |> create config.UI Presenter.present
    |> start

[<EntryPoint>]
let main argv =
    withSetup() |> exitCode
