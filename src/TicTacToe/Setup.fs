module Setup

open UI

type Configuration =
    { UI      : UI
    ; Player1 : Player<Game>
    ; Player2 : Player<Game>
    }

let private playerTypePrompt =
    sprintf "Player %d Type - Human or Computer? (h/c) "

let (|Human|_|) (ui, playerType) =
    match playerType with
    | Some "h" -> Some (Strategy.human ui)
    | _        -> None

let (|Computer|_|) (_, playerType) =
    match playerType with
    | Some "c" -> Some Strategy.randomSpace
    | _        -> None

let private toPlayer ui playerType =
    match (ui, playerType) with
    | Human strategy
    | Computer strategy -> Some strategy
    | _                 -> None

let private promptForPlayerType (ui:UI) playerNumber  =
    ui.Prompt <| playerTypePrompt playerNumber <| toPlayer ui

let private createPlayer strategy marker=
    Player.create strategy marker

let run (ui:UI) =
    { UI = ui
    ; Player1 = createPlayer <| promptForPlayerType ui 1 <| "X"
    ; Player2 = createPlayer <| promptForPlayerType ui 2 <| "O"
    }
