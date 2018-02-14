open System
open Microsoft.Extensions.Logging
open Giraffe.Tasks
open Orleans
open Orleans.Runtime.Configuration
open Orleans.Hosting
open Interfaces.Say
open Games

let worker (client : IClusterClient) = 
    task {
        let friend = client.GetGrain<IHello> 0L
        let! response = friend.SayHello ("morning")
        Console.WriteLine response
        let! rsp2 = friend.SayHello2 "aaa" "bbb"
        Console.WriteLine rsp2
    }

let worker1 (client : IClusterClient) = 
    task {
        let game = client.GetGrain<IGame<Game1.GameState, Game1.GameAction>> "game1::ms::darting"
        let! state = game.GetState ()
        printfn "init state: %i" state
        let! state = game.Play state Game1.GameAction.Inc
        printfn "1) play: %i" state
        let! state = game.Play state Game1.GameAction.Inc
        printfn "2) play: %i" state
        let! state = game.Play state Game1.GameAction.Inc
        printfn "3) play: %i" state
    }

let worker2 (client : IClusterClient) = 
    task {
        let game = client.GetGrain<IGame<Games.Game2.GameState, Game2.GameAction>> "game2::ms::darting"
        let! state = game.GetState ()
        printfn "init state: %s" state
        let! state = game.Play state Game2.GameAction.Rev
        printfn "1) play: %s" state
        let! state = game.Play state Game2.GameAction.Rev
        printfn "2) play: %s" state
        let! state = game.Play state Game2.GameAction.Rev
        printfn "3) play: %s" state
    }

let worker3 (client : IClusterClient) = 
    let printState = function 
        | Games.Game3.Spin (balance, result) ->
            printfn "spin: %M ==> %A" balance result
        | Games.Game3.FreeSpin (balance, result) ->
            printfn "free spin: %M ==> %A" balance result
    task {
        let game = client.GetGrain<IGame<Game3.GameState, Game3.GameAction>> "game3::ms::darting"
        let! state = game.GetState ()
        printState state
        let! state = game.Play state (Game3.GameAction.Spin 1m)
        printState state
        let! state = game.Play state (Game3.GameAction.Spin 2m)
        printState state
        let! state = game.Play state (Game3.GameAction.Spin 3m)
        printState state
        let! state = game.Play state (Game3.GameAction.Spin 4m)
        printState state
        let! state = game.Play state (Game3.GameAction.Spin 5m)
        printState state
    }

let worker4 (client : IClusterClient) = 
    let printPlayerState state = 
        printfn "player: %s" (match state with | Games.Adventure.PlayerStore.Alive x -> x.Name | _ -> "die")

    task {
        let game = client.GetGrain<IGame<Games.Adventure.WorldStore.State, Games.Adventure.WorldStore.Action>> "adventure::darting"
        let! ((playerState, roomState) as state) = game.GetState ()
        printPlayerState playerState

        let! state = game.Play state (Games.Adventure.WorldStore.Action.Rename "darting")
        printPlayerState playerState
        // let! state = game.Play state Game2.GameAction.Rev
        // printfn "1) play: %s" state
        // let! state = game.Play state Game2.GameAction.Rev
        // printfn "2) play: %s" state
        // let! state = game.Play state Game2.GameAction.Rev
        // printfn "3) play: %s" state
    }

let creator () =
    task {
        let t = typeof<IHello>
        let config = ClientConfiguration.LocalhostSilo()
        let client = ClientBuilder()
                        .UseConfiguration(config)
                        .ConfigureApplicationParts(fun parts -> parts.AddApplicationPart(t.Assembly).WithCodeGeneration() |> ignore )
                        .ConfigureLogging(fun logging -> logging.AddConsole() |> ignore)
                        .Build()
        return client
    }


[<EntryPoint>]
let main argv =

    printfn "Hello World from F#!"

    let t = task {
        use! client = creator ()
        do! client.Connect ()
        Console.WriteLine("Client successfully connect to silo host")
        Console.ReadLine () |> ignore
        do! worker client
        do! worker1 client
        do! worker2 client
        do! worker3 client
        do! worker4 client
        Console.ReadLine () |> ignore
    }
    t.Wait()
    0
