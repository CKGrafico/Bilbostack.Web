// --------------------------------------------------------------------------------------
// Start up Suave.io
// --------------------------------------------------------------------------------------
#r "System.Xml.Linq"
#r "packages/FAKE/tools/FakeLib.dll"
#r "packages/FSharp.Data/lib/net40/FSharp.Data.dll"
#r "packages/Suave/lib/net40/Suave.dll"
#r "packages/Suave.DotLiquid/lib/net40/Suave.DotLiquid.dll"
#r "packages/DotLiquid/lib/net40/DotLiquid.dll"

open Fake
open Suave                 
open Suave.Successful      
open Suave.Web
open Suave.Filters
open Suave.Operators
open FSharp.Data
open Suave.DotLiquid
open System.Net


type Movies =
  { Id: int
    Poster : string
    Title : string
    VoteAvg : float
    ReleaseDate : System.DateTime
  }

type News =
  { ThumbUrl : string
    LinkUrl : string
    Title : string
    Description : string }
    
type Home = { 
    Movies: seq<Movies>
    News: seq<News> }


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

// ----------------------------------------------------------------------------
// Getting News from RSS feed and formatting it
// ----------------------------------------------------------------------------

type RSS = XmlProvider<"http://feeds.bbci.co.uk/news/rss.xml">

let getNews() = async {
  let! res = RSS.AsyncGetSample()
  return
    [ for item in res.Channel.Items |> Seq.take 15 do
      if item.Thumbnails |> Seq.length > 0 then
        let thumb = item.Thumbnails |> Seq.maxBy (fun t -> t.Width)
        yield
          { ThumbUrl = thumb.Url; LinkUrl = item.Link;
            Title = item.Title; Description = item.Description } ] }


let app : WebPart = fun ctx -> async {
  let movies = getMovies()
  let! news = getNews()
  DotLiquid.setTemplatesDir("./site")
  return! DotLiquid.page "index.html" { Movies = movies; News = news } ctx }
  
