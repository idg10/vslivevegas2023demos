namespace PocHierarchyWithoutRequiredUsingCtors;

public class ProductBase : CategorisedItem
{
    public ProductBase(
        string id,
        string category,
        string sku,
        string name,
        DateOnly dateIntroduced) : base(id, category)
    {
        Sku = sku;
        Name = name;
        DateIntroduced = dateIntroduced;
    }

    public string Sku { get; set; }

    public string Name { get; set; }

    public DateOnly DateIntroduced { get; set; }

    public DateOnly? DateDiscontinued { get; set; }
}
