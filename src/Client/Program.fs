open System
open Microsoft.Extensions.Logging
open Giraffe.Tasks
open Orleans
open Orleans.Configuration
open Orleans.Hosting
open Orleans.Runtime
open Orleans.Runtime.Configuration
open Interfaces.Say
open Games
open System.Net

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

// not working also
// Unhandled Exception: System.AggregateException: One or more errors occurred. 
// (Named type "Games.Games.Games.Adventure.PlayerStore.State" is invalid: Type string "Games.Games.Games.Adventure.PlayerStore.State" 
// cannot be resolved.) ---> System.TypeAccessException: Named type "Games.Games.Games.Adventure.PlayerStore.State" is invalid: 
// Type string "Games.Games.Games.Adventure.PlayerStore.State" cannot be resolved.
let worker4 (client : IClusterClient) = 
    let show state = 
        printfn "player: %s" (match state with | Games.Adventure.PlayerStore.Alive x -> x.Name | _ -> "die")
    let initState = AdventureSetup.load ()
    task {
        let game = client.GetGrain<IGame<Games.Adventure.WorldStore.State, Games.Adventure.WorldStore.Action>> "adventure::darting"
        // let! ((playerState, roomState) as state) = game.GetState ()
        // show playerState
        let! ((playerState, roomState) as state) = game.Play initState (Games.Adventure.WorldStore.Action.Rename "darting")
        show playerState
    }

let creator () =
    task {
        let port = 30000
        let siloAddr = IPAddress.Loopback
        let endpoint = IPEndPoint(siloAddr, port)
        let client = ClientBuilder()
                        .ConfigureCluster(fun (x : ClusterOptions) -> 
                            x.ClusterId <- "orleans-fsharp")
                        .UseStaticClustering(fun (x : StaticGatewayListProviderOptions) -> 
                            x.Gateways.Add(endpoint.ToGatewayUri()))
                        .ConfigureApplicationParts(fun parts -> 
                            parts.AddApplicationPart((typeof<IHello>).Assembly)
                                 .AddApplicationPart((typeof<IGameEngine<_,_>>).Assembly)
                                 .WithCodeGeneration() |> ignore)
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
        //do! worker client
        do! worker1 client
        do! worker2 client
        do! worker3 client
        // do! worker4 client
        Console.ReadLine () |> ignore
    }
    t.Wait()
    0
