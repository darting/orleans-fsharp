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
    services.AddSingleton<IGameEngine<TestGame.State, TestGame.Action>>(fun _ -> TestGame.create())
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
                            parts.AddApplicationPart((typeof<IGame<_,_>>).Assembly)
                                 .AddApplicationPart((typeof<GameGrain<_,_>>).Assembly)
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
