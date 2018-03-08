namespace Interfaces

open System.Threading.Tasks

type IHello =
    inherit Orleans.IGrainWithIntegerKey
    abstract member SayHello : string -> Task<string>
    abstract member SayHello2 : string -> string -> Task<string>

type IGame<'GameState, 'GameAction> = 
    inherit Orleans.IGrainWithStringKey
    abstract member GetState : unit -> Task<'GameState>
    abstract member Play : 'GameState -> 'GameAction -> Task<'GameState>


