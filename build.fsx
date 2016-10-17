// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Testing

let buildDir  = "./build/"
let deployDir = "./deploy/"
let testDir = "./test/"

let appReferences  =
    !! "/**/*.csproj"
    ++ "/**/*.fsproj"

let version = "0.1"  // or retrieve from CI server

Target "Clean" (fun _ ->
    CleanDirs [
        buildDir;
        deployDir;
        "./test/TicTacToe.Test/bin/";
        "./test/TicTacToe.Test/obj/"
    ])

Target "Build" (fun _ ->
    MSBuildDebug buildDir "Build" appReferences
    |> Log "AppBuild-Output: ")

Target "BuildTest" (fun _ ->
    let setParams defaults =
        { defaults with
            Verbosity = Some MSBuildVerbosity.Quiet
            Targets = ["Build"]
            Properties =
              [
                "Optimize", "True"
                "DebugSymbols", "True"
                "Configuration", "Debug"
              ]
         }
    build setParams "test/TicTacToe.Test/TicTacToe.Test.fsproj"
      |> DoNothing)

Target "Test" (fun _ ->
    !! (testDir + "/**/bin/Debug/*.Test.dll")
    |> NUnit3 (fun p ->
            {p with
                ToolPath = "packages/NUnit.ConsoleRunner/tools/nunit3-console.exe"}))

Target "Deploy" (fun _ ->
    !! (buildDir + "/**/*.*")
    -- "*.zip"
    |> Zip buildDir (deployDir + "ApplicationName." + version + ".zip"))


"Clean"
  ==> "BuildTest"
  ==> "Test"
  ==> "Build"
  ==> "Deploy"

RunTargetOrDefault "Build"
