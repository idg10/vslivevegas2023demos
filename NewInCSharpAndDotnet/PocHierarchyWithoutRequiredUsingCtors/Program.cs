
using PocHierarchyWithoutRequiredUsingCtors;

SmoothieProduct passionFruitAndMangoSmoothie = new(
    id: "27b/6",
    sku: "5038862340106",
    name: "Passion Fruit and Mango Smoothie",
    category: "Soft drink",
    dateIntroduced: new DateOnly(2005, 2, 4),
    flavour: "Passion fruit and mango",
    volumeMl: 250);


Console.WriteLine(passionFruitAndMangoSmoothie.Flavour);
Console.WriteLine(passionFruitAndMangoSmoothie.Id);