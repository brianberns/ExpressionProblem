using System;

namespace Algebra
{
    // "object algebra"
    // https://oleksandrmanzyuk.wordpress.com/2014/06/18/from-object-algebras-to-finally-tagless-interpreters-2/
    interface IExprFactory<T>
    {
        T Literal(int n);
        T Add(T a, T b);
    }

    interface IEval
    {
        int Eval();
    }

    class EvalImpl : IEval
    {
        public EvalImpl(Func<int> func)
        {
            _func = func;
        }
        Func<int> _func;

        public int Eval()
            => _func();
    }

    class EvalFactory : IExprFactory<IEval>
    {
        public IEval Literal(int n)
            => new EvalImpl(() => n);

        public IEval Add(IEval a, IEval b)
            => new EvalImpl(() => a.Eval() + b.Eval());
    }

    static class Algebra
    {
        public static T CreateTestExpr<T>(IExprFactory<T> factory)
            => factory.Add(
                factory.Literal(1),
                factory.Add(
                    factory.Literal(2),
                    factory.Literal(3)));

        public static void Test()
        {
            Console.WriteLine();
            Console.WriteLine("Algebra test");

            var expr = CreateTestExpr(new EvalFactory());
            Console.WriteLine($"   1 + (2 + 3) = {expr.Eval()}");
        }
    }
}

namespace AlgebraExt
{
    using Algebra;

    interface IExprFactoryExt<T> : IExprFactory<T>
    {
        T Mult(T a, T b);
    }

    class EvalFactoryExt : EvalFactory, IExprFactoryExt<IEval>
    {
        public IEval Mult(IEval a, IEval b)
            => new EvalImpl(() => a.Eval() * b.Eval());
    }

    interface IStringify
    {
        string Stringify();
    }

    class StringifyImpl : IStringify
    {
        public StringifyImpl(Func<string> func)
        {
            _func = func;
        }
        Func<string> _func;

        public string Stringify()
            => _func();
    }

    class StringifyFactory : IExprFactoryExt<IStringify>
    {
        public IStringify Literal(int n)
            => new StringifyImpl(() => n.ToString());

        public IStringify Add(IStringify a, IStringify b)
            => new StringifyImpl(() => $"{a.Stringify()} + {b.Stringify()}");

        // we could've skipped this if it wasn't needed by inheriting directly from IExprFactor<T> instead
        public IStringify Mult(IStringify a, IStringify b)
            => new StringifyImpl(() => $"{a.Stringify()} * {b.Stringify()}");
    }

    static class AlgebraExt
    {
        // 4 * (5 + 6)
        public static T CreateTestExpr<T>(IExprFactoryExt<T> factory)
            => factory.Mult(
                factory.Literal(4),
                factory.Add(
                    factory.Literal(5),
                    factory.Literal(6)));

        public static void Test()
        {
            Console.WriteLine();
            Console.WriteLine("SimpleExt test");

            var eval = CreateTestExpr(new EvalFactoryExt());
            Console.WriteLine($"   4 * (5 + 6) = {eval.Eval()}");

            var stringify = CreateTestExpr<IStringify>(new StringifyFactory());
            Console.WriteLine($"   Stringify: {stringify.Stringify()}");
        }
    }
}