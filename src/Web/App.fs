module Web.App

open Giraffe
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open FSharp.Control.Tasks.ContextInsensitive
open Orleans
open Interfaces


let root : HttpHandler = 
    GET >=>
        choose [
            route "/" >=> text "index"
        ]
