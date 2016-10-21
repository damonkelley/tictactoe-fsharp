[<AutoOpen>]
module GameTypes

type PlayerType = Human | Computer

[<CustomComparison>]
[<CustomEquality>]
type Player<'a> =
    { Marker : string
    ; Type : PlayerType
    ; Strategy : 'a -> int
    }

    with
    override this.Equals other =
        match other with
        | :? Player<'a> as p -> p.Marker = this.Marker && p.Type = this.Type
        | _ -> false

    override this.GetHashCode() =
        hash (this.Marker + this.Type.ToString())

    interface System.IComparable with
        member this.CompareTo y = 0

type Space<'a> = Marker of 'a | Vacant
type Board<'a> = Map<int, Space<'a>>

type Outcome<'a> =
    | Winner of 'a
    | Draw
    | InProgress

type Game =
    { Outcome: Outcome<Player<Game>>
    ; Board:   Board<Player<Game>>
    ; Players: Player<Game> * Player<Game>
    ; Turn:    Player<Game>
    }
