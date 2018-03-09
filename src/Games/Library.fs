namespace Games

open System
open System.Threading.Tasks

type IGameStore<'GameState, 'GameAction> = 
    abstract GetInitialState : unit -> TaskResult<'GameState, StatusCode>
    abstract GetState : unit -> TaskResult<'GameState, StatusCode>
    abstract Reducer : 'GameState -> 'GameAction -> TaskResult<'GameState, StatusCode>
    // abstract SetState : 'GameState -> TaskResult<unit, StatusCode>
