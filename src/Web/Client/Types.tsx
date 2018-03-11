

export interface User {
    ID: string,
    nickname: string,
}

export interface CounterState {
    counter: number
}

export interface AppState {
    player: User,
    players: User[],
}
