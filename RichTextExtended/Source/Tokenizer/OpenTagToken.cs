namespace RichTextExtended.Source.Tokenizer;

public class OpenTagToken : IToken
{
    public string Name { get; }

    public string[] Args { get; }


    public OpenTagToken(string name, string[] args)
    {
        Name = name;
        Args = args;
    }

    private bool IsIndexOutOfBounds(int index) => index < 0 || index >= Args.Length;

    public string GetArg(int index, string defaultValue = "")
    {
        if (IsIndexOutOfBounds(index))
        {
            return defaultValue;
        }

        return Args[index];
    }

    public bool TryGetArg(int index, out string value)
    {
        if (IsIndexOutOfBounds(index))
        {
            value = string.Empty;
            return false;
        }

        value = Args[index];
        return true;
    }

    public override string ToString()
    {
        return $"Name: '{Name}', Args: '{string.Join("' '", Args)}'";
    }
}
