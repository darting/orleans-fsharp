module GiraffeClient.Hubs.Streaming

open System
open Microsoft.AspNetCore.SignalR
open System.Reactive.Linq

type StreamingHub () =
    inherit Hub ()

    member __.ObservableCounter (count : int, delay : float) =
        Observable.Interval(TimeSpan.FromMilliseconds(delay))
                  .Select(fun i -> i)
                  .Take(count)

    