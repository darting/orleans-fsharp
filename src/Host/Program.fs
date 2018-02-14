open System
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Orleans
open Orleans.Runtime.Configuration
open Orleans.Hosting
open Giraffe.Tasks
open Grains.Say
open Interfaces.Say
open Games

let addGameEngines (services : IServiceCollection) =
    services.AddSingleton<IGameEngine<Game1.GameState, Game1.GameAction>>(fun _ -> Game1.create())
            .AddSingleton<IGameEngine<Game2.GameState, Game2.GameAction>>(fun _ -> Game2.create())
            .AddSingleton<IGameEngine<Games.Adventure.WorldStore.State, Games.Adventure.WorldStore.Action>>(fun _ -> Games.Adventure.WorldStore.create())
            |> ignore

let server () =
    task {
        let config = ClusterConfiguration.LocalhostPrimarySilo()
        config.AddMemoryStorageProvider("memoryStore") |> ignore

        let builder = SiloHostBuilder()
                        .UseConfiguration(config)
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
