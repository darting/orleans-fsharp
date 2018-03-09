[<AutoOpen>]
module Prelude

open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive


type TaskResult<'T, 'Error> = Task<Result<'T, 'Error>>


[<Sealed>]
type TaskResultBuilder () =
    // 'T -> M<'T>
    member __.Return value : TaskResult<'T, 'Error> =
        Task.FromResult (Ok value)

    // M<'T> -> M<'T>
    member __.ReturnFrom (m : TaskResult<'T, 'Error>) = m

    // unit -> M<'T>
    member this.Zero () : TaskResult<unit, 'Error> =
        this.Return ()

    // (unit -> M<'T>) -> M<'T>
    member inline __.Delay (generator : unit -> TaskResult<'T, 'Error>) : TaskResult<'T, 'Error> =
        generator ()

    // M<unit> -> M<'T> -> M<'T>
    member __.Combine (r1 : TaskResult<unit, 'Error>, r2 : TaskResult<'T, 'Error>) : TaskResult<'T, 'Error> =
        task {
            let! r1' = r1
            match r1' with
            | Error error ->
                return Error error
            | Ok () ->
                return! r2
        }

    // M<'T> * ('T -> M<'U>) -> M<'U>
    member __.Bind (value : TaskResult<'T, 'Error>, binder : 'T -> TaskResult<'U, 'Error>)
        : TaskResult<'U, 'Error> =
        task {
            let! value' = value
            match value' with
            | Error error ->
                return Error error
            | Ok x ->
                return! binder x
        }

let taskResult = TaskResultBuilder ()

[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module TaskResult =

    [<CompiledName("Error")>]
    let inline error value : TaskResult<'T, 'Error> =
        Task.FromResult (Error value)
        