module Player.Test

open NUnit.Framework
open FsUnit
open TestHelpers

[<Test>]
let ``create makes a new player record`` () =
    let expectedPlayer =
        { Marker = "X"
        ; Type = Human
        ; Strategy = (fun _ -> 1)
        }

    Player.create "X" |> shouldEqual expectedPlayer
    Player.create "O" |> shouldEqual {expectedPlayer with Marker = "O"}

[<Test>]
let ``players records are equal based on Marker and Type`` () =
    let playerX = {Player.create "O" with Type = Player.Human}
    let playerO = {Player.create "O" with Type = Player.Computer}

    playerX |> should not' (equal playerO)
