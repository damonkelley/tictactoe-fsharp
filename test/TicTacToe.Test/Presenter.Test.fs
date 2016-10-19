module Presenter.Test

open NUnit.Framework
open FsUnit
open TestHelpers

[<Test>]
let ``present presents the empty board`` () =
    let expected = " 1 | 2 | 3\n\
                    ---+---+---\n \
                     4 | 5 | 6\n\
                    ---+---+---\n \
                     7 | 8 | 9\n"

    Game.create "X" "O"
    |> Presenter.present
    |> shouldEqual expected

[<Test>]
let ``present presents a board with markers`` () =
    let expected = " 1 | 2 | 3\n\
                    ---+---+---\n \
                     4 | X | 6\n\
                    ---+---+---\n \
                     7 | 8 | O\n"

    Game.create "X" "O"
    |> Game.move 5 "X"
    |> Game.move 9 "O"
    |> Presenter.present
    |> shouldEqual expected
