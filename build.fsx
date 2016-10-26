// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"
#r @"./packages/FSharpLint/FSharpLint.FAKE.dll"

open Fake
open Fake.Testing
open FSharpLint.FAKE

let buildDir  = "./build/"
let deployDir = "./deploy/"
let testDir = "./test/"

Target "Clean" (fun _ ->
    CleanDirs [
        buildDir;
        deployDir;
        "./test/TicTacToe.Test/bin/";
        "./test/TicTacToe.Test/obj/"
    ])

let buildReleaseSetParams defaults =
    { defaults with
        Verbosity = Some MSBuildVerbosity.Minimal
        Targets = ["Build"]
        Properties =
          [
            "Optimize", "True"
            "Configuration", "Release"
            "NoWarn", "0760"
          ]
     }

let buildTestSetParams defaults =
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

Target "Build" (fun _ ->
    build buildReleaseSetParams "src/TicTacToe/TicTacToe.fsproj" |> DoNothing)

Target "BuildTest" (fun _ ->
    build buildTestSetParams "test/TicTacToe.Test/TicTacToe.Test.fsproj" |> DoNothing)

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
    trace "Ran all tests")

Target "Lint" (fun _ ->
    !! "**/**/*.fsproj" |> Seq.iter (FSharpLint id))

"BuildTest"
  ==> "LongTests"

"Test"
  ==> "LongTests"
  ==> "AllTests"

"Clean"
  ==> "BuildTest"
  ==> "Lint"
  ==> "Test"
  ==> "Build"

RunTargetOrDefault "Build"
