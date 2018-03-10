namespace Grains

open Orleans
open Games
open Interfaces


type Actor<'State, 'Action> (store: IStore<'State>,
                             reducer: IReducer<'State, 'Action>) = 
    inherit Grain ()
    interface IActor<'State, 'Action> with

        member __.GetState () =
            store.GetState ()

        member __.Dispatch action = 
            taskResult {
                let! prevState = store.GetState()
                let! newState = reducer prevState action
                do! store.SetState newState
                return newState
            }
                

