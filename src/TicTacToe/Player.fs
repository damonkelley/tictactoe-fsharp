module Player

let create marker =
    { Marker = marker
    ; Type = Human
    ; Strategy = (fun game -> 1)
    }
