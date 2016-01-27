#r "System.Xml.Linq"
#r "packages/FSharp.Data/lib/net40/FSharp.Data.dll"
#r "packages/Suave/lib/net40/Suave.dll"
#r "packages/Suave.DotLiquid/lib/net40/Suave.DotLiquid.dll"
#r "packages/DotLiquid/lib/net40/DotLiquid.dll"

open Suave                 
open Suave.Successful      
open Suave.Web
open Suave.Filters
open Suave.Operators
open FSharp.Data
open Suave.DotLiquid

// ----------------------------------------------------------------------------
// Hello world
// ----------------------------------------------------------------------------

//let app = OK "Hello world!"
// ----------------------------------------------------------------------------


let serverConfig = 
    let port = getBuildParamOrDefault "port" "8083" |> Sockets.Port.Parse
    { defaultConfig with bindings = [ HttpBinding.mk HTTP IPAddress.Loopback port ] }

startWebServer serverConfig 
    (OK  
        ("Hello World! It's Suave.io on Azure Websites.<br /><ul>" + 
          "<li>Sample git Repo: <a href='https://github.com/shanselman/suavebootstrapper'>https://github.com/shanselman/suavebootstrapper</a></li>" +
          "<li>Intro blog post: <a href='http://www.hanselman.com/blog/RunningSuaveioAndFWithFAKEInAzureWebAppsWithGitAndTheDeployButton.aspx'>http://www.hanselman.com/blog/RunningSuaveioAndFWithFAKEInAzureWebAppsWithGitAndTheDeployButton.aspx</a></li>" +
          "</ul>"))

  
  
// TODO: Display using DotLiquid page

// TODO: Get current weather using JSON type provider
// (http://api.openweathermap.org/data/2.5/forecast/daily?q=London,UK&mode=json&units=metric&cnt=10)
// DEMO: Covert UNIX time stamps

// DEMO: Add async entry-point
// TODO: Make the data reading async

// TODO: Define NewsFilters.niceDate ('D') & add to template
// TODO: Register filters by name
