[<AutoOpen>]
module Prelude

open System.Diagnostics
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive

/// Maybe computation expression builder, copied from ExtCore library
/// https://github.com/jack-pappas/ExtCore/blob/master/ExtCore/Control.fs
[<Sealed>]
type MaybeBuilder () =
    // 'T -> M<'T>
    [<DebuggerStepThrough>]
    member inline __.Return value: 'T option = Some value

    // M<'T> -> M<'T>
    [<DebuggerStepThrough>]
    member inline __.ReturnFrom value: 'T option = value

    // unit -> M<'T>
    [<DebuggerStepThrough>]
    member inline __.Zero (): unit option = Some ()     // TODO: Should this be None?

    // (unit -> M<'T>) -> M<'T>
    [<DebuggerStepThrough>]
    member __.Delay (f: unit -> 'T option): 'T option = f ()

    // M<'T> -> M<'T> -> M<'T>
    // or
    // M<unit> -> M<'T> -> M<'T>
    [<DebuggerStepThrough>]
    member inline __.Combine (r1, r2: 'T option): 'T option =
        match r1 with
        | None -> None
        | Some () -> r2

    // M<'T> * ('T -> M<'U>) -> M<'U>
    [<DebuggerStepThrough>]
    member inline __.Bind (value, f: 'T -> 'U option): 'U option = Option.bind f value

    // 'T * ('T -> M<'U>) -> M<'U> when 'U :> IDisposable
    [<DebuggerStepThrough>]
    member __.Using (resource: ('T :> System.IDisposable), body: _ -> _ option): _ option =
        try body resource
        finally if not <| obj.ReferenceEquals (null, box resource) then resource.Dispose ()

    // (unit -> bool) * M<'T> -> M<'T>
    [<DebuggerStepThrough>]
    member x.While (guard, body: _ option): _ option =
        if guard () then
            // OPTIMIZE: This could be simplified so we don't need to make calls to Bind and While.
            x.Bind (body, (fun () -> x.While (guard, body)))
        else x.Zero ()

    // seq<'T> * ('T -> M<'U>) -> M<'U>
    // or
    // seq<'T> * ('T -> M<'U>) -> seq<M<'U>>
    [<DebuggerStepThrough>]
    member x.For (sequence: seq<_>, body: 'T -> unit option): _ option =
        // OPTIMIZE: This could be simplified so we don't need to make calls to Using, While, Delay.
        x.Using (sequence.GetEnumerator (), fun enum ->
            x.While (enum.MoveNext,
                x.Delay (fun () -> body enum.Current)
            )
        )

    member x.Either (m1 : 'T option) (m2 : 'T option) = 
        match m1 with
        | Some x -> Some x
        | None -> m2  

let maybe = MaybeBuilder()

module Maybe = 

    module Operators =
        let (>>=) m f = Option.bind f m
        let (>=>) f1 f2 x = f1 x >>= f2
        let (>>%) m v = m >>= (fun _ -> maybe.Return v)
        let (>>.) m1 m2 = m1 >>= (fun _ -> m2)
        let (.>>) m1 m2 = m1 >>= (fun x -> m2 >>% x)
        let (.>>.) m1 m2 = m1 >>= (fun x -> m2 >>= (fun y -> maybe.Return (x, y)))
        let (<|>) m1 m2 = maybe.Either m1 m2


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
        