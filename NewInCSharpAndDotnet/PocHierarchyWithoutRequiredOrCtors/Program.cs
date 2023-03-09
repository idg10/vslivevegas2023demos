
using PocHierarchyWithoutRequiredOrCtors;

SmoothieProduct passionFruitAndMangoSmoothie = new()
{
    Id = "27b/6",
    Sku = "5038862340106",
    Name = "Passion Fruit and Mango Smoothie",
    Category = "Soft drink",
    DateIntroduced = new DateOnly(2005, 2, 4),

    Flavour = "Passion fruit and mango",
    VolumeMl = 250
};


Console.WriteLine(passionFruitAndMangoSmoothie.Flavour);
Console.WriteLine(passionFruitAndMangoSmoothie.Id);