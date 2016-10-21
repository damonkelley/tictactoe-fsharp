module Player

type Type = Human | Computer

[<CustomComparison>]
[<CustomEquality>]
type Player<'a> =
    { Marker : string
    ; Type : Type
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

let create marker =
    { Marker = marker
    ; Type = Human
    ; Strategy = (fun game -> 1)
    }
