// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"
#r @"./packages/FSharpLint/FSharpLint.FAKE.dll"

open Fake
open Fake.Testing
open FSharpLint.FAKE

let buildDir  = "./build/"
let testDir = "./test/"

Target "Clean" (fun _ ->
    CleanDirs [
        buildDir;
        "./test/TicTacToe.Test/bin/";
        "./test/TicTacToe.Test/obj/"
    ])

let appReferences  =
    !! "/**/*.fsproj"

Target "Build" (fun _ ->
    let properties a =
        [
            "Optimize", "True"
            "Configuration", "Release"
            "NoWarn", "0760"
        ]

    MSBuildWithProjectProperties buildDir "Build" properties appReferences
    |> Log "AppBuild-Output: ")


Target "BuildTest" (fun _ ->
    let setParams defaults =
        { defaults with
            Verbosity = Some MSBuildVerbosity.Quiet
            Targets = ["Build"]
            Properties =
              [
                "Optimize", "False"
                "DebugSymbols", "True"
                "Configuration", "Debug"
                "NoWarn", "0760"
              ]
         }
    build setParams "test/TicTacToe.Test/TicTacToe.Test.fsproj" |> DoNothing)

let nunitSetParams where defaults =
    { defaults with
        Where = where
        ShadowCopy = true;
        Labels = LabelsLevel.All;
        ToolPath = "packages/NUnit.ConsoleRunner/tools/nunit3-console.exe" }

Target "Test" (fun _ ->
    !! (testDir + "/**/bin/Debug/*.Test.dll")
    |> NUnit3 (nunitSetParams "cat!=Long"))

Target "LongTests" (fun _ ->
    !! (testDir + "/**/bin/Debug/*.Test.dll")
    |> NUnit3 (nunitSetParams "cat==Long"))

Target "AllTests" (fun _ ->
    !! (testDir + "/**/bin/Debug/*.Test.dll")
    |> NUnit3 (nunitSetParams ""))

Target "Lint" (fun _ ->
    !! "**/**/*.fsproj" |> Seq.iter (FSharpLint id))

"BuildTest" ==> "LongTests"
"BuildTest" ==> "AllTests"

"Clean"
  ==> "BuildTest"
  ==> "Lint"
  ==> "Test"
  ==> "Build"

RunTargetOrDefault "Build"
