class Program
{
    static void Main(string[] args)
    {
        Simple.Simple.Test();
        SimpleExt.SimpleExt.Test();

        Visitor.Visitor.Test();
        VisitorExt.VisitorExt.Test();

        Algebra.Algebra.Test();
        AlgebraExt.AlgebraExt.Test();
        Combine.Combine.Test();
    }
}
