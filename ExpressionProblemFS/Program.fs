﻿module Simple =

    type Expr =
        | Literal of int
        | Add of Expr * Expr

    let rec eval = function
        | Literal n -> n
        | Add (a, b) -> eval a + eval b

    let rec print = function
        | Literal n -> n.ToString()
        | Add (a, b) -> sprintf "(%s + %s)" (print a) (print b)

    let test () =
        let expr = Add (Literal 1, (Add (Literal 2, Literal 3)))
        printfn ""
        printfn "Simple test"
        printfn "   Eval: %A" <| eval expr
        printfn "   Print: %s" <| print expr

module Algebra =

    type ExprAlgebra<'T> =
        abstract member Literal : int -> 'T
        abstract member Add : 'T -> 'T -> 'T

    type EvalAlgebra () =
        interface ExprAlgebra<int> with
            member __.Literal n = n
            member __.Add a b = a + b

    type PrintAlgebra () =
        interface ExprAlgebra<string> with
            member __.Literal n = n.ToString()
            member __.Add a b = sprintf "(%s + %s)" a b

    let test () =

        let createTestExpr (factory : ExprAlgebra<_>) =
            factory.Add
                (factory.Literal 1)
                (factory.Add
                    (factory.Literal 2)
                    (factory.Literal 3))

        let eval = createTestExpr <| EvalAlgebra ()
        let print = createTestExpr <| PrintAlgebra ()
        printfn ""
        printfn "Algebra test"
        printfn "   Eval: %A" <| eval
        printfn "   Print: %s" <| print

[<EntryPoint>]
let main argv =
    Simple.test ()
    Algebra.test ()
    0
