namespace GiraffeClient.Hubs

open System
open Microsoft.AspNetCore.SignalR
open System.Reactive.Linq
open FSharp.Control.Tasks.ContextInsensitive
open System.Threading.Tasks


[<CLIMutable>]
type Chat = {
    User : string
    Message : string
}

type IndexHub () =
    inherit Hub ()

    override this.OnConnectedAsync() =
        this.Clients.All.InvokeAsync("newConnection", this.Context.ConnectionId)

    member this.Chat (msg : string) =
        this.Clients.All.InvokeAsync("chat", {
            User = this.Context.ConnectionId
            Message = msg
        })
