namespace Web

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Orleans
open Orleans.Configuration
open Orleans.Hosting
open Orleans.Runtime
open System.Net
open Interfaces


module Program =
    let exitCode = 0

    let buildClusterClient () =
        let port = 30000
        let siloAddr = IPAddress.Loopback
        let endpoint = IPEndPoint(siloAddr, port)
        let client = ClientBuilder()
                        .ConfigureCluster(fun (x : ClusterOptions) -> 
                            x.ClusterId <- "orleans-fsharp")
                        .UseStaticClustering(fun (x : StaticGatewayListProviderOptions) -> 
                            x.Gateways.Add(endpoint.ToGatewayUri()))
                        .ConfigureApplicationParts(fun parts -> 
                            parts.AddApplicationPart((typeof<IHello>).Assembly)
                                 .WithCodeGeneration() |> ignore)
                        .ConfigureLogging(fun logging -> logging.AddConsole() |> ignore)
                        .Build()
        client

    let errorHandler (ex : Exception) (logger : ILogger) =
        logger.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")
        clearResponse >=> setStatusCode 500 >=> text ex.Message

    let webApp = 
        choose [
            GET >=> route "/" >=> text "index"
        ]

    let configureApp (app: IApplicationBuilder) =
        app.UseGiraffeErrorHandler(errorHandler)
           .UseGiraffe webApp
        app.UseCors(fun x -> x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin() |> ignore) |> ignore
        app.UseStaticFiles()  
        //    .UseSignalR(fun routes -> 
        //         routes.MapHub<IndexHub>("index") |> ignore
        //         routes.MapHub<StreamingHub>("streaming") |> ignore
        //     )
           .UseStaticFiles() |> ignore

    let configureLogging (loggerBuilder : ILoggingBuilder) =
        loggerBuilder.AddFilter(fun lvl -> lvl >= LogLevel.Warning)
                     .AddConsole()
                     .AddDebug() |> ignore             
   

    [<EntryPoint>]
    let main args =

        let clusterClient = buildClusterClient ()
        clusterClient.Connect().Wait()

        WebHost.CreateDefaultBuilder(args)
           .Configure(Action<IApplicationBuilder> configureApp)
           .ConfigureServices(fun services -> 
                services.AddSingleton<IClusterClient>(clusterClient)
                        .AddDataProtection() |> ignore     

                services.AddSignalR () |> ignore      
                services.AddCors() |> ignore 
           )
           .ConfigureLogging(configureLogging)
           .Build()
           .Run()
        exitCode
