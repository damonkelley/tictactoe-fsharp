module Board.Test

open NUnit.Framework
open FsUnit
open TestHelpers

[<Test>]
let ``create a new board`` () =
    create()
    |> Map.forall (fun k v -> match v with | Board.Empty -> true | _ -> false)
    |> should equal <| true

[<Test>]
let ``move updates the board`` () =
    create()
    |> move 0 "X"
    |> shouldEqual <| (create().Add(0, Board.Marker "X"))

[<Test>]
let ``the board can be parititioned into rows`` () =
    let partitions =
        create()
        |> move 0 "A"
        |> move 1 "B"
        |> move 2 "C"
        |> move 3 "D"
        |> move 4 "E"
        |> move 5 "F"
        |> move 6 "G"
        |> move 7 "H"
        |> move 8 "I"
        |> partition

    partitions |> should contain [Marker "A"; Marker "B"; Marker "C"]
    partitions |> should contain [Marker "D"; Marker "E"; Marker "F"]
    partitions |> should contain [Marker "G"; Marker "H"; Marker "I"]

[<Test>]
let ``the board can be parititioned into columns`` () =
    let partitions =
        create()
        |> move 0 "0"
        |> move 1 "1"
        |> move 2 "2"
        |> move 3 "3"
        |> move 4 "4"
        |> move 5 "5"
        |> move 6 "6"
        |> move 7 "7"
        |> move 8 "8"
        |> partition

    partitions |> should contain [Marker "0"; Marker "3"; Marker "6"]
    partitions |> should contain [Marker "1"; Marker "4"; Marker "7"]
    partitions |> should contain [Marker "2"; Marker "5"; Marker "8"]

[<Test>]
let ``the board can be partitioned into diagonals`` () =
    let partitions =
        create()
        |> move 0 "A"
        |> move 2 "D"
        |> move 4 "X"
        |> move 6 "C"
        |> move 8 "B"
        |> partition

    partitions |> should contain [Marker "A"; Marker "X"; Marker "B"]
    partitions |> should contain [Marker "C"; Marker "X"; Marker "D"]
    partitions.Length |> should equal 8
