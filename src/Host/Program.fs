open System
open Microsoft.Extensions.Logging
open FSharp.Control.Tasks
open Orleans
open Orleans.Runtime.Configuration
open Orleans.Hosting
open Grains.Say

let server () =
    task {
        let config = ClusterConfiguration.LocalhostPrimarySilo()
        config.AddMemoryStorageProvider() |> ignore

        let builder = SiloHostBuilder()
                        .UseConfiguration(config)
                        .ConfigureApplicationParts(fun parts -> parts.AddFromAppDomain().WithCodeGeneration() |> ignore) 
                        // .ConfigureApplicationParts(fun parts -> parts.AddApplicationPart((typeof<HelloGrain>).Assembly).WithCodeGeneration() |> ignore)
                        .ConfigureLogging(fun logging -> logging.AddConsole() |> ignore)
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
    t.GetAwaiter().GetResult()
    0
