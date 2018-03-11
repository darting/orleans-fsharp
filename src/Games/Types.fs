namespace Adventure

open System

type IDescribable = 
    abstract Name : string with get
    abstract Description : string with get

type Direction =
    | North
    | South
    | East 
    | West
    | Down
    | Up

type Item = {
    ID : Guid
    Name : string
}

type Race = Human | Elven | Dwarven

type Role = Thief | Hunter | Warrior | Merchant | Priest | Wizard

type Gender = Male | Female

type Level = int32

type Character = {
    ID : string
    Name : string
    Race : Race
    Role : Role
    Gender : Gender
    Level : Level
    Inventory : Item list
}


module Character = 

    let addItem (item : Item) (character : Character) = 
        { character with Inventory = item :: character.Inventory }

    let findItem (name : string) (character : Character) =
        character.Inventory |> List.tryFind (fun x -> x.Name = name)

    let removeItem (name : string) (character : Character) =
        let inventory = character.Inventory |> List.filter (fun x -> x.Name <> name)
        { character with Inventory = inventory }


type RoomID = Guid

type Room = {
    ID : RoomID
    Name : string
    Description : string
    Characters : Character list
    Adjacents : RoomID list
}

type World = {
    Rooms : Room list
    Characters : Character list
}

type Command = 
    | Look
    | Say
    | Take
    | Inventory
    | Drop
