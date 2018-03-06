module Interfaces

open System.Threading.Tasks

type IGame<'GameState, 'GameAction> = 
    inherit Orleans.IGrainWithStringKey
    abstract member GetState : unit -> Task<'GameState>
    abstract member Play : 'GameState -> 'GameAction -> Task<'GameState>


