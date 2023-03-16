Console.WriteLine(new TypeWithCtor(42, "The answer"));

public class TypeWithCtor
{
    private int id;
    private string displayName;

    public TypeWithCtor(int id, string displayName)
    {
        this.id = id;
        this.displayName = displayName;
    }

    public override string ToString() => $"{id}: '{displayName}'";
}