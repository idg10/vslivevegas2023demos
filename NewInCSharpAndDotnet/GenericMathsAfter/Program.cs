using System.Numerics;

static T Average<T>(T a, T b)
    where T : INumberBase<T>
{
    return (a + b) / T.CreateChecked(2);
}

Console.WriteLine(Average(10, 20));
Console.WriteLine(Average(10.0, 20.2));
Console.WriteLine(Average(new Complex(10, 0), new Complex(20, 0)));
