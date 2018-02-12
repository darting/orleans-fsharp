namespace Games

open System

type Zero<'GameState> = unit -> 'GameState
type Reducer<'GameState, 'GameAction> = 'GameState -> 'GameAction -> 'GameState

type IGameEngine<'GameState, 'GameAction> = 
    abstract Zero : Zero<'GameState>
    abstract Reducer : Reducer<'GameState, 'GameAction>


module Game1 = 
    type GameState = int
    type GameAction = Inc
    let random = Random ()
    let reducer (prevState : GameState) = function
        | Inc -> prevState + (random.Next())
    let zero () : GameState = 0
    let create () =
        { new IGameEngine<GameState, GameAction> with
            member __.Zero = zero
            member __.Reducer = reducer }

module Game2 = 
    type GameState = string
    type GameAction = Rev
    let reducer (prevState : GameState) = function
        | Rev -> Seq.rev prevState |> Array.ofSeq |> String
    let zero () : GameState = "hello, this is game2"
    let create () =
        { new IGameEngine<GameState, GameAction> with
            member __.Zero = zero
            member __.Reducer = reducer }

module Game3 = 
    let random = Random ()
    type GameState = 
        | Spin of Balance * Result
        | FreeSpin of Balance * Result
    and GameAction = 
        | Spin of wager : decimal
    and Result = int list
    and Balance = decimal
    let private calculate (wager : decimal) =
        let result = List.init 5 (fun _ -> random.Next 10)
        let win = (List.sum result) % 2 = 0
        result, (if win then wager * 2m else 0m)
    let reducer (prevState : GameState) action = 
        match action with
        | Spin wager ->
            let balance = match prevState with 
                          | GameState.Spin (b, _) -> b - wager
                          | GameState.FreeSpin (b, _) -> b
            let result, winlose = calculate wager
            let balance' = balance + winlose
            if (List.head result = 0) then 
                GameState.FreeSpin (balance', result)
            else 
                GameState.Spin (balance', result)
        
    let zero () : GameState = GameState.Spin (1000m, [])
    let create () =
        { new IGameEngine<GameState, GameAction> with
            member __.Zero = zero
            member __.Reducer = reducer }

      
