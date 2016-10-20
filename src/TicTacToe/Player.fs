module Player

type Type = Human

type Player =
    { Marker : string
    ; Type : Type
    }

let create marker =
    {Marker = marker
    ; Type = Human
    }
