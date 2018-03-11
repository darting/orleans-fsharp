namespace Grains

open Microsoft.Extensions.Caching.Memory
open FSharp.Control.Tasks.ContextInsensitive
open Orleans
open Interfaces
open System
open System.Threading.Tasks


type GameGrain<'State, 'Action> (store: IStore<'State>, reducer: IReducer<'State, 'Action>) = 
    inherit Grain ()
    [<DefaultValue>] val mutable private user : User

    interface IGameGrain<'State, 'Action> with

        member this.SetUp user = 
            this.user <- user
            Task.CompletedTask

        member __.GetState () =
            store.GetState ()

        member __.Dispatch action = 
            taskResult {
                let! prevState = store.GetState()
                let! newState = reducer prevState action
                do! store.SetState newState
                return newState
            }
                
type AuthGrain () =
    inherit Grain () 
    
    let users = new MemoryCache(MemoryCacheOptions())
    let memoize token user = 
        users.Set (token, user, MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20.0))) |> ignore

    interface IAuthGrain with
        member __.Authenticate token = 
            taskResult {
                let user = { User.ID = token; NickName = token }
                memoize token user
                return user
            }

        member this.GetPlayer token =
            let factory = this.GrainFactory
            task {
                match users.TryGetValue token with
                | true, x -> 
                    let user = x :?> User
                    let grain = factory.GetGrain<IPlayerGrain> user.ID
                    do! grain.SetUp user
                    return Ok grain
                | _ -> 
                    return Error StatusCode.Unauthenticated
            }

type PlayerGrain () =
    inherit Grain ()
    [<DefaultValue>] val mutable private user : User

    interface IPlayerGrain with
        member this.SetUp user = 
            this.user <- user
            Task.CompletedTask

        member this.JoinGame () =
            let factory = this.GrainFactory
            task {
                let grain = factory.GetGrain<IGameGrain<'State, 'Action>> this.user.ID
                do! grain.SetUp this.user
                return Ok grain
            }

