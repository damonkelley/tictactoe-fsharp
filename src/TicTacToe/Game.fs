module Game

open Board

let create playerOne playerTwo =
    { Outcome  = InProgress
    ; Board    = Board.create()
    ; Players  = playerOne, playerTwo
    ; Turn     = playerOne
    }

let availableSpaces game =
    Board.collect (Vacant, game.Board)

let private allMarkersMatch markers =
    match markers with
    | Marker a :: Marker b :: Marker c :: xs when a = b && b = c -> Some a
    | _ -> None

let private onlyMarkers partition =
    not <| List.contains Vacant partition

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
