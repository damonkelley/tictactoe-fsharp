module Setup.Test

open NUnit.Framework
open FsUnit
open TestHelpers

open UI
open Console

type TestUI(input:string list) =
    inherit Console.Console()

    member val Input = input with get,set
    member val Output = [] with get,set

    interface UI with
        member this.ReadLine() =
            match this.Input with
            | line :: xs -> this.Input <- xs; Some line
            | _ -> None

        member this.Write out =
            this.Output <- out :: this.Output

[<Test>]
let ``it prompts the user to indicate the type of player 1 and player 2`` () =
    let configuration =
        TestUI(["h"; "h"])
        |> Setup.run

    let ui = (configuration.UI :?> TestUI)
    ui.Output |> should contain "Player 1 Type - Human or Computer? (h/c) "
    ui.Output |> should contain "Player 2 Type - Human or Computer? (h/c) "
    ui.Input  |> shouldEqual []

[<Test>]
let ``it will create a human vs human configuration`` () =
    let config =
        TestUI(["h"; "h"; "1"; "2"])
        |> Setup.run

    let game = Game.create config.Player1 config.Player2

    config.Player1.Strategy game |> should equal 1
    config.Player2.Strategy game |> should equal 2

[<Test>]
let ``it will create a computer vs human configuration`` () =
    let config =
        TestUI(["c"; "h"; "1";])
        |> Setup.run

    let game = Game.create config.Player1 config.Player2

    config.Player2.Strategy game |> should equal 1
    [1..9] |> should contain (config.Player1.Strategy game)
