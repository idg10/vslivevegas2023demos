namespace PocHierarchyWithoutRequiredUsingCtors;

public class SmoothieProduct : ProductBase
{
    public SmoothieProduct(
        string id,
        string category,
        string sku,
        string name,
        DateOnly dateIntroduced,
        string flavour,
        int volumeMl) : base(id, category, sku, name, dateIntroduced)
    {
        Flavour = flavour;
        VolumeMl = volumeMl;
    }

    public string Flavour { get; set; }

    public int VolumeMl { get; set; }
}
