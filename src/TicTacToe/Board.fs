module Board

type Space<'a> = Marker of 'a | Empty
type Board<'a> = Map<int, Space<'a>>

let create () : Board<'a> =
    [for i in [0..8] -> i, Empty] |> Map.ofList

let move (space:int) (marker:'a) (board:Board<'a>) : Board<'a> =
    board.Add(space, Marker marker)
