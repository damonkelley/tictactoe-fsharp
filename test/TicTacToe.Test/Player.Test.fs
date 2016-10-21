module Player.Test

open NUnit.Framework
open FsUnit
open TestHelpers

let testStrategy a =
    1

[<Test>]
let ``create makes a new player record`` () =
    let expectedPlayer =
        { Marker = "X"
        ; Type = Human
        ; Strategy = (fun _ -> 1)
        }

    Player.create testStrategy "X" |> shouldEqual expectedPlayer
    Player.create testStrategy "O" |> shouldEqual {expectedPlayer with Marker = "O"}

[<Test>]
let ``players records are equal based on Marker and Type`` () =
    let playerX = {Player.create testStrategy "O" with Type = Human}
    let playerO = {Player.create testStrategy "O" with Type = Computer}

    playerX |> should not' (equal playerO)

[<Test>]
let ``a player is created with a strategy`` () =
    let strategy input = 1
    (Player.create strategy "X").Strategy() |> should equal 1

    let strategy input = 5
    (Player.create strategy "X").Strategy() |> should equal 5
