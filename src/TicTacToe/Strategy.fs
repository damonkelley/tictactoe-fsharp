module Strategy

open UI
open InputTransformer

let randomSpace game =
    let random = System.Random()
    let available = Game.availableSpaces game

    available.Item (random.Next <| available.Length)

let human prompt (ui:UI) game =
    let transformer = toWhitelistedInteger << Game.availableSpaces
    ui.Prompt prompt <| transformer game
