namespace Interfaces

module Say = 

    open System.Threading.Tasks

    type IHello =
        inherit Orleans.IGrainWithIntegerKey
        abstract member SayHello : string -> Task<string>
        abstract member SayHello2 : string -> string -> Task<string>