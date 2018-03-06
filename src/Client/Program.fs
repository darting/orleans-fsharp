open System
open Microsoft.Extensions.Logging
open Giraffe.Tasks
open Orleans
open Orleans.Configuration
open Orleans.Hosting
open Orleans.Runtime
open Orleans.Runtime.Configuration
open Interfaces
open Games
open System.Net


let worker4 (client : IClusterClient) = 
    task {
        let game = client.GetGrain<IGame<TestGame.State, TestGame.Action>> "adventure::darting"
        let! x = game.Play (TestGame.zero()) (TestGame.Action.Spin 1m)
        printfn "%A" x
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
                            parts.AddApplicationPart((typeof<IGame<_,_>>).Assembly)
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
        do! worker4 client
        Console.ReadLine () |> ignore
    }
    t.Wait()
    0
