namespace Games

module TicTacToe =
    type State = Board * Context
    and Board = int list
    and Context = {
        Turn : int
        CurrentPlayer : int
        NumPlayers : int
        EndGame : bool
    }
    type Action = 
        | Move of position : int

    let empty = -1
    let winLines = 
        [ [0;1;2]
          [3;4;5]
          [6;7;8]
          [0;3;6]
          [1;4;7]
          [2;5;8]
          [0;4;8]
          [2;4;6] ]

    let isVictory (board : Board) =
        // winLines |> List.exists (fun line ->
        //                 board.[line.[0]] = board.[line.[1]] = board.[line.[2]]
        //             )
        false
        
        

    let zero () = List.init 9 (fun _ -> empty), { Turn = 0; CurrentPlayer = 0; NumPlayers = 2; EndGame = false }

    let reducer ((board, ctx) as state : State) = function
        | Move position -> 
            if not ctx.EndGame && List.item position board = empty then
                let newBoard = board |> List.mapi (fun i x -> if i = position then ctx.CurrentPlayer else x)
                newBoard, 
                { ctx with 
                    Turn = ctx.Turn + 1
                    CurrentPlayer = (ctx.CurrentPlayer + 1) % ctx.NumPlayers
                    EndGame = isVictory newBoard }
            else 
                state

    let create () =
        { new IGameEngine<State, Action> with
            member __.Zero = zero
            member __.Reducer = reducer }
