namespace Grains

module Say =

    open System.Threading.Tasks
    open Orleans
    open Interfaces.Say

    type HelloGrain () =
        inherit Grain ()
        interface IHello with 
            member this.SayHello (greet : string) : Task<string> = 
                greet |> sprintf "hello, %s" |> Task.FromResult


