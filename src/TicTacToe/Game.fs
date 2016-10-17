module Game

type Outcome = Winner | InProgress
type Player = string
type Game = {
    Outcome: Outcome;
    Board: Board.Board<Player>;
}

let create () =
    {
        Outcome = InProgress;
        Board = Board.create()
    }

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
    match findWinner game with
    | Some winner -> {game with Outcome = Winner}
    | None -> game

let move space player game =
    {game with Board = Board.move space player game.Board;}
    |> updateOutcome
