type Expr =
    | Literal of int
    | Add of Expr * Expr

let rec eval = function
    | Literal n -> n
    | Add (a, b) -> eval a + eval b

let rec print = function
    | Literal n -> n.ToString()
    | Add (a, b) -> sprintf "(%s + %s)" (print a) (print b)

[<EntryPoint>]
let main argv =
    let expr = Add (Literal 1, (Add (Literal 2, Literal 3)))
    printfn "Eval: %A" <| eval expr
    printfn "Print: %s" <| print expr
    0
