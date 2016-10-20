module Strategy

let randomSpace game =
    let random = System.Random()
    let available = Game.availableSpaces game

    available.Item (random.Next <| available.Length)

