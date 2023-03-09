namespace PocHierarchyWithoutRequiredUsingCtors;

public class CategorisedItem : ItemWithId
{
    public CategorisedItem(
        string id,
        string category) : base(id)
    {
        Category = category;
    }

    public string Category { get; set; }
}
