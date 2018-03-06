module Grains

open Orleans
open Interfaces
open Games
open System.Threading.Tasks

type GameGrain<'GameState, 'GameAction> (engine: IGameEngine<'GameState, 'GameAction>) = 
    inherit Grain ()
    interface IGame<'GameState, 'GameAction> with
        member __.GetState () =
            engine.Zero () |> Task.FromResult

        member __.Play prevState action = 
            engine.Reducer prevState action |> Task.FromResult

// type GameGrain (engine: IGameEngine<TestGame.State, TestGame.Action>) = 
//     inherit Grain ()
//     interface IGame<TestGame.State, TestGame.Action> with
//         member __.GetState () =
//             engine.Zero () |> Task.FromResult

//         member __.Play prevState action = 
//             engine.Reducer prevState action |> Task.FromResult

// type GameGrain (engine: IGameEngine<Adventure.WorldStore.State, Adventure.WorldStore.Action>) = 
//     inherit Grain ()
//     interface IGame<Adventure.WorldStore.State, Adventure.WorldStore.Action> with
//         member __.GetState () =
//             engine.Zero () |> Task.FromResult

//         member __.Play prevState action = 
//             engine.Reducer prevState action |> Task.FromResult
