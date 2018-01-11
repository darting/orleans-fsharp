namespace Web.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Orleans.Runtime
open Orleans
open Interfaces.Say

[<Route("api/[controller]")>]
type ValuesController (client: IClusterClient) =

    inherit Controller()

    [<HttpGet>]
    member this.Get() =
        let grain = client.GetGrain<IHello> 0L
        grain.SayHello "from web api"

    [<HttpGet("{id}")>]
    member this.Get(id:int) =
        let grain = client.GetGrain<IHello> (int64 id)
        grain.SayHello (id.ToString())

    [<HttpPost>]
    member this.Post([<FromBody>]value:string) =
        ()

    [<HttpPut("{id}")>]
    member this.Put(id:int, [<FromBody>]value:string ) =
        ()

    [<HttpDelete("{id}")>]
    member this.Delete(id:int) =
        ()
