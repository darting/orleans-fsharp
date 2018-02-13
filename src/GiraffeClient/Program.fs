open System
open Giraffe
open System.IO
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Cors
open Microsoft.AspNetCore.WebSockets
open Microsoft.AspNetCore.SignalR
open Microsoft.AspNetCore.Http.Features
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authentication.Cookies
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Orleans
open Orleans.Runtime.Configuration
open Orleans.Hosting
open Interfaces.Say
open Microsoft.AspNetCore.SignalR
open GiraffeClient.Hubs

module Startup = 

    let errorHandler (ex : Exception) (logger : ILogger) =
        logger.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")
        clearResponse >=> setStatusCode 500 >=> text ex.Message

    let buildClusterClient () =
        let config = ClientConfiguration.LocalhostSilo()
        let client = ClientBuilder()
                        .UseConfiguration(config)
                        .ConfigureApplicationParts(fun parts -> parts.AddApplicationPart(typeof<IHello>.Assembly).WithCodeGeneration() |> ignore)
                        .ConfigureLogging(fun logging -> logging.AddConsole() |> ignore)
                        .Build()
        client

    let sayHelloHandler = 
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let cluster = ctx.GetService<IClusterClient>()
                let grain = cluster.GetGrain<IHello> 0L
                let! response = grain.SayHello "from giraffe"
                return! text response next ctx
            }

    let noticeAllHandler next (ctx : HttpContext) = 
        task {
            let hub = ctx.GetService<StreamingHub>()
            do! hub.Clients.All.InvokeAsync("notice", "hi, all") 
                |> Async.AwaitTask 
                |> Async.StartAsTask
            return! next ctx
        }

    let webApp = 
        choose [
            GET >=> route "/" >=> text "index"
            GET >=> route "/sayhello" >=> sayHelloHandler
            route "/noticeall" >=> noticeAllHandler
        ]

    let configureApp (app : IApplicationBuilder) =
        app.UseGiraffeErrorHandler(errorHandler)
           .UseGiraffe webApp
        app.UseCors(fun x -> x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin() |> ignore) |> ignore
        app.UseStaticFiles()  
           .UseSignalR(fun routes -> 
                routes.MapHub<IndexHub>("index") |> ignore
                routes.MapHub<StreamingHub>("streaming") |> ignore
            )
            .UseStaticFiles() |> ignore


    let configureServices (services : IServiceCollection) =
        let clusterClient = buildClusterClient ()
        clusterClient.Connect().Wait()
        services.AddSingleton<IClusterClient>(clusterClient)
                .AddDataProtection() |> ignore     

        services.AddSignalR () |> ignore      
        services.AddCors() |> ignore

    let configureLogging (loggerBuilder : ILoggingBuilder) =
        loggerBuilder.AddFilter(fun lvl -> lvl.Equals LogLevel.Error)
                     .AddConsole()
                     .AddDebug() |> ignore  


[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"

    WebHost.CreateDefaultBuilder()
           .Configure(Action<IApplicationBuilder> Startup.configureApp)
           .ConfigureServices(Startup.configureServices)
           .ConfigureLogging(Startup.configureLogging)
           .Build()
           .Run()

    0
