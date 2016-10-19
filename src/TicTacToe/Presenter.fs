module Presenter

open Board

let rowSeparator = "---+---+---\n"
let newline = "\n"
let pipe = "|"

let private sectionFor space =
    match  space with
    | 3, s
    | 6, s -> sprintf " %s%s%s" s newline rowSeparator
    | 9, s -> sprintf " %s%s" s newline
    | _, s -> sprintf " %s %s" s pipe

let private composeBoard spaces =
    List.fold (fun board space -> board + (sectionFor space)) "" spaces

let private presentSpace = function
    | id, (Marker marker) -> (id, marker)
    | id, Empty -> (id, (sprintf "%i" id))

let present (game:Game.Game) =
    game.Board
    |> Board.toList
    |> List.map presentSpace
    |> composeBoard
