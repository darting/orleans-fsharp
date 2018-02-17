namespace Games

open Adventure

module AdventureSetup = 

    let monsters : MonsterInfo list = 
        [
            { ID = 1; Name = "An Angry Customer"; }
            { ID = 2; Name = "The Wicked Witch of the East"; }
        ]

    let users : UserInfo list = 
        [
            { ID = 1; Name = "User 1"; }
            { ID = 2; Name = "User 2"; }
        ]

    let rooms : RoomInfo list = 
        [
            { ID = 1
              Name = "west-of-house"
              Description = "You are standing in an open field west of a white house with a boarded front door.\nThere is a forest to the north."
              Directions = [ North, 2 ] }
            { ID = 2
              Name = "forest"
              Description = "This is a dimly lit forest, with large trees all around.\nThere is a clearing to the north, and a house to the south."
              Directions = [ North, 3; South, 1 ] }
            { ID = 3
              Name = "clearing"
              Description = "You are in a clearing, with a forest surrounding you on the west and south.\nThere is a forest to the south, and a canyon to the west."
              Directions = [ West, 4; South, 2 ] }
            { ID = 4
              Name = "canyon"
              Description = "You are at the top of the great canyon on its south wall.\nThere is a beach to the west, and a clearing to the east."
              Directions = [ West, 5; East, 3 ] }
            { ID = 5
              Name = "beach"
              Description = "You are on a small beach on the Frigid River, past the falls.\nThere is a faint smell of sulfur coming from the ground.\nThere is a clearing to the north, and a canyon to the east."
              Directions = [ West, 4; Down, 6 ] }
            { ID = 6
              Name = "styx"
              Description = "In front of you is the River Styx"
              Directions = [ ] }
        ]

    let load () : WorldStore.State = 
        let room : RoomStore.State = List.head rooms, List.map Creation.Monster monsters
        PlayerStore.zero(), room


