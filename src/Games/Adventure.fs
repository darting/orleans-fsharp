namespace Games

module Adventure = 

    type UserInfo = {
        Name : string
    }

    type RoomInfo = {
        Name : string
    }

    module PlayerStore = 
        type State = UserInfo
        and Action = string
        let zero () = { UserInfo.Name = "" }
        let reducer prevState action = prevState

    module RoomStore = 
        type State = RoomInfo
        and Action = string
        let zero () : State = { Name = "" }
        let reducer room action = room

    module WorldStore = 
        [<AutoSerializable(true)>]
        type State = string
        and [<AutoSerializable(true)>] Action = string

        let zero () : State = ""
        let reducer prevState action = prevState

        let create () =
            { new IGameEngine<State, Action> with
                member __.Zero = zero
                member __.Reducer = reducer }
