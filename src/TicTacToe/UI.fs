module UI

type UI =
    abstract member ReadLine : unit -> string option
    abstract member Write : string -> UI
    abstract member Prompt : string -> (string option -> 'c option) -> 'c
    abstract member Update : unit -> UI
