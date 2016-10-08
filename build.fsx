#I @"packages/FAKE/tools/"
#r @"FakeLib.dll"
#load @"SuaveAndWebSocket/Utility/FileLog.fs"

open Fake
open Fake.Azure
open System
open System.IO

open SuaveAndWebsocket

Environment.CurrentDirectory <- __SOURCE_DIRECTORY__
let solutionFile = "SuaveAndWebsocket.sln"

Target "BuildSolution" (fun _ ->
    solutionFile
    |> MSBuildHelper.build (fun defaults ->
        { defaults with
            Verbosity = Some Minimal
            Targets = [ "Build" ]
            Properties = [ "Configuration", "Release"
                           "OutputPath", Kudu.deploymentTemp ] })
    |> ignore)

Target "StageWebsiteAssets" (fun _ ->
    FileLog.Log "Msg from build.fsx!!!!!!"
  
    let blacklist =
        [ "typings"
          ".fs"
          ".config"
          ".references"
          "tsconfig.json" ]
    let shouldInclude (file:string) =
        blacklist
        |> Seq.forall(not << file.Contains)
    Kudu.stageFolder (Path.GetFullPath @"SuaveAndWebsocket\wwwroot") shouldInclude)

Target "Deploy" Kudu.kuduSync

"StageWebsiteAssets"
==> "BuildSolution"
==> "Deploy"


RunTargetOrDefault "Deploy"