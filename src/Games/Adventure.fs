namespace Games

module Adventure = 

    type Direction =
        | North
        | South
        | East 
        | West
        | Down
        | Up
    type Creation = 
        | Player of UserInfo
        | Monster of MonsterInfo
    and UserInfo = {
        ID : int
        Name : string
    }
    and MonsterInfo = {
        ID : int
        Name : string
    }
    and RoomID = int
    and RoomInfo = {
        ID : RoomID
        Name : string
        Description : string
        Directions : (Direction * RoomID) list
    }

    module PlayerStore = 
        type State = 
            | Alive of UserInfo
            | Die of UserInfo
        and Action = 
            | Rename of string

        let zero () = Alive { ID = 0; Name = "" }

        let reducer prevState action = 
            match prevState with
            | Alive player -> 
                match action with
                | Rename name -> Alive { player with Name = name }
            | Die _ -> prevState

        let rename prevState name = 
            reducer prevState (Rename name)

    module RoomStore = 
        type State = RoomInfo * Creation list
        and Action = 
            | SetInfo of RoomInfo
            | Join of Creation
            | Leave of Creation
            | Go of Direction
        let zero () : State = { ID = 0; Name = ""; Description = ""; Directions = [] }, []
        let reducer (room, creations) action = 
            match action with
            | SetInfo info -> info, creations
            | Join creation -> room, creation :: creations
            | Leave creation -> room, List.except [ creation ] creations
            | Go direction -> { ID = 0; Name = direction.ToString (); Description = direction.ToString (); Directions = [] }, []

        let go prevState direction = reducer prevState (Go direction)
        let setInfo prevState info = reducer prevState (SetInfo info)
        let join prevState creation = reducer prevState (Join creation)
        let leave prevState creation = reducer prevState (Leave creation)

    module WorldStore = 
        type State = PlayerStore.State * RoomStore.State
        and Action = 
            | Rename of string
            | Go of Direction
            | SetRoomInfo of RoomInfo
            | Join of Creation
            | Leave of Creation

        let zero () : State = PlayerStore.zero (), RoomStore.zero ()
        let reducer (playerState, roomState) action = 
            match action with
            | Rename x -> PlayerStore.rename playerState x, roomState
            | Go direction -> playerState, RoomStore.go roomState direction
            | SetRoomInfo info -> playerState, RoomStore.setInfo roomState info
            | Join creation -> playerState, RoomStore.join roomState creation
            | Leave creation -> playerState, RoomStore.leave roomState creation

        let create () =
            { 
                new IGameStore<State, Action> with
                    member __.GetInitialState () = 
                        taskResult {
                            return zero ()
                        }
                    member __.GetState () = 
                        taskResult {
                            return zero ()
                        }
                    member __.Reducer prevState action = 
                        taskResult {
                            return reducer prevState action                            
                        } 
                    // member __.SetState state = 
                    //     taskResult {
                    //         return ()
                    //     }
            }
