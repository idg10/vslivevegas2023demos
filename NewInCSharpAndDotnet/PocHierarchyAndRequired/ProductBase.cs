namespace PocHierarchyAndRequired;

public class ProductBase : CategorisedItem
{
    public required string Sku { get; set; }

    public required string Name { get; set; }

    public required DateOnly DateIntroduced { get; set; }

    public DateOnly? DateDiscontinued { get; set; }
}
