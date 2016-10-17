module Game

type Outcome = InProgress
type Space = Player | Empty
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

let move space game =
    {game with Board = Board.move space "X" game.Board}
