module Player

let create strategy marker =
    { Marker = marker
    ; Type = Human
    ; Strategy = strategy
    }
