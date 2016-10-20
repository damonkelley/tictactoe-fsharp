module TicTacToe

open UI
open Game
open InputTransformer

type TicTacToe =
    { UI : UI
    ; Game : Game
    ; Presenter : Game -> string
}

let create ui presenter p1 p2 =
    { UI = ui
    ; Game = Game.create p1 p2
    ; Presenter = presenter
    }

let private write ttt output =
    ttt.UI.Write(output) |> ignore
    ttt

let private transformer =
    toWhitelistedInteger << availableSpaces

let private userMove ttt =
    ttt.UI.Prompt "-> " (transformer ttt.Game)

let private present ttt =
    Presenter.present ttt.Game
    |> write ttt

let private move ttt =
    {ttt with Game = Game.move <| userMove ttt <| ttt.Game}

let private doTurn = move >> present

let rec private loop ttt =
    match doTurn ttt with
    | {Game = {Outcome = InProgress}} as ttt -> loop ttt
    | ttt -> ttt

let start (ui:UI) game =
    create ui (fun _ -> "") "X" "O"
    |> present
    |> loop

[<EntryPoint>]
let main argv =
    start <| Console.Console() <| Game.create "X" "O" |> ignore
    0 // return an integer exit code
