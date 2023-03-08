

void ShowOld(string individual)
{
    Console.WriteLine($"{individual} {individual switch { "I" or "You" or "They" => "have", _ => "has", }} {individual switch { "I" => "given a confidential security briefing", "You" => "leaked", _ => "been charged under section 2a of the Official Secrets Act"}}.");
}

void ShowNew(string individual)
{
    Console.WriteLine($"{individual} {individual switch
    {
        "I" or "You" or "They" => "have",
        _ => "has",
    }} {individual switch
    {
        "I" => "given a confidential security briefing",
        "You" => "leaked",
        _ => "been charged under section 2a of the Official Secrets Act"
    }}.");
}


ShowOld("I");
ShowNew("You");
ShowNew("She");