using System.Numerics;

static T Average<T>(T one, T two)
    where T: INumber<T>
    => (one + two) / T.CreateChecked(2);

Console.WriteLine(Average(10, 20));
