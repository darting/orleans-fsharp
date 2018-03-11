import { CounterState } from "../Types";

export enum ActionTypeKeys {
    Inc = "Inc",
    Dec = "Dec",
}

export interface IncrementAction {
    type: ActionTypeKeys.Inc,
    by: number
}

export interface DecrementAction {
    type: ActionTypeKeys.Dec,
    by: number
}

export type ActionTypes =
    | IncrementAction
    | DecrementAction

export function inc(by: number) {
    return { type: ActionTypeKeys.Inc, by: by };
}

export function dec(by: number) {
    return { type: ActionTypeKeys.Dec, by: by };
}

export function counterReducer(state: CounterState, action: ActionTypes) {
    switch (action.type) {
        case ActionTypeKeys.Inc:
            return { counter: state.counter + action.by };
        case ActionTypeKeys.Dec:
            return { counter: state.counter - action.by };
        default: return state;
    }
}
