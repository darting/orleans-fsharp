namespace Grains

open Orleans
open Games
open Interfaces


 type GameGrain<'GameState, 'GameAction> (store: IGameStore<'GameState, 'GameAction>) = 
    inherit Grain ()

    interface IGameGrain<'GameState, 'GameAction> with

        member __.GetState () =
            store.GetState ()

        member __.Dispatch action = 
            taskResult {
                let! prevState = store.GetState()
                return! store.Reducer prevState action
            }
            

