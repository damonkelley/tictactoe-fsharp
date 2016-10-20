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

let private humanMove (ui:UI) game =
    let transformer =
        toWhitelistedInteger << availableSpaces

    ui.Prompt prompt <| transformer game

let private userMove {Game = game; UI = ui} =
    match game with
    | {Turn = {Type = Player.Human}} -> humanMove ui game

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
    Game.create <| Player.create "X" <| Player.create "O"
    |> create (Console.Console()) (Presenter.present)
    |> start
    |> exitCode
