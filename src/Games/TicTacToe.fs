namespace Games

module TicTacToe =
    type State = Board * Context
    and Board = int list
    and Context = {
        Turn : int
        CurrentPlayer : int
        NumPlayers : int
    }
    type Action = 
        | Move of position : int

    let empty = 8

    let zero () = List.init 9 (fun _ -> empty), { Turn = 0; CurrentPlayer = 0; NumPlayers = 2 }

    let reducer ((board, ctx) as state : State) = function
        | Move position -> 
            if List.item position board = empty then
                board |> List.mapi (fun i x -> if i = position then ctx.CurrentPlayer else x), 
                { ctx with 
                    Turn = ctx.Turn + 1
                    CurrentPlayer = (ctx.CurrentPlayer + 1) % ctx.NumPlayers }
            else 
                state

    let create () =
        { new IGameEngine<State, Action> with
            member __.Zero = zero
            member __.Reducer = reducer }
