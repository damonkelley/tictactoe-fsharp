module Setup

open UI

type Configuration =
    { UI      : UI
    ; Player1 : Player<Game>
    ; Player2 : Player<Game>
    }

let (|Human|_|) (ui, playerType) =
    match playerType with
    | Some "h" -> Some (Strategy.human ui)
    | _        -> None

let (|Computer|_|) (_, playerType) =
    match playerType with
    | Some "c" -> Some Strategy.computer
    | _        -> None

let private toStrategy ui playerType =
    match (ui, playerType) with
    | Human strategy
    | Computer strategy -> Some strategy
    | _                 -> None

let private promptForPlayerType (ui:UI) playerNumber  =
    ui.Prompt <| Prompts.playerType playerNumber <| toStrategy ui

let private promptForOrder (ui:UI) players =
    ui.Prompt Prompts.firstPlayer (InputTransformer.playerOrder players)

let private createPlayer strategy marker=
    Player.create strategy marker

let run (ui:UI) =
    let playerX = createPlayer <| promptForPlayerType ui "X" <| "X"
    let playerO = createPlayer <| promptForPlayerType ui "O" <| "O"

    let (player1, player2) = promptForOrder ui (playerX, playerO)

    { UI = ui
    ; Player1 = player1
    ; Player2 = player2
    }
