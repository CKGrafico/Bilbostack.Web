#load "./site/app.fsx"

open App
open System
open Suave

let serverConfig =
  let port = int (Environment.GetEnvironmentVariable("PORT"))
  { Web.defaultConfig with
      homeFolder = Some __SOURCE_DIRECTORY__
      logger = Logging.Loggers.saneDefaultsFor Logging.LogLevel.Warn
      bindings = [ HttpBinding.mk HTTP "0.0.0.0" port ] }

Web.startWebServer serverConfig app