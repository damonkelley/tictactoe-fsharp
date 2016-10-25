module Presenter

open Board

type Presenter = Outcome | Board | Game

let rowSeparator = "---+---+---\n"
let newline = "\n"
let pipe = "|"

let colors =
    Map.ofList [ (0, (sprintf "\x1b[31m%s\x1b[0m"))
               ; (1, (sprintf "\x1b[32m%s\x1b[0m"))
               ; (2, (sprintf "\x1b[33m%s\x1b[0m"))
               ; (3, (sprintf "\x1b[34m%s\x1b[0m"))
               ; (4, (sprintf "\x1b[35m%s\x1b[0m"))
               ; (5, (sprintf "\x1b[36m%s\x1b[0m"))]

let colorize marker =
    let colorCode = (hash marker) % colors.Count
    (Map.find colorCode colors) marker

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
    | id, (Marker (player:Player<Game>)) -> (id, (colorize player.Marker))
    | id, Vacant           -> (id, (sprintf "%i" id))

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
