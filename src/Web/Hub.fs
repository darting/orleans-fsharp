module Web.Hubs

open System
open Microsoft.AspNetCore.SignalR

type GameHub () =
    inherit Hub ()

    override this.OnConnectedAsync() =
        this.Clients.All.SendAsync("newConnection", this.Context.ConnectionId)
