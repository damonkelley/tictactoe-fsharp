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

let private view ttt game =
    ttt.UI.Update()
    game |> ttt.Presenter |> ttt.UI.Write
    game

let start ttt =
    Game.play (view ttt) ttt.Game

let withSetup () =
    let config = Setup.run(Console.Console())

    Game.create config.Player1 config.Player2
    |> create config.UI Presenter.present
    |> start

let private exitCode _ =
    0

[<EntryPoint>]
let main argv =
    withSetup() |> exitCode
