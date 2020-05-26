using System;

namespace Simple
{
    interface IExpr
    {
        int Eval();
    }

    class Literal : IExpr
    {
        public Literal(int n)
        {
            N = n;
        }

        public int N { get; }

        public int Eval()
            => N;
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

        public int Eval()
            => A.Eval() + B.Eval();
    }

    static class Simple
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
            Console.WriteLine("Simple test");
            Console.WriteLine($"   1 + (2 + 3) = {expr.Eval()}");
        }
    }
}

namespace SimpleExt
{
    using Simple;

    // New expression type
    class Mult : IExpr
    {
        public Mult(IExpr a, IExpr b)
        {
            A = a;
            B = b;
        }

        public IExpr A { get; }
        public IExpr B { get; }

        public int Eval()
            => A.Eval() * B.Eval();
    }

    // Can't add new behavior
}
