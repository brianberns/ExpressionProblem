using System;

namespace Visitor
{
    interface IExpr
    {
        TResult Accept<TResult>(IVisitor<TResult> visitor);
    }

    class Literal : IExpr
    {
        public Literal(int n)
        {
            N = n;
        }

        public int N { get;  }

        public TResult Accept<TResult>(IVisitor<TResult> visitor)
            => visitor.VisitLiteral(this);
    }

    class Add : IExpr
    {
        public Add(IExpr a, IExpr b)
        {
            A = a;
            B = b;
        }

        public IExpr A { get; }
        public IExpr B { get; }

        public TResult Accept<TResult>(IVisitor<TResult> visitor)
            => visitor.VisitAdd(this);
    }

    interface IVisitor<TResult>
    {
        TResult VisitLiteral(Literal literal);
        TResult VisitAdd(Add add);
    }

    class EvalVisitor : IVisitor<int>
    {
        public int VisitLiteral(Literal literal)
            => literal.N;

        public int VisitAdd(Add add)
            => add.A.Accept(this) + add.B.Accept(this);
    }

    class StringifyVisitor: IVisitor<string>
    {
        public string VisitLiteral(Literal literal)
            => literal.N.ToString();

        public string VisitAdd(Add add)
            => String.Format($"({add.A.Accept(this)} + {add.B.Accept(this)})");
    }

    static class Visitor
    {
        public static void Test()
        {
            IExpr expr =
                new Add(
                    new Literal(1),
                    new Add(
                        new Literal(2),
                        new Literal(3)));
            Console.WriteLine();
            Console.WriteLine("Visitor test");
            Console.WriteLine($"   Eval: {expr.Accept(new EvalVisitor())}");
            Console.WriteLine($"   Stringify: {expr.Accept(new StringifyVisitor())}");
        }
    }
}

// Can add new behavior (i.e. new visitor)
// Can't add a new expression type (because it requires a change to IVisitor)
