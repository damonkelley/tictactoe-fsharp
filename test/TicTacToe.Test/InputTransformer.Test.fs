module InputTransformer.Test

open NUnit.Framework
open FsUnit

[<Test>]
let ``toWhitelistedInteger transforms a valid string into an integer`` () =
    InputTransformer.toWhitelistedInteger [1..20] (Some "5")
    |> should equal <| Some 5

[<Test>]
let ``toWhitelistedInteger only accepts integers in the whitelist`` () =
    InputTransformer.toWhitelistedInteger [1; 2; 3] (Some "5")
    |> should equal None

[<Test>]
let ``toWhitelistedInteger returns None if it receives a non-numeral`` () =
    InputTransformer.toWhitelistedInteger [1; 2; 3] (Some "one")
    |> should equal None

[<Test>]
let ``confirm transforms y, Y, and yes to true`` () =
    InputTransformer.confirm (Some "y")
    |> should equal <| Some true

    InputTransformer.confirm (Some "Y")
    |> should equal <| Some true

    InputTransformer.confirm (Some "yes")
    |> should equal <| Some true

[<Test>]
let ``confirm transforms n, N, and no to false`` () =
    InputTransformer.confirm (Some "n")
    |> should equal <| Some false

    InputTransformer.confirm (Some "N")
    |> should equal <| Some false

    InputTransformer.confirm (Some "no")
    |> should equal <| Some false

[<Test>]
let ``playerOrder returns the players in the specified order`` () =
    InputTransformer.playerOrder (1, 2) (Some "X")
    |> should equal <| Some (1, 2)

    InputTransformer.playerOrder (1, 2) (Some "O")
    |> should equal <| Some (2, 1)

    InputTransformer.confirm (Some "A")
    |> should equal <| None
