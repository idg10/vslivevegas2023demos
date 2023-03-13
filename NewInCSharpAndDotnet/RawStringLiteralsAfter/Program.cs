string j1 = """{"myProp":  42}""";

Console.WriteLine(j1);

string gettingSillyNow = """"This contains """ just to make life hard"""";

Example.UseJson(42);

public static class Example
{
    public static void UseJson(int value)
    {
        string j2 = $$"""
            {
              "prop1": {{value}},
              "prop2": {
                "nested": [1, 2, 3]
              }
            }
            """;

        Console.WriteLine(j2);
    }
}