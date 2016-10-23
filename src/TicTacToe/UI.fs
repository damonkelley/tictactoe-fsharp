module UI

type UI =
    abstract member ReadLine : unit -> string option
    abstract member Write : string -> unit
    abstract member Prompt : string -> (string option -> 'c option) -> 'c
    abstract member Update : unit -> unit
