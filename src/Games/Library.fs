namespace Games

open System
open System.Threading.Tasks

type Zero<'GameState> = unit -> 'GameState
type Reducer<'GameState, 'GameAction> = 'GameState -> 'GameAction -> 'GameState

type IGameEngine<'GameState, 'GameAction> = 
    abstract Zero : Zero<'GameState>
    abstract Reducer : Reducer<'GameState, 'GameAction>


type IGameStore<'GameState, 'GameAction> = 
    abstract GetState : unit -> TaskResult<'GameState, StatusCode>
    abstract Reducer : 'GameState -> 'GameAction -> TaskResult<'GameState, StatusCode>
