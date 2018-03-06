namespace Games
open System

type Zero<'GameState> = unit -> 'GameState
type Reducer<'GameState, 'GameAction> = 'GameState -> 'GameAction -> 'GameState

type IGameEngine<'GameState, 'GameAction> = 
    abstract Zero : Zero<'GameState>
    abstract Reducer : Reducer<'GameState, 'GameAction>


module TestGame =
    let random = Random ()
    type State = 
        | Spin of Balance * Result
        | FreeSpin of Balance * Result
    and Action = 
        | Spin of wager : decimal
    and Result = int list
    and Balance = decimal
    let private calculate (wager : decimal) =
        let result = List.init 5 (fun _ -> random.Next 10)
        let win = (List.sum result) % 2 = 0
        result, (if win then wager * 2m else 0m)
    let reducer (prevState : State) action = 
        match action with
        | Spin wager ->
            let balance = match prevState with 
                          | State.Spin (b, _) -> b - wager
                          | State.FreeSpin (b, _) -> b
            let result, winlose = calculate wager
            let balance' = balance + winlose
            if (List.head result = 0) then 
                State.FreeSpin (balance', result)
            else 
                State.Spin (balance', result)
        
    let zero () : State = State.Spin (1000m, [])
    let create () =
        { new IGameEngine<State, Action> with
            member __.Zero = zero
            member __.Reducer = reducer }

