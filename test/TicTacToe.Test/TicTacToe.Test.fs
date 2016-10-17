module TicTacToe.Test

open NUnit.Framework
open FsUnit
open TicTacToe

[<Test>]
let ``Example Test`` () =
    1 |> should equal 1

[<Test>]
let ``import and test`` () =
    add 1 2 |> should equal 3
