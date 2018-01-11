open System
open Microsoft.Extensions.Logging
open FSharp.Control.Tasks
open Orleans
open Orleans.Runtime.Configuration
open Orleans.Hosting
open Interfaces.Say

let worker (client : IClusterClient) = 
    task {
        let friend = client.GetGrain<IHello> 0L
        let! response = friend.SayHello ("morning")
        Console.WriteLine(response)
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
    }
    t.Wait()
    0 // return an integer exit code
