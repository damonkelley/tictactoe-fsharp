module InputTransformer.Test

open NUnit.Framework
open FsUnit

[<Test>]
let ``toWhitelistedInteger transforms a valid string into an integer`` () =
    InputTransformer.toWhitelistedInteger [1..20] "5"
    |> should equal <| Some 5

[<Test>]
let ``toWhitelistedInteger only accepts integers in the whitelist`` () =
    InputTransformer.toWhitelistedInteger [1; 2; 3] "5"
    |> should equal None

[<Test>]
let ``toWhitelistedInteger returns None if it receives a non-numeral`` () =
    InputTransformer.toWhitelistedInteger [1; 2; 3] "one"
    |> should equal None
