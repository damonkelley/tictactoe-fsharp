module InputTransformer

let private (|Integer|_|) str =
    match System.Int32.TryParse(str) with
    | (true, integer) -> Some integer
    | _               -> None

let toWhitelistedInteger whitelist (input:string option) =
    match input with
    | Some(Integer i) when Seq.contains i whitelist -> Some i
    | _ -> None

let private (|Confirmation|_|) yes =
    match yes with
    | "y" | "Y" | "yes" -> Some true
    | "n" | "N" | "no"  -> Some false
    | _                 -> None

let confirm (input:string option) =
    match input with
    | Some (Confirmation c) -> Some c
    | _                     -> None
