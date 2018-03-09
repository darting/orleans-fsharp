namespace Games

open System
open System.Threading.Tasks

type IGameStore<'GameState, 'GameAction> = 
    abstract GetState : unit -> TaskResult<'GameState, StatusCode>
    abstract Reducer : 'GameState -> 'GameAction -> TaskResult<'GameState, StatusCode>
