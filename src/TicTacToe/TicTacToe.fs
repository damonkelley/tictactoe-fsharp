module TicTacToe

open UI
open Game
open InputTransformer

type TicTacToe =
    { UI : UI
    ; Game : Game
    ; Presenter : Game -> string
    }

let prompt = "-> "

let create ui presenter game =
    { UI = ui
    ; Game = game
    ; Presenter = presenter
    }

let private write ttt output =
    ttt.UI.Write(output) |> ignore
    ttt

let private present ttt =
    ttt.Presenter ttt.Game
    |> write ttt

let private human =
    Strategy.human prompt

let private move ttt =
    let game = ttt.Game
    {ttt with Game = Game.move <| ttt.Game.Turn.Strategy(game) <| ttt.Game}

let private doTurn = move >> present

let rec private loop ttt =
    match doTurn ttt with
    | {Game = {Outcome = InProgress}} as ttt -> loop ttt
    | ttt -> ttt

let start = present >> loop

let private exitCode _ =
    0

[<EntryPoint>]
let main argv =
    let ui = Console.Console()

    (Player.create (human ui) "X", Player.create (human ui) "O")
    ||> Game.create
    |> create ui (Presenter.present)
    |> start
    |> exitCode
