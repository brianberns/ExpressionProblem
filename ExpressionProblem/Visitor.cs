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

    static class Visitor
    {
        // 1 + (2 + 3)
        public static IExpr CreateTestExpr()
            => new Add(
                new Literal(1),
                new Add(
                    new Literal(2),
                    new Literal(3)));

        public static void Test()
        {
            var expr = CreateTestExpr();

            Console.WriteLine();
            Console.WriteLine("Visitor test");
            Console.WriteLine($"   1 + (2 + 3) = {expr.Accept(new EvalVisitor())}");
        }
    }
}

namespace VisitorExt
{
    using Visitor;

    // New behavior
    class PrintVisitor : IVisitor<string>
    {
        public string VisitLiteral(Literal literal)
            => literal.N.ToString();

        public string VisitAdd(Add add)
            => String.Format($"({add.A.Accept(this)} + {add.B.Accept(this)})");
    }

    static class VisitorExt
    {
        public static void Test()
        {
            var expr = Visitor.CreateTestExpr();

            Console.WriteLine();
            Console.WriteLine("VisitorExt test");
            Console.WriteLine($"   Print: {expr.Accept(new PrintVisitor())}");
        }

        // Can't add a new expression type (because it requires a change to IVisitor)
    }
}