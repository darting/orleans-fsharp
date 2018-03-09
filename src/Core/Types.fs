[<AutoOpen>]
module Types

type StatusCode = 
    | Custom of string
    | Unknown
    | Success
    | BadRequest
    | Unauthenticated
    | InsufficientBalance
    | InternalServerError of exn
    | NotImplemented
    | ServiceUnavailable
    member this.Code =
        match this with
        | Custom _ -> 0
        | Success -> 200
        | BadRequest -> 400
        | Unauthenticated -> 401
        | InsufficientBalance -> 402
        | InternalServerError _ -> 500
        | NotImplemented -> 501
        | ServiceUnavailable -> 503
        | Unknown -> 520
