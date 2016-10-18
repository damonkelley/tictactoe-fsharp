module Board

type Space<'a> = Marker of 'a | Empty
type Board<'a> = Map<int, Space<'a>>

let create () : Board<'a> =
    [for i in [1..9] -> i, Empty] |> Map.ofList

let move (space:int) (marker:'a) (board:Board<'a>) : Board<'a> =
    Map.add space (Marker marker) board

let private collectEmpty board =
    board
    |> Map.toList
    |> List.choose (function
                    | id, Empty -> Some id
                    | _, _ -> None)


let collect = function
    | (Empty), board -> collectEmpty board
    | (Marker x), board -> []

let private rows =
    [ [1; 2; 3]
    ; [4; 5; 6]
    ; [7; 8; 9]
    ]

let private columns =
    [ [1; 4; 7]
    ; [2; 5; 8]
    ; [3; 6; 9]
    ]

let private diagonals =
    [ [1; 5; 9]
    ; [7; 5; 3]
    ]

let private partitionWith indexes (board:Board<'a>)=
    [for row in indexes ->
        [for i in row ->
            Map.find i board]]

let partition board =
    let rows = partitionWith rows board
    let columns = partitionWith columns board
    let diagonals = partitionWith diagonals board

    List.concat (rows :: columns :: [diagonals])
