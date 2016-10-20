module Presenter

open Board

type Presenter = Outcome | Board | Game

let rowSeparator = "---+---+---\n"
let newline = "\n"
let pipe = "|"

let private sectionFor space =
    match  space with
    | 3, s
    | 6, s -> sprintf " %s%s%s" s newline rowSeparator
    | 9, s -> sprintf " %s%s" s newline
    | _, s -> sprintf " %s %s" s pipe

let private accumlateBoardSections board space =
    board + (sectionFor space)

let private composeBoard spaces =
    List.fold accumlateBoardSections "" spaces

let private presentSpace = function
    | id, (Marker marker) -> (id, marker)
    | id, Empty           -> (id, (sprintf "%i" id))

let private presentBoard (game:Game.Game) =
    game.Board
    |> Board.toList
    |> List.map presentSpace
    |> composeBoard

let private presentOutcome = function
    | {Game.Outcome = Game.Draw}        -> "Draw"
    | {Game.Outcome = Game.Winner w}    -> sprintf "%s wins!" w
    | {Game.Outcome = _; Game.Turn = p} -> sprintf "%s is up!" p

let private presentGame game =
    sprintf "%s\n%s\n" (presentBoard game) (presentOutcome game)

let presentFor presentation game =
    match presentation with
    | Board -> presentBoard game
    | Outcome -> presentOutcome game
    | Game -> presentGame game

let present = presentFor Game
