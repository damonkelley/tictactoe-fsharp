module Console

open UI

type Console() =
    let clearScreen = "\x1b[2J\x1b[H"

    interface UI with
        member this.ReadLine () =
           match  System.Console.ReadLine() with
           | null -> None
           | line -> Some line

        member this.Write (output:string) =
            System.Console.Write(output)
            (this :> UI)

        member this.Prompt phrase transformer =
            let console = (this :> UI)
            console.Write(phrase + " ") |> ignore

            match transformer <| console.ReadLine() with
            | None -> console.Prompt phrase transformer
            | Some input -> input

        member this.Update () =
            (this :> UI).Write(clearScreen)
