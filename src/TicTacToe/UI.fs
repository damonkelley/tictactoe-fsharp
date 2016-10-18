module UI

type UI =
    abstract member ReadLine : unit -> string option
    abstract member Write : string -> UI
    abstract member Prompt : string -> (string -> 'c) -> 'c
