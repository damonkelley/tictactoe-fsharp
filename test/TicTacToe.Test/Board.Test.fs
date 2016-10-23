module Board.Test

open NUnit.Framework
open FsUnit
open TestHelpers

[<Test>]
let ``create a new board`` () =
    create()
    |> Map.forall (fun k v -> match v with | Vacant -> true | _ -> false)
    |> should equal <| true

[<Test>]
let ``move updates the board`` () =
    create()
    |> move 1 "X"
    |> shouldEqual <| (create().Add(1, Marker "X"))

[<Test>]
let ``toList returns a list of tuples`` () =
    let board = create() |> move 1 "X"

    board
    |> Board.toList
    |> shouldEqual <| Map.toList board

[<Test>]
let ``the board can be parititioned into rows`` () =
    let partitions =
        create()
        |> move 1 "A"
        |> move 2 "B"
        |> move 3 "C"
        |> move 4 "D"
        |> move 5 "E"
        |> move 6 "F"
        |> move 7 "G"
        |> move 8 "H"
        |> move 9 "I"
        |> partition

    partitions |> should contain [Marker "A"; Marker "B"; Marker "C"]
    partitions |> should contain [Marker "D"; Marker "E"; Marker "F"]
    partitions |> should contain [Marker "G"; Marker "H"; Marker "I"]

[<Test>]
let ``the board can be parititioned into columns`` () =
    let partitions =
        create()
        |> move 1 "1"
        |> move 2 "2"
        |> move 3 "3"
        |> move 4 "4"
        |> move 5 "5"
        |> move 6 "6"
        |> move 7 "7"
        |> move 8 "8"
        |> move 9 "9"
        |> partition

    partitions |> should contain [Marker "1"; Marker "4"; Marker "7"]
    partitions |> should contain [Marker "2"; Marker "5"; Marker "8"]
    partitions |> should contain [Marker "3"; Marker "6"; Marker "9"]

[<Test>]
let ``the board can be partitioned into diagonals`` () =
    let partitions =
        create()
        |> move 1 "A"
        |> move 3 "D"
        |> move 5 "X"
        |> move 7 "C"
        |> move 9 "B"
        |> partition

    partitions |> should contain [Marker "A"; Marker "X"; Marker "B"]
    partitions |> should contain [Marker "C"; Marker "X"; Marker "D"]
    partitions.Length |> should equal 8

[<Test>]
let ``collect can get all of the space ids that are vacant`` () =
    let board = Board.create()

    collect (Vacant, board)
    |> shouldEqual [1; 2; 3; 4; 5; 6; 7; 8; 9]

    let board = board |> move 1 "X" |> move 2 "O"

    collect (Vacant, board)
    |> shouldEqual [3; 4; 5; 6; 7; 8; 9]
