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
        TestUI(["h"; "h"; "X"])
        |> Setup.run

    let ui = (configuration.UI :?> TestUI)
    ui.Output |> should contain "Is X a Human or Computer? (h/c) "
    ui.Output |> should contain "Is O a Human or Computer? (h/c) "
    ui.Input  |> shouldEqual []

[<Test>]
let ``it asks which player will go first`` () =
    let configuration =
        TestUI(["h"; "h"; "X"])
        |> Setup.run

    let ui = (configuration.UI :?> TestUI)
    ui.Output |> should contain "Which player will go first? (X/O) "
    ui.Input  |> shouldEqual []

[<Test>]
let ``it will correctly order the players`` () =
    let config =
        TestUI(["h"; "h"; "O";])
        |> Setup.run

    let game = Game.create config.Player1 config.Player2
    game.Turn.Marker |> should equal "O"

[<Test>]
let ``it will create a human vs human configuration`` () =
    let config =
        TestUI(["h"; "h"; "O"; "1"; "2"])
        |> Setup.run

    let game = Game.create config.Player1 config.Player2

    config.Player1.Strategy game |> should equal 1
    config.Player2.Strategy game |> should equal 2

[<Test>]
let ``it will create a computer vs human configuration`` () =
    let config =
        TestUI(["c"; "h"; "X"; "1";])
        |> Setup.run

    let game = Game.create config.Player1 config.Player2

    config.Player2.Strategy game |> should equal 1
    [1..9] |> should contain (config.Player1.Strategy game)
