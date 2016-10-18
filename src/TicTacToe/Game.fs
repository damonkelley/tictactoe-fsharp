module Game

type Player = string

type Outcome =
    | Winner of Player
    | Draw
    | InProgress

type Game =
    { Outcome: Outcome
    ; Board: Board.Board<Player>
    }

let create () =
    { Outcome = InProgress
    ; Board = Board.create()
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

let move space player game =
    {game with Board = Board.move space player game.Board;}
    |> updateOutcome
