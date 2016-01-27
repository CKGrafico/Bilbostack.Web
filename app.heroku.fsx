#load "app.fsx"

open App
open Fake
open System
open Suave
open Suave.Successful 
open Suave.Web  
open System.Net

let serverConfig =
  let port = int (Environment.GetEnvironmentVariable("PORT"))
  { Web.defaultConfig with
      homeFolder = Some __SOURCE_DIRECTORY__
      logger = Logging.Loggers.saneDefaultsFor Logging.LogLevel.Warn
      bindings = [ HttpBinding.mkSimple HTTP "0.0.0.0" port ] }

Web.startWebServer serverConfig app