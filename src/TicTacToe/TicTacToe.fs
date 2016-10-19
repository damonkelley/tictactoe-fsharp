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

let presentOutcome = function
    | {Game = {Outcome = Draw}} -> "Draw"
    | {Game = {Outcome = Winner w}} -> sprintf "%s wins!" w
    | _ -> ""

let private transformer =
    toWhitelistedInteger << availableSpaces

let private userMove ttt =
    ttt.UI.Prompt "-> " (transformer ttt.Game)

let private present ttt =
    presentOutcome ttt
    |> ttt.UI.Write
    |> ignore
    ttt

let private doTurn ttt =
    {ttt with Game = play(ttt.Game, [userMove ttt])}
    |> present

let rec private loop ttt =
    match doTurn ttt with
    | {Game = {Outcome = InProgress}} as ttt -> loop ttt
    | ttt -> ttt

let rec start (ui:UI) game =
    create ui (fun _ -> "") "X" "O"
    |> loop

[<EntryPoint>]
let main argv =
    0 // return an integer exit code
