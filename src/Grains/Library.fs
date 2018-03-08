namespace Grains

open System.Threading.Tasks
open Orleans
open FSharp.Control.Tasks.ContextInsensitive
open Games
open Interfaces

type HelloGrain () =
    inherit Grain ()
    interface IHello with 
        member __.SayHello (greet : string) : Task<string> = 
            greet |> sprintf "hello, %s" |> Task.FromResult
        member __.SayHello2 (a : string) (b : string) : Task<string> =
            a + " - " + b |> Task.FromResult


 type GameGrain<'GameState, 'GameAction> (engine: IGameEngine<'GameState, 'GameAction>) = 
    inherit Grain ()
    interface IGame<'GameState, 'GameAction> with
        member __.GetState () =
            engine.Zero () |> Task.FromResult

        member __.Play prevState action = 
            engine.Reducer prevState action |> Task.FromResult

