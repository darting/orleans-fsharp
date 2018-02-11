namespace Grains

module Say =

    open System.Threading.Tasks
    open Orleans
    open Giraffe.Tasks
    open Games
    open Interfaces.Say

    type HelloGrain () =
        inherit Grain ()
        interface IHello with 
            member __.SayHello (greet : string) : Task<string> = 
                greet |> sprintf "hello, %s" |> Task.FromResult
            member __.SayHello2 (a : string) (b : string) : Task<string> =
                a + " - " + b |> Task.FromResult

    // Not working
    // type GameGrain<'GameState, 'GameAction> (engine: IGameEngine<'GameState, 'GameAction>) = 
    //     inherit Grain ()
    //     interface IGame<'GameState, 'GameAction> with
    //         member __.GetState () =
    //             engine.Zero () |> Task.FromResult

    //         member __.Play prevState action = 
    //             engine.Reducer prevState action |> Task.FromResult


    type Game1Grain (engine : IGameEngine<Games.Game1.GameState, Games.Game1.GameAction>) = 
        inherit Grain ()
        interface IGame<Game1.GameState, Games.Game1.GameAction> with
            member __.GetState () =
                engine.Zero () |> Task.FromResult
            member __.Play prevState action = 
                engine.Reducer prevState action |> Task.FromResult

    type Game2Grain (engine : IGameEngine<Games.Game2.GameState, Games.Game2.GameAction>) = 
        inherit Grain ()
        interface IGame<Game2.GameState, Games.Game2.GameAction> with
            member __.GetState () =
                engine.Zero () |> Task.FromResult
            member __.Play prevState action = 
                engine.Reducer prevState action |> Task.FromResult

    type Game3Grain () = 
        inherit Grain ()
        let engine = Games.Game3.create ()
        interface IGame<Game3.GameState, Games.Game3.GameAction> with
            member __.GetState () =
                engine.Zero () |> Task.FromResult
            member __.Play prevState action = 
                engine.Reducer prevState action |> Task.FromResult       
