namespace PocHierarchyWithoutRequiredOrCtors;

public class ProductBase : CategorisedItem
{
    public string Sku { get; set; }

    public string Name { get; set; }

    public DateOnly DateIntroduced { get; set; }

    public DateOnly? DateDiscontinued { get; set; }
}
