namespace Web

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Giraffe.Tasks
open Interfaces.Say
open Microsoft.Extensions.Logging
open Orleans
open Orleans.Runtime.Configuration
open Orleans.Hosting


module private Internal = 

    let buildClusterClient () =
        let config = ClientConfiguration.LocalhostSilo()
        let client = ClientBuilder()
                        .UseConfiguration(config)
                        .ConfigureApplicationParts(fun parts -> parts.AddApplicationPart(typeof<IHello>.Assembly).WithCodeGeneration() |> ignore)
                        .ConfigureLogging(fun logging -> logging.AddConsole() |> ignore)
                        .Build()
        client

type Startup private () =
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration

    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        this.ClusterClient <- Internal.buildClusterClient ()
        this.ClusterClient.Connect().Wait()
        services.AddSingleton<IClusterClient>(this.ClusterClient) |> ignore

        // Add framework services.
        services.AddMvc() |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, lifetime: IApplicationLifetime, env: Microsoft.AspNetCore.Hosting.IHostingEnvironment) =
        app.UseMvc() |> ignore
        lifetime.ApplicationStopping.Register(Action this.Shutdown) |> ignore

    member this.Shutdown () =
        this.ClusterClient.Dispose() |> ignore

    member val Configuration : IConfiguration = null with get, set
    member val ClusterClient : IClusterClient = null with get, set
