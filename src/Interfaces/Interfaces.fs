namespace Interfaces

open System.Threading.Tasks


type IGameGrain<'State, 'Action> = 
    inherit Orleans.IGrainWithStringKey
    abstract SetUp : User -> Task
    abstract GetState : unit -> TaskResult<'State, StatusCode>
    abstract Dispatch : 'Action -> TaskResult<'State, StatusCode>

type IPlayerGrain = 
    inherit Orleans.IGrainWithStringKey
    abstract SetUp : User -> Task
    abstract JoinGame<'State, 'Action> : unit -> TaskResult<IGameGrain<'State, 'Action>, StatusCode>

type IAuthGrain = 
    inherit Orleans.IGrainWithStringKey
    abstract Authenticate : Token -> TaskResult<User, StatusCode>
    abstract GetPlayer : Token -> TaskResult<IPlayerGrain, StatusCode>

