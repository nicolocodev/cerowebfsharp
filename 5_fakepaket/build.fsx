// include Fake libs
#r "./packages/build/FAKE/tools/FakeLib.dll"

open Fake
open System
open System.IO

// Directories
let buildDir  = "./build/"
let deployDir = "./deploy/"


// Filesets
let appReferences  =
    !! "/**/*.csproj"
    ++ "/**/*.fsproj"

// version info
let version = "0.1"  // or retrieve from CI server

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; deployDir]
)

// --------------------------------------------------------------------------------------
// Build library & test project

let serverPath = "./src/" |> FullName

let dotnetExePath = "dotnet.exe"


Target "Build" (fun _ ->
    let result =
        ExecProcess (fun info ->
            info.FileName <- dotnetExePath
            info.WorkingDirectory <- serverPath
            info.Arguments <- "restore") TimeSpan.MaxValue
    if result <> 0 then failwith "Restore failed"

    let result =
        ExecProcess (fun info ->
            info.FileName <- dotnetExePath
            info.WorkingDirectory <- serverPath
            info.Arguments <- "build") TimeSpan.MaxValue
    if result <> 0 then failwith "Build failed"
)

let ipAddress = "localhost"
let port = 8080

Target "Run" (fun _ ->
    let dotnetwatch = async {
        let result =
            ExecProcess (fun info ->
                info.FileName <- dotnetExePath
                info.WorkingDirectory <- serverPath
                info.Arguments <- "watch run") TimeSpan.MaxValue
        if result <> 0 then failwith "Website shut down." }
    
    let openBrowser = async {
        System.Threading.Thread.Sleep(5000)
        Diagnostics.Process.Start("http://"+ ipAddress + sprintf ":%d" port) |> ignore }

    Async.Parallel [| dotnetwatch;  openBrowser |]
    |> Async.RunSynchronously
    |> ignore
)

// Build order
"Clean"
  ==> "Build"
  ==> "Run"
 

// start build
RunTargetOrDefault "Build"
