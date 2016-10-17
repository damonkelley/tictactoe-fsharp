module Board

type Space<'a> = Marker of 'a | Empty
type Board<'a> = Map<int, Space<'a>>

let create () : Board<'a> =
    [for i in [0..8] -> i, Empty] |> Map.ofList

let move (space:int) (marker:'a) (board:Board<'a>) : Board<'a> =
    Map.add space (Marker marker) board

let private rows = [
    [0; 1; 2];
    [3; 4; 5];
    [6; 7; 8]]

let private columns = [
    [0; 3; 6];
    [1; 4; 7];
    [2; 5; 8]]

let private diagonals = [
    [0; 4; 8];
    [6; 4; 2]]

let private partitionWith indexes (board:Board<'a>)=
    [for row in indexes ->
        [for i in row ->
            Map.find i board]]

let partition board =
    let partitions =
        List.append
        <| partitionWith rows board
        <| partitionWith columns board

    List.append partitions <| partitionWith diagonals board
