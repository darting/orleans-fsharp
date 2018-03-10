namespace Interfaces


type IActor<'State, 'Action> = 
    inherit Orleans.IGrainWithStringKey
    abstract member GetState : unit -> TaskResult<'State, StatusCode>
    abstract member Dispatch : 'Action -> TaskResult<'State, StatusCode>


