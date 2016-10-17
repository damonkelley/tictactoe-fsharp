module TestHelpers

open NUnit.Framework

let inspect value  =
    printf "%A" value
    value

let shouldEqual (x: 'a) (y: 'a) =
    Assert.AreEqual(x, y, sprintf "Expected: %A\nActual: %A" x y)
