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
    | id, (Marker (player:Player<Game>)) -> (id, player.Marker)
    | id, Empty           -> (id, (sprintf "%i" id))

let private presentBoard (game:Game) =
    game.Board
    |> Board.toList
    |> List.map presentSpace
    |> composeBoard

let private presentOutcome = function
    | {Game.Outcome = Draw}        -> "Draw"
    | {Game.Outcome = Winner w}    -> sprintf "%s wins!" w.Marker
    | {Game.Outcome = _; Turn = p} -> sprintf "%s is up!" p.Marker

let private presentGame game =
    sprintf "%s\n%s\n" (presentBoard game) (presentOutcome game)

let presentFor presentation game =
    match presentation with
    | Board -> presentBoard game
    | Outcome -> presentOutcome game
    | Game -> presentGame game

let present = presentFor Game
