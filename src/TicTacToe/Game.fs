module Game

type Outcome =
    | Winner of Player.Player
    | Draw
    | InProgress

type Game =
    { Outcome: Outcome
    ; Board:   Board.Board<Player.Player>
    ; Players: Player.Player * Player.Player
    ; Turn:    Player.Player
    }

let create playerOne playerTwo =
    { Outcome  = InProgress
    ; Board    = Board.create()
    ; Players  = playerOne, playerTwo
    ; Turn     = playerOne
    }

let availableSpaces game =
    Board.collect (Board.Empty, game.Board)

let private allMarkersMatch markers =
    match markers with
    | Board.Marker a :: Board.Marker b :: Board.Marker c :: xs when a = b && b = c -> Some a
    | _ -> None

let private onlyMarkers partition =
    not <| List.contains Board.Empty partition

let findWinner game =
    game.Board
    |> Board.partition
    |> List.filter onlyMarkers
    |> List.tryPick allMarkersMatch

let updateOutcome game =
    match (findWinner game), (availableSpaces game) with
    | Some player , _ -> {game with Outcome = Winner player}
    | None, []        -> {game with Outcome = Draw}
    | _, _            ->  game

let swapTurn game =
    match game with
    | {Turn = player; Players = p1, p2} when player = p1 -> {game with Turn = p2}
    | {Players = p1, _}                                  -> {game with Turn = p1}

let move space game =
    {game with Board = Board.move space game.Turn game.Board;}
    |> updateOutcome
    |> swapTurn

let private playWithMoves game moves =
    moves
    |> List.fold (fun game m -> move m game) game

let play = function
    | game, moves -> playWithMoves game moves
