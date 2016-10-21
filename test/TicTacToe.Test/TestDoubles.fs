module TestDoubles

open UI
open Console

let testStrategy game =
    1

type TestUI(input:string) =
    inherit Console.Console() with
    let input = input

    interface UI with
        member this.ReadLine() = Some input
        member this.Write _ = (this :> UI)

