module Web.App

open Giraffe
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open FSharp.Control.Tasks.ContextInsensitive
open Orleans
open Interfaces
open Games



let getState (next : HttpFunc) (ctx : HttpContext) = 
    task {
        let cluster = ctx.GetService<IClusterClient>()
        let actor = cluster.GetGrain<IActor<Adventure.WorldStore.State, Adventure.WorldStore.Action>> "playerid"
        let! state = actor.GetState()
        return! (json state) next ctx
    }

let testAction (next : HttpFunc) (ctx : HttpContext) = 
    task {
        let cluster = ctx.GetService<IClusterClient>()
        let actor = cluster.GetGrain<IActor<Adventure.WorldStore.State, Adventure.WorldStore.Action>> "playerid"
        let! state = actor.Dispatch (Adventure.WorldStore.Action.Rename "")
        return! (json state) next ctx
    }

let root : HttpHandler = 
    GET >=>
        choose [
            route "/" >=> text "index"
            route "/getstate" >=> getState
            route "/rename" >=> testAction
        ]
