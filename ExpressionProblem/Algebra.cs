﻿using System;

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

    class EvalAlgebraExt : EvalAlgebra, IExprAlgebraExt<IEvalExpr>
    {
        public IEvalExpr Mult(IEvalExpr a, IEvalExpr b)
            => new EvalExpr(() => a.Eval() * b.Eval());
    }

    interface IPrintExpr
    {
        string Print();
    }

    class PrintExpr : IPrintExpr
    {
        public PrintExpr(Func<string> print)
        {
            _print = print;
        }
        Func<string> _print;

        public string Print()
            => _print();
    }

    class PrintAlgebra : IExprAlgebraExt<IPrintExpr>
    {
        public IPrintExpr Literal(int n)
            => new PrintExpr(() => n.ToString());

        public IPrintExpr Add(IPrintExpr a, IPrintExpr b)
            => new PrintExpr(() => $"{a.Print()} + {b.Print()}");

        // we could've skipped this if it wasn't needed by inheriting directly from IExprFactor<T> instead
        public IPrintExpr Mult(IPrintExpr a, IPrintExpr b)
            => new PrintExpr(() => $"{a.Print()} * {b.Print()}");
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

            var evalExpr = CreateTestExprExt(new EvalAlgebraExt());
            Console.WriteLine($"   4 * (5 + 6) = {evalExpr.Eval()}");

            var printExprA = Algebra.CreateTestExpr(new PrintAlgebra());
            Console.WriteLine($"   Print: {printExprA.Print()}");

            var printExprB = CreateTestExprExt(new PrintAlgebra());
            Console.WriteLine($"   Print: {printExprB.Print()}");
        }
    }
}

namespace Combine
{
    using Algebra;
    using AlgebraExt;

    class ExprAlgCombine<T, U, IExprAlgebraT, IExprAlgebraU> : IExprAlgebra<(T, U)>
        where IExprAlgebraT : IExprAlgebra<T>
        where IExprAlgebraU : IExprAlgebra<U>
    {
        public ExprAlgCombine(IExprAlgebraT tAlg, IExprAlgebraU uAlg)
        {
            _tAlg = tAlg;
            _uAlg = uAlg;
        }
        private IExprAlgebraT _tAlg;
        private IExprAlgebraU _uAlg;

        public (T, U) Literal(int n)
            => (_tAlg.Literal(n),
                _uAlg.Literal(n));

        public (T, U) Add((T, U) a, (T, U) b)
            => (_tAlg.Add(a.Item1, b.Item1),
                _uAlg.Add(a.Item2, b.Item2));
    }

    static class ExprAlgCombine
    {
        public static ExprAlgCombine<T, U, IExprAlgebra<T>, IExprAlgebra<U>> Create<T, U>(IExprAlgebra<T> tAlg, IExprAlgebra<U> uAlg)
            => new ExprAlgCombine<T, U, IExprAlgebra<T>, IExprAlgebra<U>>(tAlg, uAlg);
    }

    static class Combine
    {
        public static void Test()
        {
            Console.WriteLine();
            Console.WriteLine("Combine test");

            var alg = ExprAlgCombine.Create(new EvalAlgebra(), new PrintAlgebra());
            var expr = Algebra.CreateTestExpr(alg);
            Console.WriteLine($"   1 + (2 + 3) = {expr.Item1.Eval()}");
            Console.WriteLine($"   Print: {expr.Item2.Print()}");
        }
    }
}
