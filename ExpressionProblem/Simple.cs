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

    static class SimpleExt
    {
        // 4 * (5 + 6)
        public static IExpr CreateTestExpr()
            => new Mult(
                new Literal(4),
                new Add(
                    new Literal(5),
                    new Literal(6)));

        public static void Test()
        {
            var expr = CreateTestExpr();

            Console.WriteLine();
            Console.WriteLine("SimpleExt test");
            Console.WriteLine($"   4 * (5 + 6) = {expr.Eval()}");
        }
    }

    // Can't add new behavior
}
