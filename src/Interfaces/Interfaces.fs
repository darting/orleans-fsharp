namespace Interfaces


type IGameGrain<'GameState, 'GameAction> = 
    inherit Orleans.IGrainWithStringKey
    abstract member GetState : unit -> TaskResult<'GameState, StatusCode>
    abstract member Dispatch : 'GameAction -> TaskResult<'GameState, StatusCode>


