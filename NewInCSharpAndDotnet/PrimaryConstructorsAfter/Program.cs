Console.WriteLine(new WithPrimaryConstructor(42, "The answer"));

public class WithoutPrimaryConstructors
{
    private int id;
    private string displayName;

    public WithoutPrimaryConstructors(int id, string displayName)
    {
        this.id = id;
        this.displayName = displayName;
    }

    public override string ToString() => $"{id}: '{displayName}'";
}

public class WithPrimaryConstructor(int id, string displayName)
{
    public override string ToString() => $"{id}: '{displayName}'";
}