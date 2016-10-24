module Strategy

open UI
open InputTransformer

let randomSpace game =
    let random = System.Random()
    let available = Game.availableSpaces game

    available.Item (random.Next <| available.Length)

let human (ui:UI) game =
    let transformer = toWhitelistedInteger << Game.availableSpaces
    ui.Prompt "" (transformer game)

type Score =
    { Score : int
    ; Space : int
    }

let maxScore a b =
    if a.Score = (max a.Score b.Score) then a else b

let minScore a b =
    if a.Score = (min a.Score b.Score) then a else b

let setMove move score =
    {score with Space = move}

let score (game, player, lastMove) depth =
    match game with
    | {Outcome = Winner winner} when player = winner  -> {Score = 10 + depth;  Space = lastMove}
    | {Outcome = Winner _ }                           -> {Score = -10 - depth; Space = lastMove}
    | _                                               -> {Score = 0;           Space = lastMove}

let rec minimax (game, player, lastMove) depth maximizingPlayer =
    match game, depth, maximizingPlayer with
    | {Outcome = InProgress}, _, true  -> forMaximizing (game, player, lastMove) depth
    | {Outcome = InProgress}, _, false -> forMinimizing (game, player, lastMove) depth
    | _, 0, _
    | _, _, _                          -> score (game, player, lastMove) depth

and forMinimizing (game, player, lastMove) depth =
        let scoreMove acc move =
            minimax (Game.move move game, player, move) (depth - 1) true
            |> setMove move
            |> minScore acc

        game
        |> Game.availableSpaces
        |> List.fold scoreMove {Space = 0; Score = System.Int32.MaxValue}

and forMaximizing (game, player, lastMove) depth =
        let scoreMove acc move =
            minimax (Game.move move game, player, move) (depth - 1) false
            |> setMove move
            |> maxScore acc

        game
        |> Game.availableSpaces
        |> List.fold scoreMove {Space = 0; Score = System.Int32.MinValue}

let computer game =
    let score = minimax (game, game.Turn, 0) 7 true
    score.Space

