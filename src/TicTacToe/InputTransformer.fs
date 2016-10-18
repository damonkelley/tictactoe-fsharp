module InputTransformer

let private (|Integer|_|) str =
    match System.Int32.TryParse(str) with
    | (true, integer) -> Some integer
    | _ -> None

let toWhitelistedInteger whitelist (input:string) =
    match input with
    | Integer i when Seq.contains i whitelist -> Some i
    | _ -> None
