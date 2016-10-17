module Game

type Outcome = InProgress
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

let move space player game =
    {game with Board = Board.move space player game.Board}
