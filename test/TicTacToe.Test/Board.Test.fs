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
