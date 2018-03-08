open System
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Orleans
open Orleans.Configuration
open Orleans.Hosting
open Giraffe.Tasks
open Grains
open Interfaces
open Games
open System.Net

let addGameEngines (services : IServiceCollection) =
    services.AddSingleton<IGameEngine<Game1.GameState, Game1.GameAction>>(fun _ -> Game1.create())
            .AddSingleton<IGameEngine<Game2.GameState, Game2.GameAction>>(fun _ -> Game2.create())
            .AddSingleton<IGameEngine<Game3.GameState, Game3.GameAction>>(fun _ -> Game3.create())
            .AddSingleton<IGameEngine<Games.Adventure.WorldStore.State, Games.Adventure.WorldStore.Action>>(fun _ -> Games.Adventure.WorldStore.create())
            |> ignore

let server () =
    task {
        let siloPort = 11111
        let gatewayPort = 30000
        let siloAddr = IPAddress.Loopback
        let builder = SiloHostBuilder()
                        .Configure(fun (x : ClusterOptions) -> 
                            x.ClusterId <- "orleans-fsharp")
                        .UseDevelopmentClustering(fun (x : DevelopmentMembershipOptions) -> 
                            x.PrimarySiloEndpoint <- IPEndPoint(siloAddr, siloPort))
                        .ConfigureEndpoints(siloAddr, siloPort, gatewayPort)
                        .ConfigureApplicationParts(fun parts -> 
                            parts.AddApplicationPart((typeof<IHello>).Assembly)
                                 .AddApplicationPart((typeof<HelloGrain>).Assembly)
                                 .AddApplicationPart((typeof<IGameEngine<_,_>>).Assembly)
                                 .WithCodeGeneration() |> ignore)
                        .ConfigureLogging(fun logging -> logging.AddConsole() |> ignore)
                        .ConfigureServices(addGameEngines)
        let host = builder.Build()
        return host
    }

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"

    let t = task {
        let! host = server ()
        do! host.StartAsync ()

        Console.WriteLine("Press Enter to terminate...")
        Console.ReadLine () |> ignore

        do! host.StopAsync ()

        Console.WriteLine("Server is stopped")
    } 
    t.Wait()
    0
