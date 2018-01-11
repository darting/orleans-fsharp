namespace Grains

module Say =

    open System.Threading.Tasks
    open Orleans
    open Interfaces.Say

    type HelloGrain () =
        inherit Grain ()
        interface IHello with 
            member __.SayHello (greet : string) : Task<string> = 
                greet |> sprintf "hello, %s" |> Task.FromResult
            member __.SayHello2 (a : string) (b : string) : Task<string> =
                a + " - " + b |> Task.FromResult
