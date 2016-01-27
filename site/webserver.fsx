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

// DEMO: Define the domain model
type Movies =
  { Id: int
    Poster : string
    Title : string
    VoteAvg : float
    ReleaseDate : System.DateTime
  }

type Home = { Movies: seq<Movies> }


// TODO: Get current news from BBC
// (http://feeds.bbci.co.uk/news/rss.xml)
type Forecast = JsonProvider<"https://api.themoviedb.org/3/discover/movie?sort_by=popularity.desc&api_key=f0daa403a993eb9b7ee555b903aae2fd">

let posterUrl s =
    "http://image.tmdb.org/t/p/w185" + s

let getMovies() = 
  let res = Forecast.Load("https://api.themoviedb.org/3/discover/movie?sort_by=popularity.desc&api_key=f0daa403a993eb9b7ee555b903aae2fd")
  [ for item in res.Results ->
      { Id = item.Id
        Poster = item.PosterPath |> posterUrl
        Title = item.Title
        VoteAvg = float item.VoteAverage
        ReleaseDate = item.ReleaseDate
         } ] 


let app : WebPart = fun ctx -> async {
  let movies = getMovies()
  return! DotLiquid.page "index.html" { Movies = movies } ctx }
  
let serverConfig = 
    let port = getBuildParamOrDefault "port" "8083" |> Sockets.Port.Parse
    { defaultConfig with bindings = [ HttpBinding.mk HTTP IPAddress.Loopback port ] }

startWebServer serverConfig app

  
  
// TODO: Display using DotLiquid page

// TODO: Get current weather using JSON type provider
// (http://api.openweathermap.org/data/2.5/forecast/daily?q=London,UK&mode=json&units=metric&cnt=10)
// DEMO: Covert UNIX time stamps

// DEMO: Add async entry-point
// TODO: Make the data reading async

// TODO: Define NewsFilters.niceDate ('D') & add to template
// TODO: Register filters by name
