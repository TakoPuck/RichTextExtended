namespace RichTextExtended.Source;

public class OpenTagToken : IToken
{
    public string Name { get; }

    public string[] Args { get; }


    public OpenTagToken(string name, string[] args)
    {
        Name = name;
        Args = args;
    }

    public override string ToString()
    {
        return $"Name: '{Name}', Args: '{string.Join("' '", Args)}'";
    }
}
