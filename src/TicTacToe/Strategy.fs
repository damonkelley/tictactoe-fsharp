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
    ; Space : int option
    }

let maxScore a b =
    if a.Score = (max a.Score b.Score) then a else b

let minScore a b =
    if a.Score = (min a.Score b.Score) then a else b

let setMove move score =
    {score with Space = Some move}

let score game player depth =
    match Game.getWinner game with
    | Some winner when player = winner -> {Score = 10 + depth;  Space = None}
    | Some _                           -> {Score = -10 - depth; Space = None}
    | None                             -> {Score = 0;           Space = None}

let rec minimax (game, player) depth maximize =
    if Game.isOver game || depth = 0 then
        score game player depth
    else
        match maximize with
        | true  -> maximizeWin (game, player) depth
        | false -> minimizeLoss (game, player) depth

and minimizeLoss acc depth  =
    scoreChildren acc depth false minScore {Space = None; Score = System.Int32.MaxValue}

and maximizeWin acc depth  =
    scoreChildren acc depth true maxScore {Space = None; Score = System.Int32.MinValue}

and scoreChildren (game, player) depth maximize comparator startingScore =
    let scoreMove acc move =
        minimax (Game.move move game, player) (depth - 1) (not maximize)
        |> setMove move
        |> comparator acc

    game
    |> Game.availableSpaces
    |> List.fold scoreMove startingScore

let computer game =
    match minimax (game, game.Turn) 7 true with
    | {Space = Some move} -> move
    | _ -> failwith "Unable to choose space"

