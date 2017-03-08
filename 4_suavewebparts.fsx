#r "./packages/Suave/lib/net40/Suave.dll"

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful

// type WebPart = HttpContext -> Async<HttpContext option>

// HttpContext: an F# record type that includes the HTTP request, the HTTP response, and a few other things

// >=> Kleisli composition: chains together Async options

let app =
  choose
    [ GET >=> choose
        [ path "/hello" >=> OK "Hello GET"
          path "/goodbye" >=> OK "Good bye GET" ]
      POST >=> choose
        [ path "/hello" >=> OK "Hello POST"
          path "/goodbye" >=> OK "Good bye POST" ] ]

startWebServer defaultConfig app

