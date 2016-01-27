#r "packages/FAKE/tools/FakeLib.dll"
#load "app.fsx"

open App
open Fake
open System
open Suave
open Suave.Successful 
open Suave.Web  
open System.Net

let serverConfig = 
    let port = getBuildParamOrDefault "port" "8083" |> Sockets.Port.Parse
    { defaultConfig with bindings = [ HttpBinding.mk HTTP IPAddress.Loopback port ] }

startWebServer serverConfig app