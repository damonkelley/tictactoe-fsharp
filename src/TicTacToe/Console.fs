module Console

open UI

type Console() =
    interface UI with
        member this.ReadLine () =
           match  System.Console.ReadLine() with
           | null -> None
           | line -> Some line

        member this.Write (output:string) =
            System.Console.Write(output)
            (this :> UI)

        member this.Prompt phrase transformer =
            System.Console.ReadLine()
            |> transformer
