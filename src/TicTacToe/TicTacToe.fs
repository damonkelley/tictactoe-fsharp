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

let private userMove ttt =
    let transformer =
        toWhitelistedInteger << availableSpaces

    ttt.UI.Prompt prompt <| transformer ttt.Game

let private move ttt =
    {ttt with Game = Game.move <| userMove ttt <| ttt.Game}

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
    Game.create "X" "O"
    |> create (Console.Console()) (Presenter.present)
    |> start
    |> exitCode
