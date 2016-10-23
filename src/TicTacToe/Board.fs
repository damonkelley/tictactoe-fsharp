module Board

let create () : Board<'a> =
    [for i in [1..9] -> i, Vacant] |> Map.ofList

let move (space:int) (marker:'a) (board:Board<'a>) : Board<'a> =
    Map.add space (Marker marker) board

let toList board =
    Map.toList board

let private collectVacant board =
    board
    |> Map.toList
    |> List.choose (function
                    | id, Vacant -> Some id
                    | _, _ -> None)

let collect = function
    | (Vacant), board -> collectVacant board
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
