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
            var expr = CreateTestExpr(new EvalFactoryExt());

            Console.WriteLine();
            Console.WriteLine("SimpleExt test");
            Console.WriteLine($"   4 * (5 + 6) = {expr.Eval()}");
        }
    }
}