#r "./bin/Debug/netstandard2.0/Games.dll"

open Games.TicTacToe
let printBoard (board : Board) =
    List.iteri (fun i x -> 
        if x = empty then printf " _ " else printf " %i " x
        if (i + 1) % 3 = 0 then printfn ""
    ) board


let initState = zero ()

[ Move 0; Move 1; Move 2; Move 5; Move 8]
|> List.scan reducer initState 
|> List.iter (fun (board, ctx) -> 
    printBoard board
    printfn "turn: %i  player: %i" ctx.Turn ctx.CurrentPlayer
) 

