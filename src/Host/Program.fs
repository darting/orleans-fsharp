open System
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Orleans
open Orleans.Configuration
open Orleans.Hosting
open FSharp.Control.Tasks.ContextInsensitive
open Grains
open Interfaces
open Adventure
open System.Net

let addGameStores (services : IServiceCollection) =
    // services.AddSingleton(WorldStore.createReducer())
    //         .AddSingleton(WorldStore.createStore())
    //         |> ignore
    ()

let server () =
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
                        parts.AddApplicationPart((typeof<IPlayerGrain>).Assembly)
                             .AddApplicationPart((typeof<PlayerGrain>).Assembly)
                             .WithCodeGeneration() |> ignore)
                    .ConfigureLogging(fun logging -> logging.AddConsole() |> ignore)
                    .ConfigureServices(addGameStores)
    let host = builder.Build()
    host

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"

    let t = task {
        let host = server ()
        do! host.StartAsync ()

        Console.WriteLine("Press Enter to terminate...")
        Console.ReadLine () |> ignore

        do! host.StopAsync ()

        Console.WriteLine("Server is stopped")
    } 
    t.Wait()
    0
