module Player.Test

open NUnit.Framework
open FsUnit
open TestHelpers

[<Test>]
let ``create makes a new player record`` () =
    let expectedPlayer =
        { Marker = "X"
        ; Type = Human
        }

    Player.create "X" |> shouldEqual expectedPlayer
    Player.create "O" |> shouldEqual {expectedPlayer with Marker = "O"}
