using System.Numerics;

static T Average<T>(T a, T b)
    where T : INumber<T>
    => (a + b) / T.CreateChecked(2);

Console.WriteLine(Average(10, 20));
