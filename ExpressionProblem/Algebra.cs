using System;

namespace Algebra
{
    // "object algebra interface" - like an abstract factory
    // https://oleksandrmanzyuk.wordpress.com/2014/06/18/from-object-algebras-to-finally-tagless-interpreters-2/
    interface IExprAlgebra<T>
    {
        T Literal(int n);
        T Add(T a, T b);
    }

    interface IEvalExpr
    {
        int Eval();
    }

    class EvalExpr : IEvalExpr
    {
        public EvalExpr(Func<int> eval)
        {
            _eval = eval;
        }
        Func<int> _eval;

        public int Eval()
            => _eval();
    }

    // "object algebra" - like a concrete factory
    class EvalAlgebra : IExprAlgebra<IEvalExpr>
    {
        public IEvalExpr Literal(int n)
            => new EvalExpr(() => n);

        public IEvalExpr Add(IEvalExpr a, IEvalExpr b)
            => new EvalExpr(() => a.Eval() + b.Eval());
    }

    static class Algebra
    {
        public static T CreateTestExpr<T>(IExprAlgebra<T> factory)
            => factory.Add(
                factory.Literal(1),
                factory.Add(
                    factory.Literal(2),
                    factory.Literal(3)));

        public static void Test()
        {
            Console.WriteLine();
            Console.WriteLine("Algebra test");

            var expr = CreateTestExpr(new EvalAlgebra());
            Console.WriteLine($"   1 + (2 + 3) = {expr.Eval()}");
        }
    }
}

namespace AlgebraExt
{
    using Algebra;

    interface IExprAlgebraExt<T> : IExprAlgebra<T>
    {
        T Mult(T a, T b);
    }

    class EvalAlgrebraExt : EvalAlgebra, IExprAlgebraExt<IEvalExpr>
    {
        public IEvalExpr Mult(IEvalExpr a, IEvalExpr b)
            => new EvalExpr(() => a.Eval() * b.Eval());
    }

    interface IStringifyExpr
    {
        string Stringify();
    }

    class StringifyExpr : IStringifyExpr
    {
        public StringifyExpr(Func<string> stringify)
        {
            _stringify = stringify;
        }
        Func<string> _stringify;

        public string Stringify()
            => _stringify();
    }

    class StringifyAlgebra : IExprAlgebraExt<IStringifyExpr>
    {
        public IStringifyExpr Literal(int n)
            => new StringifyExpr(() => n.ToString());

        public IStringifyExpr Add(IStringifyExpr a, IStringifyExpr b)
            => new StringifyExpr(() => $"{a.Stringify()} + {b.Stringify()}");

        // we could've skipped this if it wasn't needed by inheriting directly from IExprFactor<T> instead
        public IStringifyExpr Mult(IStringifyExpr a, IStringifyExpr b)
            => new StringifyExpr(() => $"{a.Stringify()} * {b.Stringify()}");
    }

    static class AlgebraExt
    {
        // 4 * (5 + 6)
        public static T CreateTestExprExt<T>(IExprAlgebraExt<T> factory)
            => factory.Mult(
                factory.Literal(4),
                factory.Add(
                    factory.Literal(5),
                    factory.Literal(6)));

        public static void Test()
        {
            Console.WriteLine();
            Console.WriteLine("AlgebraExt test");

            var evalExpr = CreateTestExprExt(new EvalAlgrebraExt());
            Console.WriteLine($"   4 * (5 + 6) = {evalExpr.Eval()}");

            var stringifyExprA = Algebra.CreateTestExpr(new StringifyAlgebra());
            Console.WriteLine($"   Stringify: {stringifyExprA.Stringify()}");

            var stringifyExprB = CreateTestExprExt(new StringifyAlgebra());
            Console.WriteLine($"   Stringify: {stringifyExprB.Stringify()}");
        }
    }
}